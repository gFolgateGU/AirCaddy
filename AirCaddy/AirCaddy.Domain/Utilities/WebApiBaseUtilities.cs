using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace AirCaddy.Domain.Utilities
{
    public abstract class WebApiBaseUtilities
    {
        protected string ApiKey { get; set; }

        protected WebApiBaseUtilities()
        {
            ApiKey = "";
        }

        protected WebApiBaseUtilities(string apiKey)
        {
            ApiKey = apiKey;
        }

        protected virtual WebRequest BuildWebRequest(string httpRequestType, string hostEndpoint)
        {
            if (!ValidateHttpRequestType(httpRequestType))
            {
                return null;
            }
            var bearerString = "Bearer " + ApiKey;
            var uriBuilder = new UriBuilder(hostEndpoint);

            var request = WebRequest.Create(uriBuilder.ToString());

            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Content_Type", "application/x-www-form-urlencoded");
            request.Method = httpRequestType;

            return request;
        }

        protected virtual async Task<JObject> GetWebRequestResponse(WebRequest webRequest)
        {
            try
            {
                var httpResponse = await webRequest.GetResponseAsync() as HttpWebResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    var jsonData = JObject.Parse(streamReader.ReadToEnd());
                    //var reviews = (JArray)x.GetValue("reviews");
                    //var z = GenerateGolfCourseReviewsList(reviews);
                    return jsonData;
                }
            }
            catch (WebException webException)
            {
                return null;
                /*webException.Source
                Console.WriteLine("Right dude here dude");
                Console.WriteLine(webException.Status);*/
            }
        }

        private bool ValidateHttpRequestType(string httpRequestType)
        {
            var lowerCaseRequestType = httpRequestType.ToUpper();
            switch (lowerCaseRequestType)
            {
                case "GET":
                    return true;
                case "POST":
                    return true;
                default:
                    return false;
            }
        }
    }
}
