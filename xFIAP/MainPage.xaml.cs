using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace xFIAP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        
        private void btnPortfolio_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PortfolioPage));
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ClientsPage));
        }
        
        private async void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            e.Handled = true;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
            else
            {
                var messageDialog = new MessageDialog(
                    "Tem certeza que deseja encerrar?", "Atenção");

                messageDialog.Commands.Add(new UICommand("Sim"));
                messageDialog.Commands.Add(new UICommand("Não"));

                var result = await messageDialog.ShowAsync();
                if (result.Label == "Sim")
                {
                    Application.Current.Exit();
                }
            }
        }

    }
}
