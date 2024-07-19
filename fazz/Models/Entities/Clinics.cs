using System;
namespace fazz.Models.Entities
{
	public class Clinic
	{
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? WebAddress { get; set; }

        public int? Credit { get; set; }        
    }
}

