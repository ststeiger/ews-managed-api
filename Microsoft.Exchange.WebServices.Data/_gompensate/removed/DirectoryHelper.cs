
namespace Microsoft.Exchange.WebServices.Autodiscover
{
    using System.Collections.Generic;
    using Microsoft.Exchange.WebServices.Data; // for ExchangeServiceBase
    //using System.DirectoryServices;
    //using System.DirectoryServices.ActiveDirectory;


    /// <summary>
    /// Represents a set of helper methods for using Active Directory services.
    /// </summary>
    internal class DirectoryHelper111
    {

        /// <summary>
        /// Maximum number of SCP hops in an SCP host lookup call.
        /// </summary>
        private const int AutodiscoverMaxScpHops = 10;

        /// <summary>
        /// GUID for SCP URL keyword
        /// </summary>
        private const string ScpUrlGuidString = @"77378F46-2C66-4aa9-A6A6-3E7A48B19596";

        /// <summary>
        /// GUID for SCP pointer keyword
        /// </summary>
        private const string ScpPtrGuidString = @"67661d7F-8FC4-4fa7-BFAC-E1D7794C1F68";

        /// <summary>
        /// Filter string to find SCP Ptrs and Urls.
        /// </summary>
        private const string ScpFilterString = "(&(objectClass=serviceConnectionPoint)(|(keywords=" + ScpPtrGuidString + ")(keywords=" + ScpUrlGuidString + ")))";
        
        
        private ExchangeServiceBase service;
        
        
        internal ExchangeServiceBase Service
        {
            get { return this.service; }
        }
        
        
        public DirectoryHelper111(ExchangeServiceBase service)
        {
            this.service = service;
        }
        
        
        /// <summary>
        /// Traces message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void TraceMessage(string message)
        {
            this.Service.TraceMessage(TraceFlags.AutodiscoverConfiguration, message);
        }
        
        
        private string GetSiteName()
        {
            // https://msdn.microsoft.com/en-us/library/system.directoryservices.activedirectory.activedirectorysite.getcomputersite(v=vs.110).aspx
            // Gets the site that this computer is a member of.
            
            // https://serverfault.com/questions/486518/how-does-a-client-system-in-an-active-directory-network-find-in-which-site-it-re
            // http://www.itprotoday.com/windows-8/q-how-can-client-computer-determine-which-site-it-belongs
            // https://blogs.technet.microsoft.com/heyscriptingguy/2005/11/07/how-can-i-list-all-the-sites-in-active-directory-as-well-as-all-the-servers-in-those-sites/
            
            // When the client finds a DC, the client issues a UDP LDAP request
            // asking for Netlogon-service information from the DC;
            // the DC returns a SearchResponse (4) message, which lists the DC's local site
            // and the client's site name, according to the client's IP address,
            // if the queried DC isn't from the client's current local site.
            // If the DNS query can't match a client's IP address to a defined site,
            // it doesn't return a recommended site, only the DC's current site. 
            
            // HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters\DynamicSiteName 
            // HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters\SiteNameTimeout 
            
            // ActiveDirectorySite.GetComputerSite()
            throw new System.NotImplementedException();
            
            // Not found ? return NULL
            return null;
        }


        /// <summary>
        /// Search Active Directory for any related SCP URLs for a given domain name.
        /// </summary>
        /// <param name="domainName">Domain name to search for SCP information</param>
        /// <param name="ldapPath">LDAP path to start the search</param>
        /// <param name="maxHops">The number of remaining allowed hops</param>
        private List<string> GetScpUrlList(
            string domainName,
            string ldapPath,
            ref int maxHops)
        {
            if (maxHops <= 0)
            {
                throw new ServiceLocalException(Strings.MaxScpHopsExceeded);
            }

            maxHops--;

            this.TraceMessage(
                string.Format("Starting SCP lookup for domainName='{0}', root path='{1}'", domainName, ldapPath));

            string scpUrl = null;
            string fallBackLdapPath = null;
            string rootDsePath = null;
            string configPath = null;

            // The list of SCP URLs.
            List<string> scpUrlList = new List<string>();

            // Get the LDAP root path.
            rootDsePath = (ldapPath == null) ? "LDAP://RootDSE" : ldapPath + "/RootDSE";
            
            throw new System.NotImplementedException();

            return null;
        }


        // This is the method called from outside
        public List<string> GetAutodiscoverScpUrlsForDomain(string domainName)
        {
#if (NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETSTANDARD1_7 || NETSTANDARD1_8 || NETSTANDARD1_9 || NETSTANDARD2_0 || NETSTANDARD2_1 || NETSTANDARD2_2)
            //todo: implement ldap autodiscover
            return new System.Collections.Generic.List<string>();
#else
            DirectoryHelper helper = new DirectoryHelper(this);
            return helper.GetAutodiscoverScpUrlsForDomain(domainName);
#endif
            
            int maxHops = AutodiscoverMaxScpHops;
            List<string> scpUrlList;

            try
            {
                scpUrlList = this.GetScpUrlList(domainName, null, ref maxHops);
            }
            catch (System.InvalidOperationException e)
            {
                this.TraceMessage(
                    string.Format("LDAP call failed, exception: {0}", e.ToString()));
                scpUrlList = new List<string>();
            }
            catch (System.NotSupportedException e)
            {
                this.TraceMessage(
                    string.Format("LDAP call failed, exception: {0}", e.ToString()));
                scpUrlList = new List<string>();
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                this.TraceMessage(
                    string.Format("LDAP call failed, exception: {0}", e.ToString()));
                scpUrlList = new List<string>();
            }
            
            return scpUrlList; 
        }
        
        
    }
    
    
}
