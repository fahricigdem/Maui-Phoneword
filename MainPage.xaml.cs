using System.ComponentModel;
using System.Diagnostics;

namespace Phoneword
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        string translatedNumber = "";

        private void OnTranslate(object sender, EventArgs e)
        {
            translatedNumber = PhonewordTranslator.ToNumber(PhoneNumberText.Text);
            if (!string.IsNullOrEmpty(translatedNumber) ) 
            {
                CallButton.IsEnabled = true;
                CallButton.Text = "Call " + translatedNumber;
            }
            else
            {
                CallButton.IsEnabled=false;
                CallButton.Text = "Call";
            }
        }

        async void OnCall(object sender, EventArgs e)
        {
            bool isNumberApproved = await GetResultOfDialog();

            if (isNumberApproved == true)
            {
                try
                {
                    if (PhoneDialer.IsSupported)
                    {
                        PhoneDialer.Default.Open(translatedNumber);
                    }
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (Exception)
                {
                    // Other error has occurred.
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }

            }
        }

        async Task<bool> GetResultOfDialog()
        {
            string alertHeader = "Dial a Number";
            string alertText = "Would you like to call " + translatedNumber + "?";
            string approvalButtonText = "Yes";
            string disapprovalButtonText = "No";
            bool shouldTheNumberCalled = await this.DisplayAlert(alertHeader, alertText, approvalButtonText, disapprovalButtonText);
            return shouldTheNumberCalled;
        }

      
    }

}
