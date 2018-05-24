
namespace CrapDAP
{


    // To restore all nugets from dependencies, edit project: 
    // <PropertyGroup>
    //    <RestoreProjectStyle>PackageReference</RestoreProjectStyle>
    // </PropertyGroup>
    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
#if false
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif
            // TestHelper.Test();
            // TestLdap.GetGroups();
            // TestLdap.GetUsers();
            TestLdap.GetExchange();

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main


    } // End Class Program 


} // End Namespace CrapDAP 
