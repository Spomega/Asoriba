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
    public partial class FindChurchPage : PhoneApplicationPage
    {
        public FindChurchPage()
        {
            InitializeComponent();
        }




        public class Churches
        {
            public string ImageUrl
            {
                get;
                set;
            }
            public string Church
            {
                get;
                set;
            }

            public string Branch
            {
                get;
                set;
            }
        }

        private async void AppBarSearch_Click(object sender, EventArgs e)
        {
            PhoneTextBox textBox = new PhoneTextBox()
            {
                 PlaceholderText="What is the name of your church?",
                 Margin = new Thickness(0, 30, 12, 24),
                 Name =  "txtChurch"
            };

            CustomMessageBox customMessage = new CustomMessageBox()
            {
                Title ="Find Your Church",
                Content = textBox,
                LeftButtonContent ="Cancel",
                RightButtonContent ="Submit"

            };

             var ph =  customMessage.Content as PhoneTextBox;

            switch (await customMessage.ShowAsync())
            {
                case CustomMessageBoxResult.LeftButton:
                    // Do something.
                    break;
                case CustomMessageBoxResult.RightButton:
                    if (ph.Text.Equals(String.Empty))
                    {
                        MessageBox.Show("Enter Church");
                    }
                    else 
                    {
                        WaitIndicator.IsVisible = true;
                        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
                        //request server url
                        String url = AsoribaConstants.server_url_test + AsoribaConstants.FIND_CHURCH;

                        MessageBox.Show("phone number " + Register.phonenumber);
                        //add request parameters
                        var parameters = new List<KeyValuePair<String, String>>{
                new KeyValuePair<String,String>("device","windows"),
               
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
                    break;
                case CustomMessageBoxResult.None:
                    // Do something.
                    break;
                default:
                    break;
            }

        }

        private void AppBarlibrary_Click(object sender, EventArgs e)
        {

        }



        public class Church
        {
            public int id { get; set; }
            public string church_name { get; set; }
            public object logo { get; set; }
            public object thumbnail_40x40 { get; set; }
        }

        public class BranchInfo
        {
            public int id { get; set; }
            public Church church { get; set; }
            public string branch_name { get; set; }
            public string logo { get; set; }
            public string thumbnail_40x40 { get; set; }
        }

        public class Results
        {
            public List<BranchInfo> branch_info { get; set; }
            public bool suggested { get; set; }
        }

        public class RootObject
        {
            public Results results { get; set; }
        }
    }
}