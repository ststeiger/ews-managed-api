
namespace MySamples
{
    public class TestSettings
    {

        public static string ldapHost = SecretManager.GetSecret<string>("ldapHost"); // args[0];
        public static int ldapPort = 389;//System.Convert.ToInt32(args[1]);
        public static string loginDN = SecretManager.GetSecret<string>("loginDN"); // args[2];
        public static string password = SecretManager.GetSecret<string>("Password"); // args[3];
    }
}
