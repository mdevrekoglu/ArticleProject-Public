using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.AffiliatonModels
{
    public class AffiliationDataOutputModel : MessageOutputModel
    {
        public long Id { get; set; }
        public string AffiliationName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public long ScopusId { get; set; }
        public string OrganizationType { get; set; }
    }
}