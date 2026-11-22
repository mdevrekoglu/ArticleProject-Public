using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; }
        public int StudentCount { get; set; }
        public int ResearcherCount { get; set; }

        public long FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public Department()
        { }

        public void Update(string departmentName, int studentCount, int researcherCount, long facultyId)
        {
            DepartmentName = departmentName;
            StudentCount = studentCount;
            ResearcherCount = researcherCount;
            FacultyId = facultyId;
        }
    }
}