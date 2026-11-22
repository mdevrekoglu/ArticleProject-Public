using ArticleProject.Model.Constants;

namespace ArticleProject.Model.Models.CommonModels.InputModels
{
    public class BaseGetOptionsByKeywordModel
    {
        public string Keyword { get; set; }
        public int OptionsSize { get; set; } = QuerySizeConstants.OptionsLimit;
    }
}