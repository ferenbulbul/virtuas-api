using System;
namespace fazz.Models.Responses
{
	public class LoginResponse : BaseResponse
	{
		public string? Role { get; set; }
		public string? Username { get; set; }
		public int Id { get; set; }

		public int ClinicId { get; set; }

	}
}

