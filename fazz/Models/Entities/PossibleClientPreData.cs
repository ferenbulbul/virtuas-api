using System;
namespace fazz.Models.Entities
{
	public class PossibleClientPreData
	{
		public int ApplicationId { get; set; }
        public int UserId { get; set; }
		public string CategoryTitle { get; set; }
		public string UserName { get; set; }
        public string UserSurname { get; set; }
		public List<AnswerAndQuestion> Answers { get; set; }
    }
}

