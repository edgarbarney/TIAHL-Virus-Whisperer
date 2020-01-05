using System;
using System.Windows;
using System.Media;

namespace VirusSilen3000
{
    public partial class ErrorWindow : Window
    {
        public bool isCritical;

        public ErrorWindow(string errorContent, string windowHeader, bool isc)
        {
            isCritical = isc;
            InitializeComponent();
            this.Title = windowHeader;
            HataIcerik.Content = errorContent;
            SystemSounds.Exclamation.Play();
            if (!isCritical)
            {
                ProgKpt.IsEnabled = false;
            }
            return;
        }
        public void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            return;
        }
        public void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            return;
        }
    }
}