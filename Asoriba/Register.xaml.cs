using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;

namespace Asoriba
{
    public partial class Register : PhoneApplicationPage
    {

        public static string phonenumber = string.Empty;
        public Register()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtPhonenumber.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Phone Number");
            }
            if (txtCountryCode.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Country Code");
            }
            else
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AsoribaConstants.server_url_test + AsoribaConstants.SIGNUP;
                phonenumber = "+"+txtCountryCode.Text+txtPhonenumber.Text;
                MessageBox.Show("phone number " + phonenumber); 
                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("phone_no",phonenumber),
                new KeyValuePair<String,String>("app_id","")
            };
               

                String response = await Util.httpHelperPost(url, parameters);
                MessageBox.Show("response " + response);


                //when timer elapses
                timer.Tick += delegate
                {
                    if (response.Length > 0)
                    {
                        timer.Stop();
                        WaitIndicator.IsVisible = false;
                        parseJSONResponse(response);
                    }
                    else
                    {
                        timer.Stop();
                        WaitIndicator.IsVisible = false;
                        MessageBox.Show(AsoribaConstants.ERROR_MESSAGE);
                       
                    }
                };
                timer.Start();
            }
        }

        private void parseJSONResponse(string response)
        {
            try
            {
                RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

                if(returnObject.details.Length>0 || !returnObject.details.Equals(null))
                {
                    NavigationService.Navigate(new Uri("/VerifyPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Registeration Failed!,Please check if your entered a valid phonenumber");
                }
            }
            catch(Exception e)
            {

            }
          
        }



        public class RootObject
        {
            public string details { get; set; }
        }
    }
}