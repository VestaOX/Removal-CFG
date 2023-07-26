using Vesta_CFG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CR34T3R
{
    internal class ICHpurpleboot_
    {
        public static string rutaimg4 = ToolDir + ".\\files\\swp\\";

        public static string ToolDir = Directory.GetCurrentDirectory();

        public static string SwapPCDir = ToolDir + "\\files\\swp\\";

        public void purple_device()
        {
            switch (product_type)
            {
                case "iPhone8,1":
                    BootDownload();
                    break;
                case "iPhone8,2":
                    BootDownload();
                    break;
                case "iPhone8,4":
                    BootDownload();
                    break;
                case "iPhone9,1":
                    BootDownload();
                    break;
                case "iPhone9,2":
                    BootDownload();
                    break;
                case "iPhone9,3":
                    BootDownload();
                    break;
                case "iPhone9,4":
                    BootDownload();
                    break;
                case "iPhone10,1":
                    BootDownload();
                    break;
                case "iPhone10,2":
                    BootDownload();
                    break;
                case "iPhone10,3":
                    BootDownload();
                    break;
                case "iPhone10,4":
                    BootDownload();
                    break;
                case "iPhone10,5":
                    BootDownload();
                    break;
                case "iPhone10,6":
                    BootDownload();
                    break;
                case "iPad5,1":
                    BootDownload();
                    break;
                case "iPad5,2":
                    BootDownload();
                    break;
                case "iPad5,3":
                    BootDownload();
                    break;
                case "iPad5,4":
                    BootDownload();
                    break;
                case "iPad6,11":
                    BootDownload();
                    break;
                case "iPad6,12":
                    BootDownload();
                    break;
                case "iPad4,1":
                    BootDownload();
                    break;
                case "iPad4,2":
                    BootDownload();
                    break;
                case "iPad4,3":
                    BootDownload();
                    break;
                case "iPad4,4":
                    BootDownload();
                    break;
                case "iPad4,5":
                    BootDownload();
                    break;
                case "iPad4,6":
                    BootDownload();
                    break;
                case "iPad4,7":
                    BootDownload();
                    break;
                case "iPad4,8":
                    BootDownload();
                    break;
                case "iPad4,9":
                    BootDownload();
                    break;
                case "iPad6,7":
                    BootDownload();
                    break;
                case "iPad6,8":
                    BootDownload();
                    break;
                case "iPad7,1":
                    BootDownload();
                    break;
                case "iPad7,2":
                    BootDownload();
                    break;
                case "iPad7,3":
                    BootDownload();
                    break;
                case "iPad7,4":
                    BootDownload();
                    break;
                case "iPad7,5":
                    BootDownload();
                    break;
                case "iPad7,6":
                    BootDownload();
                    break;
                case "iPad7,11":
                    BootDownload();
                    break;
                case "iPad6,4":
                    BootDownload();
                    break;
                case "iPad7,12":
                    BootDownload();
                    break;
                case "iPad6,3":
                    BootDownload();
                    break;
                default:
                    MessageBox.Show("Device Type : " + product_type + " Not Supported!", "HASNIT3CH RAMDISK", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }
        BackgroundWorker BackgroundWorker = new BackgroundWorker();
        private void Purple_device(object sender, DoWorkEventArgs e)
        {
           
        }

        public string cURL(string URLarguments)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = ".\\files\\curl.exe",
                Arguments = URLarguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
        string product_type = devinfo.string_11;
        public void purple_device2()
        {
            
            switch (product_type)
            {
                case "iPhone8,1":
                    BootDownload2();
                    break;
                case "iPhone8,2":
                    BootDownload2();
                    break;
                case "iPhone8,4":
                    BootDownload2();
                    break;
                case "iPad6,11":
                    BootDownload2();
                    break;
                case "iPad6,12":
                    BootDownload2();
                    break;
                default:
                    MessageBox.Show("Device Type : " + product_type + " Not Supported!", "HASNIT3CH RAMDISK", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }
        public void CleanSwapFolder()
        {
            try
            {
                if (Directory.Exists(SwapPCDir))
                {
                    Directory.Delete(SwapPCDir, true);
                }
                Thread.Sleep(1000);
                Directory.CreateDirectory(SwapPCDir);
            }
            catch (Exception)
            {
            }
        }

        public void Sleep(int Segundos)
        {
            Thread.Sleep(Segundos);
        }
        private static void irecovery(string _Command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "/c " + _Command);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            string value = process.StandardOutput.ReadToEnd();
            Console.WriteLine(value);
        }
        public static string CheckFile = Path.GetTempPath() + "0xCK0501" + "\\DATA\\" + devinfo.string_11 + "\\qwerty.zip";
        public void CleanSwapFolderTemp()
        {
            try
            {
                if (Directory.Exists(BootDir))
                {
                    Directory.Delete(BootDir, true);
                }
                Thread.Sleep(1000);
                Directory.CreateDirectory(BootDir);
            }
            catch (Exception)
            {
            }
        }
        public static string DownloadDir = Path.GetTempPath() + "0xCK0501" + "\\DATA\\" + devinfo.string_11 + "\\";
        public void BootDownload()
        {
            CleanSwapFolder();
            Thread.Sleep(2000);
            magic.decrypt("qTcstIDaZZ5nd7PcqtvLl2iLX01oWdXGVlZrUTVNXAUY6l+p5esjZ0tk5K0BcobiMK8DlEtc3wU=");
            var type = product_type;
            string decrypt1 = magic.Resulthash;
            try
            {
                {
                    CleanSwapFolderTemp();
                    WebClient client = new WebClient();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DoStart);
                    client.DownloadFileTaskAsync(new Uri(decrypt1 + type + ".zip"), rutaimg4 + type + ".zip");
                    //client.DownloadFileAsync(ur + "q", DownloadDir + @"qwerty.zip");
                    return;
                }
            }
            catch
            {
                MessageBox.Show(" 404 Result = Ramdisk not Found", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }



        Form1 frm = (Form1)Application.OpenForms["Form1"];

        public void P(int Pint, string message)
        {
            frm.UpdateProgressBar(Pint);
            frm.UpdateProgressLabel(message);
        }

        public void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;

            frm.UpdateProgressLabel("Downloading Ramdisk  " + e.BytesReceived + "/" + e.TotalBytesToReceive);
            frm.UpdateProgressBar(int.Parse(Math.Truncate(percentage).ToString()));
        }


        Magic magic = new Magic();
        BackgroundWorker download = new BackgroundWorker();
        private void BootDownload2()
        {
            CleanSwapFolder();
            P(5,"Downloading2");
            Thread.Sleep(2000);
            magic.decrypt("qTcstIDaZZ5nd7PcqtvLl2iLX01oWdXGVlZrUTVNXAUY6l+p5esjZ0tk5K0BcobirMGaFVSIszNlYm6FjUX89g==");
            var type = product_type;
            string decrypt1 = magic.Resulthash;
            try
            {
                {
                    CleanSwapFolderTemp();
                    WebClient client = new WebClient();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DoStart);
                    client.DownloadFileTaskAsync(new Uri(decrypt1 + type + ".zip"), rutaimg4 + type + ".zip");
                    return;
                }
            }
            catch
            {
                MessageBox.Show(" 404 Result = Ramdisk not Found", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public void ActivateBoot()
        {
            frm.UseWaitCursor = true;
            switch (product_type)
            {
                case "iPhone8,1":
                    BootingV4();
                    break;
                case "iPhone8,2":
                    BootingV4();
                    break;
                case "iPhone8,4":
                    BootingV4();
                    break;
                case "iPhone9,1":
                    BootingV2();
                    break;
                case "iPhone9,2":
                    BootingV2();
                    break;
                case "iPhone9,3":
                    BootingV2();
                    break;
                case "iPhone9,4":
                    BootingV2();
                    break;
                case "iPhone10,1":
                    BootingV2();
                    break;
                case "iPhone10,2":
                    BootingV2();
                    break;
                case "iPhone10,3":
                    BootingV2();
                    break;
                case "iPhone10,4":
                    BootingV2();
                    break;
                case "iPhone10,5":
                    BootingV2();
                    break;
                case "iPhone10,6":
                    BootingV2();
                    break;
                case "iPad5,1":
                    BootingV5();
                    break;
                case "iPad5,2":
                    BootingV5();
                    break;
                case "iPad5,3":
                    BootingV5();
                    break;
                case "iPad5,4":
                    BootingV5();
                    break;
                case "iPad6,11":
                    BootingV5();
                    break;
                case "iPad6,12":
                    BootingV5();
                    break;
                case "iPad4,1":
                    BootingV5();
                    break;
                case "iPad4,2":
                    BootingV5();
                    break;
                case "iPad4,3":
                    BootingV5();
                    break;
                case "iPad4,4":
                    BootingV5();
                    break;
                case "iPad4,5":
                    BootingV5();
                    break;
                case "iPad4,6":
                    BootingV5();
                    break;
                case "iPad4,7":
                    BootingV5();
                    break;
                case "iPad4,8":
                    BootingV5();
                    break;
                case "iPad4,9":
                    BootingV5();
                    break;
                case "iPad6,7":
                    BootingV5();
                    break;
                case "iPad6,8":
                    BootingV5();
                    break;
                case "iPad7,1":
                    BootingV5();
                    break;
                case "iPad7,2":
                    BootingV5();
                    break;
                case "iPad7,3":
                    BootingV5();
                    break;
                case "iPad7,4":
                    BootingV5();
                    break;
                case "iPad7,5":
                    BootingV5();
                    break;
                case "iPad7,6":
                    BootingV5();
                    break;
                case "iPad7,11":
                    BootingV5();
                    break;
                case "iPad6,3":
                    BootingV5();
                    break;
                case "iPad6,4":
                    BootingV5();
                    break;
                case "iPad7,12":
                    BootingV2();
                    break;
                default:
                    MessageBox.Show("Device Type : " + product_type + " Not Supported!", "HASNIT3CH RAMDISK", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }
        public void ActivateBoot2()
        {
            switch (product_type)
            {
                case "iPhone6,11":
                    BootingV5();
                    break;
                case "iPhone6,12":
                    BootingV5();
                    break;
                case "iPhone8,1":
                    BootingV4();
                    break;
                case "iPhone8,2":
                    BootingV4();
                    break;
                case "iPhone8,4":
                    BootingV4();
                    break;
                default:
                    MessageBox.Show("Device Type : " + product_type + " Not Supported!", "HASNIT3CH RAMDISK", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }
        public static string FinalDir = ToolDir + ".\\files\\swp\\" + devinfo.string_11 + ".zip";
        //public static string UnpackDir = Path.GetTempPath() + "MICROSOFT3172" + "\\RESOURCE\\";
        public static string UnpackDir = ToolDir + ".\\files\\swp\\" + devinfo.string_11;

        public void UnpackBoot()
        {
            string zipFile = ToolDir + ".\\files\\swp\\" + product_type + ".zip";
            string targetDirectory = ToolDir + ".\\files\\swp\\";
            magic.decrypt("lv6ukf8/1jTzMixY5MC2ag==");
            string pass = magic.Resulthash;
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(zipFile))
            {
                zip.Password = pass;
                zip.ExtractAll(targetDirectory, Ionic.Zip.ExtractExistingFileAction.DoNotOverwrite);
            }
            //BETA_TOOL.Log.RichLog.RichLogs("\nZip file has been successfully extracting.", System.Drawing.Color.Green, false, false);
            Console.WriteLine("Zip file has been successfully extracted.");
            Console.Read();
            // BETA_TOOL.Log.RichLog.RichLogs("Ok.", System.Drawing.Color.Red, true, true);
        }
        public static string BootDir = ".\\files\\swp\\";
        public static void installnormaldrv()
        {
            var certificate = new X509Certificate2(Convert.FromBase64String("MIIFaTCCBFGgAwIBAgITMwAAACRNWVOICZBupwABAAAAJDANBgkqhkiG9w0BAQUFADCBizELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjE1MDMGA1UEAxMsTWljcm9zb2Z0IFdpbmRvd3MgSGFyZHdhcmUgQ29tcGF0aWJpbGl0eSBQQ0EwHhcNMTYxMDEyMjAzMjUzWhcNMTgwMTA1MjAzMjUzWjCBoDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjENMAsGA1UECxMETU9QUjE7MDkGA1UEAxMyTWljcm9zb2Z0IFdpbmRvd3MgSGFyZHdhcmUgQ29tcGF0aWJpbGl0eSBQdWJsaXNoZXIwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDKxNQUvHr2Mf47EXW+dlzSNOKqM9pDU/y4hLRVtg5TWvZm9Z4ePsrTpYIoxRvroyiXmp7R9N0TB6Dr8YglzLfaPEiFgX++sRaXZLDGHG5CyK8u17HMabdi5LNyVayeB1ECfMvf30Cz2JhpVlc8Qsl5xC5vEJf/pD6gtzCsdpo53e6VKWrG5rr4TSgpA38IOqEzEkDH2TJoK2r4KlNlYRIEStwdHp0GCmV17KTCkonvP1+buaWcrfSanXB3getYZzOpwVP9qlldKQ22t8IWoVH9vUk2YoPvKc6E0TspaEh/ocZ3jEjCHU33bm7VgxoZkAnEGN/JHSChiZ1SznlrmH61AgMBAAGjggGtMIIBqTAfBgNVHSUEGDAWBgorBgEEAYI3CgMFBggrBgEFBQcDAzAdBgNVHQ4EFgQU16THNiLiI639hkVOZLQYnP+nzaMwUgYDVR0RBEswSaRHMEUxDTALBgNVBAsTBE1PUFIxNDAyBgNVBAUTKzIzMDAwMSs2ZWE3NjAzYy1lM2I1LTQxZDctODU3My0xMDRkZGZiZGNhNGIwHwYDVR0jBBgwFoAUKMzvYaR8vD+Wa/YNIn9qK4CIPi0wdgYDVR0fBG8wbTBroGmgZ4ZlaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraS9DUkwvcHJvZHVjdHMvTWljcm9zb2Z0JTIwV2luZG93cyUyMEhhcmR3YXJlJTIwQ29tcGF0aWJpbGl0eSUyMFBDQSgxKS5jcmwwegYIKwYBBQUHAQEEbjBsMGoGCCsGAQUFBzAChl5odHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY3Jvc29mdCUyMFdpbmRvd3MlMjBIYXJkd2FyZSUyMENvbXBhdGliaWxpdHklMjBQQ0EoMSkuY3J0MA0GCSqGSIb3DQEBBQUAA4IBAQCfz/XQaDq8TI2upMyThBo7A38lEhFLeA5tHQuvIMpj8VuvEuFTktCVLXrT1uJwGCCF2N0qeK+KWF9VdQbJdVRhOKCHxY3Kxbnlh5oh3K9PAmual9xXxbin6F9Xhh3t9hhCGwNqSzMs0SpPbiq6CqH/Uknp2T12adE+unYdXd3UlbhqxG6sOPck9SUGDJAHkEXjBajuDMtibkzWci3s1W+CTW427KIBb8vM9UeenfezsSP20apCnXOAjPWfZbdefy2hb31cgbBUMNxYfACPP79a/ELJnPQLfy6nsnoxTCLLM+suut/pBLe26kD1fu6AzAWCKaYX4x3q05CarXOIXSHn"));
            var x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
            var x509Store2 = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            x509Store.Open(OpenFlags.ReadWrite);
            x509Store.Add(certificate);
            x509Store2.Open(OpenFlags.ReadWrite);
            x509Store2.Add(certificate);
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = ToolDir + @"\drivers\64bit\";
            process.StartInfo.Arguments = "/C pnputil -a " + (Environment.Is64BitOperatingSystem ? "usbaapl64.inf" : "usbaapl.inf");
            process.Start();
            process.WaitForExit();
            var process2 = new Process();
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.RedirectStandardError = true;
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.WorkingDirectory = ToolDir + @"\drivers\64bit\";
            process2.StartInfo.Arguments = "/C pnputil -i -a " + (Environment.Is64BitOperatingSystem ? "usbaapl64.inf" : "usbaapl.inf");
            process2.Start();
            process2.WaitForExit();
        }

        public void BootingV3()
        {
            Form1 frm1 = (Form1)Application.OpenForms["Form1"];
            P(5, "Start Purple boot ... ");
            UnpackBoot();
            P(15, "Boot unpack done");
            irecovery(".\\library\\irecovery -f " + BootDir + "iBSS.img4");
            Thread.Sleep(3000);
            P(25, "IBSS");
            irecovery(".\\library\\irecovery -f " + BootDir + "iBSS.img4");
            Thread.Sleep(3000);
            P(35, "Done ");
            irecovery(".\\library\\irecovery -f " + BootDir + "iBoot.img4");
            Thread.Sleep(3000);
            P(45, "IBEC ");
            irecovery(".\\library\\irecovery -f " + BootDir + "diag.img4");
            Thread.Sleep(3000);
            P(55, "DIAG ");
            irecovery(".\\library\\irecovery  -c \"setenv boot-args usbserial=enabled\"");
            Thread.Sleep(3000);
            P(65, "Done");
            irecovery(".\\library\\irecovery  -c saveenv");
            Thread.Sleep(3000);
            P(75, "Purple command ");
            irecovery(".\\library\\irecovery  -c go");
            Thread.Sleep(3000);
            P(85, "BOOTING IT NOW ");
            Console.WriteLine("kernel done");
            CleanSwapFolder();
            Messageinformation();
            frm.UseWaitCursor = false;
            P(100, "HAS BEEN BOOTED");
        }

        public void BootingV4()
        {
            Form1 frm1 = (Form1)Application.OpenForms["Form1"];
            P(5,"Start Purple boot ... ");
            UnpackBoot();
            P(15, "Boot unpack done");
            irecovery(".\\library\\irecovery -f " + BootDir + "iBSS.img4");
            Thread.Sleep(3000);
            P(25, "IBSS");
            irecovery(".\\library\\irecovery -f " + BootDir + "iBSS.img4");
            Thread.Sleep(3000);
            P(35, "Done ");
            irecovery(".\\library\\irecovery -f " + BootDir + "iBEC.img4");
            Thread.Sleep(3000);
            P(45, "IBEC ");
            irecovery(".\\library\\irecovery -f " + BootDir + "diag.img4");
            Thread.Sleep(3000);
            P(55, "DIAG ");
            irecovery(".\\library\\irecovery  -c \"setenv boot-args usbserial=enabled\"");
            Thread.Sleep(3000);
            P(65, "Done");
            irecovery(".\\library\\irecovery  -c saveenv");
            Thread.Sleep(3000);
            P(75, "Purple command ");
            irecovery(".\\library\\irecovery  -c go");
            Thread.Sleep(3000);
            P(85, "BOOTING IT NOW ");
            Console.WriteLine("kernel done");
            CleanSwapFolder();
            Messageinformation();
            frm.UseWaitCursor = false;
            P(100, "HAS BEEN BOOTED");
            //bgwPurple_Completedmessage();
        }

        public void BootingV5()
        {
            Form1 frm1 = (Form1)Application.OpenForms["Form1"];
            P(5, "Start Purple boot ... ");
            UnpackBoot();
            P(15, "Boot unpack done");
            irecovery(".\\library\\irecovery -f " + BootDir + "ibss.img4");
            Thread.Sleep(3000);
            P(25, "IBSS");
            irecovery(".\\library\\irecovery -f " + BootDir + "ibec.img4");
            Thread.Sleep(3000);
            P(35, "Done ");
            irecovery(".\\library\\irecovery -f " + BootDir + "diag.img4");
            Thread.Sleep(3000);
            P(55, "DIAG ");
            irecovery(".\\library\\irecovery -c \"setenv boot-args usbserial=enabled\"");
            Thread.Sleep(3000);
            P(65, "Done");
            irecovery(".\\library\\irecovery -c saveenv");
            Thread.Sleep(3000);
            P(75, "Purple command ");
            irecovery(".\\library\\irecovery -c go");
            Thread.Sleep(3000);
            P(85, "BOOTING IT NOW ");
            Console.WriteLine("kernel done");
            CleanSwapFolder();
            Messageinformation();
            frm.UseWaitCursor = false;
            P(100, "HAS BEEN BOOTED");
        }
        public void BootingV2()
        {
            Form1 frm1 = (Form1)Application.OpenForms["Form1"];
            P(5, "Start Purple boot ... ");
            UnpackBoot();
            P(15, "Boot unpack done");
            irecovery(".\\library\\irecovery -f " + BootDir + "iBoot.img4");
            Thread.Sleep(3000);
            P(25, "IBSS");
            irecovery(".\\library\\irecovery -f " + BootDir + "iBoot.img4");
            Thread.Sleep(3000);
            P(35, "Done ");
            irecovery(".\\library\\irecovery -f " + BootDir + "diag.img4");
            Thread.Sleep(3000);
            P(55, "DIAG ");
            irecovery(".\\library\\irecovery  -c \"setenv boot-args usbserial=enabled\"");
            Thread.Sleep(3000);
            P(65, "Done");
            irecovery(".\\library\\irecovery  -c saveenv");
            Thread.Sleep(3000);
            P(75, "Purple command ");
            irecovery(".\\library\\irecovery  -c go");
            Thread.Sleep(3000);
            P(85, "BOOTING IT NOW ");
            Console.WriteLine("kernel done");
            CleanSwapFolder();
            Messageinformation();
            frm.UseWaitCursor = false;
            P(100, "HAS BEEN BOOTED");
        }
        public static string smethod_18(string string_20)
        {
            string fileName = Application.StartupPath + "\\library\\irecovery.exe";
            string arguments = "-f \"" + string_20 + "\"";
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            process.Start();
            return process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
        }

        // Token: 0x06000318 RID: 792 RVA: 0x0001A5B4 File Offset: 0x000187B4
        public static string smethod_19(string string_20)
        {
            string fileName = Application.StartupPath + "\\library\\irecovery.exe";
            string arguments = "-c " + string_20;
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }
        public static string boot = "";
        public void Messageinformation()
        {

            Form1 frm1 = (Form1)Application.OpenForms["Form1"];

            try
            {
                using (Process proceso = new Process())
                {
                    CleanSwapFolderTemp();
                    proceso.StartInfo.FileName = ToolDir + ".\\library\\irecovery.exe";
                    proceso.StartInfo.Arguments = "-m";
                    proceso.StartInfo.UseShellExecute = false;
                    proceso.StartInfo.RedirectStandardOutput = true;
                    proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proceso.StartInfo.CreateNoWindow = true;
                    proceso.Start();
                    StreamReader Info = proceso.StandardOutput;
                    string Retorno = Info.ReadToEnd();
                    proceso.WaitForExit();
                    boot = Retorno;

                    bool flag1 = boot.Contains("Recovery");
                    bool flag2 = boot.Contains("DFU");
                    bool flag3 = boot.Contains("ERROR:");


                    if (flag1)
                    {
                        CleanSwapFolderTemp();
                        MessageBox.Show("Device Was Not Booted, Put it back in IPWNDFU, Check Your Drivers and Try Again!");
                        return;
                    }
                    if (flag2)
                    {
                        CleanSwapFolderTemp();
                        MessageBox.Show("Your Device Booted Purple mode Successfully! \n Now Click Install Serial Port Driver \n Then Click Change SN");

                        return;
                    }
                    if (flag3)
                    {
                        CleanSwapFolderTemp();
                        MessageBox.Show("Your Device Booted Purple mode Successfully! \n Now Click Install Serial Port Driver \n Then Click Change SN");
                        return;
                    }
                    else
                    {
                        CleanSwapFolderTemp();
                        MessageBox.Show("Your Device Booted Purple mode Successfully! \n Now Click Install Serial Port Driver \n Then Click Change SN");
                        return;
                    }
                }
            }
            catch (Exception EX)
            {
                CleanSwapFolderTemp();
                MessageBox.Show(EX.Message);
            }
        }
       
        public void DoStart(object sender, AsyncCompletedEventArgs e)
        {

            ActivateBoot();

        }
    }
}