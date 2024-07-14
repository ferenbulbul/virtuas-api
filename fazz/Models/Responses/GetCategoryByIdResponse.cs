using System;
using fazz.Models.Entities;

namespace fazz.Models.Responses
{
	public class GetCategoryByIdResponse : BaseResponse
	{
		public Category? Category { get; set; }
	}
}

