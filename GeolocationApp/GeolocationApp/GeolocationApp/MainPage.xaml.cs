using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GeolocationApp.Services;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace GeolocationApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GetPosition();
        }

        private async void GetPosition()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                if (position == null)
                    return;


                var gn=new GeoNames(position.Latitude,position.Longitude);
               var result=  await   gn.CountryCode();

                var res = await gn.CountryInfo("MX");


                // GeoNames(position.Latitude,position.Longitude);

                //Console.WriteLine("Position Status: {0}", position.Timestamp);
                //Console.WriteLine("Position Latitude: {0}", position.Latitude);
                //Console.WriteLine("Position Longitude: {0}", position.Longitude);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
        }

        private async void GeoNames(double latitude,double longitude)
        {
            HttpClient client;
            string url = $"http://api.geonames.org/countryCode?lat={latitude}&lng={longitude}&username=maxvilla21";

            //var response


            //http://api.geonames.org/countryCode?lat=47.03&lng=10.2&username=demo
        }
    }
}
