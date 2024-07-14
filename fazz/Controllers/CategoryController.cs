using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CategoryController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult Add(AddOrUpdateCategoryRequest request)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM categories WHERE title = @Title";
                var existingCategory = connection.QueryFirstOrDefault<Category>(query, new { Title = request.Title });

                if (existingCategory != null)
                {
                    return Conflict(new { Message = "Title already in use" }); // Title zaten kullanılıyorsa 409 döndür
                }

                var category = new Category
                {
                    Title = request.Title,
                    Description = request.Description,
                    Credit = request.Credit
                };

                var query2 = "INSERT INTO categories (title, description, credit) VALUES (@Title, @Description, @Credit)";
                var result = connection.Execute(query2, category);
                var isCreated = result > 0;

                if (!isCreated)
                {
                    return StatusCode(500, new { Message = " could not be created" }); // Category oluşturulamazsa 500 döndür
                }

                return Ok(new { Message = "Category created successfully" });
            }
        }
               
        [HttpGet]
        public IActionResult Get()
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM categories";
                var categories = connection.Query<Category>(query);                
                return Ok(categories);
            }
        }
       
        [HttpGet]
        public IActionResult GetById(int id)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM categories WHERE Id=@Id ";
                var category = connection.QueryFirstOrDefault<Category>(query, new { Id = id });

                if (category == null)
                {
                    return NotFound();
                }
                var response = new GetCategoryByIdResponse() { Category = category, IsSuccessful = true };


                return Ok(response);
            }
        }


        [HttpPost]
        public IActionResult Update(AddOrUpdateCategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var categoryToUpdate = new Category
            {
                Title = request.Title,
                Description = request.Description,
                Id = request.Id,
                Credit = request.Credit
            };

            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "UPDATE categories SET Title = @Title, Description = @Description, Credit=@Credit WHERE Id = @Id";
                var result = connection.Execute(query, categoryToUpdate);

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

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteQuestionsQuery = "DELETE FROM questions WHERE category_id = @CategoryId";
                        connection.Execute(deleteQuestionsQuery, new { CategoryId = id }, transaction);

                        var deleteCategoryQuery = "DELETE FROM categories WHERE id = @Id";
                        var result = connection.Execute(deleteCategoryQuery, new { Id = id }, transaction);

                        transaction.Commit();

                        if (result > 0)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        return StatusCode(500, "An error occurred while deleting the category and its questions.");
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult GetByClinicId(int clinicId)
        {
            string connectionString = _config.GetConnectionString("schoolPortal");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "select ca.* from fazz.clinics c " +
                            "join fazz.clinic_categories cm on c.id = cm.clinic_id " +
                            "join fazz.categories ca on ca.id = cm.category_id " +
                            "where c.id = @id";
                var categories = connection.Query<Category>(query, new { id = clinicId });
                return Ok(categories);
            }
        }

    }
}
