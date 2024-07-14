using System;
namespace fazz.Models.Requests
{
	public class AddCategoryWithQuestionsRequest : AddOrUpdateCategoryRequest 
	{
		public List<string>? Questions { get; set; }
    }
}
