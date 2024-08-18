using System;

namespace fazz.Models.Entities
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string CategoryTitle { get; set; }
        public  string CategoryDescription { get; set; }
        public List<AnswerAndQuestion> Answers { get; set; }
        public int OfferCount { get; set; }
        public List<string> OfferedClinics { get; set; }
    }
}

