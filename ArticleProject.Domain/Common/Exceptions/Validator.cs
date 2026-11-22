using ArticleProject.Domain.Common.Interfaces.Entity;

namespace ArticleProject.Domain.Common.Exceptions
{
    public static class Validator
    {
        public static T? ThrowIfNull<T>(this T? obj, string? message = null) where T : class, IEntity
        {
            if (obj is null)
            {
                throw new NullReferenceException(message ?? $"{nameof(obj)} cannot be null.");
            }
            return obj;
        }
    }
}