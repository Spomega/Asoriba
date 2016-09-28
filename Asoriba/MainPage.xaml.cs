using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Asoriba.Resources;
using System.Windows.Threading;

namespace Asoriba
{
    public partial class MainPage : PhoneApplicationPage
    {

        public static bool visited = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DrawerLayout.InitializeDrawerLayout();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();


            //feeds.Add(new Feeds() { ImageUri = "/Assets/th.jpg", Title = " The Lord's Favour", Type = "  Devotion" });

           // feedList.ItemsSource = feeds;

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!visited)
            {
                WaitIndicator.IsVisible = true;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };


                //request  url
                // string url = AsoribaConstants.server_url_test + AsoribaConstants.LOAD_FEEDS+"page=1&page_size=50&feed_type=all&member_id=&content_category=all&multimedia_type=all";
                string url = "https://devapi.asoriba.com/api/v1.0/mobile/users/content/get_news/?page=1&page_size=50";
                //string url = AsoribaConstants.server_url_live+ AsoribaConstants.LOAD_FEEDS+"page=1&page_size=50&feed_type=all&member_id=&content_category=all&multimedia_type=all";


                String response = await Util.httpHelperGetWithToken(url);
              //  MessageBox.Show("response " + response);

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
                        //NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                    }
                };
                timer.Start();
            }


            this.BackKeyPress += MainPage_BackKeyPress;
        }

        private void Item_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
              var grid = sender as Grid;
              if (grid != null)
              {
                  string menuItemName = grid.Name;
              

                  switch (menuItemName)
                  {
                      case "Item1":
                          NavigationService.Navigate(new Uri("/Welcome.xaml  ", UriKind.Relative));
                          break;

                  }
              }

        }


        private void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (DrawerLayout.IsDrawerOpen)
            {
                DrawerLayout.CloseDrawer();
                e.Cancel = true;
            }
        }




        private void DrawerIcon_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            else
                DrawerLayout.OpenDrawer();
        }


        private void parseJSONResponse(String response)
        {
            try
            {
                RootObject returnObject = SimpleJson.DeserializeObject<RootObject>(response);

                if (!returnObject.results.Equals(null)) 
                {
                    List<Feeds> feeds = new List<Feeds>();
                    
                    foreach(Result r in returnObject.results.messages.results)
                    {
                        feeds.Add(new Feeds() { ImageUri = r.photo ,Title=r.title,Type=r.type,Message=r.message});
                    }


                    feedList.ItemsSource = feeds;
                    visited = true;

                }
                else
                {
                    MessageBox.Show("Feeds Not Loaded");
                }

              
            }
            catch (Exception e)
            {
                MessageBox.Show(AsoribaConstants.ERROR_MESSAGE);
            }



        }


        public class Feeds
        {
            public string ImageUri
            {
                get;
                set;
            }

            public string Title
            {
                get;
                set;
            }

            public string Type
            {
                get;
                set;
            }

            public string Message
            {
                get;
                set;
            }
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

       //JSON helper class
        public class Quotation
        {
            public int id { get; set; }
            public string book { get; set; }
            public int chapter { get; set; }
            public string verse { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string title { get; set; }
            public string author { get; set; }
            public string type { get; set; }
            public string message { get; set; }
            public string send_date { get; set; }
            public string modified { get; set; }
            public object start_date { get; set; }
            public object end_date { get; set; }
            public string photo { get; set; }
            public object location { get; set; }
            public object media_url { get; set; }
            public int opens { get; set; }
            public int shares { get; set; }
            public int favourites { get; set; }
            public object media_file { get; set; }
            public object location_coordinates { get; set; }
            public List<object> gallery { get; set; }
            public List<Quotation> quotations { get; set; }
            public int amens { get; set; }
            public int no_of_comments { get; set; }
            public object has_amened { get; set; }
            public bool has_commented { get; set; }
            public int impressions { get; set; }
        }

        public class Messages
        {
            public int count { get; set; }
            public object next { get; set; }
            public object previous { get; set; }
            public List<Result> results { get; set; }
        }

        public class Results
        {
            public Messages messages { get; set; }
        }

        public class RootObject
        {
            public Results results { get; set; }
        }
    }
}