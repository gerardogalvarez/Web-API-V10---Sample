using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ERPV10.PurchaseOrderSample;
using ERPV10.WebAPISample;
using Newtonsoft.Json;

namespace ERPV10.SalesOrderSample
{
    class Program
    {

        private static string option;

        public static string userName { get; private set; }
        public static string password { get; private set; }
        public static string ERPCompany { get; private set; }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting ...");

                DoIt().Wait();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }


        private static async Task<int> DoIt()
        {

            Console.Write("User: ");
#if DEBUG
            userName = "gerardo.gomez";
            Console.WriteLine(userName);
#else
            userName = Console.ReadLine();
#endif 

            Console.Write("Password: ");
#if DEBUG
            password = "";
            Console.WriteLine(password);
#else
            password = Console.ReadLine();
#endif 

            Console.Write("ERPCompany: ");
#if DEBUG
            ERPCompany = "demov10";
            Console.WriteLine(ERPCompany);
#else
            ERPCompany = Console.ReadLine();
#endif 

            string accessToken = string.Empty;

            do
            {

                option = GetMenuOption().ToUpper();

                switch (option)
                {
                    case "0":

                        // Get the Access Token.
                        accessToken = await GetAccessToken();
                        Console.WriteLine(accessToken != null ? string.Format("Got Token ({0})", accessToken) : "No Token found");
                        break;

                    case "1":

                        // Get the Access Token.
                        accessToken = await GetAccessToken();
                        Console.WriteLine(accessToken != null ? "Got Token" : "No Token found");

                        if (accessToken != null)
                        {
                            SalesOrdersController.Token = accessToken;

                            await SalesOrdersController.FillRelatedData();
                        }
                        break;

                    case "2":

                        // Get the Access Token.
                        accessToken = await GetAccessToken();
                        Console.WriteLine(accessToken != null ? "Got Token" : "No Token found");

                        if (accessToken != null)
                        {
                            SalesOrdersController.Token = accessToken;

                            await SalesOrdersController.CreateSalesOrder();
                        }
                        break;

                    case "3":

                        // Get the Access Token.
                        accessToken = await GetAccessToken();
                        Console.WriteLine(accessToken != null ? "Got Token" : "No Token found");

                        if (accessToken != null)
                        {
                            CustomersController.Token = accessToken;

                            await CustomersController.GetCustomers();
                        }
                        break;

                    case "4":

                        // Get the Access Token.
                        accessToken = await GetAccessToken();
                        Console.WriteLine(accessToken != null ? "Got Token" : "No Token found");

                        if (accessToken != null)
                        {
                            PurchasesOrdersController.Token = accessToken;

                            await PurchasesOrdersController.CreatePurchaseOrder();
                        }
                        break;

                    case "X":
                        break;

                    default:
                        Console.WriteLine(string.Format("[{0}] is not a valid option.", option));
                        break;
                }
            }
            while (option != "X");

            return 0;
        }

        private static string GetMenuOption()
        {

            String menuOption = string.Empty;
            WriteMenu();
            menuOption = Console.ReadLine();

            return menuOption;

        }

        private static void WriteMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("----");
            Console.WriteLine("Menu");
            Console.WriteLine("----");
            Console.WriteLine("[0] Get Token");
            Console.WriteLine("[1] Sales doc. - Fill Related Data");
            Console.WriteLine("[2] Sales doc. - Create");
            Console.WriteLine("[3] Get customers");
            Console.WriteLine("[4] Purchase doc. - Create");
            Console.WriteLine("[X] Exit");
        }

        private static async Task<string> GetAccessToken()
        {

            using (var client = new HttpClient())
            {
                try
                {
                    
                    client.BaseAddress = new Uri(Constants.baseUrl);

                    // We want the response to be JSON.
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Build up the data to POST.
                    List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();

                    postData.Add(new KeyValuePair<string, string>("grant_type", Constants.grant_type));
                    postData.Add(new KeyValuePair<string, string>("username", userName));
                    postData.Add(new KeyValuePair<string, string>("password", password));
                    postData.Add(new KeyValuePair<string, string>("instance", Constants.instance));
                    postData.Add(new KeyValuePair<string, string>("company", ERPCompany));
                    postData.Add(new KeyValuePair<string, string>("line", "Executive"));

                    FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                    // Post to the server and parse the response.
                    HttpResponseMessage response = await client.PostAsync(Constants.baseUrl, content);
                    string jsonString = await response.Content.ReadAsStringAsync();
                    object responseData = JsonConvert.DeserializeObject(jsonString);

                    // return the Access Token.
                    return ((dynamic)responseData).access_token;

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error get token. {0}", ex.Message));
                }
            }
        }
    }
}
