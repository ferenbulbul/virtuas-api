using System;
namespace fazz.Models.Requests
{
	public class AddOrUpdateQuestionRequest
	{
		public int Id { get; set; }

		public string? Title { get; set; }

        public int CategoryId { get; set; }
    }
}

