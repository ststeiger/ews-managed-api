
namespace RedmineMailService
{
    class UserData
    {
        public static string POP = SecretManager.GetSecret<string>("POP", "RedmineMailService");
        public static string IMAP = SecretManager.GetSecret<string>("IMAP", "RedmineMailService");
        public static string SMTP = SecretManager.GetSecret<string>("SMTP", "RedmineMailService");
        
        public static string Email = SecretManager.GetSecret<string>("Email", "RedmineMailService");
        public static string Password = SecretManager.GetSecret<string>("Password", "RedmineMailService");

        public static string info = SecretManager.GetSecret<string>("info", "RedmineMailService");
        public static string RSN = SecretManager.GetSecret<string>("RSN", "RedmineMailService");
        // public static string RSNA = AES.DeCrypt(SecretManager.GetSecret<string>("RSNA", "RedmineMailService"));
    }
}
