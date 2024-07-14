using System;
using System.Collections.Generic;

namespace fazz.Models.Entities
{
    using System.Collections.Generic;

    public class SummaryData
    {
        public Category Category { get; }
        public Dictionary<int, string> Answers { get; }
        public List<Question> Questions { get; }

        public SummaryData(Category category, Dictionary<int, string> answers, List<Question> questions)
        {
            Category = category;
            Answers = answers;
            Questions = questions;
        }
                
    }

}

