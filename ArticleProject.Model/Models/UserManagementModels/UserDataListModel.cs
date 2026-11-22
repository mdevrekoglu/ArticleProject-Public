using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.UserManagementModels
{
    public class UserDataListModel : BaseListModel
    {
        public List<UserDataModel> Data { get; set; }

        public UserDataListModel() : base()
        {
            Data = new List<UserDataModel>();
        }
    }
}