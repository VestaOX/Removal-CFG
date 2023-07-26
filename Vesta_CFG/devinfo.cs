using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vesta_CFG
{
    class devinfo
    {
        public static string string_0 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string string_1 = devinfo.string_0 + "\\HASNIT3CHFILES\\APPLE\\diag_files\\";

        public static string string_7 = "";

        public static string string_8 = "";

        public static string string_9 = "";

        public static string string_10 = "";

        public static string string_11 = "";

        public static string string_12 = "";

        public static string string_13 = "";

        public static string string_14 = "";

        public static string string_15 = "";

        public static string string_16 = "";

        public static string string_pwnd = "";

        private static ProcessStartInfo processStartInfo_0;

        private static Process process_1;

        public static void smethod_0(int int_0)
        {
            DateTime t = DateAndTime.Now.AddMilliseconds((double)int_0);
            bool flag = false;
            DateTime t2;
            while (!flag)
            {
                t2 = DateAndTime.Now;
                if (DateTime.Compare(t2, t) > 0)
                {
                    flag = true;
                }
                Application.DoEvents();
            }
            t2 = DateTime.MinValue;
            t = DateTime.MinValue;
        }
        public static MatchCollection smethod_3(string string_20, string string_21, RegexOptions regexOptions_0 = RegexOptions.ExplicitCapture)
        {
            Regex regex = new Regex(string_21, regexOptions_0);
            return regex.Matches(string_20);
        }

        public static void smethod_8(string string_20, ref string string_21, ref string string_22)
        {
            try
            {
                devinfo.process_1?.Kill();
            }
            catch (Exception ex)
            {
                // Handle exception or remove empty catch block
            }

            devinfo.processStartInfo_0 = new ProcessStartInfo("cmd.exe", "/c " + string_20);
            Encoding encoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);
            ProcessStartInfo processStartInfo = devinfo.processStartInfo_0;
            processStartInfo.Verb = "runas";
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.StandardOutputEncoding = encoding;
            processStartInfo.StandardErrorEncoding = encoding;

            devinfo.process_1 = new Process
            {
                StartInfo = devinfo.processStartInfo_0
            };
            devinfo.process_1.Start();
            devinfo.process_1.WaitForExit(5000);

            using (StreamReader standardOutputReader = devinfo.process_1.StandardOutput)
            using (StreamReader standardErrorReader = devinfo.process_1.StandardError)
            {
                string_21 = standardOutputReader.ReadToEnd().Trim();
                string_22 = standardErrorReader.ReadToEnd().Trim();
            }

            devinfo.process_1.Close();
        }
        public static object smethod_9()
        {
            bool flag = false;
            try
            {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID FROM Win32_PnPEntity where DeviceID Like 'USB%' AND (DeviceID LIKE '%PWND%' OR DeviceID LIKE '%CHECKM8%')");
                flag = (managementObjectSearcher.Get().Count >= 1);
                managementObjectSearcher.Dispose();
            }
            catch (ManagementException ex)
            {
            }
            return flag;
        }
        public static object smethod_10()
        {
            bool flag = false;
            try
            {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,Name,Service FROM Win32_PnPEntity where DeviceID Like '%ECID%'");
                if (managementObjectSearcher.Get().Count >= 1)
                {
                    try
                    {
                        string text = Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                        {
                        "Name"
                        }, null));
                        string string_ = Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                        {
                        "DeviceID"
                        }, null));
                        string text2 = Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                        {
                        "Service"
                        }, null));
                        if (Operators.CompareString(text.ToLower(), "apple mobile device usb device", true) == 0 | text2.ToLower().Contains("libusbk"))
                        {
                            Form1 frm = new Form1();
                            if (frm.get_device_info)
                            {
                                devinfo.smethod_12(string_, true, true);
                            }
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    return true;
                }
                managementObjectSearcher.Dispose();
            }
            catch (ManagementException ex2)
            {
            }
            return flag;
        }
        public static string smethod_11(string string_20)
        {
            string result = "";
            string string_21 = "";
            string string_22 = "pnputil /enum-devices /instanceid \"" + string_20 + "\" /connected";
            string text = "";
            devinfo.smethod_8(string_22, ref string_21, ref text);
            try
            {
                result = devinfo.smethod_3(string_21, "oem(.*?).inf", RegexOptions.ExplicitCapture)[0].Value.ToString().Trim();
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public static void smethod_12(string string_20, bool bool_0 = true, bool bool_1 = true)
        {
            string text = "";
            string str = devinfo.smethod_11(string_20);
            string string_21 = "pnputil -d " + str + " -f -u";
            string text2 = "";
            devinfo.smethod_8(string_21, ref text, ref text2);
            if (bool_1)
            {
                devinfo.smethod_23();
            }
            if (bool_0)
            {
                string string_22 = "pnputil /scan-devices";
                text2 = "";
                devinfo.smethod_8(string_22, ref text, ref text2);
            }
        }
        public static string smethod_13()
        {
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,Name FROM Win32_PnPEntity where Name Like '%Apple Mobile Device%'");
            string result;
            if (managementObjectSearcher.Get().Count < 1)
            {
                result = null;
            }
            else
            {
                string text = Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                {
                "DeviceID"
                }, null));
                result = text;
            }
            return result;
        }
        public static void smethod_14(int int_1 = 10000)
        {
            string text = devinfo.smethod_13();
            DateTime t = DateAndTime.Now.AddMilliseconds((double)int_1);
            bool flag = false;
            DateTime t2;
            while (text != null & !flag)
            {
                text = devinfo.smethod_13();
                devinfo.smethod_12(text, true, false);
                t2 = DateAndTime.Now;
                if (DateTime.Compare(t2, t) > 0)
                {
                    flag = true;
                }
                Application.DoEvents();
            }
            t2 = DateTime.MinValue;
            t = DateTime.MinValue;
        }
        public static string smethod_17(string string_20, string string_21)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = string_20,
                Arguments = string_21,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            process.Start();
            return process.StandardOutput.ReadToEnd();
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
        public static void smethod_20()
        {
            devinfo.string_8 = "";
            devinfo.string_9 = "";
            devinfo.string_11 = "";
            devinfo.string_12 = "";
            devinfo.string_13 = "";
            devinfo.string_14 = "";
            devinfo.string_15 = "";
            devinfo.string_16 = "";
            devinfo.string_pwnd = "";
            checked
            {
                if (!Conversions.ToBoolean(Operators.NotObject(devinfo.smethod_10())))
                {
                    string text = devinfo.smethod_17(Application.StartupPath + "\\library\\irecovery.exe", "-q");
                    string[] array = text.Split(new char[]
                    {
                    '\n'
                    });
                    string[] array2 = array;
                    int num = array2.Length - 1;
                    for (int i = 0; i <= num; i++)
                    {
                        string text2 = array2[i].Replace("\r", "");
                        if (!text2.StartsWith("ECID: "))
                        {
                            if (text2.StartsWith("NAME: "))
                            {
                                string text3 = text2.Replace("NAME: ", "").Trim();
                                devinfo.string_9 = text3;
                            }
                            else if (text2.StartsWith("PRODUCT: "))
                            {
                                string text4 = text2.Replace("PRODUCT: ", "").Trim();
                                devinfo.string_11 = text4;
                            }
                            else if (!text2.StartsWith("MODE: "))
                            {
                                if (!text2.StartsWith("SNON: "))
                                {
                                    if (!text2.StartsWith("SRNM: "))
                                    {
                                        if (text2.StartsWith("CPID: "))
                                        {
                                            string text5 = text2.Replace("CPID: ", "").Replace("0x", "").Trim();
                                            devinfo.string_15 = text5;
                                        }
                                        else if (text2.StartsWith("MODEL: "))
                                        {
                                            string text6 = text2.Replace("MODEL: ", "").Trim();
                                            devinfo.string_16 = text6;
                                        }
                                        else if (text2.StartsWith("PWND: "))
                                        {
                                            string text11 = text2.Replace("PWND: ", "").Trim();
                                            devinfo.string_pwnd = text11;
                                        }
                                    }
                                    else
                                    {
                                        string text7 = text2.Replace("SRNM: ", "").Trim();
                                        devinfo.string_14 = text7;
                                    }
                                }
                                else
                                {
                                    string text8 = text2.Replace("SNON: ", "").Trim();
                                    devinfo.string_13 = text8;
                                }
                            }
                            else
                            {
                                string text9 = text2.Replace("MODE: ", "").Trim();
                                devinfo.string_12 = text9;
                            }
                        }
                        else
                        {
                            string text10 = text2.Replace("ECID: ", "").Replace("0x", "").Trim();
                            devinfo.string_8 = text10;
                        }
                    }
                }
            }
        }
        public static void smethod_22()
        {
            X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String("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"));
            X509Store x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
            X509Store x509Store2 = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            x509Store.Open(OpenFlags.ReadWrite);
            x509Store.Add(certificate);
            x509Store2.Open(OpenFlags.ReadWrite);
            x509Store2.Add(certificate);
            Process process = new Process();
            process.StartInfo.Verb = "runas";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\drivers\\libusbK\\";
            process.StartInfo.Arguments = "/c pnputil -a Apple_Mobile_Device_(DFU_Mode).inf";
            process.Start();
            process.WaitForExit();
            Process process2 = new Process();
            process2.StartInfo.Verb = "runas";
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.RedirectStandardError = true;
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\drivers\\libusbK\\";
            process2.StartInfo.Arguments = "/c pnputil -i -a Apple_Mobile_Device_(DFU_Mode).inf";
            process2.Start();
            process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();
        }
        public static void smethod_23()
        {
            X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String("MIIFaTCCBFGgAwIBAgITMwAAACRNWVOICZBupwABAAAAJDANBgkqhkiG9w0BAQUFADCBizELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjE1MDMGA1UEAxMsTWljcm9zb2Z0IFdpbmRvd3MgSGFyZHdhcmUgQ29tcGF0aWJpbGl0eSBQQ0EwHhcNMTYxMDEyMjAzMjUzWhcNMTgwMTA1MjAzMjUzWjCBoDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjENMAsGA1UECxMETU9QUjE7MDkGA1UEAxMyTWljcm9zb2Z0IFdpbmRvd3MgSGFyZHdhcmUgQ29tcGF0aWJpbGl0eSBQdWJsaXNoZXIwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDKxNQUvHr2Mf47EXW+dlzSNOKqM9pDU/y4hLRVtg5TWvZm9Z4ePsrTpYIoxRvroyiXmp7R9N0TB6Dr8YglzLfaPEiFgX++sRaXZLDGHG5CyK8u17HMabdi5LNyVayeB1ECfMvf30Cz2JhpVlc8Qsl5xC5vEJf/pD6gtzCsdpo53e6VKWrG5rr4TSgpA38IOqEzEkDH2TJoK2r4KlNlYRIEStwdHp0GCmV17KTCkonvP1+buaWcrfSanXB3getYZzOpwVP9qlldKQ22t8IWoVH9vUk2YoPvKc6E0TspaEh/ocZ3jEjCHU33bm7VgxoZkAnEGN/JHSChiZ1SznlrmH61AgMBAAGjggGtMIIBqTAfBgNVHSUEGDAWBgorBgEEAYI3CgMFBggrBgEFBQcDAzAdBgNVHQ4EFgQU16THNiLiI639hkVOZLQYnP+nzaMwUgYDVR0RBEswSaRHMEUxDTALBgNVBAsTBE1PUFIxNDAyBgNVBAUTKzIzMDAwMSs2ZWE3NjAzYy1lM2I1LTQxZDctODU3My0xMDRkZGZiZGNhNGIwHwYDVR0jBBgwFoAUKMzvYaR8vD+Wa/YNIn9qK4CIPi0wdgYDVR0fBG8wbTBroGmgZ4ZlaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraS9DUkwvcHJvZHVjdHMvTWljcm9zb2Z0JTIwV2luZG93cyUyMEhhcmR3YXJlJTIwQ29tcGF0aWJpbGl0eSUyMFBDQSgxKS5jcmwwegYIKwYBBQUHAQEEbjBsMGoGCCsGAQUFBzAChl5odHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY3Jvc29mdCUyMFdpbmRvd3MlMjBIYXJkd2FyZSUyMENvbXBhdGliaWxpdHklMjBQQ0EoMSkuY3J0MA0GCSqGSIb3DQEBBQUAA4IBAQCfz/XQaDq8TI2upMyThBo7A38lEhFLeA5tHQuvIMpj8VuvEuFTktCVLXrT1uJwGCCF2N0qeK+KWF9VdQbJdVRhOKCHxY3Kxbnlh5oh3K9PAmual9xXxbin6F9Xhh3t9hhCGwNqSzMs0SpPbiq6CqH/Uknp2T12adE+unYdXd3UlbhqxG6sOPck9SUGDJAHkEXjBajuDMtibkzWci3s1W+CTW427KIBb8vM9UeenfezsSP20apCnXOAjPWfZbdefy2hb31cgbBUMNxYfACPP79a/ELJnPQLfy6nsnoxTCLLM+suut/pBLe26kD1fu6AzAWCKaYX4x3q05CarXOIXSHn"));
            X509Store x509Store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
            X509Store x509Store2 = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            x509Store.Open(OpenFlags.ReadWrite);
            x509Store.Add(certificate);
            x509Store2.Open(OpenFlags.ReadWrite);
            x509Store2.Add(certificate);
            Process process = new Process();
            process.StartInfo.Verb = "runas";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\drivers\\usb\\" + (Environment.Is64BitOperatingSystem ? "x64\\" : "x86\\");
            process.StartInfo.Arguments = "/c pnputil -a " + ((!Environment.Is64BitOperatingSystem) ? "usbaapl.inf" : "usbaapl64.inf");
            process.Start();
            process.WaitForExit();
            Process process2 = new Process();
            process2.StartInfo.Verb = "runas";
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.RedirectStandardError = true;
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\drivers\\usb\\" + ((!Environment.Is64BitOperatingSystem) ? "x86\\" : "x64\\");
            process2.StartInfo.Arguments = "/c pnputil -i -a " + (Environment.Is64BitOperatingSystem ? "usbaapl64.inf" : "usbaapl.inf");
            process2.Start();
            process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();
        }
        public static void smethod_25()
        {
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,Name FROM Win32_PnPEntity where Name Like '%Apple Mobile Device%'");
            if (managementObjectSearcher.Get().Count >= 1)
            {
                try
                {
                    Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                    {
                    "Name"
                    }, null));
                    string string_ = Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                    {
                    "DeviceID"
                    }, null));
                    devinfo.smethod_12(string_, false, false);
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static bool smethod_26()
        {
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,Name FROM Win32_PnPEntity where Name Like '%Apple Mobile Device%'");
            return managementObjectSearcher.Get().Count >= 1;
        }
        public static void smethod_27()
        {
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,Name FROM Win32_PnPEntity where Name Like '%Apple Mobile Device%'");
            if (managementObjectSearcher.Get().Count >= 1)
            {
                try
                {
                    Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                    {
                    "Name"
                    }, null));
                    string string_ = Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                    {
                    "DeviceID"
                    }, null));
                    devinfo.smethod_12(string_, false, false);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static void smethod_29(string string_20)
        {
            try
            {
                foreach (Process process in Process.GetProcessesByName(string_20))
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static object smethod_30(string string_20)
        {
            Regex regex = new Regex("^([0-9a-fA-F]{2}(?:(?:[0-9a-fA-F]{2}){5}|(?::[0-9a-fA-F]{2}){5}|[0-9a-fA-F]{10}))$");
            object result;
            if (regex.IsMatch(string_20))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
