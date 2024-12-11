using System;
using System.Drawing;
using System.Management;
using System.Windows.Forms;

namespace DonanımBilgileri
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#002147");

            string computerName = Environment.MachineName;
            pcIsim.Text = computerName;

            string videoCardInfo = GetVideoCardInfo();
            ekranKarti.Text = "Ekran Kartı: " + videoCardInfo;

            string cpuSpeed = GetCPUSpeed();
            islemciHiz.Text = "İşlemci Hızı: " + cpuSpeed + " MHz";

            string biosVersion = GetBiosVersion();
            biosSurum.Text = "BIOS Sürümü: " + biosVersion;

            string ramInfo = GetRAMInfo();
            ram.Text = "RAM: " + ramInfo;

            string networkInfo = GetNetworkInfo();
            ipAdres.Text = "IP Adresi: " + networkInfo.Split('|')[0];
            macAdres.Text = "MAC Adresi: " + networkInfo.Split('|')[1];

            diskBilgisi.Text = "Disk Bilgisi: " + GetDiskInfo();

        }

        private string GetVideoCardInfo()
        {
            string videoCardInfo = "Bilgi Alınamadı";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");

            foreach (ManagementObject obj in searcher.Get())
            {
                videoCardInfo = obj["Name"].ToString();
            }

            return videoCardInfo;
        }

        private string GetCPUSpeed()
        {
            string cpuSpeed = "Bilgi Alınamadı";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                cpuSpeed = obj["CurrentClockSpeed"].ToString();
            }

            return cpuSpeed;
        }

        private string GetBiosVersion()
        {
            string biosVersion = "Bilgi Alınamadı";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");

            foreach (ManagementObject obj in searcher.Get())
            {
                biosVersion = obj["Version"].ToString();
            }

            return biosVersion;
        }

        private string GetRAMInfo()
        {
            long totalRam = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

            foreach (ManagementObject obj in searcher.Get())
            {
                totalRam += Convert.ToInt64(obj["Capacity"]);
            }

            return (totalRam / 1024 / 1024 / 1024).ToString() + " GB";
        }


        private string GetNetworkInfo()
        {
            string ipAddress = "Bilgi Alınamadı";
            string macAddress = "Bilgi Alınamadı";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = true");

            foreach (ManagementObject obj in searcher.Get())
            {
                string[] ipAddresses = (string[])obj["IPAddress"];
                ipAddress = ipAddresses.Length > 0 ? ipAddresses[0] : "Bilgi Alınamadı";
                macAddress = obj["MACAddress"].ToString();
            }

            return ipAddress + "|" + macAddress;
        }

        private string GetDiskInfo()
        {
            string diskInfo = "Bilgi Alınamadı";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject obj in searcher.Get())
            {
                string model = obj["Model"].ToString();
                string size = obj["Size"] != null ? (Convert.ToInt64(obj["Size"]) / 1024 / 1024 / 1024).ToString() + " GB" : "Bilinmiyor";
                diskInfo = $"Model: {model}, Kapasite: {size}";
                break;  
            }

            return diskInfo;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
