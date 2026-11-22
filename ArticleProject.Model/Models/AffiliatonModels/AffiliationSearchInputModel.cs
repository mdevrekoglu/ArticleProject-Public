using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.AffiliatonModels
{
    public class AffiliationSearchInputModel : BaseSearchModel
    {
        public string? AffiliationName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public long? ScopusId { get; set; }
        public string? OrganizationType { get; set; }

        public AffiliationSearchInputModel() : base()
        {
            SortingColumn = "affiliationName";
            SortingType = "asc";
        }
    }
}