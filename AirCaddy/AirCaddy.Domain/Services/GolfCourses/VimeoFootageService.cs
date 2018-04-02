using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Domain.ViewModels.GolfCourses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirCaddy.Domain.Services.GolfCourses
{
    public interface IVimeoFootageService
    {
        Task<string> UploadCourseFootageAsync(UploadCourseVideoViewModel courseVideoContents);

        Task<bool> DeleteCourseFootageAsync(string vimeoVideoId);
    }

    public class VimeoFootageService : IVimeoFootageService
    {
        private readonly string _vimeoUploadAccessToken;
        private const string VimeoUploadEndpoint = "https://api.vimeo.com/me/video";
        private const string VimeoDeleteEndpoint = "https://api.vimeo.com/videos/";


        public VimeoFootageService(string vimeoUploadAccessToken)
        {
            _vimeoUploadAccessToken = "bearer " + vimeoUploadAccessToken;
        }

        public async Task<string> UploadCourseFootageAsync(UploadCourseVideoViewModel courseVideoContents)
        {
            try
            {
                var initialPostClient = new WebClient();
                initialPostClient.Headers.Clear();
                initialPostClient.Headers.Add("Authorization", _vimeoUploadAccessToken);

                var vimeoTicket =
                    JsonConvert.DeserializeObject<JObject>(initialPostClient.UploadString(
                        VimeoUploadEndpoint, "POST",
                        ""));

                var uploadLink = vimeoTicket["upload"]["upload_link"].ToString();

                var file = File.ReadAllBytes(courseVideoContents.HoleVideoPath);

                initialPostClient.Headers.Clear();
                var result = await initialPostClient.UploadDataTaskAsync(uploadLink, "PUT", file);

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
                var vimeoEndpoint = VimeoDeleteEndpoint + vimeoVideoId;
                var deleteRequestMessage = new HttpRequestMessage(HttpMethod.Delete, vimeoEndpoint);
                deleteRequestMessage.Headers.Add("Authorization", _vimeoUploadAccessToken);

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
    }
}

