using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirCaddy.Domain.Services.GolfCourses
{
    public interface IVimeoFootageService
    {
        string UploadCourseFootage(string physicalFilePath);

        Task<bool> DeleteCourseFootageAsync(string vimeoVideoId);
    }

    public class VimeoFootageService : IVimeoFootageService
    {
        private readonly string _vimeoUploadAccessToken;

        public VimeoFootageService(string vimeoUploadAccessToken)
        {
            _vimeoUploadAccessToken = vimeoUploadAccessToken;
        }

        public string UploadCourseFootage(string physicalFilePath)
        {
            try
            {
                var initialPostClient = new WebClient();
                initialPostClient.Headers.Clear();
                initialPostClient.Headers.Add("Authorization", "bearer fede41d7e6e65029882a5f662b20ce38");

                var vimeoTicket =
                    JsonConvert.DeserializeObject<JObject>(initialPostClient.UploadString(
                        "https://api.vimeo.com/me/videos", "POST",
                        ""));
                var uploadLink = vimeoTicket["upload"]["upload_link"].ToString();

                var file = File.ReadAllBytes(physicalFilePath);

                initialPostClient.Headers.Clear();
                var result = initialPostClient.UploadData(uploadLink, "PUT", file);

                var uploaderClient = new WebClient();
                uploaderClient.Headers.Clear();
                uploaderClient.Headers.Add("Content-Range", "bytes */*");
                try
                {
                    uploaderClient.UploadData(uploadLink, "PUT", new byte[0]);
                }
                catch (Exception uploadException)
                {
                    uploadException.GetBaseException();
                    //It is returning a 308 which seems to be okay because it is getting to vimeo
                    //now grab the id.. a bit hacky..
                    var uriWithVimeoId = vimeoTicket["uri"].ToString();
                    var vimeoId = uriWithVimeoId.Replace("/videos/", "");
                    return vimeoId;
                }
            }
            catch (Exception generalException)
            {
                generalException.GetBaseException();
                return string.Empty;
            }
            //This must mean we did not get the 308.
            return string.Empty;
        }

        public async Task<bool> DeleteCourseFootageAsync(string vimeoVideoId)
        {
            try
            {
                var vimeoEndpoint = "https://api.vimeo.com/videos/" + vimeoVideoId;
                var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete, vimeoEndpoint);
                deleteRequestMessage.Headers.Add("Authorization", "bearer fede41d7e6e65029882a5f662b20ce38");

                var httpClient = new HttpClient(new HttpClientHandler {AllowAutoRedirect = false});
                var response = await httpClient.SendAsync(deleteRequestMessage);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return true;
                }
            }
            catch (Exception webRequestException)
            {
                webRequestException.GetBaseException();
                return false;
            }
            return false;
        }

        //public void Test()
        //{
        //    try
        //    {
        //        WebClient wc = new WebClient();
        //        wc.Headers.Clear();
        //        wc.Headers.Add("Authorization", "bearer fede41d7e6e65029882a5f662b20ce38");
        //        wc.Headers.Add("type", "streaming");

        //        var vimeoTicket = JsonConvert.DeserializeObject<JObject>(wc.UploadString("https://api.vimeo.com/me/videos", "POST", ""));

        //        var yz = vimeoTicket["uri"].ToString();
        //        var gz = vimeoTicket["upload"]["upload_link"].ToString();
        //        var file = File.ReadAllBytes(@"C:\Three.mp4");
        //        File.ReadAllBytes()

        //        wc.Headers.Clear();

        //        var result = wc.UploadData(gz, "PUT", file);

        //        WebClient wc1 = new WebClient();
        //        wc1.Headers.Clear();
        //        wc1.Headers.Add("Content-Range", "bytes */*");

        //        //This line will get me an execption {"The remote server returned an error: (308) Resume Incomplete."}
        //        try
        //        {
        //            var ff1 = wc1.UploadData(gz, "PUT", new byte[0]);
        //        }
        //        catch (Exception ec)
        //        {
        //            ec.GetBaseException();
        //        }

        //        var x = 22;
        //        var y = 23;
        //    }
        //    catch (Exception h)
        //    {

        //        throw;
        //    }
        //}
    }
}

