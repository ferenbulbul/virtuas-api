using System;
using fazz.Models.Entities;

namespace fazz.Models.Responses
{
	public class GetQuestionByIdResponse : BaseResponse
	{
        public Question? Question { get; set; }
    }
}

