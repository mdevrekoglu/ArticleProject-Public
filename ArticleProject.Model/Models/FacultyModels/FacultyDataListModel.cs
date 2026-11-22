using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.FacultyModels
{
    public class FacultyDataListModel : BaseListModel
    {
        public List<FacultyDataModel> Data { get; set; }

        public FacultyDataListModel() : base()
        {
            Data = new List<FacultyDataModel>();
        }
    }
}