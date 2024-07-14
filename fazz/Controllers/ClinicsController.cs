using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Azure.Core;
using Dapper;
using fazz.Models.Entities;
using fazz.Models.Requests;
using fazz.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
namespace fazz.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ClinicsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult Add(AddOrUpdateClinicsRequest request)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM clinics WHERE title = @Title and isActive=1";
                var existingQuestion = connection.QueryFirstOrDefault<Clinic>(query, new { Title = request.Title });

                if (existingQuestion != null)
                {
                    return Conflict(new { Message = "Clinic already in use" }); // Title zaten kullanılıyorsa 409 döndür
                }

                var clinic = new Clinic
                {
                    Title = request.Title,
                    Description = request.Description,
                    Address = request.Address,
                    WebAddress = request.WebAddress
                };

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        int clinicId = connection.QuerySingle<int>(@"
                    INSERT INTO clinics (title, description, address, webaddress,isActive,credit)
                    VALUES (@Title, @Description, @Address, @WebAddress,1,0);

                    SELECT LAST_INSERT_ID();", clinic, transaction);

                        foreach (var category in request.Categories)
                        {
                            connection.Execute(@"
                        INSERT INTO clinic_categories ( clinic_id, category_id)
                        VALUES (@ClinicId, @CategoryId);",
                                new { ClinicId = clinicId, CategoryId = category }, transaction);
                        }

                        transaction.Commit();
                        return Ok(new { Message = "Clinic created successfully" });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                        return StatusCode(500, new { Message = " could not be created" }); // Category oluşturulamazsa 500 döndür
                    }
                }               
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM clinics where isActive = 1";
                var clinics = connection.Query<Clinic>(query);
                return Ok(clinics);
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM clinics WHERE Id=@Id ";
                var clinic = connection.QueryFirstOrDefault<Clinic>(query, new { Id = id });

                if (clinic == null)
                {
                    return NotFound();
                }


                return Ok(clinic);
            }
        }


        [HttpPost]
        public IActionResult Update(AddOrUpdateClinicsRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var clinicUpdate = new Clinic
            {
                Id = request.Id,
                Title = request.Title,
                Address = request.Address,
                WebAddress = request.WebAddress,
                Description = request.Description

            };

            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "UPDATE clinics SET Title = @Title,Address=@Address,WebAddress=@WebAddress,Description=@Description WHERE id = @Id";
                var result = connection.Execute(query, clinicUpdate);

                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var deleteClinicsQuery = "update clinics set isActive = 0 WHERE id = @id";
                var result = connection.Execute(deleteClinicsQuery, new { id });
                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
        }

        [HttpPut]
        public IActionResult AddCreditToClinic(int clinicId,int newCredit)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var deleteClinicsQuery = "update clinics set credit = @credit where id = @id";
                var result = connection.Execute(deleteClinicsQuery, new { credit = newCredit, id=clinicId  });
                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
        }





        



    }
}
