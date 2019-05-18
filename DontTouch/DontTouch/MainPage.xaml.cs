using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace DontTouch
{
    public partial class MainPage : ContentPage
    {
        SensorSpeed speed = SensorSpeed.Game; // Set speed delay for monitoring changes.

        string unlockPin = "1234";
        Color bgcolor = Color.White;
        bool stateOfAlarm = false;
        

        public MainPage()
        {
            InitializeComponent();

            EnableAccelerometer();

            warningLab.IsVisible = false;

            button.Clicked += Button_Clicked;
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string fieldPin = entry.Text;
            if(fieldPin == unlockPin)
            {
                DisplayAlert("Correct PIN", "Alarm is deactivated.", "ok");
                stateOfAlarm = false;
                warningLab.IsVisible = false;
                this.bgcolor = Color.Green;
            } else
            {
                DisplayAlert("Incorrect PIN", "Alarm is still active", "ok");
            }            
        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Process shake event            
            if(stateOfAlarm == false)
            {
                stateOfAlarm = true;
                startTimer();
                warningLab.IsVisible = true;
            }            
        }

        private void startTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                // Do something
                if(bgcolor == Color.White)
                {
                    bgcolor = Color.Red;
                } else
                {
                    bgcolor = Color.White;
                }

                this.BackgroundColor = bgcolor;

                try
                {
                    // Or use specified time
                    var duration = TimeSpan.FromSeconds(0.5);
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                }

                // True = Repeat again, False = Stop the timer
                if (stateOfAlarm)
                {
                    return true;
                } else
                {
                    return false;
                }
            });
        }
        private void EnableAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring == false)
                {
                    Accelerometer.Start(speed);
                }                    
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                DisplayAlert("Warning", "Accelerometer is not supported on this device", "ok");
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                DisplayAlert("Error", "Unknown error", "ok");
            }
        }
    }
}
