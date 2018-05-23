
namespace CrapDAP
{


    public class TestLdap
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
            else if (values.Length > 1)
            {
                throw new Novell.Directory.Ldap.LdapLocalException(
                          Novell.Directory.Ldap.Utilclass.ExceptionMessages.MULTIPLE_SCHEMA
                        , new System.Object[] { "" }
                        , Novell.Directory.Ldap.LdapException.CONSTRAINT_VIOLATION
                    );
            }
            return values[0];
        }

        private static string GetConfigurationNamingContext(Novell.Directory.Ldap.LdapConnection conn)
        {
            return GetScalarContext(conn, "configurationNamingContext");
        }

        private static string GetDefaultNamingContext(Novell.Directory.Ldap.LdapConnection conn)
        {
            return GetScalarContext(conn, "defaultNamingContext");
        }


        private static string GetDnsHostName(Novell.Directory.Ldap.LdapConnection conn)
        {
            return GetScalarContext(conn, "dnsHostName	");
        }


        public static System.Collections.Generic.List<ARSoft.Tools.Net.Dns.SrvRecord> GetLdap()
        {
            System.Net.NetworkInformation.IPGlobalProperties ipgp =
                System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();

            // IDnsResolver resolver = new RecursiveDnsResolver(); // Warning: Doesn't work
            ARSoft.Tools.Net.Dns.IDnsResolver resolver = new ARSoft.Tools.Net.Dns.DnsStubResolver();

            ARSoft.Tools.Net.DomainName dn = ARSoft.Tools.Net.DomainName.Parse("_ldap._tcp." + ipgp.DomainName);

            System.Collections.Generic.List<ARSoft.Tools.Net.Dns.SrvRecord> srvRecords = resolver.Resolve<ARSoft.Tools.Net.Dns.SrvRecord>(dn, ARSoft.Tools.Net.Dns.RecordType.Srv);

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


        // https://stackoverflow.com/questions/5162897/how-can-i-get-a-list-of-users-from-active-directory
        public static void GetUsers()
        {
            System.Collections.Generic.List<ARSoft.Tools.Net.Dns.SrvRecord> lsLdap = GetLdap();
            ARSoft.Tools.Net.Dns.SrvRecord ldap = lsLdap[0];
            
            string[] attrs = new string[] { "cn", "distinguishedName", "sAMAccountName", "userPrincipalName"
                , "displayName", "givenName", "sn", "mail", "mailNickname"
                , "memberOf", "homeDirectory", "msExchUserCulture" };

            // CN = Common Name
            // OU = Organizational Unit
            // DC = Domain Component

            // cn                 Patrick Zihlmann
            // distinguishedName  CN=Patrick Zihlmann,OU=Benutzer,OU=MY_DOMAIN,DC=my_domain,DC=local
            // sAMAccountName	  Patrick.Zihlmann
            // userPrincipalName  zihlmann@my-domain.com
            // displayName	      Patrick Zihlmann
            // givenName          Patrick
            // sn                 Zihlmann
            // mail               zihlmann@my-domain.com
            // mailNickname       Patrick.Zihlmann
            // memberOf           CN=CS_ALL,OU=Groups,OU=Sample,DC=my_domain,DC=local
            // homeDirectory      \\dom-data01\user$\firstname.lastname

            // objectClass	top
            // objectClass	person
            // objectClass	organizationalPerson
            // objectClass	user

            // msExchUserCulture	de-CH


            
            string searchFilter = "(&(objectClass=user)(objectCategory=person))";

            string ldapHost = MySamples.TestSettings.ldapHost;
            int ldapPort = MySamples.TestSettings.ldapPort;//System.Convert.ToInt32(args[1]);
            string loginDN = MySamples.TestSettings.loginDN; // args[2];
            string password = MySamples.TestSettings.password; // args[3];


            Novell.Directory.Ldap.LdapConnection lc = new Novell.Directory.Ldap.LdapConnection();
            int ldapVersion = Novell.Directory.Ldap.LdapConnection.Ldap_V3;
            try
            {
                // connect to the server
                lc.Connect(ldap.Target.ToString(), ldap.Port);
                // bind to the server
                lc.Bind(ldapVersion, loginDN, password);

                Novell.Directory.Ldap.LdapSearchConstraints cons = lc.SearchConstraints;
                cons.ReferralFollowing = true;
                lc.Constraints = cons;

                string searchBase = "DC=MY_DOMAIN,DC=local";
                searchBase = GetDefaultNamingContext(lc);

                // To enable referral following, use LDAPConstraints.setReferralFollowing passing TRUE to enable referrals, or FALSE(default) to disable referrals.

               Novell.Directory.Ldap.LdapSearchResults lsc = lc.Search(searchBase,
                                                Novell.Directory.Ldap.LdapConnection.SCOPE_SUB,
                                                searchFilter,
                                                attrs,
                                                false,
                                                (Novell.Directory.Ldap.LdapSearchConstraints)null);

                while (lsc.HasMore())
                {
                    Novell.Directory.Ldap.LdapEntry nextEntry = null;
                    try
                    {
                        nextEntry = lsc.Next();
                    }
                    catch (Novell.Directory.Ldap.LdapReferralException eR)
                    {
                        // https://stackoverflow.com/questions/46052873/ldap-referal-error
                        // The response you received means that the directory you are requesting does not contain the data you look for, 
                        // but they are in another directory, and in the response there is the information about the "referral" directory 
                        // on which you need to rebind to "redo" the search.This principle in LDAP are the referral.
                        // https://www.novell.com/documentation/developer/ldapcsharp/?page=/documentation/developer/ldapcsharp/cnet/data/bp31k5d.html
                        // To enable referral following, use LDAPConstraints.setReferralFollowing passing TRUE to enable referrals, or FALSE (default) to disable referrals.

                        // are you sure your bind user meaning
                        // auth.impl.ldap.userid=CN=DotCMSUser,OU=Service Accounts,DC=mycompany,DC=intranet
                        // auth.impl.ldap.password = mypassword123
                        // has permissions to the user that is logging in and its groups?
                        System.Diagnostics.Debug.WriteLine(eR.LdapErrorMessage);
                    }
                    catch (Novell.Directory.Ldap.LdapException e)
                    {
                        // WARNING: Here catches only LDAP-Exception, no other types...
                        System.Console.WriteLine("Error: " + e.LdapErrorMessage);
                        // Exception is thrown, go for next entry
                        continue;
                    }


                    Novell.Directory.Ldap.LdapAttribute atCN = nextEntry.getAttribute("cn");
                    Novell.Directory.Ldap.LdapAttribute atUN = nextEntry.getAttribute("sAMAccountName");
                    Novell.Directory.Ldap.LdapAttribute atDN = nextEntry.getAttribute("distinguishedName");
                    Novell.Directory.Ldap.LdapAttribute atDIN = nextEntry.getAttribute("displayName");

                    if (atCN != null)
                        System.Console.WriteLine(atCN.StringValue);
                    if (atUN != null)
                        System.Console.WriteLine(atUN.StringValue);

                    if (atDN != null)
                        System.Console.WriteLine(atDN.StringValue);

                    if (atDIN != null)
                        System.Console.WriteLine(atDIN.StringValue);


                    System.Console.WriteLine("\n" + nextEntry.DN);
                    Novell.Directory.Ldap.LdapAttributeSet attributeSet = nextEntry.getAttributeSet();

                    System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                    while (ienum.MoveNext())
                    {
                        Novell.Directory.Ldap.LdapAttribute attribute = (Novell.Directory.Ldap.LdapAttribute)ienum.Current;
                        string attributeName = attribute.Name;
                        string attributeVal = attribute.StringValue;
                        System.Console.WriteLine(attributeName + "value:" + attributeVal);
                    }
                }


            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                // disconnect with the server
                lc.Disconnect();
            }
        } // End Function GetUsers 


        public static void GetGroups()
        {
            System.Collections.Generic.List<ARSoft.Tools.Net.Dns.SrvRecord> lsLdap = GetLdap();
            ARSoft.Tools.Net.Dns.SrvRecord ldap = lsLdap[0];

            string[] attrs = new string[] { "cn", "sAMAccountName", "distinguishedName", "member" };

            // cn                 Exchange Servers
            // sAMAccountName     Exchange Servers
            // distinguishedName  CN=Exchange Servers,OU=Microsoft Exchange Security Groups,DC=my_domain,DC=local
            // member             CN=MYDOMAIN-EXCHANGE,CN=Computers,DC=my_domain,DC=local
            // name               Exchange Servers


            string searchFilter = "(objectCategory=group)";

            string ldapHost = MySamples.TestSettings.ldapHost;
            int ldapPort = MySamples.TestSettings.ldapPort;//System.Convert.ToInt32(args[1]);
            string loginDN = MySamples.TestSettings.loginDN; // args[2];
            string password = MySamples.TestSettings.password; // args[3];

            int ldapVersion = Novell.Directory.Ldap.LdapConnection.Ldap_V3;
            Novell.Directory.Ldap.LdapConnection lc = null;
            
            try
            {
                lc = new Novell.Directory.Ldap.LdapConnection();

                // connect to the server
                lc.Connect(ldap.Target.ToString(), ldap.Port);
                // bind to the server
                lc.Bind(ldapVersion, loginDN, password);


                Novell.Directory.Ldap.LdapSearchConstraints cons = lc.SearchConstraints;
                cons.ReferralFollowing = true;
                lc.Constraints = cons;

                string searchBase = "DC=MY_DOMAIN,DC=local";
                searchBase = GetDefaultNamingContext(lc);

                Novell.Directory.Ldap.LdapSearchResults lsc = lc.Search(searchBase,
                                                Novell.Directory.Ldap.LdapConnection.SCOPE_SUB,
                                                searchFilter,
                                                attrs,
                                                false,
                                                (Novell.Directory.Ldap.LdapSearchConstraints)null);

                while (lsc.HasMore())
                {
                    Novell.Directory.Ldap.LdapEntry nextEntry = null;
                    try
                    {
                        nextEntry = lsc.Next();
                    }
                    catch (Novell.Directory.Ldap.LdapException e)
                    {
                        System.Console.WriteLine("Error: " + e.LdapErrorMessage);
                        // Exception is thrown, go for next entry
                        continue;
                    }

                    Novell.Directory.Ldap.LdapAttribute attMember = nextEntry.getAttribute("member");
                    if (attMember != null)
                    {
                        foreach (string usrcn in attMember.StringValueArray)
                        {
                            System.Console.WriteLine(" -- " + usrcn);
                            // CN refers to: 
                            // Novell.Directory.Ldap.LdapEntry cn = lc.Read(usrcn);
                            // System.Console.WriteLine(cn);
                        }
                    }


                    System.Console.WriteLine("\n" + nextEntry.DN);
                    Novell.Directory.Ldap.LdapAttributeSet attributeSet = nextEntry.getAttributeSet();

                    System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                    while (ienum.MoveNext())
                    {
                        Novell.Directory.Ldap.LdapAttribute attribute = (Novell.Directory.Ldap.LdapAttribute)ienum.Current;
                        string attributeName = attribute.Name;
                        string attributeVal = attribute.StringValue;
                        System.Console.WriteLine(attributeName + "value:" + attributeVal);
                    }
                }


            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                // disconnect with the server
                if(lc != null)
                    lc.Disconnect();
            }

        } // End Function GetGroups 


    } // End Class TestLdap 


} // End Namespace CrapDAP 
