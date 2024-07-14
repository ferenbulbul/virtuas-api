using System;
namespace fazz.Models.Entities
{
	public class Question
	{
        public int Id { get; set; }
        public string? Title { get; set; }
        public int CategoryId { get; set; }
    }
}

