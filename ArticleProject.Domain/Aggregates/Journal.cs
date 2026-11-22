using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class Journal : BaseEntity
    {
        public string Name { get; private set; }
        public string AbbreviationName { get; private set; }
        public string ISSN { get; private set; }
        public string EISSN { get; private set; }
        public decimal? JIF { get; private set; }
        public decimal? JIF5Years { get; private set; }
        public int? Year { get; private set; }

        public long CategoryId { get; private set; }
        public Category Category { get; private set; }

        public Journal()
        { }

        public Journal(string name, string abbreviationName, string issn, string eissn, decimal? jif, decimal? jif5years, int? year, Category category)
        {
            Name = name;
            AbbreviationName = abbreviationName;
            ISSN = issn;
            EISSN = eissn;
            JIF = jif;
            JIF5Years = jif5years;
            Year = year;

            Category = category;
        }

        public void Update(string name, string abbreviationName, string issn, string eissn, decimal? jif, decimal? jif5years, int? year, Category category)
        {
            Name = name;
            AbbreviationName = abbreviationName;
            ISSN = issn;
            EISSN = eissn;
            JIF = jif;
            JIF5Years = jif5years;
            Year = year;
            Category = category;
        }
    }
}