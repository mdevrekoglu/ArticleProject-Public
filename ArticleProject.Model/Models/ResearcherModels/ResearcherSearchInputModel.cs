using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.ResearcherModels
{
    public class ResearcherSearchInputModel : BaseSearchModel
    {
        public bool? IsActive { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public string? Position { get; set; }
        public string? StaffTitle { get; set; }

        public long? AffiliationId { get; set; }

        public string? WoSId { get; set; }
        public string? ORCId { get; set; }
        public long? ScopusId { get; set; }

        public ResearcherSearchInputModel() : base()
        {
            SortingColumn = "firstName";
            SortingType = "asc";
        }
    }
}