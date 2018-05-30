﻿using Microsoft.Exchange.WebServices.Data;
using System;
using System.Net;

namespace Exchange101
{


    public class UrlValidator
    {


        public bool ValidateUrl(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            System.Uri redirectionUri = new System.Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            } // End if (redirectionUri.Scheme == "https") 

            return result;
        } // End Function RedirectionUrlValidationCallback 


    }

    // This sample is for demonstration purposes only.
    // Before you run this sample, make sure that the code meets the
    // coding requirements of your organization.
    static class Ex15_AddDelegates_CS1
    {
        static void Main(string[] args)
        {
            Console.Title = "EWS Test Console";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WindowHeight = Console.LargestWindowHeight * 9 / 10;
            Console.WindowWidth = Console.LargestWindowWidth / 2;
            Console.SetWindowPosition(0, 0);
            Console.SetBufferSize(200, 3000);

            // Create an instance of the custom URL redirection validator.
            UrlValidator validator = new UrlValidator();

            // Get the user's email address and password from the console.
            IUserData userData = UserDataFromConsole.GetUserData();

            // Create an ExchangeService object with the user's credentials.
            ExchangeService myService = new ExchangeService();
            myService.Credentials = new NetworkCredential(userData.EmailAddress, userData.Password);


            Console.WriteLine("Getting EWS URL using custom validator...");

		    // Call the Autodisocer service with the custom URL validator.
            myService.AutodiscoverUrl(userData.EmailAddress, validator.ValidateUrl);

            Console.WriteLine(string.Format("  EWS URL is {0}", myService.Url));
            Console.WriteLine("Complete");

            Console.WriteLine("\r\n");
            Console.WriteLine("Press or select Enter...");
            Console.ReadLine();
        }
    }
}
