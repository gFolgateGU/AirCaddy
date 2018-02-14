using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Domain.Utilities;
using AirCaddy.Domain.ViewModels.GolfCourses;
using Newtonsoft.Json.Linq;

namespace AirCaddy.Domain.Services.GolfCourses
{
    public interface IYelpGolfCourseReviewService
    {
        Task<string> FindGolfCourseGivenSearchName(string searchLocation, string searchName);

        Task<List<YelpGolfCourseReview>> GetGolfCourseReviewData(string yelpCourseId);
    }

    public class YelpGolfCourseReviewservice : WebApiBaseUtilities, IYelpGolfCourseReviewService
    {
        private string _yelpApiKey;
        private const string YelpApiSearchEndpointBase = "https://api.yelp.com/v3/businesses/search?";
        private const string YelpApiReviewEndpointBase = "https://api.yelp.com/v3/businesses/";
        private const string YelpReviewQueryString = "/reviews";
        private const string GolfSearch = "term=golf";

        public YelpGolfCourseReviewservice(string yelpApiKey) : base(yelpApiKey)
        {
            _yelpApiKey = yelpApiKey;
        }

        public async Task<string> FindGolfCourseGivenSearchName(string searchLocation, string searchName)
        {
            var yelpCourseSearchQuery = "location=" + searchLocation + "&" + GolfSearch;
            var yelpSearchEndpoint = YelpApiSearchEndpointBase + yelpCourseSearchQuery;

            var request = BuildWebRequest("GET", yelpSearchEndpoint);
            
            //do some logic if the request is null...

            var responseData = await GetWebRequestResponse(request);
            var businessesFound = (JArray) responseData.GetValue("businesses");
            var yelpApiId = GetGolfCourseYelpApiIdFromResponse(businessesFound, searchName);
            return yelpApiId;
        }

        public async Task<List<YelpGolfCourseReview>> GetGolfCourseReviewData(string yelpCourseId)
        {
            var yelpReviewDataEndpoint = YelpApiReviewEndpointBase + yelpCourseId + YelpReviewQueryString;
            var request = BuildWebRequest("GET", yelpReviewDataEndpoint);

            var responseData = await GetWebRequestResponse(request);
            var reviewData = MapReviewDataResponseToViewModel((JArray) responseData.GetValue("reviews"));
            return reviewData;
        }

        private List<YelpGolfCourseReview> MapReviewDataResponseToViewModel(JArray reviewData)
        {
            var reviewDataList = (from review in reviewData
                let jsonDate = review["time_created"].ToString()
                select new YelpGolfCourseReview
                {
                    Rating = review["rating"].ToString(),
                    User = review["user"]["name"].ToString(),
                    ReviewDate = Convert.ToDateTime(jsonDate),
                    ReviewText = review["text"].ToString()
                }).ToList();
            return reviewDataList;
        }

        private string GetGolfCourseYelpApiIdFromResponse(JArray businesses, string searchName)
        {
            var consistentSearchName = searchName.ToLower();
            var yelpApiId = 
                (from business in businesses
                let consistentBusinessName = business["name"].ToString().ToLower()
                where consistentBusinessName.Contains(consistentSearchName)
                select business["id"].ToString()).FirstOrDefault();
            return yelpApiId;
        }
    }
} 
