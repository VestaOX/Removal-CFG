using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vesta_CFG
{
    class Class8
    {

        private static ProcessStartInfo processStartInfo_0;

        private static Process process_1;

        public static MatchCollection smethod_20(string string_14, string string_15, RegexOptions regexOptions_0 = RegexOptions.ExplicitCapture)
        {
            Regex regex = new Regex(string_15, regexOptions_0);
            return regex.Matches(string_14);
        }
        public static void smethod_25(string string_14, string string_15, string string_16 = "")
        {
            try
            {
                Class8.process_1.Kill();
            }
            catch (Exception ex)
            {
            }
            Class8.processStartInfo_0 = new ProcessStartInfo("cmd.exe", "/c " + string_14);
            Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.OEMCodePage);
            ProcessStartInfo processStartInfo = Class8.processStartInfo_0;
            processStartInfo.Verb = "runas";
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.CreateNoWindow = true;
            Encoding encoding = Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.OEMCodePage);
            processStartInfo.StandardOutputEncoding = encoding;
            processStartInfo.StandardErrorEncoding = encoding;
            Class8.process_1 = new Process
            {
                StartInfo = Class8.processStartInfo_0
            };
            Class8.process_1.Start();
            Class8.process_1.WaitForExit(5000);
            StreamReader standardOutput = Class8.process_1.StandardOutput;
            StreamReader standardError = Class8.process_1.StandardError;
            string_15 = standardOutput.ReadToEnd().Trim();
            string_16 = standardError.ReadToEnd().Trim();
            standardError.Close();
            standardOutput.Close();
            Class8.process_1.Close();
        }
        public static string smethod_28(string string_14)
        {
            string result = "";
            string string_15 = "";
            string string_16 = "pnputil /enum-devices /instanceid \"" + string_14 + "\" /connected";
            string text = "";
            Class8.smethod_25(string_16, string_15, text);
            try
            {
                result = Class8.smethod_20(string_15, "oem(.*?).inf", RegexOptions.ExplicitCapture)[0].Value.ToString().Trim();
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public static void smethod_29(string string_14, bool bool_1 = true, bool bool_2 = true)
        {
            string text = "";
            string str = Class8.smethod_28(string_14);
            string string_15 = "pnputil -d " + str + " -f -u";
            string text2 = "";
            Class8.smethod_25(string_15, text, text2);
            if (bool_2)
            {
                Class8.smethod_43();
            }
            if (bool_1)
            {
                string string_16 = "pnputil /scan-devices";
                text2 = "";
                Class8.smethod_25(string_16, text, text2);
            }
        }
        
        public static void smethod_43()
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
            process.StartInfo.Arguments = "/c pnputil -a " + (Environment.Is64BitOperatingSystem ? "usbaapl64.inf" : "usbaapl.inf");
            process.Start();
            process.WaitForExit();
            Process process2 = new Process();
            process2.StartInfo.Verb = "runas";
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.RedirectStandardError = true;
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\drivers\\usb\\" + (Environment.Is64BitOperatingSystem ? "x64\\" : "x86\\");
            process2.StartInfo.Arguments = "/c pnputil -i -a " + ((!Environment.Is64BitOperatingSystem) ? "usbaapl.inf" : "usbaapl64.inf");
            process2.Start();
            process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();
        }
    }
}
