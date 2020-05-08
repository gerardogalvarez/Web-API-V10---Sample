using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ERPV10.WebAPISample;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERPV10.PurchaseOrderSample
{
    public static class PurchasesOrdersController
    {
        public static string Token { get; set; }

        public static async Task CreatePurchaseOrder()
        {
            PurchaseOrder PurchaseOrder = new PurchaseOrder();

            try
            {
                PurchaseOrder.DocumentType = "ECF";
                PurchaseOrder.DocumentSeries = "2020";
                PurchaseOrder.DocumentDate = DateTime.UtcNow;
                PurchaseOrder.Supplier = "F0001";
                PurchaseOrder.EntityType = "F";

                PurchaseOrder.Lines = new List<PurchaseOrderLine>
                {
                    new PurchaseOrderLine
                    {
                        Item= "A0001",
                        Quantity = 1,
                    }
                };

                using (var client = new HttpClient())
                {
                    string request = "Compras/Docs/CreateDocument";
                    string resourceLocation = string.Format("{0}/{1}/", Constants.baseAppUrl, request);

                    // We want the response to be JSON.
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    HttpRequestMessage postOrderMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    postOrderMessage.Content = new StringContent(JsonConvert.SerializeObject(PurchaseOrder), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderMessage))
                    {

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        if (responseContent.IsSuccessStatusCode)
                        {

                            var foreGroundColor = Console.ForegroundColor;

                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.WriteLine(string.Concat("Purchase Order Created: ", result));

                            Console.ForegroundColor =  foreGroundColor;
                        }
                        else
                        {

                            // Response parsing to show only error message:
                            dynamic resposta = JObject.Parse(result);

                            Console.WriteLine(string.Concat("Erro: ", resposta.ExceptionMessage));

                            throw new Exception("Unable to create Purchase order.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro on create Purchase order.");
            }
        }
    }
}