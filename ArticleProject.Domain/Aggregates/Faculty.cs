using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class Faculty : BaseEntity
    {
        public string FacultyName { get; set; }

        public long AffiliationId { get; set; }
        public Affiliation Affiliation { get; set; }

        public Faculty()
        { }

        public void Update(string facultyName, long affiliationId)
        {
            FacultyName = facultyName;
            AffiliationId = affiliationId;
        }
    }
}