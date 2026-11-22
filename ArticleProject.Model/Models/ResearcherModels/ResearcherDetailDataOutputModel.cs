using ArticleProject.Model.Models.SearchArticleModels;

namespace ArticleProject.Model.Models.ResearcherModels
{
    public class ResearcherDetailDataOutputModel : ResearcherDataOutputModel
    {
        public string? UserId { get; set; }
        public string? Image { get; set; }

        public List<ArticleDataModel> Articles { get; set; }
    }
}