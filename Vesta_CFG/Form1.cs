using CR34T3R;
using Downloader;
using MetroFramework.Controls;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vesta_CFG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            (new Core.DropShadow()).ApplyShadows(this);

            this.thread_0 = null;
            this.object_0 = RuntimeHelpers.GetObjectValue(new object());
            this.get_device_info = true;
            this.bool_1 = false;
            this.object_1 = false;
            this.string_0 = null;
            this.string_1 = null;
            this.bool_2 = false;
            this.serialPort_0 = new SerialPort();
            this.sn = "";
            this.mode = "";
            this.regn = "";
            this.color = "";
            this.color_housing = "";
            this.wifi = "";
            this.bmac = "";
            this.emac = "";
            this.mlb = "";
            this.model = "";
            this.model_name = "";
            this.nvsn = "";
            this.nsrn = "";
            this.lcm = "";
            this.battery = "";
            this.bcms = "";
            this.fcms = "";
            this.mtsn = "";
            this.template_color = "";
        }
        public string fcms;

        public string color_housing;

        private bool bool_2;

        private object object_0;



        private Thread thread_0;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void guna2Separator1_Click(object sender, EventArgs e)
        {

        }
        public bool get_device_info;

        private ManagementEventWatcher managementEventWatcher_0;
        public void loadSerialPort()
        {
            this.ComboBoxEx1.Items.Clear();
            this.ComboBoxEx1.Items.Add("Choose Port");
            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name FROM Win32_PnPEntity where ClassGuid='{4d36e978-e325-11ce-bfc1-08002be10318}'");
            if (searcher.Get().Count >= 1)
            {
                try
                {
                    foreach (ManagementBaseObject managementBaseObject in searcher.Get())
                    {
                        ManagementObject managementObject = (ManagementObject)managementBaseObject;
                        string string_ = Conversions.ToString(managementObject["Name"]);
                        MatchCollection matchCollection = devinfo.smethod_3(string_, "\\(COM.*?\\)", RegexOptions.ExplicitCapture);
                        if (matchCollection.Count >= 1)
                        {
                            this.ComboBoxEx1.Items.Add(matchCollection[0].Value.ToString().Replace("(", "").Replace(")", "").Trim());
                        }
                    }
                }
                finally
                {
                    searcher.Dispose();
                }
            }
            this.ComboBoxEx1.SelectedIndex = 0;
        }
        public void loadSerialPort1()
        {
            this.ComboBoxEx1.Items.Clear();
            this.ComboBoxEx1.Items.Add("Choose Port");

            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name FROM Win32_PnPEntity where ClassGuid='{4d36e978-e325-11ce-bfc1-08002be10318}'");
            if (searcher.Get().Count >= 1)
            {
                try
                {
                    var names = searcher.Get()
                        .Cast<ManagementObject>()
                        .Select(x => x["Name"].ToString());

                    foreach (var name in names)
                    {
                        var matches = devinfo.smethod_3(name, "\\(COM.*?\\)", RegexOptions.ExplicitCapture);

                        if (matches.Count >= 1)
                        {
                            var portName = matches[0].Value
                                .Replace("(", "")
                                .Replace(")", "")
                                .Trim();

                            this.ComboBoxEx1.Items.Add(portName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception
                }
                finally
                {
                    searcher.Dispose();
                }
            }

            this.ComboBoxEx1.SelectedIndex = 0;
        }
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool IsWow64Process(IntPtr intptr_0, out bool bool_3);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool Wow64DisableWow64FsRedirection(out IntPtr intptr_0);
        private void Form1_Load(object sender, EventArgs e)
        {
            bool flag = false;
            Form1.IsWow64Process(Process.GetCurrentProcess().Handle, out flag);
            if (flag)
            {
                IntPtr zero = IntPtr.Zero;
                Form1.Wow64DisableWow64FsRedirection(out zero);
            }
            Control.CheckForIllegalCrossThreadCalls = false;
            this.load_templates();
            this.loadSerialPort();
            ThreadPool.QueueUserWorkItem(delegate (object a0)
            {
                //this.checkUpdate();
            });
            this.startWatch();
        }
        public void startWatch()
        {
            this.watcher_Event();
            this.managementEventWatcher_0 = new ManagementEventWatcher();
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 or EventType = 3 GROUP WITHIN 1 ");
            this.managementEventWatcher_0.EventArrived += delegate (object sender, EventArrivedEventArgs e)
            {
                this.watcher_Event();
            };
            this.managementEventWatcher_0.Query = query;
            this.managementEventWatcher_0.Start();
        }
        private bool bool_1;

        public string model;

        public string mode;


        public Process proceso = new Process();

        public string RecoveryINFO(string Info)
        {
            string contents = "@echo off\nlibrary\\irecovery.exe -q | library\\grep.exe -w " + Info + " | library\\awk.exe '{printf $NF}'";
            File.WriteAllText("Info.cmd", contents);
            proceso = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "Info.cmd",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proceso.Start();
            StreamReader standardOutput = proceso.StandardOutput;
            string text = standardOutput.ReadToEnd();
            Delete("Info.cmd");
            return text;
        }

        private void Delete(string FileD)
        {
            if (File.Exists(FileD))
            {
                File.Delete(FileD);
            }
        }


        public void ModelRecovery()
        {
            switch (RecoveryINFO("PRODUCT"))
            {
                case "iPhone8,1":
                    {
                        lblModel.Text = "iPhone 6S (MEID)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone8,2":
                    {
                        lblModel.Text = "iPhone 6S Plus (MEID)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone8,4":
                    {
                        lblModel.Text = "iPhone SE Gen1 (MEID)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._5;

                        break;
                    }

                case "iPhone9,1":
                    {
                        lblModel.Text = "iPhone 7 (MEID)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone9,2":
                    {
                        lblModel.Text = "iPhone 7 Plus (MEID)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone9,3":
                    {
                        lblModel.Text = "iPhone 7 (GSM)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone9,4":
                    {
                        lblModel.Text = "iPhone 7 Plus (GSM)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone10,1":
                    {
                        lblModel.Text = "iPhone 8 (MEID)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone10,2":
                    {
                        lblModel.Text = "iPhone 8 Plus (MEID)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone10,3":
                    {
                        lblModel.Text = "iPhone X (MEID)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._10;

                        break;
                    }

                case "iPhone10,4":
                    {
                        lblModel.Text = "iPhone 8 (GSM)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone10,5":
                    {
                        lblModel.Text = "iPhone 8 Plus (GSM)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._6;

                        break;
                    }

                case "iPhone10,6":
                    {
                        lblModel.Text = "iPhone X (GSM)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._10;

                        break;
                    }

                case "iPhone11,6":
                    {
                        lblModel.Text = "iPhone XS Max";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.xs;

                        break;
                    }

                case "iPhone11,2":
                    {
                        lblModel.Text = "iPhone XS";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.xs;

                        break;
                    }

                case "iPhone11,4":
                    {
                        lblModel.Text = "iPhone XS Max (China)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.xs;

                        break;
                    }

                case "iPhone11,8":
                    {
                        lblModel.Text = "iPhone XR";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.xr;

                        break;
                    }

                case "iPhone12,5":
                    {
                        lblModel.Text = "iPhone 11 Pro Max";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._11;

                        break;
                    }

                case "iPhone12,1":
                    {
                        lblModel.Text = "iPhone 11";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._11;

                        break;
                    }

                case "iPhone12,3":
                    {
                        lblModel.Text = "iPhone 11 Pro";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._11;

                        break;
                    }

                case "iPhone12,8":
                    {
                        lblModel.Text = "iPhone SE (2020)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._11;

                        break;
                    }

                case "iPhone13,2":
                    {
                        lblModel.Text = "iPhone 12";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._12;

                        break;
                    }

                case "iPhone13,1":
                    {
                        lblModel.Text = "iPhone 12 mini";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._12;

                        break;
                    }

                case "iPhone13,4":
                    {
                        lblModel.Text = "iPhone 12 Pro Max";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._12;

                        break;
                    }

                case "iPhone13,3":
                    {
                        lblModel.Text = "iPhone 12 Pro";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._12;

                        break;
                    }

                case "iPhone14,2":
                    {
                        lblModel.Text = "iPhone 13 Pro";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._13;

                        break;
                    }

                case "iPhone14,3":
                    {
                        lblModel.Text = "iPhone 13 Pro Max";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._13;

                        break;
                    }

                case "iPhone14,4":
                    {
                        lblModel.Text = "iPhone 13 mini";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._13;

                        break;
                    }

                case "iPhone14,5":
                    {
                        lblModel.Text = "iPhone 13";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._13;

                        break;
                    }

                case "iPhone14,6":
                    {
                        lblModel.Text = "iPhone SE (3rd generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._10;

                        break;
                    }

                case "iPhone14,7":
                    {
                        lblModel.Text = "iPhone 14";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._14;

                        break;
                    }

                case "iPhone14,8":
                    {
                        lblModel.Text = "iPhone 14 Plus";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._14;

                        break;
                    }

                case "iPhone15,2":
                    {
                        lblModel.Text = "iPhone 14 Pro";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._14;

                        break;
                    }

                case "iPhone15,3":
                    {
                        lblModel.Text = "iPhone 14 Pro Max";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources._14;

                        break;
                    }

                case "iPad4,4":
                    {
                        lblModel.Text = "iPad Mini 2 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad4,5":
                    {
                        lblModel.Text = "iPad Mini 2 (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad4,6":
                    {
                        lblModel.Text = "iPad Mini 2 (China)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad5,1":
                    {
                        lblModel.Text = "iPad Mini 4 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad5,2":
                    {
                        lblModel.Text = "iPad Mini 4 (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad5,3":
                    {
                        lblModel.Text = "iPad Air 2 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad5,4":
                    {
                        lblModel.Text = "iPad Air 2 (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad6,3":
                    {
                        lblModel.Text = "iPad Pro 9.7-inch (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad6,4":
                    {
                        lblModel.Text = "iPad Pro 9.7-inch (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad6,7":
                    {
                        lblModel.Text = "iPad Pro 12.9-inch (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad6,8":
                    {
                        lblModel.Text = "iPad Pro 12.9-inch (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad7,1":
                    {
                        lblModel.Text = "iPad Pro 2 (12.9-inch, WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad7,2":
                    {
                        lblModel.Text = "iPad Pro 2 (12.9-inch, Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad7,3":
                    {
                        lblModel.Text = "iPad Pro (10.5-inch, WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad7,4":
                    {
                        lblModel.Text = "iPad Pro (10.5-inch, Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad7,5":
                    {
                        lblModel.Text = "iPad 6 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad8,12":
                    {
                        lblModel.Text = "iPad Pro 4 (12.9-inch, Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad8,9":
                    {
                        lblModel.Text = "iPad Pro 4 (11-inch, WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad8,11":
                    {
                        lblModel.Text = "iPad Pro 4 (12.9-inch, WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad8,10":
                    {
                        lblModel.Text = "iPad Pro 4 (11-inch, Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,1":
                    {
                        lblModel.Text = "iPad Air 4 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad11,6":
                    {
                        lblModel.Text = "iPad 8 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad11,3":
                    {
                        lblModel.Text = "iPad Air 3 64GB Space Gray";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;
                        break;
                    }

                case "iPad11,7":
                    {
                        lblModel.Text = "iPad 8 (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,2":
                    {
                        lblModel.Text = "iPad Air 4 (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,4":
                    {
                        lblModel.Text = "iPad Pro (11-inch) (3rd generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,5":
                    {
                        lblModel.Text = "iPad Pro (11-inch) (3rd generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,9":
                    {
                        lblModel.Text = "iPad Pro (12.9-inch) (5th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,11":
                    {
                        lblModel.Text = "iPad Pro (12.9-inch) (5th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,8":
                    {
                        lblModel.Text = "iPad Pro (12.9-inch) (5th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,10":
                    {
                        lblModel.Text = "iPad Pro (12.9-inch) (5th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,7":
                    {
                        lblModel.Text = "iPad Pro (11-inch) (3rd generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,6":
                    {
                        lblModel.Text = "iPad Pro (11-inch) (3rd generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad12,1":
                    {
                        lblModel.Text = "iPad 9 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad12,2":
                    {
                        lblModel.Text = "iPad 9 (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad14,1":
                    {
                        lblModel.Text = "iPad Mini 6 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad14,2":
                    {
                        lblModel.Text = "iPad Mini 6 (Cellular)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,16":
                    {
                        lblModel.Text = "iPad Air 5 (WiFi)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,18":
                    {
                        lblModel.Text = "iPad (10th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad13,19":
                    {
                        lblModel.Text = "iPad (10th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad14,6":
                    {
                        lblModel.Text = "iPad Pro (12.9-inch) (6th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad14,5":
                    {
                        lblModel.Text = "iPad Pro (12.9-inch) (6th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad14,4":
                    {
                        lblModel.Text = "iPad Pro (11-inch) (4th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                case "iPad14,3":
                    {
                        lblModel.Text = "iPad Pro (11-inch) (4th generation)";
                        pictureBox1.Image = Vesta_CFG.Properties.Resources.ipad_pro;

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }


        public void watcher_Event()
        {
            ModelRecovery();
            if (!this.bool_1)
            {
                if (!Conversions.ToBoolean(devinfo.smethod_10()))
                {
                    if (Operators.CompareString(this.Button_X3.Text, "Disconnect", true) == 0)
                    {
                        this.Button_X3.PerformClick();
                        if (this.ComboBoxEx1.SelectedIndex >= 0)
                        {
                            this.ComboBoxEx1.Items.Remove(RuntimeHelpers.GetObjectValue(this.ComboBoxEx1.SelectedItem));
                            this.ComboBoxEx1.SelectedIndex = -1;
                        }
                    }
                    if (this.get_device_info)
                    {
                        devinfo.string_8 = "";
                        this.model = "";
                        devinfo.string_11 = "";
                        this.mode = "";
                        devinfo.string_13 = "";
                        devinfo.string_14 = "";
                        devinfo.string_15 = "";
                        devinfo.string_16 = "";
                        devinfo.string_pwnd = "";
                        this.lblModel.Text = "-";
                        this.lblMode.Text = "-";
                        this.lblECID.Text = "-";
                        this.lblNandSize.Text = "-";
                        this.label30.Text = "-";
                        this.initialize_mode();
                    }
                }
                else
                {
                    devinfo.smethod_20();
                    if (!string.IsNullOrEmpty(devinfo.string_15))
                    {
                        devinfo.string_10 = devinfo.string_15;
                        if (Operators.CompareString(this.Button_X3.Text, "Disconnect", true) == 0)
                        {
                            this.Button_X3.PerformClick();
                            if (this.ComboBoxEx1.SelectedIndex >= 0)
                            {
                                this.ComboBoxEx1.Items.Remove(RuntimeHelpers.GetObjectValue(this.ComboBoxEx1.SelectedItem));
                                this.ComboBoxEx1.SelectedIndex = -1;
                            }
                        }
                        this.lblModel.Text = devinfo.string_9.ToUpper();
                        this.lblMode.Text = devinfo.string_12.ToUpper();
                        this.lblECID.Text = devinfo.string_8.ToUpper();
                        this.label30.Text = devinfo.string_pwnd.ToUpper();
                    }
                    else
                    {
                        this.lblModel.Text = "-";
                        this.lblMode.Text = "-";
                        this.lblECID.Text = "-";
                        this.lblNandSize.Text = "-";
                        this.label30.Text = "-";
                    }
                    this.initialize_mode();
                }
            }
        }
        public void initialize_mode()
        {
            if (Operators.CompareString(this.lblMode.Text, "DFU", true) == 0)
            {
                this.lblMode.ForeColor = Color.LimeGreen;
                //this.PictureBox4.Image = Resources.dfu;
            }
            else if (Operators.CompareString(this.lblMode.Text, "ENGINEERING MODE", true) != 0)
            {
                if (Operators.CompareString(this.lblMode.Text, "-", true) != 0)
                {
                    Label lblMode;
                    (lblMode = this.lblMode).Text = lblMode.Text + " (WRONG MODE)";
                    this.lblMode.ForeColor = Color.Blue;
                    //this.PictureBox4.Image = Resources.not_connected;
                }
                else
                {
                    this.lblMode.ForeColor = Color.Black;
                    //this.PictureBox4.Image = Resources.not_connected;
                }
            }
            else
            {
                this.lblMode.ForeColor = Color.Purple;
                //this.PictureBox4.Image = Resources.purple;
            }
        }
        public void load_templates()
        {
            this.templatebox.Items.Clear();
            this.templatebox.Items.Add("Choose template");
            foreach (string path in Directory.GetFiles(Environment.CurrentDirectory + "\\templates"))
            {
                this.templatebox.Items.Add(Path.GetFileNameWithoutExtension(path));
            }
            this.templatebox.SelectedIndex = 0;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        public static bool smethod_32(string string_14, string string_15, IWin32Window iwin32Window_0)
        {
            int num = (int)MessageBox.Show(iwin32Window_0, string_14, string_15, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return num == 6;
        }

        public void UpdateProgressBar(int value)
        {
            progressBar1.Value = value;
        }

        public void UpdateProgressLabel(string text)
        {
            ProgressLabel.Text = text;
        }

        private void guna2Button29_Click(object sender, EventArgs e)
        {
            if (smethod_32("On next boot please do the following steps:\r\n\r\n1. Select Troubleshoot.\r\n2. Select Advanced options.\r\n3. Select Startup Settings.\r\n4. Click on Restart.\r\n5. On the Startup Settings screen press 7 or F7 to disable driver signature enforcement.\r\n\r\nDo you want to proceed?", "", this))
            {
                this.disabledriverbtn.Text = "Disabling Driver Signature...";
                string text = "";
                string string_ = "bcdedit /set loadoptions DISABLE_INTEGRITY_CHECKS";
                string text2 = "";
                smethod_25(string_, text, text2);
                string string_2 = "bcdedit.exe /set TESTSIGNING ON";
                text2 = "";
                smethod_25(string_2, text, text2);
                string string_3 = "shutdown -r -o -f -t 0";
                text2 = "";
                smethod_25(string_3, text, text2);
                this.disabledriverbtn.Text = "Disable Driver Signature (Reboot Required)";
            }
        }
        private static ProcessStartInfo processStartInfo_0;

        private static Process process_1;
        public static void smethod_25(string string_14, string string_15, string string_16 = "")
        {
            try
            {
                process_1.Kill();
            }
            catch (Exception ex)
            {
            }

            processStartInfo_0 = new ProcessStartInfo("cmd.exe", "/c " + string_14);
            Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.OEMCodePage);
            ProcessStartInfo processStartInfo = processStartInfo_0;
            processStartInfo.Verb = "runas";
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.CreateNoWindow = true;
            Encoding encoding = Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.OEMCodePage);
            processStartInfo.StandardOutputEncoding = encoding;
            processStartInfo.StandardErrorEncoding = encoding;

            process_1 = new Process
            {
                StartInfo = processStartInfo_0
            };
            process_1.Start();
            process_1.WaitForExit(5000);
            StreamReader standardOutput = process_1.StandardOutput;
            StreamReader standardError = process_1.StandardError;
            string_15 = standardOutput.ReadToEnd().Trim();
            string_16 = standardError.ReadToEnd().Trim();
            standardError.Close();
            standardOutput.Close();
            process_1.Close();
        }

        private void guna2Button30_Click(object sender, EventArgs e)
        {
            this.repairdriverbtn.Text = "Repairing DFU Driver...";
            bool flag = false;
            try
            {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,Name,Service FROM Win32_PnPEntity where DeviceID Like '%ECID%'");
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
                        Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                        {
                            "Service"
                        }, null));
                        devinfo.smethod_12(string_, true, true);
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                managementObjectSearcher.Dispose();
            }
            catch (ManagementException ex2)
            {
            }
            if (!flag)
            {
                devinfo.smethod_12("", true, true);
            }
            MessageBox.Show(this, "DFU Driver has been repaired!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.repairdriverbtn.Text = "Repaired DFU Driver";
        }
        private void method_53(object sender, EventArgs e)
        {
            this.template_color = "";
            if (this.templatebox.SelectedIndex < 1)
            {
                this.txtSN.Text = "";
                this.txtMode.Text = "";
                this.txtRegn.Text = "";
                this.cboColor.DataSource = null;
                this.txtWifi.Text = "";
                this.txtBMac.Text = "";
                this.txtEMac.Text = "";
                this.txtMLB.Text = "";
                this.txtModel.Text = "";
                this.txtNVSN.Text = "";
                this.txtNSRN.Text = "";
                this.txtLCM.Text = "";
                this.txtBattery.Text = "";
                this.txtBCMS.Text = "";
                this.txtFCMS.Text = "";
                this.txtMTSN.Text = "";
                this.btnFlash.Enabled = false;
            }
            else
            {
                this.btnFlash.Enabled = true;
                this.loadTemplate();
            }
        }
        public void loadTemplate()
        {
            string[] array = new string[]
            {
                "SrNm",
                "Mod#",
                "Regn",
                "DClr",
                "WMac",
                "BMac",
                "EMac",
                "MLB#",
                "RMd#",
                "NvSn",
                "NSrN",
                "LCM#",
                "Batt",
                "BCMS",
                "FCMS",
                "MtSN"
            };
            string str = this.templatebox.SelectedItem.ToString();
            string text = Environment.CurrentDirectory + "\\templates\\" + str + ".ht";
            try
            {
                if (!File.Exists(text))
                {
                    throw new Exception("Template not exist " + text);
                }
                MemoryStream memoryStream = Class5.smethod_3(text);
                MemoryStream memoryStream2 = Class5.OwiicddWE(memoryStream, "!H@SNI|@293068!", (Class5.Enum0)2, null);
                string @string = Encoding.UTF8.GetString(memoryStream2.ToArray());
                string[] array2 = @string.Split(new char[]
                {
                    '\n'
                });
                foreach (string text2 in array2)
                {
                    if (text2.Contains("syscfg add"))
                    {
                        string text3 = devinfo.smethod_3(text2, "syscfg add (.*?) ", RegexOptions.ExplicitCapture)[0].Value.Replace("syscfg add ", "").Trim();
                        int num = Array.IndexOf<string>(array, text3);
                        if (num >= 0)
                        {
                            string value = devinfo.smethod_3(text2, "syscfg add .*", RegexOptions.ExplicitCapture)[0].Value.Replace("syscfg add " + text3, "").Trim();
                            this.set_cmd_key_to_text(text3, value);
                        }
                    }
                }
                this.initialize_colors(this.txtModel.Text, this.template_color);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        public string template_color;
        public void initialize_colors(string model, string color)
        {
            this.cboColor.DataSource = null;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Choose Color", "");
            if (Operators.CompareString(model, "A1586", true) == 0 | Operators.CompareString(model, "A1549", true) == 0 | Operators.CompareString(model, "A1589", true) == 0)
            {
                dictionary.Add("Gold (iPh6)", "0x00000200 0x00E1CCB5 0x00E1E4E3 0x00000000");
                dictionary.Add("Silver (iPh6)", "0x00000200 0x00D7D9D8 0x00E1E4E3 0x00000000");
                dictionary.Add("SpaceGrey (iPh6)", "0x00000200 0x00B4B5B9 0x003B3B3C 0x00000000");
            }
            if (Operators.CompareString(model, "A1522", true) == 0 | Operators.CompareString(model, "A1524", true) == 0 | Operators.CompareString(model, "A1593", true) == 0)
            {
                dictionary.Add("Gold (iPh6+)", "0x00000200 0x00E1CCB5 0x00E1E4E3 0x00000000");
                dictionary.Add("Silver (iPh6+)", "0x00000200 0x00D7D9D8 0x00E1E4E3 0x00000000");
                dictionary.Add("SpaceGrey (iPh6+)", "0x00000200 0x00B9B7BA 0x00272728 0x00000000");
                dictionary.Add("Roségold (iPh6+)", "0x00000200 0x00E4C1B9 0x00E4E7E8 0x00000000");
            }
            if (Operators.CompareString(model, "A1633", true) == 0 | Operators.CompareString(model, "A1688", true) == 0 | Operators.CompareString(model, "A1691", true) == 0 | Operators.CompareString(model, "A1700", true) == 0)
            {
                dictionary.Add("Gold (iPh6S)", "0x00000200 0x00E1CCB7 0x00E4E7E8 0x00000000");
                dictionary.Add("Silver (iPh6S)", "0x00000200 0x00DADCDB 0x00E4E7E8 0x00000000");
                dictionary.Add("SpaceGrey (iPh6S)", "0x00000200 0x00B9B7BA 0x00272728 0x00000000");
                dictionary.Add("Roségold (iPh6S)", "0x00000200 0x00E4C1B9 0x00E4E7E8 0x00000000");
            }
            if (Operators.CompareString(model, "A1634", true) == 0 | Operators.CompareString(model, "A1687", true) == 0 | Operators.CompareString(model, "A1690", true) == 0 | Operators.CompareString(model, "A1699", true) == 0)
            {
                dictionary.Add("Gold (iPh6S+)", "0x00000200 0x00E1CCB7 0x00E4E7E8 0x00000000");
                dictionary.Add("Silver (iPh6S+)", "0x00000200 0x00DADCDB 0x00E4E7E8 0x00000000");
                dictionary.Add("SpaceGrey (iPh6S+)", "0x00000200 0x00B9B7BA 0x00272728 0x00000000");
                dictionary.Add("Roségold (iPh6S+)", "0x00000200 0x00E4C1B9 0x00E4E7E8 0x00000000");
            }
            if (Operators.CompareString(model, "A1660", true) == 0 | Operators.CompareString(model, "A1779", true) == 0 | Operators.CompareString(model, "A1780", true) == 0 | Operators.CompareString(model, "A1778", true) == 0)
            {
                dictionary.Add("Gold (iPh7)", "0x00000001 0x00000000 0x00000000 0x00000003");
                dictionary.Add("Silver (iPh7)", "0x00000001 0x00000000 0x00000000 0x00000002");
                dictionary.Add("Black (iPh7)", "0x00000001 0x00000000 0x00000000 0x00000001");
                dictionary.Add("DiamondBlack(iPh7)", "0x00000001 0x00000000 0x00000000 0x00000005");
                dictionary.Add("Roségold (iPh7)", "0x00000001 0x00000000 0x00000000 0x00000004");
                dictionary.Add("Red (iPh7)", "0x00000001 0x00000000 0x00000000 0x00000006");
            }
            if (Operators.CompareString(model, "A1661", true) == 0 | Operators.CompareString(model, "A1785", true) == 0 | Operators.CompareString(model, "A1786", true) == 0 | Operators.CompareString(model, "A1784", true) == 0)
            {
                dictionary.Add("Gold (iPh7+)", "0x00000001 0x00000000 0x00000000 0x00000003");
                dictionary.Add("Silver (iPh7+)", "0x00000001 0x00000000 0x00000000 0x00000002");
                dictionary.Add("Black (iPh7+)", "0x00000001 0x00000000 0x00000000 0x00000001");
                dictionary.Add("DiamondBlack(iPh7+)", "0x00000001 0x00000000 0x00000000 0x00000005");
                dictionary.Add("Roségold (iPh7+)", "0x00000001 0x00000000 0x00000000 0x00000004");
                dictionary.Add("Red (iPh7+)", "0x00000001 0x00000000 0x00000000 0x00000006");
            }
            if (Operators.CompareString(model, "A1863", true) == 0 | Operators.CompareString(model, "A1906", true) == 0 | Operators.CompareString(model, "A1907", true) == 0 | Operators.CompareString(model, "A1905", true) == 0)
            {
                dictionary.Add("Black (iPh8)", "0x00000001 0x00000000 0x00000000 0x00000008");
                dictionary.Add("Silver (iPh8)", "0x00000001 0x00000000 0x00000000 0x00000002");
                dictionary.Add("Red (iPh8)", "0x00000001 0x00000000 0x00000000 0x00000006");
                dictionary.Add("Gold (iPh8)", "0x00000001 0x00000000 0x00000000 0x00000007");
            }
            if (Operators.CompareString(model, "A1864", true) == 0 | Operators.CompareString(model, "A1898", true) == 0 | Operators.CompareString(model, "A1899", true) == 0 | Operators.CompareString(model, "A1897", true) == 0)
            {
                dictionary.Add("Black (iPh8+)", "0x00000001 0x00000000 0x00000000 0x00000001");
                dictionary.Add("Silver (iPh8+)", "0x00000001 0x00000000 0x00000000 0x00000002");
                dictionary.Add("Red (iPh8+)", "0x00000001 0x00000000 0x00000000 0x00000006");
                dictionary.Add("Gold (iPh8+)", "0x00000001 0x00000000 0x00000000 0x00000003");
            }
            if (Operators.CompareString(model, "A1865", true) == 0 | Operators.CompareString(model, "A1902", true) == 0 | Operators.CompareString(model, "A1901", true) == 0)
            {
                dictionary.Add("Black (iPhX)", "0x00000001 0x00000000 0x00000000 0x00000001");
                dictionary.Add("White (iPhX)", "0x00000001 0x00000000 0x00000000 0x00000002");
            }
            this.cboColor.DataSource = new BindingSource(dictionary, null);
            this.cboColor.DisplayMember = "Value";
            this.cboColor.DisplayMember = "Key";
            if (Operators.CompareString(model, "A1660", true) == 0 | Operators.CompareString(model, "A1779", true) == 0 | Operators.CompareString(model, "A1780", true) == 0 | Operators.CompareString(model, "A1778", true) == 0 | Operators.CompareString(model, "A1661", true) == 0 | Operators.CompareString(model, "A1785", true) == 0 | Operators.CompareString(model, "A1786", true) == 0 | Operators.CompareString(model, "A1784", true) == 0 | Operators.CompareString(model, "A1863", true) == 0 | Operators.CompareString(model, "A1906", true) == 0 | Operators.CompareString(model, "A1907", true) == 0 | Operators.CompareString(model, "A1905", true) == 0 | Operators.CompareString(model, "A1864", true) == 0 | Operators.CompareString(model, "A1898", true) == 0 | Operators.CompareString(model, "A1899", true) == 0 | Operators.CompareString(model, "A1897", true) == 0 | Operators.CompareString(model, "A1865", true) == 0 | Operators.CompareString(model, "A1902", true) == 0 | Operators.CompareString(model, "A1901", true) == 0)
            {
            }
            try
            {
                foreach (KeyValuePair<string, string> keyValuePair in dictionary)
                {
                    if (Operators.CompareString(keyValuePair.Value, color.Trim(), true) == 0)
                    {
                        this.cboColor.SelectedIndex = this.cboColor.FindStringExact(keyValuePair.Key);
                        break;
                    }
                }
            }
            finally
            {
                Dictionary<string, string>.Enumerator enumerator = dictionary.GetEnumerator();
                enumerator.Dispose();
            }
        }
        public List<string> makeHex(string input)
        {
            List<string> list = new List<string>();
            string[] array = input.Split(new char[]
            {
                ' '
            });
            checked
            {
                foreach (string text in array)
                {
                    string text2 = text.Replace("0x", "");
                    while (text2.Count<char>() != 0)
                    {
                        string item = text2.Substring(text2.Length - 2);
                        if (text2.Count<char>() < 2)
                        {
                            break;
                        }
                        text2 = text2.Substring(0, text2.Length - 2);
                        list.Add(item);
                    }
                }
                return list;
            }
        }
        public void set_cmd_key_to_text(string key, string value)
        {
            if (Operators.CompareString(key, "SrNm", true) == 0)
            {
                value = this.removeDangerousCharsForSysCFG(value);
                this.txtSN.Text = value;
            }
            else if (Operators.CompareString(key, "Mod#", true) != 0)
            {
                if (Operators.CompareString(key, "Regn", true) != 0)
                {
                    if (Operators.CompareString(key, "DClr", true) != 0)
                    {
                        if (Operators.CompareString(key, "WMac", true) != 0)
                        {
                            if (Operators.CompareString(key, "BMac", true) == 0)
                            {
                                value = this.removeDangerousCharsForSysCFG(value);
                                this.txtBMac.Text = string.Join(":", this.makeHex(value)).Substring(0, 17);
                            }
                            else if (Operators.CompareString(key, "EMac", true) != 0)
                            {
                                if (Operators.CompareString(key, "MLB#", true) != 0)
                                {
                                    if (Operators.CompareString(key, "RMd#", true) != 0)
                                    {
                                        if (Operators.CompareString(key, "NvSn", true) != 0)
                                        {
                                            if (Operators.CompareString(key, "NSrN", true) != 0)
                                            {
                                                if (Operators.CompareString(key, "LCM#", true) == 0)
                                                {
                                                    value = this.removeDangerousCharsForSysCFG(value);
                                                    this.txtLCM.Text = value;
                                                }
                                                else if (Operators.CompareString(key, "Batt", true) != 0)
                                                {
                                                    if (Operators.CompareString(key, "BCMS", true) == 0)
                                                    {
                                                        value = this.removeDangerousCharsForSysCFG(value);
                                                        this.txtBCMS.Text = value;
                                                    }
                                                    else if (Operators.CompareString(key, "FCMS", true) != 0)
                                                    {
                                                        if (Operators.CompareString(key, "MtSN", true) == 0)
                                                        {
                                                            value = this.removeDangerousCharsForSysCFG(value);
                                                            this.txtMTSN.Text = value;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        value = this.removeDangerousCharsForSysCFG(value);
                                                        this.txtFCMS.Text = value;
                                                    }
                                                }
                                                else
                                                {
                                                    value = this.removeDangerousCharsForSysCFG(value);
                                                    this.txtBattery.Text = value;
                                                }
                                            }
                                            else
                                            {
                                                value = this.removeDangerousCharsForSysCFG(value);
                                                this.txtNSRN.Text = value;
                                            }
                                        }
                                        else
                                        {
                                            value = this.removeDangerousCharsForSysCFG(value);
                                            this.txtNVSN.Text = value;
                                        }
                                    }
                                    else
                                    {
                                        value = this.removeDangerousCharsForSysCFG(value);
                                        this.txtModel.Text = value;
                                    }
                                }
                                else
                                {
                                    value = this.removeDangerousCharsForSysCFG(value);
                                    this.txtMLB.Text = value;
                                }
                            }
                            else
                            {
                                value = this.removeDangerousCharsForSysCFG(value);
                                this.txtEMac.Text = string.Join(":", this.makeHex(value)).Substring(0, 17);
                            }
                        }
                        else
                        {
                            value = this.removeDangerousCharsForSysCFG(value);
                            this.txtWifi.Text = string.Join(":", this.makeHex(value)).Substring(0, 17);
                        }
                    }
                    else
                    {
                        value = this.removeDangerousCharsForSysCFG(value);
                        this.template_color = value;
                    }
                }
                else
                {
                    value = this.removeDangerousCharsForSysCFG(value);
                    this.txtRegn.Text = value;
                }
            }
            else
            {
                value = this.removeDangerousCharsForSysCFG(value);
                this.txtMode.Text = value;
            }
        }
        public string removeDangerousCharsForSysCFG(string input)
        {
            string pattern = "[^A-Za-z0-9\\-/_:+ ]";
            return Regex.Replace(input, pattern, "");
        }
        private void templatebox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.template_color = "";
            if (this.templatebox.SelectedIndex < 1)
            {
                this.txtSN.Text = "";
                this.txtMode.Text = "";
                this.txtRegn.Text = "";
                this.cboColor.DataSource = null;
                this.txtWifi.Text = "";
                this.txtBMac.Text = "";
                this.txtEMac.Text = "";
                this.txtMLB.Text = "";
                this.txtModel.Text = "";
                this.txtNVSN.Text = "";
                this.txtNSRN.Text = "";
                this.txtLCM.Text = "";
                this.txtBattery.Text = "";
                this.txtBCMS.Text = "";
                this.txtFCMS.Text = "";
                this.txtMTSN.Text = "";
                this.btnFlash.Enabled = false;
            }
            else
            {
                this.btnFlash.Enabled = true;
                this.loadTemplate();
            }
        }
        public bool confirm(string question, string title)
        {
            int num = (int)MessageBox.Show(this, question, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return num == 6;
        }
        private void btnFlash_Click(object sender, EventArgs e)
        {
            if (this.templatebox.SelectedIndex < 0)
            {
                MessageBox.Show(this, "No template selected.", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.confirm("Please make sure that the selected SysCFG-Backup is valid for your device. All existing data will be overwritten!\r\n\r\nTHE DEVELOPER OF THIS SOFTWARE DOES NOT LIABILITY ON POSIBLE PERMANENT INJURY.\r\n\r\nCONTINUE IF YOU ACCEPT THE RISK", "WARNING"))
            {
                Thread thread = new Thread(delegate ()
                {
                    this.proceed_syscfg_flash_template(this.templatebox.SelectedItem.ToString());
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start();
            }
        }


        public void showProgress(string progress_label = "Please wait...")
        {
            this.progressBar1.Value = 0;
            this.progressBar1.Text = Conversions.ToString(this.progressBar1.Value) + "%";
            this.ProgressLabel.Text = progress_label;
        }
        public void proceed_syscfg_flash_template(string template_name)
        {
            this.showProgress("Please wait...");
            checked
            {
                try
                {
                    string path = Environment.CurrentDirectory + "\\templates\\" + template_name + ".ht";
                    if (!File.Exists(path))
                    {
                        throw new Exception("Template " + template_name + " not found!");
                    }
                    MemoryStream memoryStream = Class5.smethod_3(path);
                    MemoryStream memoryStream2 = Class5.OwiicddWE(memoryStream, "!H@SNI|@293068!", (Class5.Enum0)2, null);
                    string @string = Encoding.UTF8.GetString(memoryStream2.ToArray());
                    string[] array = @string.Split(new char[]
                    {
                        '\n'
                    });
                    int num = 1;
                    foreach (string text in array)
                    {
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            string str = "";
                            try
                            {
                                str = devinfo.smethod_3(text, "syscfg add (.*?) ", RegexOptions.ExplicitCapture)[0].Value.Replace("syscfg add ", "");
                                goto IL_15E;
                            }
                            catch (Exception ex)
                            {
                                str = text;
                                goto IL_15E;
                            }
                        IL_EB:
                            int val = (int)Math.Round(unchecked((double)num / (double)array.Length * 100.0));
                            this.setProgress(val, "Flashing " + str + "...");
                            this.com_write_chunk(text, 250);
                            this.waitForComResponse(10000);
                            this.clean_output_string(this.string_1);
                            this.string_1 = null;
                            goto IL_14D;
                        IL_15E:
                            if (text.Contains("syscfg") | text.Contains("rtc --set"))
                            {
                                goto IL_EB;
                            }
                        }
                    IL_14D:
                        num++;
                    }
                    this.btnReadSysCFG.PerformClick();
                    MessageBox.Show(this, "SysCFG Successfully flashed!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(this, ex2.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                this.hideProgress();
            }
        }
        public string clean_output_string(string raw)
        {
            string text = "";
            checked
            {
                try
                {
                    string[] array = raw.Split(new char[]
                    {
                        '\r'
                    });
                    string text2 = string.Join("\r\n", array, 1, array.Length - 1);
                    string[] array2 = text2.Split(new char[]
                    {
                        '\n'
                    });
                    text = string.Join("\r\n", array2, 0, array2.Length - 1);
                }
                catch (Exception ex)
                {
                    text = raw;
                }
                if (!string.IsNullOrWhiteSpace(text))
                {
                    text.Trim();
                }
                return text;
            }
        }
        public void waitForComResponse(int timeout = 10000)
        {
            DateTime t = DateAndTime.Now.AddMilliseconds((double)timeout);
            bool flag = false;
            while (Information.IsNothing(this.string_1) & !flag)
            {
                DateTime now = DateAndTime.Now;
                if (DateTime.Compare(now, t) > 0)
                {
                    flag = true;
                }
                Application.DoEvents();
            }
        }
        public void com_write_chunk(string cmd, int chunk_size = 250)
        {
            bool flag = false;
            checked
            {
                int num = cmd.Length - 1;
                int num2 = 0;
                while ((chunk_size >> 31 ^ num2) <= (chunk_size >> 31 ^ num))
                {
                    int num3 = num2 + chunk_size;
                    string cmd2;
                    if (num3 <= cmd.Length)
                    {
                        cmd2 = cmd.Substring(num2, chunk_size);
                        if (num3 == cmd.Length)
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        int num4 = cmd.Length - num2;
                        cmd2 = cmd.Substring(cmd.Length - num4);
                        flag = true;
                    }
                    if (flag)
                    {
                        this.com_writer(cmd2, true);
                    }
                    else
                    {
                        this.com_writer(cmd2, false);
                        this.string_1 = null;
                    }
                    num2 += chunk_size;
                }
            }
        }
        private SerialPort serialPort_0;

        private string string_1;

        public void com_writer(string cmd, bool write_newline = true)
        {
            if (this.serialPort_0.IsOpen)
            {
                this.string_1 = null;
                if (!write_newline)
                {
                    this.serialPort_0.Write(cmd);
                }
                else
                {
                    this.serialPort_0.WriteLine(cmd);
                }
                return;
            }
            throw new Exception("Device is not connected.");
        }
        public void hideProgress()
        {
            this.ProgressLabel.Text = "";
            this.progressBar1.Value = 0;
            this.progressBar1.Text = Conversions.ToString(this.progressBar1.Value) + "%";
        }
        public void setProgress(int val, string progress_label = "Please wait...")
        {
            this.progressBar1.Value = val;
            this.progressBar1.Text = Conversions.ToString(this.progressBar1.Value) + "%";
            this.ProgressLabel.Text = progress_label;
        }
        public bool com_disconnect()
        {
            this.serialPort_0.Close();
            this.serialPort_0.DataReceived -= this.serial_data_received;
            this.string_1 = null;
            return !this.serialPort_0.IsOpen;
        }
        public void clearAll()
        {
            this.txtSN.Text = "";
            this.txtMode.Text = "";
            this.txtRegn.Text = "";
            this.cboColor.DataSource = null;
            this.txtWifi.Text = "";
            this.txtBMac.Text = "";
            this.txtEMac.Text = "";
            this.txtMLB.Text = "";
            this.txtModel.Text = "";
            this.txtNVSN.Text = "";
            this.txtNSRN.Text = "";
            this.txtLCM.Text = "";
            this.txtBattery.Text = "";
            this.txtBCMS.Text = "";
            this.txtFCMS.Text = "";
            this.txtMTSN.Text = "";
            this.lblNandSize.Text = "-";
            this.lblModel.Text = "-";
            this.lblMode.Text = "-";
            this.lblECID.Text = "-";
            this.load_templates();
        }
        public void serial_data_received(object sender, SerialDataReceivedEventArgs e)
        {
            if (this.serialPort_0.IsOpen)
            {
                try
                {
                    this.string_1 = this.serialPort_0.ReadTo(":-)");
                }
                catch (Exception ex)
                {
                }
            }
        }
        public string method_20()
        {
            string text = "";
            this.com_writer("chipid", true);
            this.waitForComResponse(10000);
            try
            {
                string text2 = this.clean_output_string(this.string_1);
                text = text2;
                this.string_1 = null;
                text = devinfo.smethod_3(text, "ECID    : (.*?)\\s", RegexOptions.ExplicitCapture)[0].Value.ToString().Replace("ECID    : ", "").Replace("0x", "").Trim();
            }
            catch (Exception ex)
            {
            }
            devinfo.string_7 = text;
            return text;
        }
        public bool com_connect(string port)
        {
            bool result = false;
            try
            {
                this.com_disconnect();
                this.serialPort_0 = new SerialPort();
                this.serialPort_0.DataReceived += this.serial_data_received;
                this.serialPort_0.PortName = port;
                this.serialPort_0.BaudRate = 115200;
                this.serialPort_0.Parity = Parity.None;
                this.serialPort_0.DataBits = 8;
                this.serialPort_0.StopBits = StopBits.One;
                this.serialPort_0.Open();
                this.serialPort_0.DiscardInBuffer();
                this.serialPort_0.DiscardOutBuffer();
                this.method_20();
                if (string.IsNullOrEmpty(devinfo.string_7))
                {
                    result = false;
                    this.com_disconnect();
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        private void ButtonX3_Click(object sender, EventArgs e)
        {
            if (Operators.CompareString(this.Button_X3.Text, "Connect", true) != 0)
            {
                if (this.com_disconnect())
                {
                    this.btnWriteAll.Enabled = false;
                    this.Button_X3.Text = "Connect";
                    this.clearAll();
                    this.initialize_mode();
                }
                else
                {
                    MessageBox.Show(this, "Unable to disconnect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else if (this.ComboBoxEx1.SelectedIndex > 0)
            {
                this.Button_X3.Text = "Connecting...";
                this.Button_X3.Enabled = false;
                if (this.com_connect(this.ComboBoxEx1.SelectedItem.ToString()))
                {
                    this.btnWriteAll.Enabled = true;
                    //this.panBasicRepair.Enabled = true;
                    //this.panAdvanceRepair.Enabled = true;
                    this.Button_X3.Text = "Disconnect";
                    this.Button_X3.Enabled = true;
                    this.btnReadSysCFG.PerformClick();
                }
                else
                {
                    this.btnWriteAll.Enabled = false;
                    //this.panBasicRepair.Enabled = false;
                    // this.panAdvanceRepair.Enabled = false;
                    this.Button_X3.Enabled = true;
                    this.Button_X3.Text = "Connect";
                    MessageBox.Show(this, "Unable to connect in " + this.ComboBoxEx1.SelectedItem.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                MessageBox.Show(this, "Please select com port", "No Serial Port Selected", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnReadSysCFG_Click(object sender, EventArgs e)
        {
            this.btnReadSysCFG.Enabled = false;
            this.btnReadSysCFG.Text = "Reading SysCFG...";
            try
            {
                this.txtSN.Text = this.getSN();
                this.txtMode.Text = this.getMode();
                this.txtRegn.Text = this.getRgn();
                this.cboColor.Text = this.getDClr();
                this.txtWifi.Text = this.getWMac();
                this.txtBMac.Text = this.getBMac();
                this.txtEMac.Text = this.getEMac();
                this.txtMLB.Text = this.getMLB();
                this.txtModel.Text = this.getModel();
                this.initialize_colors(this.model, this.color);
                this.txtNVSN.Text = this.getNvSn();
                this.txtNSRN.Text = this.getNSrN();
                this.txtLCM.Text = this.getLCM();
                this.txtBattery.Text = this.getBatt();
                this.txtBCMS.Text = this.getBCMS();
                this.txtFCMS.Text = this.getFCMS();
                this.txtMTSN.Text = this.getMtSN();
                this.lblNandSize.Text = Conversions.ToString(this.method_21());
                //this.lblModel.Text = this.getModelInfo(this.model).ToUpper();
                this.lblMode.Text = "ENGINEERING MODE";
                this.lblECID.Text = devinfo.string_7.ToUpper();
                this.initialize_mode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "ERROR", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            this.btnReadSysCFG.Enabled = true;
            this.btnReadSysCFG.Text = "Read SysCFG";
        }
        public string getSN()
        {
            string text = "";
            this.com_writer("syscfg print SrNm", true);
            this.waitForComResponse(10000);
            if (!Information.IsNothing(this.string_1))
            {
                string text2 = this.clean_output_string(this.string_1);
                if (text2.Contains("Serial:"))
                {
                    text = text2.Replace("Serial:", "").Trim();
                }
                else if (text2.ToLower().Contains("not found"))
                {
                    text = "Not Found";
                }
                this.string_1 = null;
            }
            this.sn = text;
            if (text.ToLower().Contains("not found"))
            {
                text = "";
            }
            return text;
        }
        public string sn;

        public string getMode()
        {
            this.com_writer("syscfg print Mod#", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string regn;
        public string getRgn()
        {
            this.com_writer("syscfg print Regn", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.regn = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string color;
        public string getDClr()
        {
            this.com_writer("syscfg print DClr", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.color = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string wifi;
        public string getWMac()
        {
            string text = "";
            this.com_writer("syscfg print WMac", true);
            this.waitForComResponse(10000);
            string input = this.clean_output_string(this.string_1);
            try
            {
                text = string.Join(":", this.makeHex(input)).Substring(0, 17);
            }
            catch (Exception ex)
            {
            }
            this.string_1 = null;
            this.wifi = text;
            if (text.ToLower().Contains("not found"))
            {
                text = "";
            }
            return text;
        }
        public string bmac;
        public string getBMac()
        {
            string text = "";
            this.com_writer("syscfg print BMac", true);
            this.waitForComResponse(10000);
            string input = this.clean_output_string(this.string_1);
            try
            {
                text = string.Join(":", this.makeHex(input)).Substring(0, 17);
            }
            catch (Exception ex)
            {
            }
            this.string_1 = null;
            this.bmac = text;
            if (text.ToLower().Contains("not found"))
            {
                text = "";
            }
            return text;
        }
        public string emac;
        public string getEMac()
        {
            string text = "";
            this.com_writer("syscfg print EMac", true);
            this.waitForComResponse(10000);
            string input = this.clean_output_string(this.string_1);
            try
            {
                text = string.Join(":", this.makeHex(input)).Substring(0, 17);
            }
            catch (Exception ex)
            {
            }
            this.string_1 = null;
            this.emac = text;
            if (text.ToLower().Contains("not found"))
            {
                text = "";
            }
            return text;
        }
        public string mlb;
        public string getMLB()
        {
            this.com_writer("syscfg print MLB#", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.mlb = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string getModel()
        {
            this.com_writer("syscfg print RMd#", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.model = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string nvsn;
        public string getNvSn()
        {
            this.com_writer("syscfg print NvSn", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.nvsn = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string nsrn;
        public string getNSrN()
        {
            this.com_writer("syscfg print NSrN", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.nsrn = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string lcm;
        public string getLCM()
        {
            this.com_writer("syscfg print LCM#", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.lcm = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string battery;
        public string getBatt()
        {
            this.com_writer("syscfg print Batt", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.battery = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string bcms;
        public string getBCMS()
        {
            this.com_writer("syscfg print BCMS", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.bcms = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string getFCMS()
        {
            this.com_writer("syscfg print FCMS", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string mtsn;
        public string getMtSN()
        {
            this.com_writer("syscfg print MtSN", true);
            this.waitForComResponse(10000);
            string text = this.clean_output_string(this.string_1);
            string text2 = text;
            this.string_1 = null;
            this.mtsn = text2;
            if (text2.ToLower().Contains("not found"))
            {
                text2 = "";
            }
            return text2;
        }
        public string model_name;
        public string getModelInfo(string find_value)
        {
            string result = "";
            JArray jarray = JArray.Parse(Class6.string_0);
            try
            {
                foreach (JToken jtoken in jarray)
                {
                    if (jtoken["ANumber"].ToString().Contains(find_value))
                    {
                        result = jtoken["name"].ToString();
                        this.model_name = result;
                        break;
                    }
                }
            }
            finally
            {
                IEnumerator<JToken> enumerator = null;
                if (enumerator != null)
                {
                    enumerator.Dispose();
                }
            }
            return result;
        }

        public object method_21()
        {
            this.com_writer("nandsize", true);
            this.waitForComResponse(10000);
            string input = this.clean_output_string(this.string_1);
            this.string_1 = null;
            return this.parseNANDSize(input);
        }
        public string parseNANDSize(string input)
        {
            input = input.Replace("NAND SIZE :", "").Trim();
            ulong value = (ulong)Conversions.ToUInteger("&H" + input);
            string str = Convert.ToInt32(decimal.Divide(decimal.Multiply(new decimal(value), 1024m), 1000000000m)).ToString();
            return str + "GB";
        }

        private void btnBackupSysCFG_Click(object sender, EventArgs e)
        {
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = string.Concat(new string[]
                {
                this.model_name,
                "_",
                this.model,
                "_",
                this.sn,
                ".ht"
                });
                sfd.Filter = "Hasnitech File (*.ht*)|*.ht";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Thread thread = new Thread(delegate ()
                    {
                        this.startSysCFGBackup(sfd.FileName);
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }

        public void startSysCFGBackup(string output_file)
        {
            this.showProgress("Prepairing to backup SysCFG...");
            checked
            {
                try
                {
                    this.com_writer("syscfg list", true);
                    this.waitForComResponse(10000);
                    string text = this.clean_output_string(this.string_1);
                    text = text.Replace("Key:", "syscfg add");
                    text = text.Replace("Value: ", "");
                    StringBuilder stringBuilder = new StringBuilder();
                    string[] array = text.Split(new char[]
                    {
                        '\r'
                    });
                    int num = 1;
                    string str = "";
                    foreach (string text2 in array)
                    {
                        if (!text2.Contains("Not Found"))
                        {
                            stringBuilder.Append(text2);
                        }
                        try
                        {
                            str = devinfo.smethod_3(text2, "syscfg add (.*?) ", RegexOptions.ExplicitCapture)[0].Value.Replace("syscfg add ", "");
                        }
                        catch (Exception ex)
                        {
                        }
                        int val = (int)Math.Round(unchecked((double)num / (double)array.Count<string>() * 100.0));
                        this.setProgress(val, "Generating " + str + " backup...");
                        num++;
                    }
                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
                    Class5.smethod_2(memoryStream, "!H@SNI|@293068!", output_file, (Class5.Enum0)1, null);
                    this.string_1 = null;
                    this.setProgress(100, "SysCFG Successfully backup!");
                    MessageBox.Show(this, "SysCFG Successfully backup!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(this, ex2.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                this.hideProgress();
            }
        }

        private void btnRestoreSysCFG_Click(object sender, EventArgs e)
        {
            if (this.confirm("Please make sure that the selected SysCFG-Backup is valid for your device. All existing data will be overwritten!\r\n\r\nTHE DEVELOPER OF THIS SOFTWARE DOES NOT LIABILITY ON POSIBLE PERMANENT INJURY.\r\n\r\nCONTINUE IF YOU ACCEPT THE RISK", "WARNING"))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string path = "";
                openFileDialog.Filter = "Hasnitech File (*.ht*)|*.ht";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                    Thread thread = new Thread(delegate ()
                    {
                        this.proceed_syscfg_restore(path);
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }

        public void proceed_syscfg_restore(string backup_path)
        {
            this.showProgress("Please wait...");
            checked
            {
                try
                {
                    int num = 0;
                    int num2 = 0;
                    List<string> list = new List<string>();
                    MemoryStream memoryStream = Class5.smethod_3(backup_path);
                    MemoryStream memoryStream2 = Class5.OwiicddWE(memoryStream, "!H@SNI|@293068!", (Class5.Enum0)2, null);
                    string @string = Encoding.UTF8.GetString(memoryStream2.ToArray());
                    string[] array = @string.Split(new char[]
                    {
                        '\n'
                    });
                    int num3 = 1;
                    foreach (string text in array)
                    {
                        string text2 = devinfo.smethod_3(text, "syscfg add (.*?) ", RegexOptions.ExplicitCapture)[0].Value.Replace("syscfg add ", "");
                        int val = (int)Math.Round(unchecked((double)num3 / (double)array.Length * 100.0));
                        this.setProgress(val, "Restoring " + text2 + "...");
                        this.com_write_chunk(text, 250);
                        this.waitForComResponse(10000);
                        string text3 = this.clean_output_string(this.string_1);
                        this.string_1 = null;
                        if (!text3.ToLower().Contains("finish"))
                        {
                            list.Add(text2);
                            num2++;
                            this.setProgress(val, text2 + " fail to restore.");
                        }
                        else
                        {
                            num++;
                            this.setProgress(val, text2 + " Restored.");
                        }
                        num3++;
                    }
                    this.btnReadSysCFG.PerformClick();
                    if (num2 != 0)
                    {
                        MessageBox.Show(this, string.Concat(new string[]
                        {
                            "One or more SysCFG failed to restore\r\n\r\nSysCFG Restored: ",
                            num.ToString(),
                            "\r\nSysCFG Failed: ",
                            Conversions.ToString(num2),
                            "\r\n\r\nSysCFG Failed Keys\r\n\r\n",
                            string.Join(", ", list)
                        }), "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show(this, "All SysCFG Successfully Restored!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                this.hideProgress();
            }
        }

        private void ButtonX34_Click(object sender, EventArgs e)
        {
            try
            {
                this.com_writer("syscfg delete WCAL", true);
                this.waitForComResponse(10000);
                this.clean_output_string(this.string_1);
                MessageBox.Show(this, "WiFi Successfully unlocked!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.string_1 = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ButtonX32_Click(object sender, EventArgs e)
        {
            int num = (int)MessageBox.Show(this, "Click \"Yes\" if your device is A9 and up otherwise click \"No\" if your device is A5-A8", "IMPORTANT", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (num == 6)
            {
                try
                {
                    this.com_writer("syscfg add SwBh 0x00000011 0x00000000 0x00000000 0x00000000", true);
                    this.waitForComResponse(10000);
                    this.clean_output_string(this.string_1);
                    this.string_1 = null;
                    MessageBox.Show(this, "Camera fix successful!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            if (num == 7)
            {
                try
                {
                    this.com_writer("syscfg add Regn MY/A", true);
                    this.waitForComResponse(10000);
                    this.clean_output_string(this.string_1);
                    this.string_1 = null;
                    MessageBox.Show(this, "Camera fix successful!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(this, ex2.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            MessageBox.Show(this, "Operation cancelled", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void ButtonX31_Click(object sender, EventArgs e)
        {
            try
            {
                this.com_writer("reset", true);
                this.waitForComResponse(10000);
                this.string_1 = null;
                this.ButtonX3_Click(RuntimeHelpers.GetObjectValue(sender), e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ButtonX30_Click(object sender, EventArgs e)
        {
            if (this.confirm("You are about to factory reset your device. This will erase all device content and settings. This can't be undone after restarted!\r\n\r\nAre you sure that you want to continue?", "WARNING!"))
            {
                try
                {
                    this.com_writer("nvram --set oblit-inprogress 5", true);
                    this.waitForComResponse(10000);
                    this.com_writer("nvram --save", true);
                    this.waitForComResponse(10000);
                    this.com_writer("reset", true);
                    this.string_1 = null;
                    MessageBox.Show(this, "Commands successfully sent to your device! When rebooting your device, It will start to erase all content and settings from the device.\r\nKeep the device connected to a power source and wait until it booted up successfully.", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.ButtonX3_Click(RuntimeHelpers.GetObjectValue(sender), e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void ButtonX35_Click(object sender, EventArgs e)
        {
            try
            {
                this.com_writer("syscfg add SwBh 0x00000001 0x00000000 0x00000000 0x00000000", true);
                this.waitForComResponse(10000);
                this.clean_output_string(this.string_1);
                MessageBox.Show(this, "Facetime fixed!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.string_1 = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ButtonX2_Click(object sender, EventArgs e)
        {
            if (this.confirm("Please make sure that you backup SysCFG of this device for future restore and recovery. All existing data will be erased!\r\n\r\nTHE DEVELOPER OF THIS SOFTWARE DOES NOT LIABILITY ON POSIBLE PERMANENT INJURY.\r\n\r\nCONTINUE IF YOU ACCEPT THE RISK", "WARNING"))
            {
                Thread thread = new Thread(new ThreadStart(this.startSysCFGFormatNAND));
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        public void startSysCFGFormatNAND()
        {
            this.showProgress("Prepairing to format NAND...");
            checked
            {
                try
                {
                    this.com_writer("syscfg list", true);
                    this.waitForComResponse(10000);
                    string text = this.clean_output_string(this.string_1);
                    text = text.Replace("Key:", "syscfg delete");
                    text = text.Replace("Value: ", "");
                    string[] array = text.Split(new char[]
                    {
                        '\r'
                    });
                    int num = 1;
                    foreach (string text2 in array)
                    {
                        if (!text2.Contains("Not Found"))
                        {
                            string text3 = devinfo.smethod_3(text2, "syscfg delete (.*?) ", RegexOptions.ExplicitCapture)[0].Value.ToString().Trim();
                            int val = (int)Math.Round(unchecked((double)num / (double)array.Count<string>() * 100.0));
                            this.setProgress(val, "Formating " + text3.Replace("syscfg delete", "").Trim() + "...");
                            this.com_writer(text3, true);
                            this.waitForComResponse(10000);
                            this.clean_output_string(this.string_1);
                            this.string_1 = null;
                        }
                        num++;
                    }
                    this.string_1 = null;
                    this.btnReadSysCFG.PerformClick();
                    this.setProgress(100, "NAND Successfully fomatted!");
                    MessageBox.Show(this, "NAND Successfully fomatted!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                this.hideProgress();
            }
        }
        private object object_1;

        private string string_0;
        private void method_12(object sender, Downloader.DownloadProgressChangedEventArgs e)
        {
            this.object_1 = true;
            this.ProgressLabel.Text = "Downloading Resources (" + Conversions.ToString(Math.Ceiling(e.ProgressPercentage)) + "%)...";
        }
        private void method_13(object sender, AsyncCompletedEventArgs e)
        {
            this.object_1 = false;
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    this.string_0 = null;
                }
                else
                {
                    this.string_0 = e.Error.Message;
                }
            }
            else
            {
                this.string_0 = "Cancelled";
            }
        }
        private void method_1(Action action_0)
        {
            object obj = this.object_0;
            ObjectFlowControl.CheckForSyncLockOnValueType(obj);
            lock (obj)
            {
                if (base.InvokeRequired)
                {
                    try
                    {
                        base.Invoke(new MethodInvoker(delegate ()
                        {
                            action_0();
                        }));
                        goto IL_63;
                    }
                    catch (Exception ex)
                    {
                        goto IL_63;
                    }
                }
                action_0();
            IL_63:;
            }
        }
        private void ButtonX36_Click(object sender, EventArgs e)
        {
            try
            {
                this.showProgress("Please wait...");
                this.setProgress(10, "Reading device information...");
                devinfo.smethod_20();
                if (string.IsNullOrEmpty(devinfo.string_15) || string.IsNullOrEmpty(devinfo.string_11))
                {
                    throw new Exception("Unable to read device information!");
                }
                string string_ = devinfo.string_11;
                string string_2 = devinfo.string_16;
                string string_3 = devinfo.string_8;
                backgroundWorker1.RunWorkerAsync();
                this.setProgress(80, "Booted...");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            this.setProgress(100, "Done....");
            this.hideProgress();
        }
        Magic decrypt = new Magic();

        public void runiPwndfu(int timeout = 30000)
        {
            this.get_device_info = false;
            this.setProgress(10, "Checking connected DFU Device...");
            if (!devinfo.smethod_26())
            {
                throw new Exception("Please Enter Your Device in DFU Mode.");
            }
            this.setProgress(20, "Installing usb drivers...");
            this.method_1(() => {
                devinfo.smethod_25();
                devinfo.smethod_22();
            });
            devinfo.smethod_0(5000);
            this.setProgress(30, "Sending exploit to device...");
            string text = "";
            string text2 = "";
            DateTime t = DateAndTime.Now.AddMilliseconds((double)timeout);
            bool flag = false;
            DateTime t2;
            while (!text.Contains("Now you can boot untrusted images.") & !flag)
            {
                this.setProgress(40, "Sending exploit2 to device...");
                Class7.NqAmkMkovw(timeout, text, text2);
                this.setProgress(50, "Sending exploit3 to device...");
                this.method_1(() => {
                    try
                    {
                        ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,Name,Service FROM Win32_PnPEntity where DeviceID Like '%ECID%'");
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
                                Conversions.ToString(NewLateBinding.LateIndexGet(managementObjectSearcher.Get().Cast<object>().ElementAtOrDefault(0), new object[]
                                {
                            "Service"
                                }, null));
                                devinfo.smethod_12(string_, true, true);
                                flag = true;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        managementObjectSearcher.Dispose();
                    }
                    catch (ManagementException ex2)
                    {
                    }
                    if (!flag)
                    {
                        devinfo.smethod_12("", true, true);
                    }
                });
                this.setProgress(70, "Installing DFU Driver");
                devinfo.smethod_0(5000);
                t2 = DateAndTime.Now;
                if (DateTime.Compare(t2, t) > 0)
                {
                    flag = true;
                }
                Application.DoEvents();
            }
            this.setProgress(80, "Installed");
            t2 = DateTime.MinValue;
            t = DateTime.MinValue;
            this.get_device_info = true;
            if (label30.Text == null)
            {
                MessageBox.Show("Unable to exploit device \nPlease re-enter DFU Mode.","ERROR");
            }
            else if (label30?.Text != null)
            {
                this.setProgress(100, "Device has been successfully PWNED.");
            }
            hideProgress();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
        }

        public void startCOMRepair()
        {
            this.guna2Button1.Text = "Repairing COM Driver...";
            devinfo.smethod_27();
            devinfo.smethod_14(10000);
            string text = "";
            string str = "";
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            string str2 = Environment.CurrentDirectory + "\\drivers\\usb\\cdc_serial\\Diags_CDC_Serial.inf";
            string text2 = folderPath + "\\inf\\usbser.inf";
            string str3 = Environment.CurrentDirectory + "\\drivers\\usb\\serial\\inf\\usbser.inf";
            int num = 0;
            checked
            {
                if (!File.Exists(text2))
                {
                    devinfo.smethod_8("pnputil -i -a \"" + str3 + "\"", ref text, ref str);
                    text += str;
                    if (text.Contains("added successfully"))
                    {
                        num++;
                        goto IL_1A1;
                    }
                    try
                    {
                        string[] array = text.Split(new char[]
                        {
                            ':'
                        });
                        int num2 = Conversions.ToInteger(array[array.Count<string>() - 1].ToString().Trim());
                        if (num2 >= 1)
                        {
                            num++;
                        }
                        goto IL_1A1;
                    }
                    catch (Exception ex)
                    {
                        goto IL_1A1;
                    }
                }
                devinfo.smethod_8("pnputil -i -a \"" + text2 + "\"", ref text, ref str);
                text += str;
                if (!text.Contains("added successfully"))
                {
                    try
                    {
                        string[] array2 = text.Split(new char[]
                        {
                            ':'
                        });
                        int num3 = Conversions.ToInteger(array2[array2.Count<string>() - 1].ToString().Trim());
                        if (num3 >= 1)
                        {
                            num++;
                        }
                        goto IL_1A1;
                    }
                    catch (Exception ex2)
                    {
                        goto IL_1A1;
                    }
                }
                num++;
            IL_1A1:
                devinfo.smethod_8("pnputil -i -a \"" + str2 + "\"", ref text, ref str);
                text += str;
                if (text.Contains("added successfully"))
                {
                    num++;
                }
                else
                {
                    try
                    {
                        string[] array3 = text.Split(new char[]
                        {
                            ':'
                        });
                        int num4 = Conversions.ToInteger(array3[array3.Count<string>() - 1].ToString().Trim());
                        if (num4 >= 1)
                        {
                            num++;
                        }
                    }
                    catch (Exception ex3)
                    {
                    }
                }
                if (num >= 1)
                {
                    devinfo.smethod_8("pnputil /scan-devices", ref text, ref str);
                    MessageBox.Show(this, "USB Serial Port Driver has been repaired!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show(this, "Failed to Repair USB Serial Port, Please Disable Driver Signature First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                this.guna2Button1.Text = "Repaired COM Driver";
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.guna2Button2.Enabled = false;
            this.ComboBoxEx1.Enabled = false;
            this.loadSerialPort();
            this.ComboBoxEx1.Enabled = true;
            this.guna2Button2.Enabled = true;
        }

        private void ComboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button43_Click(object sender, EventArgs e)
        {
            this.runiPwndfu();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (label30.Text == "GASTER")
            {
                if (checkBox12.Checked)
                {
                    ICHpurpleboot_ purple = new ICHpurpleboot_();
                    purple.purple_device2();
                }
                else
                {
                    ICHpurpleboot_ purple = new ICHpurpleboot_();
                    purple.purple_device();
                }
            }
            else
            {
                MessageBox.Show("Failed. iDevice isn't in IPWNDFU", "ERROR");
            }
        }
        private void CheckAllCheckBoxes()
        {
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = true;
                    guna2Button25.Text = "Deselect ALL";
                }
            }
        }
        private void UnCheckAllCheckBoxes()
        {
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = false;
                    guna2Button25.Text = "Select ALL";
                }
            }
        }
        private void guna2Button25_Click(object sender, EventArgs e)
        {
            if (guna2Button25.Text == "Select ALL")
            {
                CheckAllCheckBoxes();
            }
            else
            {
                UnCheckAllCheckBoxes();
            }
        }

        private void guna2Button24_Click(object sender, EventArgs e)
        {
            this.txtSN.Text = "";
            this.txtMode.Text = "";
            this.txtRegn.Text = "";
            this.cboColor.DataSource = null;
            this.txtWifi.Text = "";
            this.txtBMac.Text = "";
            this.txtEMac.Text = "";
            this.txtMLB.Text = "";
            this.txtModel.Text = "";
            this.txtNVSN.Text = "";
            this.txtNSRN.Text = "";
            this.txtLCM.Text = "";
            this.txtBattery.Text = "";
            this.txtBCMS.Text = "";
            this.txtFCMS.Text = "";
            this.txtMTSN.Text = "";
            this.lblNandSize.Text = "-";
            this.lblModel.Text = "-";
            this.lblMode.Text = "-";
            this.lblECID.Text = "-";
            this.load_templates();
        }

        private void btnWriteAll_Click(object sender, EventArgs e)
        {
            if (this.confirm("Please make sure that you backup SysCFG of this device for future restore and recovery. All existing data will be overwritten!\r\n\r\nTHE DEVELOPER OF THIS SOFTWARE DOES NOT LIABILITY ON POSIBLE PERMANENT INJURY.\r\n\r\nCONTINUE IF YOU ACCEPT THE RISK", "WARNING"))
            {
                Thread thread = new Thread(new ThreadStart(this.startWriteAllSelectedSysCFG));
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start();
            }
        }
        public void startWriteAllSelectedSysCFG()
        {
            this.showProgress("Preparing to write SysCFG...");
            List<string> list = new List<string>();
            if (this.chkSN.Checked)
            {
                list.Add(Conversions.ToString(this.chkSN.Tag));
            }
            if (this.chkMode.Checked)
            {
                list.Add(Conversions.ToString(this.chkMode.Tag));
            }
            if (this.chkRegn.Checked)
            {
                list.Add(Conversions.ToString(this.chkRegn.Tag));
            }
            if (this.chkColor.Checked & this.cboColor.SelectedIndex >= 0)
            {
                list.Add(Conversions.ToString(this.chkColor.Tag));
            }
            if (this.chkWifi.Checked)
            {
                if (!Conversions.ToBoolean(devinfo.smethod_30(this.txtWifi.Text.Trim())))
                {
                    MessageBox.Show(this, "Invalid Wifi Mac format should be XX:XX:XX:XX:XX:XX, Wifi Mac will be skip in writing.", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    list.Add(Conversions.ToString(this.chkWifi.Tag));
                }
            }
            if (this.chkBMac.Checked)
            {
                if (!Conversions.ToBoolean(devinfo.smethod_30(this.txtBMac.Text.Trim())))
                {
                    MessageBox.Show(this, "Invalid BMac format should be XX:XX:XX:XX:XX:XX, BMac will be skip in writing.", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    list.Add(Conversions.ToString(this.chkBMac.Tag));
                }
            }
            if (this.chkEmac.Checked)
            {
                if (Conversions.ToBoolean(devinfo.smethod_30(this.txtEMac.Text.Trim())))
                {
                    list.Add(Conversions.ToString(this.chkEmac.Tag));
                }
                else
                {
                    MessageBox.Show(this, "Invalid EMac format should be XX:XX:XX:XX:XX:XX. EMac will be skip in writing.", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if (this.chkMLB.Checked)
            {
                list.Add(Conversions.ToString(this.chkMLB.Tag));
            }
            if (this.chkModel.Checked)
            {
                list.Add(Conversions.ToString(this.chkModel.Tag));
            }
            if (this.chkNVSN.Checked)
            {
                list.Add(Conversions.ToString(this.chkNVSN.Tag));
            }
            if (this.chkNSRN.Checked)
            {
                list.Add(Conversions.ToString(this.chkNSRN.Tag));
            }
            if (this.chkLCM.Checked)
            {
                list.Add(Conversions.ToString(this.chkLCM.Tag));
            }
            if (this.chkBattery.Checked)
            {
                list.Add(Conversions.ToString(this.chkBattery.Tag));
            }
            if (this.chkBCMS.Checked)
            {
                list.Add(Conversions.ToString(this.chkBCMS.Tag));
            }
            if (this.chkFCMS.Checked)
            {
                list.Add(Conversions.ToString(this.chkFCMS.Tag));
            }
            if (this.chkMTSN.Checked)
            {
                list.Add(Conversions.ToString(this.chkMTSN.Tag));
            }
            checked
            {
                try
                {
                    int num = 0;
                    int num2 = 0;
                    List<string> list2 = new List<string>();
                    int num3 = 1;
                    try
                    {
                        foreach (string text in list)
                        {
                            int val = (int)Math.Round(unchecked((double)num3 / (double)list.Count * 100.0));
                            this.setProgress(val, "Restoring " + text + "...");
                            this.com_writer(this.get_cmd_by_key(text), true);
                            this.waitForComResponse(10000);
                            string text2 = this.clean_output_string(this.string_1);
                            this.string_1 = null;
                            if (text2.ToLower().Contains("finish"))
                            {
                                num++;
                                this.setProgress(val, text + " Restored.");
                            }
                            else
                            {
                                list2.Add(text);
                                num2++;
                                this.setProgress(val, text + " fail to restore.");
                            }
                            num3++;
                        }
                    }
                    finally
                    {
                        List<string>.Enumerator enumerator = list2.GetEnumerator();
                        ((IDisposable)enumerator).Dispose();
                    }
                    this.btnReadSysCFG.PerformClick();
                    if (num2 == 0)
                    {
                        MessageBox.Show(this, "All SysCFG Successfully Written!", "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show(this, string.Concat(new string[]
                        {
                            "One or more SysCFG failed to write\r\n\r\nSysCFG Written: ",
                            num.ToString(),
                            "\r\nSysCFG Failed: ",
                            Conversions.ToString(num2),
                            "\r\n\r\nSysCFG Failed Keys\r\n\r\n",
                            string.Join(", ", list2)
                        }), "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "HASNIT3CH | CFG", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                this.hideProgress();
            }
        }
        public string get_cmd_by_key(string key)
        {
            string result = "";
            if (Operators.CompareString(key, "SrNm", true) == 0)
            {
                string text = this.txtSN.Text.Trim();
                text = this.removeDangerousCharsForSysCFG(text);
                result = "syscfg add " + key + " " + text;
            }
            else if (Operators.CompareString(key, "Mod#", true) != 0)
            {
                if (Operators.CompareString(key, "Regn", true) == 0)
                {
                    string text = this.txtRegn.Text.Trim();
                    text = this.removeDangerousCharsForSysCFG(text);
                    result = "syscfg add " + key + " " + text;
                }
                else if (Operators.CompareString(key, "DClr", true) == 0)
                {
                    string text = Conversions.ToString(NewLateBinding.LateGet(this.cboColor.SelectedItem, null, "value", new object[0], null, null, null));
                    text = this.removeDangerousCharsForSysCFG(text);
                    result = "syscfg add " + key + " " + text;
                }
                else if (Operators.CompareString(key, "WMac", true) == 0)
                {
                    string text = this.parseMacToOctal(this.txtWifi.Text.Trim());
                    text = this.removeDangerousCharsForSysCFG(text);
                    result = "syscfg add " + key + " " + text;
                }
                else if (Operators.CompareString(key, "BMac", true) == 0)
                {
                    string text = this.parseMacToOctal(this.txtBMac.Text.Trim());
                    text = this.removeDangerousCharsForSysCFG(text);
                    result = "syscfg add " + key + " " + text;
                }
                else if (Operators.CompareString(key, "EMac", true) != 0)
                {
                    if (Operators.CompareString(key, "MLB#", true) == 0)
                    {
                        string text = this.txtMLB.Text.Trim();
                        text = this.removeDangerousCharsForSysCFG(text);
                        result = "syscfg add " + key + " " + text;
                    }
                    else if (Operators.CompareString(key, "RMd#", true) == 0)
                    {
                        string text = this.txtModel.Text.Trim();
                        text = this.removeDangerousCharsForSysCFG(text);
                        result = "syscfg add " + key + " " + text;
                    }
                    else if (Operators.CompareString(key, "NvSn", true) != 0)
                    {
                        if (Operators.CompareString(key, "NSrN", true) == 0)
                        {
                            string text = this.txtNSRN.Text.Trim();
                            text = this.removeDangerousCharsForSysCFG(text);
                            result = "syscfg add " + key + " " + text;
                        }
                        else if (Operators.CompareString(key, "LCM#", true) != 0)
                        {
                            if (Operators.CompareString(key, "Batt", true) == 0)
                            {
                                string text = this.txtBattery.Text.Trim();
                                text = this.removeDangerousCharsForSysCFG(text);
                                result = "syscfg add " + key + " " + text;
                            }
                            else if (Operators.CompareString(key, "BCMS", true) != 0)
                            {
                                if (Operators.CompareString(key, "FCMS", true) == 0)
                                {
                                    string text = this.txtFCMS.Text.Trim();
                                    text = this.removeDangerousCharsForSysCFG(text);
                                    result = "syscfg add " + key + " " + text;
                                }
                                else if (Operators.CompareString(key, "MtSN", true) == 0)
                                {
                                    string text = this.txtMTSN.Text.Trim();
                                    text = this.removeDangerousCharsForSysCFG(text);
                                    result = "syscfg add " + key + " " + text;
                                }
                            }
                            else
                            {
                                string text = this.txtBCMS.Text.Trim();
                                text = this.removeDangerousCharsForSysCFG(text);
                                result = "syscfg add " + key + " " + text;
                            }
                        }
                        else
                        {
                            string text = this.txtLCM.Text.Trim();
                            text = this.removeDangerousCharsForSysCFG(text);
                            result = "syscfg add " + key + " " + text;
                        }
                    }
                    else
                    {
                        string text = this.txtNVSN.Text.Trim();
                        text = this.removeDangerousCharsForSysCFG(text);
                        result = "syscfg add " + key + " " + text;
                    }
                }
                else
                {
                    string text = this.parseMacToOctal(this.txtEMac.Text.Trim());
                    text = this.removeDangerousCharsForSysCFG(text);
                    result = "syscfg add " + key + " " + text;
                }
            }
            else
            {
                string text = this.txtMode.Text.Trim();
                text = this.removeDangerousCharsForSysCFG(text);
                result = "syscfg add " + key + " " + text;
            }
            return result;
        }
        public string parseMacToOctal(string hex)
        {
            string result = "";
            try
            {
                string[] array = hex.Split(new char[]
                {
                    ':'
                });
                result = string.Concat(new string[]
                {
                    "0x",
                    array[3].ToString(),
                    array[2].ToString(),
                    array[1].ToString(),
                    array[0].ToString(),
                    " 0x0000",
                    array[5].ToString(),
                    array[4].ToString(),
                    " 0x00000000 0x00000000"
                });
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.startCOMRepair();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMax_Click(object sender, EventArgs e)
        {

        }

        private void btnMin_Click(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
