using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinForms.Client.Interface;

namespace XamarinForms.Client
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
		{
			InitializeComponent();
		}

        private async void Connect_Clicked(object sender, EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient(); 
                    await client.ConnectAsync(IPAddress.Text, Convert.ToInt32(Port.Text));
                if (client.Connected)
                {
                    Connection.Instance.client = client;
                    Application.Current.MainPage = new NavigationPage(new OperationsPage());
                    DependencyService.Get<ToastMessage>().Alert("Connected to server!");
                }
                else
                {
                    DependencyService.Get<ToastMessage>().Alert("Connection feild!");
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
