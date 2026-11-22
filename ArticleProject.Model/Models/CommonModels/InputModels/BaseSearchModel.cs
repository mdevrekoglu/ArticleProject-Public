namespace ArticleProject.Model.Models.CommonModels.InputModels
{
    public class BaseSearchModel
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public virtual string SortingColumn { get; set; } = "id";
        private string sortingType { get; set; } = "asc";

        public virtual string SortingType
        {
            get
            {
                if (sortingType.ToLower() == "ascend")
                    return "asc";
                else if (sortingType.ToLower() == "descend")
                    return "desc";
                else
                    return sortingType;
            }
            set
            {
                sortingType = value;
            }
        }

        public BaseSearchModel()
        { }

        public string GetSortingExpression()
        {
            return $"{SortingColumn} {SortingType}";
        }
    }
}