using System;
namespace fazz.Models.Requests
{
	public class AddOrUpdateClinicsRequest
	{
		public int Id { get; set; }

		public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? WebAddress { get; set; }

        public List<int> Categories { get; set; }


    }
}

