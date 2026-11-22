using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; }
        public int? Quartile { get; private set; }
        public int? Rank { get; private set; }
        public int? TotalJournals { get; private set; } // Bu kategorideki toplam dergi sayısı

        public Category()
        { }

        public Category(string name, int? quartile, int? rank, int? totalJournals)
        {
            Name = name;
            Quartile = quartile;
            Rank = rank;
            TotalJournals = totalJournals;
        }

        public void Update(string name, int? quartile, int? rank, int? totalJournals)
        {
            Name = name;
            Quartile = quartile;
            Rank = rank;
            TotalJournals = totalJournals;
        }
    }
}