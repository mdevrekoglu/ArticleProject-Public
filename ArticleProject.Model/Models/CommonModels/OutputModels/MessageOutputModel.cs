namespace ArticleProject.Model.Models.CommonModels.OutputModels
{
    public class MessageOutputModel
    {
        public List<string> Messages { get; set; }
        public bool IsSuccessed { get; set; }

        public MessageOutputModel()
        {
            Messages = new List<string>();
            IsSuccessed = false;
        }
    }
}