using System;
namespace fazz.Models.Entities
{
	public class ClientData
	{
		public int ApplicationId { get; set; }
		public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
		public DateTime OfferDate { get; set; }
        public string CategoryTitle { get; set; }
		public int Cost { get; set; }
		public List<AnswerAndQuestion> Answers { get; set; }
    }
}

