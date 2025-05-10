using AuthService.DB;
using AuthService.Services.DTO;
using AutoMapper;
using System;

namespace AuthService.Services
{
    public class UserServiceMapper : Profile
    {
        public UserServiceMapper()
        {
            CreateMap<DBUser, User>();
        }
    }
}
