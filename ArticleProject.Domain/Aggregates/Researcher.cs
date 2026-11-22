using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class Researcher : BaseEntity
    {
        public bool IsActive { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? DocumentCount { get; set; }

        public string Position { get; set; }
        public string StaffTitle { get; set; }

        public long AffiliationId { get; set; }
        public Affiliation Affiliation { get; set; }

        public long? MajorDisciplineId { get; set; }
        public MajorDiscipline? MajorDiscipline { get; set; }

        public User? User { get; set; }

        public string WoSId { get; set; }
        public string ORCId { get; set; }
        public long ScopusId { get; set; }

        public Researcher()
        { }

        public Researcher(string firstName, string lastName, string email, int? documentCount, string position, string staffTitle, long affiliationId, long? majorDisciplineId, string wosId, string orcId, long scopusId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DocumentCount = documentCount;
            Position = position;
            StaffTitle = staffTitle;
            AffiliationId = affiliationId;
            MajorDisciplineId = majorDisciplineId;
            WoSId = wosId;
            ORCId = orcId;
            ScopusId = scopusId;
            IsActive = true;
        }

        public void Update(long affiliationId, int documentCount)
        {
            AffiliationId = affiliationId;
            DocumentCount = documentCount;
        }
    }
}