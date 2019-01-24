using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChupeLupe.Models;
using FireSharp.Interfaces;
using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using ChupeLupe.Services;

[assembly: Dependency(typeof(WebServicesApi))]
namespace ChupeLupe.Services
{
    public class WebServicesApi : IWebServicesApi
    {
        const string _baseUrl = "https://chupelupe-3836c.firebaseio.com/";
        const string _authSecret = "xT8FsojgSPLiKW3RPYpEiyWxC2HDnV8EOBBHPUyV";

        public IFirebaseClient Client { get; set; }

        public WebServicesApi()
        {
            if(Client == null)
            {
                IFirebaseConfig config = new FirebaseConfig
                {
                    AuthSecret = _authSecret,
                    BasePath = _baseUrl
                };
                Client = new FirebaseClient(config);
            }
        }

        public async Task<List<Promotion>> GetPromotions()
        {
            List<Promotion> promotions = new List<Promotion>();

            try
            {
                //TODO: Check for internet connection...

                FirebaseResponse response = await Client.GetAsync("promotions");

                if (response == null)
                {
                    return promotions;
                }

                if(string.IsNullOrEmpty(response.Body))
                {
                    return promotions;
                }

                var jsonResponse = response.Body;
                var jsonObject = JObject.Parse(jsonResponse);

                foreach (var item in jsonObject)
                {
                    var promotion = await Task.Run(() => JsonConvert.DeserializeObject<Promotion>(item.Value.ToJson()));
                    if(promotion == null)
                    {
                        continue;
                    }
                    promotions.Add(promotion);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return promotions;
        }
    }
}
