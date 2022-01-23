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