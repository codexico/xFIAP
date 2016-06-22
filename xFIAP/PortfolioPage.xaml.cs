using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace xFIAP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PortfolioPage : Page
    {
        public PortfolioPage()
        {
            this.InitializeComponent();
            loadPortfolio();
        }

        private async void loadPortfolio()
        {
            ObservableCollection<Product> dataList = new ObservableCollection<Product>();
            
            Uri uriProdutos = new Uri("http://www.wopek.com/xml/produtos.xml");
            XmlDocument xmlProdutos = await XmlDocument.LoadFromUriAsync(uriProdutos);

            XmlElement root = xmlProdutos.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("/produtos/produto");

            foreach (XmlElement el in nodes)
            {
                string xmlDescricao = el.SelectSingleNode("descricao").InnerText;
                string xmlCategoria = el.SelectSingleNode("categoria").InnerText;
                int xmlPrecounitario = Int32.Parse(el.SelectSingleNode("precounitario").InnerText);
                int xmlQuantidade = Int32.Parse(el.SelectSingleNode("quantidade").InnerText);
                int xmlId = Int32.Parse(el.GetAttribute("id"));

                Product p = new Product()
                {
                    Descricao = xmlDescricao,
                    Quantidade = xmlQuantidade,
                    Categoria = xmlCategoria,
                    Precounitario = xmlPrecounitario,
                    id = xmlId
                };

                dataList.Add(p);
            }
            
            PortfolioList.ItemsSource = dataList;
        }

        private void PortfolioList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product p = (sender as ListView).SelectedItem as Product;
            
            this.Frame.Navigate(typeof(DetailsPage), p);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
