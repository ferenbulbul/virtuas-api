using System;
namespace fazz.Models.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int QuestionId { get; set; }
        public int ApplicationId { get; set; }
    }
}

