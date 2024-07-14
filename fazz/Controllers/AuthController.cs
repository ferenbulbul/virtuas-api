using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using fazz.Models.Entities;
using fazz.Models.Requests;
using fazz.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fazz.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM users WHERE email = @Email AND password = @Password";
                var user =  connection.QueryFirstOrDefault<User>(query, new { Email = request.Email, Password = request.Password });

                if (user == null)
                {
                    return Unauthorized(new LoginResponse{ IsSuccessful = false, Role = "" }); // Kullanıcı bulunamazsa 401 döndür
                }

                return Ok(new LoginResponse { IsSuccessful = true, Role = user.Role, Username = user.Name ,Id=user.Id});                
            }
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest request)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM users WHERE email = @Email";
                var existingUser = connection.QueryFirstOrDefault<User>(query, new { Email = request.Email });

                if (existingUser != null)
                {
                    return Conflict(new { Message = "Email already in use" }); // Email zaten kullanılıyorsa 409 döndür
                }

                var user = new User
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Email = request.Email,
                    Password = request.Password, // Parolayı hash'lemeyi düşünün
                    Role = request.Role,
                     PhoneNumber = request.PhoneNumber

                };

                var query2 = "INSERT INTO users (name, surname, email, password, role,phoneNumber) VALUES (@Name, @Surname, @Email, @Password, @Role, @PhoneNumber)";
                var result =  connection.Execute(query2, user);
                var isCreated = result > 0;

                if (!isCreated)
                {
                    return StatusCode(500, new { Message = "User could not be created" }); // Kullanıcı oluşturulamazsa 500 döndür
                }

                return Ok(new { Message = "User created successfully" });
            }
        }





    }
}

