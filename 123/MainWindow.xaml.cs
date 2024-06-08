using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Net.NetworkInformation;
using System.Net;
using System.Windows;
using Microsoft.Win32;
using System.Management;
using System.Linq;

namespace _123
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Visibility = Visibility.Hidden;

            TaskbarIcon.TrayMouseDoubleClick += TaskbarIcon_DoubleClick;

            Closing += MainWindow_Closing;

            ShowInfo();

            //var windowsVersion = GetWindowsVersion();

            //lblWindowsVersion.Content = $"Version de Windows : {windowsVersion}";

        }

        private void TaskbarIcon_DoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                Show();
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Close();
            Application.Current.Shutdown();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            Hide();
        }



        private void ShowInfo()
        {
            string macAddress = GetMacAddress();
            string ipAddress = GetIpAddress();
            string machineName = GetMachineName();
            string osInfo = GetOperatingSystemInfo();
            string windowsVersion = GetWindowsVersion();
            string windowsVersion2 = GetOperatingSystemInfo2();
            string ip2 = GetIpAddress2();
            string ip3 = GetRealIPAddress();

            infoTextBlock.Text = $"Adresse MAC: {macAddress}\nAdresse IP: {ipAddress}\nNom de la machine: {machineName}\nSystème d'exploitation: {osInfo}\nVersion de Windows: {windowsVersion}\nVersion de Windows: {windowsVersion2}\nIP: {ip2}\nIP3: {ip3}";
        }

        private string GetMacAddress()
        {
            string macAddress = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress = nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddress;
        }

        private string GetIpAddress()
        {
            string ipAddress = "";
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                    break;
                }
            }
            return ipAddress;
        }

        private string GetIpAddress2()
        {
            string ipAddress = "";

            string host = Dns.GetHostName();

            string ip = Dns.GetHostByName(host).AddressList[0].ToString();

            ipAddress = ip.ToString() + host.ToString();

            return ipAddress;
        }

        static string GetRealIPAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback);

            foreach (var networkInterface in networkInterfaces)
            {
                var ipProperties = networkInterface.GetIPProperties();
                foreach (var ip in ipProperties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.Address.ToString();
                    }
                }
            }

            return null;
        }

        private string GetMachineName()
        {
            return Environment.MachineName;
        }

        private string GetOperatingSystemInfo()
        {
            //return $"{Environment.OSVersion.Platform} {Environment.OSVersion.Version}";
            return Environment.OSVersion.ToString();
        }

        private string GetWindowsVersion()
        {
            string version = string.Empty;
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false))
            {
                if (key != null)
                {
                    object productName = key.GetValue("ProductName");
                    if (productName != null)
                    {
                        version = productName.ToString();
                    }
                }
            }
            return version;
        }

        public static string GetOperatingSystemInfo2()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT Caption, Version FROM Win32_OperatingSystem"))
            {
                foreach (var os in searcher.Get())
                {
                    return $"{os["Caption"]} - Version {os["Version"]}";
                }
            }
            return "Non trouvé";
        }


        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeApiUrl_Click(object sender, RoutedEventArgs e)
        {


        }

        private void ModifySettings_Click(object sender, RoutedEventArgs e)
        {
            //API_URL.IsEnabled = true;
        }

        private void OpenFAQButton_Click(object sender, RoutedEventArgs e)
        {
            FAQWindow faqWindow = new FAQWindow();
            faqWindow.Show();
        }
    }
}
