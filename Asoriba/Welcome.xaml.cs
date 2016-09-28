using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Asoriba
{
    public partial class Welcome : PhoneApplicationPage
    {
        String[] images;
        public Welcome()
        {
            InitializeComponent();
            images = new string[]{"/Assets/1.jpg","/Assets/2.jpg","/Assets/3.jpg","/Assets/4.jpg","/Assets/5.jpg"};

            List<Images> imgList = new List<Images>();

            for (int i = 0; i < images.Length;i++)
            {
                imgList.Add(new Images() { Image = images[i] });
            }

            SlideImg.ItemsSource = imgList;
        }


        public class Images
        {
            public string Image { get; set; }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Register.xaml  ", UriKind.Relative));
        }
    }
}