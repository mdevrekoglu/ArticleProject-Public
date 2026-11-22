using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class MajorDiscipline : BaseEntity
    {
        public string MajorDisciplineName { get; set; }

        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        public MajorDiscipline()
        { }

        public void Update(string majorDisciplineName, long departmentId)
        {
            MajorDisciplineName = majorDisciplineName;
            DepartmentId = departmentId;
        }
    }
}