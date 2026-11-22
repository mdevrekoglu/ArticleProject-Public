namespace ArticleProject.Model.Models.CommonModels.OutputModels
{
    public class BaseListModel : MessageOutputModel
    {
        public int Count { get; set; }

        public BaseListModel() : base()
        {
            Count = 0;
        }
    }
}