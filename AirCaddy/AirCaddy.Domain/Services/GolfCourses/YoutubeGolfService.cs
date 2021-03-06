﻿//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using AirCaddy.Domain.ViewModels.GolfCourses;

//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Services;
//using Google.Apis.Upload;
//using Google.Apis.Util.Store;
//using Google.Apis.YouTube.v3;
//using Google.Apis.YouTube.v3.Data;
//using System.Web.Hosting;
//using System.Web.UI.WebControls;

//namespace AirCaddy.Domain.Services.GolfCourses
//{
//    public interface IYoutubeGolfService
//    {
//        Task<bool> UploadCourseFootageAsync(UploadCourseVideoViewModel uploadedCourse);

//        Task<string> DeleteCourseFootageAsync(string youtubeVideoId);

//        string GetUploadedVideoYouTubeIdentifier();
//    }

//    public class YoutubeGolfService : IYoutubeGolfService
//    {
//        private Video _receivedVideoProperties;

//        public YoutubeGolfService()
//        {
//            _receivedVideoProperties = null;
//        }

//        public async Task<bool> UploadCourseFootageAsync(UploadCourseVideoViewModel uploadedCourse)
//        {
//            UserCredential credential = null;
//            var path = HostingEnvironment.MapPath("~");
//            var pathWithSecrets = path + "client_secrets.json";
//            using (var stream = new FileStream(pathWithSecrets, FileMode.Open, FileAccess.Read))
//            {
//                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
//                    GoogleClientSecrets.Load(stream).Secrets,
//                    // This OAuth 2.0 access scope allows an application to upload files to the
//                    // authenticated user's YouTube channel, but doesn't allow other types of access.
//                    new[] { YouTubeService.Scope.YoutubeUpload},
//                    "user",
//                    CancellationToken.None
//                );
//            }

//            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
//            {
//                HttpClientInitializer = credential,
//                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
//            });

//            var video = new Video();
//            video.Snippet = new VideoSnippet();
//            video.Snippet.Title = uploadedCourse.CourseName + " Hole " + uploadedCourse.HoleNumber.ToString();
//            video.Snippet.Description = "Air Caddy footage of " + uploadedCourse.CourseName + " Hole " + uploadedCourse.HoleNumber.ToString();
//            video.Snippet.Tags = new string[] { "golf", "drone footage" };
//            video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
//            video.Status = new VideoStatus();
//            video.Status.PrivacyStatus = "public"; // or "private" or "public"
//            var filePath = uploadedCourse.HoleVideoPath; // Replace with path to actual movie file.

//            using (var fileStream = new FileStream(filePath, FileMode.Open))
//            {
//                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
//                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
//                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

//                videosInsertRequest.Upload();

//                return _receivedVideoProperties != null;
//            }
//        }

//        public async Task<string> DeleteCourseFootageAsync(string youtubeVideoId)
//        {
//            UserCredential credential = null;
//            var path = HostingEnvironment.MapPath("~");
//            //var pathWithSecrets = path + "client_secrets.json";
//            var pathWithSecrets = @"C:\\Sandbox\AirCaddy\AirCaddy\AirCaddy.Web\client_secrets.json";
//            using (var stream = new FileStream(pathWithSecrets, FileMode.Open, FileAccess.Read))
//            {
//                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
//                    GoogleClientSecrets.Load(stream).Secrets,
//                    // This OAuth 2.0 access scope allows an application to upload files to the
//                    // authenticated user's YouTube channel, but doesn't allow other types of access.
//                    new[] {YouTubeService.Scope.YoutubeForceSsl, YouTubeService.Scope.Youtube, YouTubeService.Scope.Youtubepartner},
//                    "user",
//                    CancellationToken.None
//                );
//            }

//            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
//            {
//                HttpClientInitializer = credential,
//                ApiKey = "AIzaSyCXhN0iyf7y3ESU3y3lYaTLT2gM8L4yroc",
//                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
//            });

//            var youtubeVideoDeleteRequest = youtubeService.Videos.Delete(youtubeVideoId);
//            var response = await youtubeVideoDeleteRequest.ExecuteAsync();
//            return response;
//        }

//        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
//        {
//            switch (progress.Status)
//            {
//                case UploadStatus.Uploading:
//                    var x = 2;
//                    //Console.WriteLine("{0} bytes sent.", progress.BytesSent);
//                    break;

//                case UploadStatus.Failed:
//                    var y = 3;
//                    //Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
//                    break;
//            }
//        }

//        void videosInsertRequest_ResponseReceived(Video video)
//        {
//            _receivedVideoProperties = video;
//            //Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
//        }

//        public string GetUploadedVideoYouTubeIdentifier()
//        {
//            return _receivedVideoProperties?.Id;
//        }
//    }
//}
