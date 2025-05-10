using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDay = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Admin = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RevokedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);


            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Admin", "BirthDay", "CreatedBy", "CreatedOn", "Gender", "Login", "ModifiedBy", "ModifiedOn", "Name", "Password", "RevokedBy", "RevokedOn" },
                values: new object[] { Guid.Empty, true, null, "system", DateTime.Now, 2, "admin", "system", DateTime.Now, "", "AQAAAAIAAYagAAAAEFRq2TcQJPOKlB0yBC5iJdeMbr/csGC3qAobuTN+tGW3zt7CpEzp4gGvqF2PqOUaCQ==", null, null });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
