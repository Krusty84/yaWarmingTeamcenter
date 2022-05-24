using System;
using System.Threading.Tasks;
using TCUtils;

namespace Main
{

    public class Request
    {
        public string httpMethod { get; set; }
        public string body { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string tcURL { get; set; }
    }

    //OOTB, default reposnse
    public class Response
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }

        public Response(int statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }
    }

    public class Handler
    {
       
        public async Task FunctionHandler(Request request)
        {
            
            var task2LoginToTC = TeamcenterUtils.LoginToTC();
            await task2LoginToTC;
            //
            Console.WriteLine("TC Version: " + task2LoginToTC.Result.serverInfo.Version);
            Console.WriteLine("TC Server ID: "+ task2LoginToTC.Result.serverInfo.TcServerID);
            Console.WriteLine("TC User ID: " + task2LoginToTC.Result.serverInfo.UserID);
            
        }
    }
}