using System;
using System.Collections.Generic;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsuario.Text.ToLower();
            string passworrd = pwdSenha.Password;

            Uri uriUsuarios = new Uri("http://www.wopek.com/xml/usuarios.xml");
            XmlDocument xmlUsuarios = await XmlDocument.LoadFromUriAsync(uriUsuarios);

            XmlElement root = xmlUsuarios.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("/usuarios/usuario");

            bool isUser = false;

            foreach (XmlElement el in nodes)
            {
                string xmlUsername = el.SelectSingleNode("username").InnerText.ToLower();
                string xmlPassword = el.SelectSingleNode("password").InnerText;

                if (username == xmlUsername && passworrd == xmlPassword)
                {
                    isUser = true;
                }
            }

            if (isUser)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {

                var messageDialog = new MessageDialog(
                        "Usuário ou senha inválidos", "Tente Novamente");
                await messageDialog.ShowAsync();
            }


        }
    }
}
