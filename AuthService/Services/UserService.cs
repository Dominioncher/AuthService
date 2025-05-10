using AuthService.DB;
using AuthService.Services.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Services
{
    public class UserService
    {
        private readonly PostgreSQLContext _context;

        private readonly IMapper _mapper;

        public UserService(PostgreSQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<User> GetActiveUsers()
        {
            var users = _context.Users.Where(x => x.RevokedOn == null).OrderBy(x => x.CreatedOn);
            return users.ProjectTo<User>(_mapper.ConfigurationProvider).ToList();
        }

        public List<User> GetAllUsers()
        {
            var users = _context.Users.OrderBy(x => x.CreatedOn);
            return users.ProjectTo<User>(_mapper.ConfigurationProvider).ToList();
        }

        public User GetUser(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            return _mapper.Map<User>(user);
        }

        public User GetUser(string login)
        {
            var user = _context.Users.FirstOrDefault(x => x.Login == login);
            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            return _mapper.Map<User>(user);
        }

        public bool VerifyUser(Guid id, string password)
        {
            var user = _context.Users.Find(id);
            if (user == null || user.RevokedOn != null)
            {
                throw new ValidationException("User not found");
            }

            var hasher = new PasswordHasher<DBUser>();
            return hasher.VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Failed;
        }

        public void RestoreUser(Guid id, Guid initiator)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            if (user.RevokedOn != null)
            {
                throw new ValidationException("User already active");
            }

            user.ModifiedBy = initiator.ToString();
            user.ModifiedOn = DateTime.Now;
            user.RevokedBy = null;
            user.RevokedOn = null;
            _context.SaveChanges();
        }

        public void ChangePassword(ChangePasswordDto dto)
        {
            var user = _context.Users.Find(dto.UserId);
            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            var hasher = new PasswordHasher<DBUser>();
            var verify = hasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (verify == PasswordVerificationResult.Success)
            {
                throw new ValidationException("Password must be different from the previous one");
            }

            user.Password = hasher.HashPassword(user, dto.Password);
            user.ModifiedBy = (dto.Initiator ?? dto.UserId).ToString();
            user.ModifiedOn = DateTime.Now;
            _context.SaveChanges();
        }

        public void ChangeLogin(ChangeLoginDto dto)
        {
            var user = _context.Users.Find(dto.UserId);
            if (user == null || user.Id == Guid.Empty)
            {
                throw new ValidationException("User not found");
            }

            if (_context.Users.FirstOrDefault(x => x.Login == dto.Login) != null)
            {
                throw new ValidationException("User with same login already exists");
            }

            user.Login = dto.Login;
            user.ModifiedBy = (dto.Initiator ?? dto.UserId).ToString();
            user.ModifiedOn = DateTime.Now;
            _context.SaveChanges();
        }

        public User CreateUser(CreateUserDto dto)
        {
            if (_context.Users.FirstOrDefault(x => x.Login == dto.Login) != null)
            {
                throw new ValidationException("User with same login already exists");
            }

            var now = DateTime.Now;
            var creator = dto.Initiator.ToString();
            var user = new DBUser()
            {
                Login = dto.Login,
                Name = dto.Name,
                Gender = (int)dto.Gender,
                Admin = dto.Admin,
                CreatedBy = creator,
                CreatedOn = now,
                ModifiedBy = creator,
                ModifiedOn = now
            };
            var hasher = new PasswordHasher<DBUser>();
            user.Password = hasher.HashPassword(user, dto.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return _mapper.Map<User>(user);
        }

        public void RemoveUser(Guid id, Guid initiator, bool permanent = false)
        {
            var user = _context.Users.Find(id);
            if (user == null || user.Id == Guid.Empty)
            {
                throw new ValidationException("User not found");
            }

            if (permanent)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return;
            }

            if (user.RevokedOn != null)
            {
                throw new ValidationException("User already unactive");
            }

            var now = DateTime.Now;
            var revoker = initiator.ToString();
            user.RevokedBy = revoker;
            user.RevokedOn = now;
            user.ModifiedBy = revoker;
            user.ModifiedOn = now;
            _context.SaveChanges();
        }

        public User UpdateUser(UpdateUserDto dto)
        {
            if (dto.Name == null && dto.Gender == null & dto.BirthDay == null)
            {
                throw new ValidationException("No values for update User");
            }

            var user = _context.Users.Find(dto.UserId);
            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            if (dto.Name != null)
            {
                user.Name = dto.Name;
            }
            if (dto.Gender != null)
            {
                user.Gender = (int)dto.Gender;
            }
            if (dto.BirthDay != null)
            {
                user.BirthDay = dto.BirthDay;
            }

            var now = DateTime.Now;
            var modifier = (dto.Initiator ?? dto.UserId).ToString();
            user.ModifiedBy = modifier;
            user.ModifiedOn = now;            
            _context.SaveChanges();
            return _mapper.Map<User>(user);
        }

    }
}
