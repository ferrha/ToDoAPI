using System;
using PerfectChannel.WebApi.Controllers;
using PerfectChannel.WebApi.Models;
using NUnit.Framework;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace PerfectChannel.WebApi.Test
{
    public class ToDoApiTest
    {
        private HttpServer Server;
        private string UrlBase = "http://localhost:5000/";

        [SetUp]
        public void Setup()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(name: "Default", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Server = new HttpServer(config);
        }

        // Initial call to GetToDoItems return OK but empty object
        [Test]
        public void GetToDoItemsTest()
        {
            var client = new HttpClient(Server);
            var request = createRequest("api/task/GetToDoItems", "application/json", HttpMethod.Get);

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsNull(response.Content);
            }
        }

        // Add a new to do item
        [Test]
        public void PostToDoItemTest()
        {
            var client = new HttpClient(Server);
            var request = createRequest("api/task/PostToDoItem", "application/json", HttpMethod.Post);
            var content = new ToDoItemModel { ItemName = "Clean my car", ItemDescription = "Clean outside and insede the car" };
            var stringPayload = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            request.Content = httpContent;

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsNotNull(response.Content);
            }
        }

        // Add a new to do item and get items
        [Test]
        public void PostToDoItemAndGetToDoItemsTest()
        {
            // Add the item
            var client = new HttpClient(Server);
            var request = createRequest("api/task/PostToDoItem", "application/json", HttpMethod.Post);
            var content = new ToDoItemModel { ItemName = "Clean my house", ItemDescription = "Clean outside and insede the house" };
            var stringPayload = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            request.Content = httpContent;

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsNotNull(response.Content);
            }

            // Get the items
            request = createRequest("api/task/GetToDoItems", "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsNotNull(response.Content);
            }
        }

        // Add a new to do item and update the status
        [Test]
        public void PostToDoItemAndPutToDoItemTest()
        {
            // Add the item
            var client = new HttpClient(Server);
            var request = createRequest("api/task/PostToDoItem", "application/json", HttpMethod.Post);
            var content = new ToDoItemModel { ItemName = "Clean my house", ItemDescription = "Clean outside and insede the house" };
            var stringPayload = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            request.Content = httpContent;

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsNotNull(response.Content);
            }

            // Get the items
            request = createRequest("api/task/GetToDoItems", "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsNotNull(response.Content);
            }

            // Update the item
            request = createRequest("api/task/PutToDoItem/1", "application/json", HttpMethod.Put);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.IsNotNull(response.Content);
            }
        }


        private HttpRequestMessage createRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(UrlBase + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;

            return request;
        }

        public void Dispose()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
        }
    }
}