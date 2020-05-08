using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ERPV10.WebAPISample;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERPV10.SalesOrderSample
{
    public static class SalesOrdersController
    {
        public static string Token { get; set; }

        public static async Task FillRelatedData()
        {
            SalesOrder salesOrder = new SalesOrder();

            try
            {
                salesOrder.DocumentType = "ECL";
                salesOrder.DocumentSeries = "2020";
                salesOrder.Customer = "sofrio";
                salesOrder.CustomerType = "C";

                using (var client = new HttpClient())
                {
                    string request = "Vendas/Docs/PreencheDadosRelacionados";
                    string resourceLocation = string.Format("{0}/{1}/", Constants.baseAppUrl, request);

                    // We want the response to be JSON.
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    HttpRequestMessage postOrderMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    postOrderMessage.Content = new StringContent(JsonConvert.SerializeObject(salesOrder), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderMessage))
                    {

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        if (responseContent.IsSuccessStatusCode)
                        {

                            var foreGroundColor = Console.ForegroundColor;

                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.WriteLine(string.Concat("Fill Related Data: ", result));

                            Console.ForegroundColor = foreGroundColor;
                        }
                        else
                        {

                            // Response parsing to show only error message:
                            dynamic resposta = JObject.Parse(result);

                            Console.WriteLine(string.Concat("Erro: ", resposta.ExceptionMessage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro on create sales order.");
            }
        }

        public static async Task CreateSalesOrder()
        {
            SalesOrder salesOrder = new SalesOrder();

            try
            {
                salesOrder.DocumentType = "ECL";
                salesOrder.DocumentSeries = "2020";
                salesOrder.DocumentDate = DateTime.UtcNow;
                salesOrder.Customer = "sofrio";
                salesOrder.CustomerType = "C";

                salesOrder.Lines = new List<SalesOrderLine>
                {
                    new SalesOrderLine
                    {
                        Item= "A0001",
                        Quantity = 1,
                    }
                };

                using (var client = new HttpClient())
                {
                    string request = "Vendas/Docs/CreateDocument";
                    string resourceLocation = string.Format("{0}/{1}/", Constants.baseAppUrl, request);

                    // We want the response to be JSON.
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    HttpRequestMessage postOrderMessage = new HttpRequestMessage(HttpMethod.Post, resourceLocation);

                    postOrderMessage.Content = new StringContent(JsonConvert.SerializeObject(salesOrder), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage responseContent = await client.SendAsync(postOrderMessage))
                    {

                        string result = await ((StreamContent)responseContent.Content).ReadAsStringAsync();

                        if (responseContent.IsSuccessStatusCode)
                        {

                            var foreGroundColor = Console.ForegroundColor;

                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.WriteLine(string.Concat("Sales Order Created: ", result));

                            Console.ForegroundColor =  foreGroundColor;
                        }
                        else
                        {

                            // Response parsing to show only error message:
                            dynamic resposta = JObject.Parse(result);

                            Console.WriteLine(string.Concat("Erro: ", resposta.ExceptionMessage));

                            throw new Exception("Unable to create sales order.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro on create sales order.");
            }
        }
    }
}