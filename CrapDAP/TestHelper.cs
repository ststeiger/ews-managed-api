
using Microsoft.Exchange.WebServices.Data;


namespace CrapDAP
{


    public class TestHelper
    {


        public static void Test()
        {
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
