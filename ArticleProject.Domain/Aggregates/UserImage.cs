using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class UserImage : BaseEntity
    {
        public byte[] Image { get; set; }

        public UserImage()
        { }

        public UserImage(byte[] image)
        {
            Image = image;
        }
    }
}