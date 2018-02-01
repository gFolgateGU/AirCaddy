using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Domain.Utilities;
using Newtonsoft.Json.Linq;

namespace AirCaddy.Domain.Services.GolfCourses
{
    public interface IYelpGolfCourseReviewService
    {
        Task<string> FindGolfCourseGivenSearchName(string searchLocation, string searchName);
    }

    public class YelpGolfCourseReviewService : WebApiBaseUtilities, IYelpGolfCourseReviewService
    {
        private string _yelpApiKey;
        private const string YelpApiSearchEndpointBase = "https://api.yelp.com/v3/businesses/search?";
        private const string GolfSearch = "term=golf";

        public YelpGolfCourseReviewService(string yelpApiKey) : base(yelpApiKey)
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
