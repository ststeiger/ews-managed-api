
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


using Microsoft.Exchange.WebServices.Data;


namespace CrapDAP
{


    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if false
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif

            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            service.Credentials = new WebCredentials(RedmineMailService.UserData.Email, RedmineMailService.UserData.Password);
            
            Microsoft.Exchange.WebServices.Data.ITraceListener listener = null; // new NoTrace();

            if (listener != null)
            {
                service.TraceListener = listener;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
            } // End if (listener != null) 

            service.Url = new System.Uri("https://webmail.cor-management.ch/ews/exchange.asmx");
            // service.AutodiscoverUrl(RedmineMailService.Trash.UserData.Email, RedirectionUrlValidationCallback);



            var helper = new Microsoft.Exchange.WebServices.Autodiscover.DirectoryHelper(service);
            //helper.GetAutodiscoverScpUrlsForDomain("cor.local");
            helper.GetAutodiscoverScpUrlsForDomain("cor-management.ch");


        }
    }
}
