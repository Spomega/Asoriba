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
    public partial class VerifyPage : PhoneApplicationPage
    {
        public VerifyPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtSecretCode.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Secret Code");
            }
           
            else
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AsoribaConstants.server_url_test + AsoribaConstants.VERIFY;

                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("device","windows"),
                new KeyValuePair<String,String>("app_id",txtSecretCode.Text)
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
                        //parseJSONResponse(response);
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




    }
}