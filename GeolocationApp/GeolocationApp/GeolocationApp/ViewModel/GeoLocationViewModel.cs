using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GeolocationApp.Model;
using GeolocationApp.Services;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace GeolocationApp.ViewModel
{
    class GeoLocationViewModel
        : INotifyPropertyChanged
    {
        //List Items
        public ObservableCollection<Country> Items { get; set; }

        //commands
        public Command RefreshCommand { get; set; }

        //constructor
        public GeoLocationViewModel()
        {
            this.Items = new ObservableCollection<Country>();
            RefreshCommand = new Command(
                async () => await GetGenOperatingSystem(),
                () => !IsBusy);

            Call();
        }



        public async void Call()
        {
            await GetGenOperatingSystem();
        }

        private bool _busy;
        public bool IsBusy
        {
            get { return _busy; }
            set
            {
                _busy = value;
                OnPropertyChanged();
                RefreshCommand.ChangeCanExecute();
            }
        }

        public async Task GetGenOperatingSystem()
        {
            if (!IsBusy)
            {
                Exception error = null;
                try
                {
                    IsBusy = true;

                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                    if (position == null)
                        return;

                    var gn = new GeoNames(position.Latitude, position.Longitude);
                    var code = await gn.CountryCode();
                    Items.Clear();
                    var res = await gn.CountryInfo(code);

                    Items.Add(res);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error{ex}");
                    error = ex;
                }
                finally
                {
                    IsBusy = false;
                }
                if (error != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", error.Message, "OK");
                }
            }
            return;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
