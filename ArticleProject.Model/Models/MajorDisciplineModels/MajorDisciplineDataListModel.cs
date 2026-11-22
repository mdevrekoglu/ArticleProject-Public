using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.MajorDisciplineModels
{
    public class MajorDisciplineDataListModel : BaseListModel
    {
        public List<MajorDisciplineDataModel> Data { get; set; }

        public MajorDisciplineDataListModel() : base()
        {
            Data = new List<MajorDisciplineDataModel>();
        }
    }
}