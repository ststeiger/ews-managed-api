using System;

namespace ldap_service
{
    class Program
    {


        private static string GetScalarContext(Novell.Directory.Ldap.LdapConnection conn, string attributeName)
        {
            string[] attributesToFetch = new string[] { attributeName };

            // Read the entries subschemaSubentry attribute. 
            // Throws an exception if no entries are returned.
            Novell.Directory.Ldap.LdapEntry ent = conn.Read("", attributesToFetch);

            Novell.Directory.Ldap.LdapAttribute attr = ent.getAttribute(attributesToFetch[0]);
            string[] values = attr.StringValueArray;
            if (values == null || values.Length < 1)
            {
                throw new Novell.Directory.Ldap.LdapLocalException(
                         Novell.Directory.Ldap.Utilclass.ExceptionMessages.NO_SCHEMA
                       , new System.Object[] { "" }
                       , Novell.Directory.Ldap.LdapException.NO_RESULTS_RETURNED
                   );
            }
            
            if (values.Length > 1)
            {
                throw new Novell.Directory.Ldap.LdapLocalException(
                          Novell.Directory.Ldap.Utilclass.ExceptionMessages.MULTIPLE_SCHEMA
                        , new System.Object[] { "" }
                        , Novell.Directory.Ldap.LdapException.CONSTRAINT_VIOLATION
                    );
            }
            
            return values[0];
        } // End Function GetScalarContext 


        private static string GetConfigurationNamingContext(Novell.Directory.Ldap.LdapConnection conn)
        {
            return GetScalarContext(conn, "configurationNamingContext");
        } // End Function GetConfigurationNamingContext 


        private static string GetDefaultNamingContext(Novell.Directory.Ldap.LdapConnection conn)
        {
            return GetScalarContext(conn, "defaultNamingContext");
        } // End Function GetDefaultNamingContext 



        public static System.Collections.Generic.List<ARSoft.Tools.Net.Dns.SrvRecord> GetLdap()
        {
            System.Net.NetworkInformation.IPGlobalProperties ipgp =
                System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();

            // IDnsResolver resolver = new RecursiveDnsResolver(); // Warning: Doesn't work
            ARSoft.Tools.Net.Dns.IDnsResolver resolver = new ARSoft.Tools.Net.Dns.DnsStubResolver();
            ARSoft.Tools.Net.DomainName dn = ARSoft.Tools.Net.DomainName.Parse("_ldap._tcp." + ipgp.DomainName);

            System.Collections.Generic.List<ARSoft.Tools.Net.Dns.SrvRecord> srvRecords =
                resolver.Resolve<ARSoft.Tools.Net.Dns.SrvRecord>(dn, ARSoft.Tools.Net.Dns.RecordType.Srv);

            //foreach (ARSoft.Tools.Net.Dns.SrvRecord thisRecord in srvRecords)
            //{
            //    // System.Console.WriteLine(thisRecord.Name);
            //    System.Console.WriteLine(thisRecord.Target);
            //    System.Console.WriteLine(thisRecord.Port);

            //    // Note: OR LDAPS:// - but Novell doesn't want these parts anyway 
            //    string url = "LDAP://" + thisRecord.Target + ":" + thisRecord.Port;
            //    System.Console.WriteLine(url);
            //} // Next thisRecord

            return srvRecords;
        } // End Function GetLdap 


        static void Main(string[] args)
        {
            GetGroupMembers();
        }

        static string CalculateMd5(string filename)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        
        static void GetGroupMembers()
        {
            string ldapHost = MySamples.TestSettings.ldapHost;
            int ldapPort = MySamples.TestSettings.ldapPort;//System.Convert.ToInt32(args[1]);

            string msldap = $"LDAP://{ldapHost}:{ldapPort}/DC=COR,DC=local";
            string ms1 = "LDAP://cor-AD02.cor.local:389/OU=Gruppen,OU=COR,DC=COR,DC=local";

            string loginDN = MySamples.TestSettings.loginDN; // args[2];
            string password = MySamples.TestSettings.password; // args[3];

            string strGroup = "COR-VMPost";
            strGroup = "G-ADM-APERTURE-UAT";

            // System.DirectoryServices.AccountManagement.
            //bool valid = false;
            //// https://stackoverflow.com/questions/326818/how-to-validate-domain-credentials
            //using (System.DirectoryServices.AccountManagement.PrincipalContext context = 
            //    new System.DirectoryServices.AccountManagement.PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain))
            //{
            //    valid = context.ValidateCredentials("username", "password");
            //}
            
            bool bException = false;
            using (System.DirectoryServices.DirectoryEntry ldapConnection = 
                new System.DirectoryServices.DirectoryEntry(msldap, loginDN, password))
            {
                try
                {
                    // deRootObject.boun
                    if (ldapConnection.NativeObject == null)
                        bException = true;
                }
                catch (System.Exception ex)
                {
                    bException = true;
                    System.Console.WriteLine(ex.Message);
                    System.Console.WriteLine(ex.StackTrace);
                    throw new System.InvalidOperationException("Cannot login with wrong credentials or LDAP-Path.");
                }
                
                using (System.DirectoryServices.DirectorySearcher dsSearcher = 
                    new System.DirectoryServices.DirectorySearcher(ldapConnection))
                {
                    dsSearcher.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                    dsSearcher.Filter = "(&(objectCategory=group)(CN=" + strGroup + "))";
                    
                    using (System.DirectoryServices.SearchResultCollection srcSearchResultCollection = 
                        dsSearcher.FindAll())
                    { 
                        
                        try
                        {
                            foreach (System.DirectoryServices.SearchResult srSearchResult in srcSearchResultCollection)
                            {
                                System.DirectoryServices.ResultPropertyCollection resultPropColl = srSearchResult.Properties;
                                System.DirectoryServices.PropertyValueCollection memberProperty = srSearchResult.GetDirectoryEntry().Properties["member"];

                                for (int i = 0; i < memberProperty.Count; ++i)
                                {
                                    string strUserName = System.Convert.ToString(memberProperty[i]);
                                    System.Console.WriteLine(strUserName);
                                } // Next i 

                            } // Next srSearchResult 

                        } // End Try 
                        catch (System.Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
                            System.Console.WriteLine(ex.StackTrace);
                        }

                    } // End using srcSearchResultCollection 

                } // End Using dsSearcher 

            } // End Using ldapConnection
            
            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }


        static void TestLogin()
        {
            string ldapHost = MySamples.TestSettings.ldapHost;
            int ldapPort = MySamples.TestSettings.ldapPort;//System.Convert.ToInt32(args[1]);
            string loginDN = MySamples.TestSettings.loginDN; // args[2];
            string password = MySamples.TestSettings.password; // args[3];


            System.Collections.Generic.List<ARSoft.Tools.Net.Dns.SrvRecord> lsLdap = GetLdap();
            ARSoft.Tools.Net.Dns.SrvRecord ldap = lsLdap[0];

            Novell.Directory.Ldap.LdapConnection lc = null;
            int ldapVersion = Novell.Directory.Ldap.LdapConnection.Ldap_V3;
            try
            {
                lc = new Novell.Directory.Ldap.LdapConnection();
                // connect to the server
                lc.Connect(ldap.Target.ToString(), ldap.Port);

                // bind to the server
                lc.Bind(ldapVersion, loginDN, password);
                //lc.Bind(ldapVersion, @"", "");
                // lc.Bind(ldapVersion, (string)null, (string)null);

                System.Console.WriteLine(lc.Bound); // True when login successfull
                


                Novell.Directory.Ldap.LdapSearchConstraints cons = lc.SearchConstraints;
                cons.ReferralFollowing = true;
                lc.Constraints = cons;


                //string dn = "CN=xxx";
                //var entry = lc.Read(dn);
                //libldapsync.Data.LdapGroup grp = libldapsync.Data.LdapGroup.FromEntry(entry);
                //System.Console.WriteLine(grp);

                // string dn = "CN=xxx";
                // var entry = lc.Read(dn);
                // libldapsync.Data.LdapUser user = libldapsync.Data.LdapUser.FromEntry(entry);
                // string json = Newtonsoft.Json.JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented);
                // System.Console.WriteLine(json);

            }
            catch (System.Exception ex)
            { }
            finally
            {
                lc.Disconnect();
            }



                System.Console.WriteLine(System.Environment.NewLine);
            Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }
    }
}
