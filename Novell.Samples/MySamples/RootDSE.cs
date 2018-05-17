
/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
* 
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
* copies of the Software, and to  permit persons to whom the Software is 
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in 
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/
//
// Samples.Search.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//


using System;
using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Utilclass;


namespace MySamples
{


    // https://www.novell.com/documentation/edir873/?page=/documentation/edir873/edir873/data/ah59jqq.html
    // https://web.archive.org/web/20131116081419/http://www.ldapguru.info/ldap/the-root-dse.html
    // https://ff1959.wordpress.com/2011/10/27/ldap-programming-best-practices/
    // https://stackoverflow.com/questions/7787165/ldap-path-problems

    // https://stackoverflow.com/questions/29341818/finding-ldap-domain-name-on-a-virtual-server
    // https://stackoverflow.com/questions/42364587/how-to-use-java-to-query-ldaps-rootdse

    // https://blogs.technet.microsoft.com/heyscriptingguy/2005/11/07/how-can-i-list-all-the-sites-in-active-directory-as-well-as-all-the-servers-in-those-sites/
    // https://social.technet.microsoft.com/Forums/en-US/bafd60c7-c8e6-428d-a965-4a6a2b724394/how-to-get-ad-site-name-from-ip-address-or-subnet?forum=winserverDS


    // https://www.netiq.com/communities/cool-solutions/rootdse-search-edirectory/
    // RootDSE stands for Root DSA (Directory Service Agent) Specific Entry,
    // which is the root of the LDAP server. This entry is a pseudo object in the tree, 
    // which means it’s an unnamed entry at the root of the tree. 
    // This entry holds the configuration information about the connected eDirectory server.
    // As mentioned, the rootDSE is an unnamed entry, a search against the eDirectory tree 
    // won’t return you the rootDSE object.
    // Set the search base to an empty string
    // Set the search filter to objectclass=* (which is the default filter of the ldapsearch tool)
    // Set the search scope to BASE

    // System.DirectoryServices.ActiveDirectory.DirectoryServices.GetComputerSite().Name 
    // Set objRootDSE = GetObject("LDAP://RootDSE")
    // strConfigurationNC = objRootDSE.Get("configurationNamingContext") 
    // strConfigurationNC = LDAP://CN=Configuration,DC=cor,DC=local
    // strSubnetsContainer = "LDAP://cn=Subnets,cn=Sites," & strConfigurationNC
    // LDAP://cn=Subnets,cn=Sites,CN=Configuration,DC=cor,DC=local

    class RootDSE 
    {

        
        public static string GetScalarContext(LdapConnection conn, string attributeName)
        {
            string[] attributesToFetch = new string[] { attributeName };

            // Read the entries subschemaSubentry attribute. 
            // Throws an exception if no entries are returned.
            LdapEntry ent = conn.Read("", attributesToFetch);

            LdapAttribute attr = ent.getAttribute(attributesToFetch[0]);
            string[] values = attr.StringValueArray;
            if (values == null || values.Length < 1)
            {
                throw new LdapLocalException(ExceptionMessages.NO_SCHEMA, new System.Object[] { "" }, LdapException.NO_RESULTS_RETURNED);
            }
            else if (values.Length > 1)
            {
                throw new LdapLocalException(ExceptionMessages.MULTIPLE_SCHEMA, new System.Object[] { "" }, LdapException.CONSTRAINT_VIOLATION);
            }
            return values[0];
        }

        public static string GetConfigurationNamingContext(LdapConnection conn)
        {
            return GetScalarContext(conn, "configurationNamingContext");
        }

        public static string GetDefaultNamingContext(LdapConnection conn)
        {
            return GetScalarContext(conn, "defaultNamingContext");
        }


        public static string GetDnsHostName(LdapConnection conn)
        {
            return GetScalarContext(conn, "dnsHostName	");
        }


        public static void GetRootDSE(LdapConnection conn)
        {
            LdapEntry rootEntry = conn.Read("");

            LdapAttributeSet attributeSet = rootEntry.getAttributeSet();
            System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
            while (ienum.MoveNext())
            {
                LdapAttribute attribute = (LdapAttribute)ienum.Current;
                string attributeName = attribute.Name;

                if (string.Equals(attributeName, "supportedCapabilities", StringComparison.InvariantCultureIgnoreCase))
                    System.Console.WriteLine("Eureka");

                // string attributeVal = attribute.StringValue;

                foreach (string temp in attribute.StringValueArray)
                {
                    string attributeVal = temp;

                    if (!Base64.isLDIFSafe(attributeVal))
                    {
                        byte[] tbyte = SupportClass.ToByteArray(attributeVal);
                        attributeVal = Base64.encode(SupportClass.ToSByteArray(tbyte));
                    } // End if (!Base64.isLDIFSafe(attributeVal)) 

                    Console.WriteLine(attributeName + "value:" + attributeVal);
                } // Next temp 

            }

        }


        [System.Runtime.InteropServices.DllImport("netapi32.dll", EntryPoint = "DsGetSiteNameW", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern int DsGetSiteName(string dcName, ref IntPtr ptr);


        public static void getsite()
        {
            System.IntPtr ptr = System.IntPtr.Zero;

            int siteName = DsGetSiteName((string)null, ref ptr);
            if (siteName != 0)
            {
                // private static int ERROR_NO_SITENAME = 1919;
                // if (siteName == ActiveDirectorySite.ERROR_NO_SITENAME)
                // throw new ActiveDirectoryObjectNotFoundException(Res.GetString("NoCurrentSite"), typeof(ActiveDirectorySite), (string)null);
                //throw ExceptionHelper.GetExceptionFromErrorCode(siteName);
                throw new System.Exception("damn");
            }

            string site = System.Runtime.InteropServices.Marshal.PtrToStringUni(ptr);

            // string dn = "CN=Sites," + (string)PropertyManager.GetPropertyValue(context, directoryEntry2, PropertyManager.ConfigurationNamingContext);
            // directoryEntry1 = DirectoryEntryManager.GetDirectoryEntry(context, dn);

            //  try
            //  {
            //      if (new ADSearcher(directoryEntry1, "(&(objectClass=site)(objectCategory=site)(name=" + Utils.GetEscapedFilterValue(siteName) + "))", new string[1]
            //      {
            //          "distinguishedName"
            //      }, SearchScope.OneLevel, false, false).FindOne() == null)
            //          throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySite), siteName);
            //      return new ActiveDirectorySite(context, siteName, true);
            //  }
            //  catch (COMException ex)
            //  {
            //      if (ex.ErrorCode == -2147016656)
            //          throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySite), siteName);
            //      throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
            //  }
        }


        public static void Test()
        {
            string ldapHost = TestSettings.ldapHost; // args[0];
            int ldapPort = TestSettings.ldapPort; // System.Convert.ToInt32(args[1]);
            string loginDN = TestSettings.loginDN; // args[2];
            string password = TestSettings.password; // args[3];


            string searchBase = "DC=cor,DC=local"; // args[4];
            searchBase = null;
            searchBase = "DC=rootDSE";
            searchBase = "DC=cor,DC=local";
            searchBase = " ";
            // searchBase = "CN=Configuration,DC=cor,DC=local";
            /// searchBase = "CN=,DC=cor,DC=local";
            // searchBase = "rootDSE";

            string searchFilter = "(objectclass=*)";
            searchFilter = "(objectClass=*)";

            try
            {
                LdapConnection conn = new LdapConnection();
                Console.WriteLine("Connecting to:" + ldapHost);
                conn.Connect(ldapHost, ldapPort);
                


                conn.Bind(loginDN, password);

                //dsServiceName
                //CN = NTDS Settings,CN = COR - AD02,CN = Servers,CN = COR - ERLEN,CN = Sites,CN = Configuration,DC = cor,DC = local

                //serverName
                //CN = COR - AD02, CN = Servers, CN = COR - ERLEN, CN = Sites, CN = Configuration, DC = cor, DC = local

                //configurationNamingContext
                //CN = Configuration, DC = cor, DC = local

                //dnsHostName
                //cor - ad02.cor.local

                string defaultNamingContext = GetDefaultNamingContext(conn);
                string configurationNamingContext = GetConfigurationNamingContext(conn);
                string dnsHostName = GetDnsHostName(conn);
                
                System.Console.WriteLine(defaultNamingContext);
                System.Console.WriteLine(configurationNamingContext);
                System.Console.WriteLine(dnsHostName);

                GetRootDSE(conn);
                


                string foo = conn.GetSchemaDN();
                System.Console.WriteLine(foo);


                LdapSearchResults lsc = conn.Search(searchBase,
                                                    LdapConnection.SCOPE_SUB,
                                                    searchFilter,
                                                    null,
                                                    false);

                while (lsc.HasMore())
                {
                    LdapEntry nextEntry = null;
                    try
                    {
                        nextEntry = lsc.Next();
                    }
                    catch (LdapException e)
                    {
                        Console.WriteLine("Error: " + e.LdapErrorMessage);
                        // Exception is thrown, go for next entry
                        continue;
                    }
                    Console.WriteLine("\n" + nextEntry.DN);
                    LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
                    System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                    while (ienum.MoveNext())
                    {
                        LdapAttribute attribute = (LdapAttribute)ienum.Current;
                        string attributeName = attribute.Name;
                        string attributeVal = attribute.StringValue;
                        if (!Base64.isLDIFSafe(attributeVal))
                        {
                            byte[] tbyte = SupportClass.ToByteArray(attributeVal);
                            attributeVal = Base64.encode(SupportClass.ToSByteArray(tbyte));
                        }

                        if (string.Equals(attributeName, "defaultNamingContext", StringComparison.InvariantCultureIgnoreCase))
                            System.Console.WriteLine("yahoo");

                        Console.WriteLine(attributeName + "value:" + attributeVal);
                    }
                }
                conn.Disconnect();
                System.Console.WriteLine("disconnected");
            }
            catch (LdapException e)
            {
                Console.WriteLine("Error:" + e.LdapErrorMessage);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return;
            }
        }
    }
}
