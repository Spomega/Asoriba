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
    public partial class JoinChurchPage : PhoneApplicationPage
    {
        public JoinChurchPage()
        {
            InitializeComponent();
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (txtChurchname.Text.Equals(String.Empty))
            {
                MessageBox.Show("Enter Church");
            }

            else
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                //request server url
                String url = AsoribaConstants.server_url_test + AsoribaConstants.ADD_CHURCH;

                MessageBox.Show("phone number " + Register.phonenumber);
                //add request parameters
                var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("device","windows"),
                new KeyValuePair<String,String>("name",txtContactname.Text),
                new KeyValuePair<String,String>("phone_no",Register.phonenumber),
                new KeyValuePair<String,String>("contact_phone_no",txtContactnumber.Text),
                new KeyValuePair<String,String>("church_name",txtChurchname.Text),
                new KeyValuePair<String,String>("remarks",txtOptionalComment.Text)
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
                       // NavigationService.Navigate(new Uri("/FindChurchPage.xaml", UriKind.Relative));
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