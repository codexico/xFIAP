using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace xFIAP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            this.InitializeComponent();
        }
        
        private async void btnVisita_Click(object sender, RoutedEventArgs e)
        {
            var appointment = new Windows.ApplicationModel.Appointments.Appointment();

            // StartTime
            var date = StartTimeDatePicker.Date;
            var time = StartTimeTimePicker.Time;
            var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            var startTime = new DateTimeOffset(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0, timeZoneOffset);
            appointment.StartTime = startTime;
    
            appointment.Subject = txtName.Text;
            appointment.Location = txtLocal.Text;


            // Duration
            if (DurationComboBox.SelectedIndex == 0)
            {
                // 30 minute duration is selected
                appointment.Duration = TimeSpan.FromMinutes(30);
            }
            else if (DurationComboBox.SelectedIndex == 1)
            {
                // 1 hour duration is selected
                appointment.Duration = TimeSpan.FromHours(1);
            }
            else if (DurationComboBox.SelectedIndex == 2)
            {
                // 1 hour duration is selected
                appointment.Duration = TimeSpan.FromHours(2);
            }
            else
            {
                // 1 hour duration is selected
                appointment.Duration = TimeSpan.FromHours(4);
            }
            
            string appointmentId = 
                await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(
                    appointment, 
                    Windows.Foundation.Rect.Empty);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        // https://msdn.microsoft.com/windows/uwp/launch-resume/launch-maps-app#display-directions-and-traffic
        private async void btnRoute_Click(object sender, RoutedEventArgs e)
        {
            // Request permission to access location
            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    //_rootPage.NotifyUser("Waiting for update...", NotifyType.StatusMessage);

                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    Geolocator geolocator = new Geolocator {};

                    // Carry out the operation
                    Geoposition pos = await geolocator.GetGeopositionAsync().AsTask();

                    traceRoute(pos, txtAddress.Text);
                    break;

                case GeolocationAccessStatus.Denied:
                    //_rootPage.NotifyUser("Access to location is denied.", NotifyType.ErrorMessage);
                    var messageDialog = new MessageDialog(
                            "Por favor libere o acesso ao GPS", "GPS bloqueado");
                    await messageDialog.ShowAsync();
                    break;

                case GeolocationAccessStatus.Unspecified:
                    //_rootPage.NotifyUser("Unspecified error.", NotifyType.ErrorMessage);
                    var errorDialog = new MessageDialog(
                            "Falha no acesso ao GPS", "Erro");
                    await errorDialog.ShowAsync();
                    break;
            }
        }

        // https://msdn.microsoft.com/en-us/windows/uwp/maps-and-location/routes-and-directions
        private async void traceRoute(Geoposition currentLocation, string address)
        {
            string posCurrentLocation = "pos." + currentLocation.Coordinate.Latitude + "_" + currentLocation.Coordinate.Longitude;
            string clientAddress = "adr." + address;
            var routeUri = new Uri(@"bingmaps:?rtp=" + posCurrentLocation + "~" + clientAddress);

            // Launch the Windows Maps app
            var launcherOptions = new Windows.System.LauncherOptions();
            launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsMaps_8wekyb3d8bbwe";
            var success = await Windows.System.Launcher.LaunchUriAsync(routeUri, launcherOptions);
        }
    }
}
