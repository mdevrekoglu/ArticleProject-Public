using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.DepartmentModels
{
    public class DepartmentDataListModel : BaseListModel
    {
        public List<DepartmentDataModel> Data { get; set; }

        public DepartmentDataListModel() : base()
        {
            Data = new List<DepartmentDataModel>();
        }
    }
}