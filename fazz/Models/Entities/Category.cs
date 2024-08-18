using System;
namespace fazz.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Credit { get; set; }
        public bool IsActive { get; set; }
        public List<Question> Questions { get; set; }
    }
}

