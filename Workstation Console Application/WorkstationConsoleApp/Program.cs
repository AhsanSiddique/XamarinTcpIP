using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WorkstationConsoleApp
{
    public static class Program
    {
        public static TcpClient client;
        private static TcpListener listener;
        private static string ipString;
        static void Main(string[] args)
        {
            IPAddress[] localIp = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIp)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipString = address.ToString();
                }
            }
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipString), 1234);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine(@"  
            ===================================================  
                   Started listening requests at: {0}:{1}  
            ===================================================",
            ep.Address, ep.Port);
            client = listener.AcceptTcpClient();
            Console.WriteLine("Connected to client!" + " \n");
            while (client.Connected)
            {
                try
                {
                    const int bytesize = 1024 * 1024;
                    byte[] buffer = new byte[bytesize];
                    string x = client.GetStream().Read(buffer, 0, bytesize).ToString();
                    var data = ASCIIEncoding.ASCII.GetString(buffer);

                    if (data.ToUpper().Contains("SLP2"))
                    {
                        Console.WriteLine("Pc is going to Sleep Mode!" + " \n");
                        Sleep();
                    }
                    else if (data.ToUpper().Contains("SHTD3"))
                    {
                        Console.WriteLine("Pc is going to Shutdown!" + " \n");

                        Shutdown();
                    }
                    else if (data.ToUpper().Contains("TSC1"))
                    {
                        Console.WriteLine("Take Screenshot!" + " \n");

                        var bitmap = SaveScreenshot();
                        var stream = new MemoryStream();
                        bitmap.Save(stream, ImageFormat.Bmp);
                        sendData(stream.ToArray(), client.GetStream());
                    }
                }
                catch (Exception exc)
                {
                    client.Dispose();
                    client.Close();
                }
            }
            void Shutdown()
            {
                System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
            }
            void Sleep()
            {
                Application.SetSuspendState(PowerState.Suspend, true, true);
            }
            Bitmap SaveScreenshot()
            {
                var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);
                return bmpScreenshot;
            }
            void sendData(byte[] data, NetworkStream stream)
            {
                int bufferSize = 1024;

                byte[] dataLength = BitConverter.GetBytes(data.Length);

                stream.Write(dataLength, 0, 4);

                int bytesSent = 0;
                int bytesLeft = data.Length;

                while (bytesLeft > 0)
                {
                    int curDataSize = Math.Min(bufferSize, bytesLeft);

                    stream.Write(data, bytesSent, curDataSize);

                    bytesSent += curDataSize;
                    bytesLeft -= curDataSize;
                }
            }
        }
    }
}
