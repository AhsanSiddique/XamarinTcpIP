using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinForms.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OperationsPage : ContentPage
	{
        public OperationsPage ()
		{
			InitializeComponent ();
		}
            //Sleep Button Command
        private void Sleep_Clicked(object sender, EventArgs e)
        {
            var client = Connection.Instance.client;
            NetworkStream stream = client.GetStream();
            String s = "SLP2";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);
        }
        //Shutdown Button Command
        private void Shutdown_Clicked(object sender, EventArgs e)
        {
            var client = Connection.Instance.client;
            NetworkStream stream = client.GetStream();
            String s = "SHTD3";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);
        }
        //Screenshot Button Command
        private void Screenshot_Clicked(object sender, EventArgs e)
        {
            var client = Connection.Instance.client;
            NetworkStream stream = client.GetStream();
            String s = "TSC1";
            byte[] message = Encoding.ASCII.GetBytes(s);
            stream.Write(message, 0, message.Length);
            var data = getData(client);
            imageView.Source = ImageSource.FromStream(() => new MemoryStream(data));
        }

        //Data Collecting from Server
        public byte[] getData(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] fileSizeBytes = new byte[4];
            int bytes = stream.Read(fileSizeBytes, 0, fileSizeBytes.Length);
            int dataLength = BitConverter.ToInt32(fileSizeBytes, 0);

            int bytesLeft = dataLength;
            byte[] data = new byte[dataLength];

            int buffersize = 1024;
            int bytesRead = 0;

            while (bytesLeft > 0)
            {
                int curDataSize = Math.Min(buffersize, bytesLeft);
                if (client.Available < curDataSize)
                    curDataSize = client.Available;//This save me

                bytes = stream.Read(data, bytesRead, curDataSize);
                bytesRead += curDataSize;
                bytesLeft -= curDataSize;
            }
            return data;
        }
    }
}