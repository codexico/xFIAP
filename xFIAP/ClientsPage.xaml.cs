using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

            string errorMessage = null;
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
    }
}
