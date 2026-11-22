using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class Affiliation : BaseEntity
    {
        public string AffiliationName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public long ScopusId { get; set; }
        public string OrganizationType { get; set; }
        //public long AuthorCount { get; set; }
        //public long DocumentCount { get; set; }

        public Affiliation()
        { }

        public Affiliation(string affiliationName, string address, string country, long scopusId, string organizationType)
        {
            AffiliationName = affiliationName;
            Address = address;
            City = country;
            Country = country;
            ScopusId = scopusId;
            OrganizationType = organizationType;
        }

        public void Update(string affiliationName, string country, string city, string address, string organizationType)
        {
            AffiliationName = affiliationName;
            Address = address;
            City = city;
            Country = country;
            OrganizationType = organizationType;
        }

        public void Update(string affiliationName, string country, string city, string address, long scopusId, string organizationType)
        {
            AffiliationName = affiliationName;
            Address = address;
            City = city;
            Country = country;
            ScopusId = scopusId;
            OrganizationType = organizationType;
        }
    }
}