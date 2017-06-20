using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GeolocationApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
 using   System.Xml;
using System.Xml.Linq;

namespace GeolocationApp.Services
{
    public class GeoNames
    {
        private HttpClient client;
        private double _latitude;
        private double _longitude;

        public GeoNames(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public Task<string> CountryCode()
        {
            return Task.Run(async () =>
            {
                string url =
                    $"http://api.geonames.org/countryCode?lat={_latitude}&lng={_longitude}&username=maxvilla21";
                var response = await client.GetAsync(url);
                string content = string.Empty;
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    content = content.Replace("\r\n", string.Empty);
                }
                return content;
            });
        }

        public Task<Country> CountryInfo(string countryCode)
        {
            return Task.Run(async () =>
            {
                string url =
                    $"http://api.geonames.org/countryInfoJSON?lang=es&country={countryCode}&username=maxvilla21";
                var response = await client.GetAsync(url);
                var country=new Country();
                if (response.IsSuccessStatusCode)
                {
                   var content = await response.Content.ReadAsStringAsync();
                    JObject jo = JObject.Parse(content);
                    var array = (JArray)jo["geonames"];
                    country= JsonConvert.DeserializeObject<Country>(array.First.ToString());
                }
                return country;
            });
        }
    }
}
