using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using Vesta_CFG;

// Token: 0x0200001C RID: 28
[StandardModule]
internal sealed class Class7
{
    // Token: 0x060002FD RID: 765 RVA: 0x00019530 File Offset: 0x00017730
    public static void NqAmkMkovw(int timeout = 30000, string output = null, string error = null)
    {
        devinfo.smethod_29("libusb-1.1.dll");
        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c \"" + Environment.CurrentDirectory + "\\libusb-1.1.dll\" pwn");
        processStartInfo.Verb = "runas";
        processStartInfo.UseShellExecute = false;
        processStartInfo.RedirectStandardError = true;
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardInput = true;
        processStartInfo.CreateNoWindow = true;
        Encoding encoding = Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.OEMCodePage);
        processStartInfo.StandardOutputEncoding = encoding;
        processStartInfo.StandardErrorEncoding = encoding;
        Process process = new Process
        {
            StartInfo = processStartInfo
        };
        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                Class7.stringBuilder_0.AppendLine(e.Data);
            }
            else
            {
                Class7.autoResetEvent_1.Set();
            }
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data == null)
            {
                Class7.autoResetEvent_0.Set();
            }
            else
            {
                Class7.stringBuilder_1.AppendLine(e.Data);
            }
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        if (process.WaitForExit(timeout) && Class7.autoResetEvent_1.WaitOne(timeout) && Class7.autoResetEvent_0.WaitOne(timeout))
        {
            output = Class7.stringBuilder_0.ToString();
            Class7.stringBuilder_0 = new StringBuilder();
        }
        else
        {
            output = "PWN_TIMEOUT";
            error = Class7.stringBuilder_1.ToString();
            Class7.stringBuilder_1 = new StringBuilder();
            devinfo.smethod_29("libusb-1.1.dll");
        }
        process.Close();
    }
    
    public static void NqAmkMkovw2(ref string string_0, ref string string_1, int int_0 = 30000)
    {
		devinfo.smethod_29("libusb-1.1.dll");
		Class7.processStartInfo_0 = new ProcessStartInfo("cmd.exe", "/c \"" + Environment.CurrentDirectory + "\\libusb-1.1.dll\" pwn");
		Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.OEMCodePage);
		ProcessStartInfo processStartInfo = Class7.processStartInfo_0;
		processStartInfo.Verb = "runas";
		processStartInfo.UseShellExecute = false;
		processStartInfo.RedirectStandardError = true;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.RedirectStandardInput = true;
		processStartInfo.CreateNoWindow = true;
        Encoding encoding = Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.OEMCodePage);
        processStartInfo.StandardOutputEncoding = encoding;
		processStartInfo.StandardErrorEncoding = encoding;
		Class7.process_0 = new Process
		{
			StartInfo = Class7.processStartInfo_0
		};
		Class7.process_0.OutputDataReceived += Class7.smethod_0;
		Class7.process_0.ErrorDataReceived += Class7.smethod_1;
		Class7.process_0.Start();
		Class7.process_0.BeginOutputReadLine();
		Class7.process_0.BeginErrorReadLine();
		if (Class7.process_0.WaitForExit(int_0) && Class7.autoResetEvent_1.WaitOne(int_0) && Class7.autoResetEvent_0.WaitOne(int_0))
		{
			string_0 = Class7.stringBuilder_0.ToString();
			Class7.stringBuilder_0 = new StringBuilder();
		}
		else
		{
			string_0 = "PWN_TIMEOUT";
			string_1 = Class7.stringBuilder_1.ToString();
			Class7.stringBuilder_1 = new StringBuilder();
			devinfo.smethod_29("libusb-1.1.dll");
		}
		Class7.process_0.Close();
	}

	// Token: 0x060002FE RID: 766 RVA: 0x000196AC File Offset: 0x000178AC
	private static void smethod_0(object sender, DataReceivedEventArgs e)
	{
		if (e.Data != null)
		{
			Class7.stringBuilder_0.AppendLine(e.Data);
		}
		else
		{
			Class7.autoResetEvent_1.Set();
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x000196E4 File Offset: 0x000178E4
	private static void smethod_1(object sender, DataReceivedEventArgs e)
	{
		if (e.Data == null)
		{
			Class7.autoResetEvent_0.Set();
		}
		else
		{
			Class7.stringBuilder_1.AppendLine(e.Data);
		}
	}

	// Token: 0x04000149 RID: 329
	private static ProcessStartInfo processStartInfo_0;

	// Token: 0x0400014A RID: 330
	private static Process process_0;

	// Token: 0x0400014B RID: 331
	private static AutoResetEvent autoResetEvent_0 = new AutoResetEvent(false);

	// Token: 0x0400014C RID: 332
	private static AutoResetEvent autoResetEvent_1 = new AutoResetEvent(false);

	// Token: 0x0400014D RID: 333
	private static StringBuilder stringBuilder_0 = new StringBuilder();

	// Token: 0x0400014E RID: 334
	private static StringBuilder stringBuilder_1 = new StringBuilder();
}
