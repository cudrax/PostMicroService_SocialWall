using PostMicroService_SocialWall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using URISUtil;

namespace PostMicroService_SocialWall.ServiceCalls
{
    public static class UserService
    {
        public static User GetUser(int UserId)
        {
            User user;
            string queryPart = "User/" + UserId.ToString();
            Uri serviceUrl = new Uri(UrlUtil.GetServiceUrl("User", "User"), queryPart);

            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, serviceUrl))
                {
                    using (HttpResponseMessage response = client.SendAsync(request, CancellationToken.None).Result)
                    {
                        user = response.Content.ReadAsAsync<User>().Result;
                    }
                }
            }
            return user;
        }

    }
}