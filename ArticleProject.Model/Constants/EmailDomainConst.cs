using ArticleProject.Model.Enums;

namespace ArticleProject.Model.Constants
{
    public class EmailDomainConst
    {
        private static readonly string _studentDomain = "@ogr.deu.edu.tr";
        private static readonly string _lecturerDomain = "@deu.edu.tr";
        private static readonly string _emailConfirmPath = "/authentication/confirm-email";
        private static readonly string _passwordResetPath = "/authentication/reset-password";

        public EmailDomainConst()
        { }

        public static string GetFullEmail(string email, int userType)
        {
            string result = "";

            switch (userType)
            {
                case ((int)UserTypeEnum.Student):
                    result = string.Concat(email, _studentDomain);
                    break;

                case ((int)UserTypeEnum.Lecturer):
                    result = string.Concat(email, _lecturerDomain);
                    break;
            }

            return result;
        }

        public static string getEmailConfirmPath()
        {
            return _emailConfirmPath;
        }

        public static string getPasswordResetPath()
        {
            return _passwordResetPath;
        }
    }
}