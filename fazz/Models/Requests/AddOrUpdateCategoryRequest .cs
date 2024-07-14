using System;
namespace fazz.Models.Requests
{
	public class AddOrUpdateCategoryRequest
	{
		public int Id { get; set; }

		public string? Title { get; set; }

        public string? Description { get; set; }
    }
}

