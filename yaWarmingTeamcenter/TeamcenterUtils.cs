
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestRestClientTC;

namespace TCUtils
{
 
    //Response from ...Core-2011-06-Session/login
    public class ServerInfo
    {
        public string DisplayVersion { get; set; }
        public string HostName { get; set; }
        public string Locale { get; set; }
        public string LogFile { get; set; }
        public string SiteLocale { get; set; }
        public string TcServerID { get; set; }
        public string UserID { get; set; }
        public string Version { get; set; }
    }

    public class Query
    {
        public string query { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class PartialErrors
    {
    }

    public class TeamcenterResponce
    {
        [JsonProperty(".QName")]
        public string QName { get; set; }
        public ServerInfo serverInfo { get; set; }
        public List<Query> queries { get; set; }
        public PartialErrors PartialErrors { get; set; }
        public int StatusCode { get; set; }
        public string AuthCookie { get; set; }
    }

    public static class TeamcenterUtils
    {

        public static async Task<TeamcenterResponce> LoginToTC()
        {
            
            string payloadSrc = "{\"header\":{\"state\":{},\"policy\":{}},\"body\":{\"credentials\":{\"user\":\"[userName]\",\"password\":\"[userPassword]\",\"role\":\"\",\"descrimator\":\"\",\"locale\":\"\",\"group\":\"\"}}}";

            var payload = payloadSrc.Replace("[userName]", Environment.GetEnvironmentVariable("userName")).Replace("[userPassword]", Environment.GetEnvironmentVariable("userPassword"));

            var cookieWithAuthKey = new CookieContainer();
            var httpHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpHandler);
            httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("tcURL") + "/tc/JsonRestServices/Core-2011-06-Session/login");
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress);
            httpRequest.Content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
            var httpResponse = await PostRequestAsync(httpRequest, httpClient);
            cookieWithAuthKey.Add(httpHandler.CookieContainer.GetCookies(new Uri(Environment.GetEnvironmentVariable("tcURL") + "/tc/JsonRestServices/Core-2011-06-Session/login")));
            TeamcenterResponce myDeserializedTCResponce = JsonConvert.DeserializeObject<TeamcenterResponce>(httpResponse.ToString());
            myDeserializedTCResponce.StatusCode = 200;
            await Helper.ysStorageSerializingClass(cookieWithAuthKey, Environment.GetEnvironmentVariable("yaAccessKeyId"), Environment.GetEnvironmentVariable("yaSecretAccessKey"), Environment.GetEnvironmentVariable("yaBucketName"), Environment.GetEnvironmentVariable("cookieFileName"));
            return myDeserializedTCResponce;
        }

        public static async Task<string> PostRequestAsync(HttpRequestMessage postRequest, HttpClient client)
        {

            var response = await client.SendAsync(postRequest);
            var responseString = string.Empty;

            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return responseString;

        }
    }
}