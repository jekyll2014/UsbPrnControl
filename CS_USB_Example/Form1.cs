//This document contains programming examples.
//Custom S.P.A. grants you a nonexclusive copyright license to use all programming code examples from which you can generate similar function tailored to your own specific needs.
//All sample code is provided by Custom S.P.A. for illustrative purposes only. These examples have not been thoroughly tested under all conditions. 
//Custom S.P.A., therefore, cannot guarantee or imply reliability, serviceability, or function of these programs.
//In no event shall Custom S.P.A. be liable for any direct, indirect, incidental, special, exemplary, or consequential damages (including, but not limited to, procurement of substitute goods or services; loss of use, data, or profits; or business interruption) however caused and on any theory of liability, whether in contract, strict liability, or tort 
//(including negligence or otherwise) arising in any way out of the use of this software, even if advised of the possibility of such damage.
//All programs contained herein are provided to you "as is" without any warranties of any kind. 
//The implied warranties of non-infringement, merchantability and fitness for a particular purpose are expressly disclaimed.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UsbPrnControl.Properties;

namespace UsbPrnControl
{
    public partial class Form1 : Form
    {
        private CePrinter Selected_Printer;
        private byte[] PRINTER_ANSWER = new byte[0];

        private List<CePrinter> CONNECTED_PRINTER_LIST = new List<CePrinter>();
        private Guid m_pGuid = new Guid(Settings.Default.GUID_PRINT);
        private const uint GENERIC_READ = 0x80000000;
        private const int FILE_SHARE_READ = 1;
        private const int OPEN_EXISTING = 3;
        private const int GENERIC_WRITE = 1073741824;
        private const int FILE_SHARE_WRITE = 2;
        private const int DIGCF_PRESENT = 0x00000002;
        private const int DIGCF_INTERFACEDEVICE = 0x00000010;
        private const int CR_SUCCESS = 0x00000000;
        private const int ERROR_NO_MORE_ITEMS = 0X103;

        #region SYSTEM

        #region SYSTEM_STRUCT

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public readonly Guid ClassGuid;
            public int DevInst;
            public readonly IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public readonly Guid interfaceClassGuid;
            public readonly int flags;
            private readonly IntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        #endregion

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool SetupDiEnumDeviceInterfaces(
            IntPtr deviceInfoSet,
            IntPtr deviceInfoData,
            ref Guid interfaceClassGuid,
            int memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetupDiGetClassDevs(
            ref Guid ClassGuid,
            IntPtr Enumerator,
            IntPtr hwndParent,
            uint Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiEnumDeviceInfo
        (
            IntPtr DeviceInfoSet,
            int MemberIndex,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern int CM_Get_Device_ID(
            int dnDevInst,
            char[] Buffer,
            uint BufferLen,
            int ulFlags
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern int CM_Get_Device_ID_Size(
            ref uint pulLen,
            int dnDevInst,
            int ulFlags);

        [DllImport("setupapi.dll")]
        private static extern int CM_Get_Parent(
            out int pdnDevInst,
            int dnDevInst,
            int ulFlags
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList
        (
            IntPtr DeviceInfoSet
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport(@"setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize,
            out uint requiredSize,
            IntPtr deviceInfoData
        );

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize,
            out uint requiredSize,
            IntPtr deviceInfoData
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
            string pipeName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplate);

        #endregion

        #region USB_FUNC

        /*****************************************************************
         RETURNS A LIST OF SYMBOLIC LINKS
         *****************************************************************/
        private List<string> GetSymbolycLinkName()
        {
            var Symb = new List<string>();

            var NumberDevices = 0;
            IntPtr hardwareDeviceInfo;
            var deviceInfoData = new SP_DEVICE_INTERFACE_DATA();
            var devData = IntPtr.Zero;
            var i = 0;
            var done = false;
            var outNameBuf = "";

            hardwareDeviceInfo = SetupDiGetClassDevs(ref m_pGuid, IntPtr.Zero, IntPtr.Zero,
                DIGCF_PRESENT | DIGCF_INTERFACEDEVICE);
            NumberDevices = 4;
            deviceInfoData.cbSize = Marshal.SizeOf(deviceInfoData);
            while (!done)
            {
                NumberDevices *= 2;
                for (; i < NumberDevices; i++)
                    if (SetupDiEnumDeviceInterfaces(hardwareDeviceInfo, devData, ref m_pGuid, i, ref deviceInfoData))
                    {
                        if (!OpenOneDevice(hardwareDeviceInfo, deviceInfoData, ref outNameBuf).IsInvalid)
                        {
                            Symb.Add(Convert.ToString(outNameBuf));
                            done = true;
                        }
                    }
                    else
                    {
                        if (ERROR_NO_MORE_ITEMS == Marshal.GetLastWin32Error())
                        {
                            done = true;
                            break;
                        }
                    }
            }

            NumberDevices = i;
            SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
            return Symb;
        }

        //*************************************************************
        //* Open One USB Device
        //*************************************************************
        private static SafeFileHandle OpenOneDevice(IntPtr HardwareDeviceInfo, SP_DEVICE_INTERFACE_DATA DeviceInfoData,
            ref string devName)
        {
            var functionClassDeviceData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
            var devinfo = IntPtr.Zero;
            uint predictedLength = 0;
            uint requiredLength = 0;
            SafeFileHandle hOut;
            SetupDiGetDeviceInterfaceDetail(HardwareDeviceInfo, ref DeviceInfoData, IntPtr.Zero, 0, out requiredLength,
                IntPtr.Zero);
            predictedLength = requiredLength;
            functionClassDeviceData.DevicePath = "";
            requiredLength = 0;
            if (IntPtr.Size == 8) // 64-bit
                functionClassDeviceData.cbSize = 8;
            else // 32-bit
                functionClassDeviceData.cbSize = 4 + 1;

            if (!SetupDiGetDeviceInterfaceDetail(HardwareDeviceInfo, ref DeviceInfoData, ref functionClassDeviceData,
                predictedLength, out requiredLength, IntPtr.Zero))
                if (!SetupDiGetDeviceInterfaceDetail(HardwareDeviceInfo, ref DeviceInfoData,
                    ref functionClassDeviceData, predictedLength, out requiredLength, IntPtr.Zero))
                    return new SafeFileHandle(new IntPtr(-1), true);
            devName = functionClassDeviceData.DevicePath;
            hOut = CreateFile(functionClassDeviceData.DevicePath, GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            return hOut;
        }

        /*****************************************************
        * ENMERATES DEVICES
        * & FILL THE LIST "CONNECTED_PRINTER_LIST"
        *****************************************************/
        private int GetDeviceList()
        {
            if (CONNECTED_PRINTER_LIST.Count > 0)
                CONNECTED_PRINTER_LIST.Clear();

            Enum_USB_device(ref CONNECTED_PRINTER_LIST);

            return CONNECTED_PRINTER_LIST.Count;
        }

        private void Enum_USB_device(ref List<CePrinter> Printer_USB_list)
        {
            IntPtr hDevInfoSet;
            SP_DEVINFO_DATA DeviceInfoData;
            var confRet = 0;

            //get all USB Symbolic links
            var Symb_Link = new List<string>();
            Symb_Link = GetSymbolycLinkName();

            //devs -> handle to device information set
            hDevInfoSet = SetupDiGetClassDevs(ref m_pGuid, IntPtr.Zero, IntPtr.Zero,
                DIGCF_PRESENT | DIGCF_INTERFACEDEVICE);
            for (var i = 0;; i++)
            {
                //SetupDiEnumDeviceInterfaces enumerates the device interfaces that 
                //are contained in a device information set
                var dia = new SP_DEVICE_INTERFACE_DATA();
                dia.cbSize = Marshal.SizeOf(dia);
                if (!SetupDiEnumDeviceInterfaces(hDevInfoSet, IntPtr.Zero, ref m_pGuid, i, ref dia))
                    break;

                //SetupDiEnumDeviceInfo returns a SP_DEVINFO_DATA structure
                DeviceInfoData = new SP_DEVINFO_DATA();
                DeviceInfoData.cbSize = Marshal.SizeOf(DeviceInfoData);
                if (!SetupDiEnumDeviceInfo(hDevInfoSet, i, ref DeviceInfoData))
                    break;

                uint len = 0;
                //CM_Get_Device_ID_Size returns size for CM_Get_Device_ID
                confRet = CM_Get_Device_ID_Size(ref len, DeviceInfoData.DevInst, 0);
                var t = new char[len];

                //CM_Get_Device_ID returns crah[] with the registry key path of the device
                confRet = CM_Get_Device_ID(DeviceInfoData.DevInst, t, len, 0);
                if (confRet == CR_SUCCESS)
                {
                    var str = "";
                    for (var f = 0; f < t.Length; f++)
                        str += t[f].ToString();

                    var temp_dev = new CePrinter();
                    if (str.Contains("MI"))
                    {
                        var old_devInst = DeviceInfoData.DevInst;
                        DeviceInfoData = new SP_DEVINFO_DATA();
                        DeviceInfoData.cbSize = Marshal.SizeOf(DeviceInfoData);
                        if (!SetupDiEnumDeviceInfo(hDevInfoSet, i, ref DeviceInfoData))
                            break;
                        //multi interface device
                        //get parent device
                        DeviceInfoData.DevInst = 0;
                        confRet = CM_Get_Parent(out DeviceInfoData.DevInst, old_devInst, 0);
                        confRet = CM_Get_Device_ID_Size(ref len, DeviceInfoData.DevInst, 0);
                        t = new char[len];
                        confRet = CM_Get_Device_ID(DeviceInfoData.DevInst, t, len, 0);
                        str = "";
                        for (var f = 0; f < t.Length; f++)
                            str += t[f].ToString();
                    }

                    //get USB ADDRESS NUMBER from str
                    temp_dev.USB_ADDRESS_NUMBER = str[str.Length - 1].ToString();
                    //get PID from registry key path
                    if (str.Contains("PID"))
                        for (var a = 0; a < str.Length - 6; a++)
                            try
                            {
                                if (str[a].ToString().ToUpper().Equals("P"))
                                    if (str[a + 1].ToString().ToUpper().Equals("I"))
                                        if (str[a + 2].ToString().ToUpper().Equals("D"))
                                            temp_dev.PRINTER_PID = str[a + 4] + str[a + 5].ToString() + str[a + 6] +
                                                                   str[a + 7];
                            }
                            catch
                            {
                            }

                    //assign USB symbolic link
                    for (var q = 0; q < Symb_Link.Count; q++)
                        if (Symb_Link[q].ToUpper().Contains("PID_" + temp_dev.PRINTER_PID))
                        {
                            //printer founded!!!
                            temp_dev.USB_SYMBOLIC_NAME = Symb_Link[q];
                            Symb_Link[q] = "";
                            break;
                        }

                    Printer_USB_list.Add(temp_dev);
                }
            }
        }

        #endregion

        private int SendComing;

        private TextLogger.TextLogger _logger;

        private enum DataDirection
        {
            Received,
            Sent,
            Info,
            Error
        }

        private readonly Dictionary<byte, string> _directions = new Dictionary<byte, string>
        {
            {(byte) DataDirection.Received, "<<"},
            {(byte) DataDirection.Sent, ">>"},
            {(byte) DataDirection.Info, "**"},
            {(byte) DataDirection.Error, "!!"}
        };

        public Form1()
        {
            InitializeComponent();
            RefreshUSB();
            ToolTipTerminal.SetToolTip(textBox_terminal, "Press left mouse button to read datas from USB manually");
        }

        private void Button_REFRESH_Click(object sender, EventArgs e)
        {
            RefreshUSB();
        }

        private void Button_OPEN_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < CONNECTED_PRINTER_LIST.Count; i++)
                if (CONNECTED_PRINTER_LIST[i].USB_SYMBOLIC_NAME.Equals(comboBox_Printer.Text))
                {
                    Selected_Printer = CONNECTED_PRINTER_LIST[i];
                    Selected_Printer.READ_TIMEOUT = 50;
                    Selected_Printer.WRITE_TIMEOUT = 1000;
                    if (Selected_Printer.OpenDevice())
                    {
                        timer1.Enabled = true;
                        button_Refresh.Enabled = false;
                        button_Open.Enabled = false;
                        comboBox_Printer.Enabled = false;
                        button_closeport.Enabled = true;
                        button_Send.Enabled = true;
                        checkBox_printer.Enabled = false;
                        checkBox_scanner.Enabled = false;
                        //button_sendFile.Enabled = true;
                        TextBox_fileName_TextChanged(this, EventArgs.Empty);
                    }
                    else
                    {
                        _logger.AddText("Port open failure", (byte) DataDirection.Error, DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                    }

                    return;
                }
        }

        private void Button_CLOSE_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (Selected_Printer != null)
            {
                Selected_Printer.CloseDevice();
                button_Refresh.Enabled = true;
                button_Open.Enabled = true;
                comboBox_Printer.Enabled = true;
                button_closeport.Enabled = false;
                button_Send.Enabled = false;
                button_sendFile.Enabled = false;
                checkBox_printer.Enabled = true;
                checkBox_scanner.Enabled = true;
            }
        }

        private void Button_WRITE_Click(object sender, EventArgs e)
        {
            if (Selected_Printer != null)
            {
                if (textBox_command.Text + textBox_param.Text != "")
                {
                    string outStr;
                    if (checkBox_hexCommand.Checked) outStr = textBox_command.Text;
                    else outStr = Accessory.ConvertStringToHex(textBox_command.Text);
                    if (checkBox_hexParam.Checked) outStr += textBox_param.Text;
                    else outStr += Accessory.ConvertStringToHex(textBox_param.Text);
                    if (outStr != "")
                    {
                        timer1.Enabled = false;
                        _logger.AddText(Accessory.ConvertHexToString(outStr), (byte) DataDirection.Sent, DateTime.Now);
                        textBox_command.AutoCompleteCustomSource.Add(textBox_command.Text);
                        textBox_param.AutoCompleteCustomSource.Add(textBox_param.Text);
                        if (Selected_Printer.GenericWrite(Accessory.ConvertHexToByteArray(outStr))) ReadUSB();
                        else _logger.AddText("Write failure", (byte) DataDirection.Error, DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                        timer1.Enabled = true;
                    }
                }
            }
            else
            {
                Button_CLOSE_Click(this, EventArgs.Empty);
            }
        }

        private void ReadUSB()
        {
            if (Selected_Printer != null)
            {
                if (Selected_Printer.GenericRead(ref PRINTER_ANSWER))
                    _logger.AddText(Encoding.GetEncoding(Settings.Default.CodePage).GetString(PRINTER_ANSWER),
                        (byte) DataDirection.Received, DateTime.Now);
            }
            else
            {
                Button_CLOSE_Click(this, EventArgs.Empty);
            }
        }

        private void RefreshUSB()
        {
            comboBox_Printer.Items.Clear();
            GetDeviceList();
            for (var i = 0; i < CONNECTED_PRINTER_LIST.Count; i++)
                comboBox_Printer.Items.Add(CONNECTED_PRINTER_LIST[i].USB_SYMBOLIC_NAME);
            if (CONNECTED_PRINTER_LIST.Count > 0)
            {
                comboBox_Printer.Text = CONNECTED_PRINTER_LIST[0].USB_SYMBOLIC_NAME;
                button_Open.Enabled = true;
            }
            else
            {
                //comboBox_Printer.Text = "No usb printers found";
                comboBox_Printer.Items.Add("No USB printers found");
                comboBox_Printer.SelectedIndex = 0;
                button_Open.Enabled = false;
            }
        }

        private void CheckBox_hexCommand_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_hexCommand.Checked) textBox_command.Text = Accessory.ConvertStringToHex(textBox_command.Text);
            else textBox_command.Text = Accessory.ConvertHexToString(textBox_command.Text);
        }

        private void TextBox_command_Leave(object sender, EventArgs e)
        {
            if (checkBox_hexCommand.Checked) textBox_command.Text = Accessory.CheckHexString(textBox_command.Text);
        }

        private void TextBox_param_Leave(object sender, EventArgs e)
        {
            if (checkBox_hexParam.Checked) textBox_param.Text = Accessory.CheckHexString(textBox_param.Text);
        }

        private void CheckBox_hexParam_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_hexParam.Checked) textBox_param.Text = Accessory.ConvertStringToHex(textBox_param.Text);
            else textBox_param.Text = Accessory.ConvertHexToString(textBox_param.Text);
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            _logger.Clear();
        }

        private void CheckBox_saveTo_CheckedChanged(object sender, EventArgs e)
        {
            textBox_saveTo.Enabled = !checkBox_saveInput.Checked;
            _logger.AutoSave = checkBox_saveInput.Checked;
        }

        private async void Button_sendFile_ClickAsync(object sender, EventArgs e)
        {
            if (SendComing > 0)
            {
                SendComing++;
            }
            else if (SendComing == 0)
            {
                timer1.Enabled = false;

                if (textBox_fileName.Text != "" && textBox_sendNum.Text != "" &&
                    ushort.TryParse(textBox_sendNum.Text, out var repeat) &&
                    ushort.TryParse(textBox_delay.Text, out var delay) &&
                    ushort.TryParse(textBox_strDelay.Text, out var strDelay))
                {
                    timer1.Enabled = false;
                    SendComing = 1;
                    button_Send.Enabled = false;
                    button_closeport.Enabled = false;
                    button_openFile.Enabled = false;
                    button_sendFile.Text = "Stop";
                    textBox_fileName.Enabled = false;
                    textBox_sendNum.Enabled = false;
                    textBox_delay.Enabled = false;
                    textBox_strDelay.Enabled = false;
                    for (var n = 0; n < repeat; n++)
                    {
                        var outStr = "";
                        var outErr = "";
                        long length = 0;
                        if (repeat > 1)
                            _logger.AddText(" Send cycle " + (n + 1) + "/" + repeat + ">> ", (byte) DataDirection.Info,
                                DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                        try
                        {
                            length = new FileInfo(textBox_fileName.Text).Length;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("\r\nError opening file " + textBox_fileName.Text + ": " + ex.Message);
                        }

                        //binary file read
                        if (!checkBox_hexFileOpen.Checked)
                        {
                            //byte-by-byte
                            if (radioButton_byByte.Checked)
                            {
                                var tmpBuffer = new byte[length];
                                try
                                {
                                    tmpBuffer = File.ReadAllBytes(textBox_fileName.Text);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\r\nError reading file " + textBox_fileName.Text + ": " +
                                                    ex.Message);
                                }

                                for (var l = 0; l < tmpBuffer.Length; l++)
                                {
                                    byte[] outByte = {tmpBuffer[l]};
                                    outStr = Encoding.GetEncoding(Settings.Default.CodePage).GetString(tmpBuffer);
                                    _logger.AddText(outStr, (byte) DataDirection.Sent, DateTime.Now);
                                    if (Selected_Printer.GenericWrite(outByte))
                                    {
                                        progressBar1.Value = (n * tmpBuffer.Length + l) * 100 /
                                                             (repeat * tmpBuffer.Length);
                                        if (strDelay > 0) await TaskEx.Delay(strDelay);
                                        ReadUSB();
                                    }
                                    else
                                    {
                                        _logger.AddText("Byte " + l + ": Write Failure", (byte) DataDirection.Error,
                                            DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                                    }

                                    if (SendComing > 1) l = tmpBuffer.Length;
                                }
                            }
                            //stream
                            else
                            {
                                var tmpBuffer = new byte[length];
                                try
                                {
                                    tmpBuffer = File.ReadAllBytes(textBox_fileName.Text);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\r\nError reading file " + textBox_fileName.Text + ": " +
                                                    ex.Message);
                                }

                                var l = 0;
                                while (l < tmpBuffer.Length)
                                {
                                    var bufsize = tmpBuffer.Length - l;
                                    if (bufsize > CePrinter.USB_PACK) bufsize = CePrinter.USB_PACK;
                                    var buf = new byte[bufsize];
                                    for (var i = 0; i < bufsize; i++)
                                    {
                                        buf[i] = tmpBuffer[l];
                                        l++;
                                    }

                                    var r = 0;
                                    if (Selected_Printer != null)
                                        while (r < 10 && !Selected_Printer.GenericWrite(buf))
                                        {
                                            _logger.AddText("USB write retry " + r, (byte) DataDirection.Error,
                                                DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                                            await TaskEx.Delay(100);
                                            Selected_Printer.CloseDevice();
                                            Selected_Printer.OpenDevice();
                                            r++;
                                        }

                                    if (r >= 10) outErr = "Block write failure";
                                    ReadUSB();
                                    if (checkBox_hexTerminal.Checked)
                                        outStr = Accessory.ConvertByteArrayToHex(buf, buf.Length);
                                    else outStr = Encoding.GetEncoding(Settings.Default.CodePage).GetString(buf);
                                    if (outErr != "")
                                        _logger.AddText(outErr + ": start", (byte) DataDirection.Error, DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);

                                    _logger.AddText(outStr, (byte) DataDirection.Sent, DateTime.Now);
                                    if (outErr != "")
                                        _logger.AddText(outErr + ": end", (byte) DataDirection.Error, DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                                    progressBar1.Value = (n * tmpBuffer.Length + l) * 100 / (repeat * tmpBuffer.Length);
                                    if (SendComing > 1) l = tmpBuffer.Length;
                                }
                            }
                        }
                        //hex file read
                        else
                        {
                            //String-by-string
                            if (radioButton_byString.Checked)
                            {
                                string[] tmpBuffer = { };
                                try
                                {
                                    tmpBuffer = File.ReadAllText(textBox_fileName.Text).Replace('\n', '\r')
                                        .Replace("\r\r", "\r").Split('\r');
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\r\nError reading file " + textBox_fileName.Text + ": " +
                                                    ex.Message);
                                }

                                for (var l = 0; l < tmpBuffer.Length; l++)
                                    if (tmpBuffer[l] != "")
                                    {
                                        tmpBuffer[l] = Accessory.CheckHexString(tmpBuffer[l]);
                                        _logger.AddText(outStr, (byte) DataDirection.Sent, DateTime.Now);
                                        if (Selected_Printer.GenericWrite(Accessory.ConvertHexToByteArray(tmpBuffer[l]))
                                        )
                                        {
                                            if (checkBox_hexTerminal.Checked) outStr = tmpBuffer[l];
                                            else outStr = Accessory.ConvertHexToString(tmpBuffer[l]);
                                            if (strDelay > 0) await TaskEx.Delay(strDelay);
                                            ReadUSB();
                                        }
                                        else //??????????????
                                        {
                                            outErr = "String" + l + ": Write failure";
                                        }

                                        if (SendComing > 1) l = tmpBuffer.Length;
                                        if (outErr != "")
                                            _logger.AddText(outErr, (byte) DataDirection.Error, DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                                        progressBar1.Value = (n * tmpBuffer.Length + l) * 100 /
                                                             (repeat * tmpBuffer.Length);
                                    }
                            }

                            //byte-by-byte
                            if (radioButton_byByte.Checked)
                            {
                                var tmpStrBuffer = "";
                                try
                                {
                                    tmpStrBuffer = Accessory.CheckHexString(File.ReadAllText(textBox_fileName.Text));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error reading file " + textBox_fileName.Text + ": " + ex.Message);
                                }

                                var tmpBuffer = new byte[tmpStrBuffer.Length / 3];
                                tmpBuffer = Accessory.ConvertHexToByteArray(tmpStrBuffer);
                                for (var l = 0; l < tmpBuffer.Length; l++)
                                {
                                    byte[] outByte = {tmpBuffer[l]};
                                    outStr = Encoding.GetEncoding(Settings.Default.CodePage).GetString(tmpBuffer);
                                    _logger.AddText(outStr, (byte) DataDirection.Sent, DateTime.Now);
                                    if (Selected_Printer.GenericWrite(outByte))
                                    {
                                        progressBar1.Value = (n * tmpBuffer.Length + l) * 100 /
                                                             (repeat * tmpBuffer.Length);
                                        if (strDelay > 0) await TaskEx.Delay(strDelay);
                                        ReadUSB();
                                    }
                                    else
                                    {
                                        _logger.AddText("Byte " + l + ": Write Failure", (byte) DataDirection.Error,
                                            DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                                    }

                                    if (SendComing > 1) l = tmpBuffer.Length;
                                }
                            }
                            //stream
                            else
                            {
                                var tmpStrBuffer = "";
                                try
                                {
                                    tmpStrBuffer = Accessory.CheckHexString(File.ReadAllText(textBox_fileName.Text));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error reading file " + textBox_fileName.Text + ": " + ex.Message);
                                }

                                var tmpBuffer = new byte[tmpStrBuffer.Length / 3];
                                tmpBuffer = Accessory.ConvertHexToByteArray(tmpStrBuffer);
                                var l = 0;
                                while (l < tmpBuffer.Length)
                                {
                                    var bufsize = tmpBuffer.Length - l;
                                    if (bufsize > CePrinter.USB_PACK) bufsize = CePrinter.USB_PACK;
                                    var buf = new byte[bufsize];
                                    for (var i = 0; i < bufsize; i++)
                                    {
                                        buf[i] = tmpBuffer[l];
                                        l++;
                                    }

                                    var r = 0;
                                    if (Selected_Printer != null)
                                        while (r < 10 && !Selected_Printer.GenericWrite(buf))
                                        {
                                            _logger.AddText("USB write retry " + r, (byte) DataDirection.Error,
                                                DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                                            await TaskEx.Delay(100);
                                            Selected_Printer.CloseDevice();
                                            Selected_Printer.OpenDevice();
                                            r++;
                                        }

                                    if (r >= 10) outErr = "Block write failure";
                                    ReadUSB();
                                    outStr = Encoding.GetEncoding(Settings.Default.CodePage).GetString(buf);
                                    if (outErr != "")
                                        _logger.AddText(outErr + " start", (byte) DataDirection.Error, DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                                    _logger.AddText(outStr, (byte) DataDirection.Sent, DateTime.Now);
                                    if (outErr != "")
                                        _logger.AddText(outErr + " end", (byte) DataDirection.Error, DateTime.Now, TextLogger.TextLogger.TextFormat.PlainText);
                                    progressBar1.Value = (n * tmpBuffer.Length + l) * 100 / (repeat * tmpBuffer.Length);
                                    if (SendComing > 1) l = tmpBuffer.Length;
                                }
                            }
                        }

                        if (repeat > 1) await TaskEx.Delay(delay);
                        if (SendComing > 1) n = repeat;
                    }

                    button_Send.Enabled = true;
                    button_closeport.Enabled = true;
                    button_openFile.Enabled = true;
                    button_sendFile.Text = "Send file";
                    textBox_fileName.Enabled = true;
                    textBox_sendNum.Enabled = true;
                    textBox_delay.Enabled = true;
                    textBox_strDelay.Enabled = true;
                }

                SendComing = 0;
                timer1.Enabled = true;
            }
        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox_fileName.Text = openFileDialog1.FileName;
        }

        private void Button_openFile_Click(object sender, EventArgs e)
        {
            if (checkBox_hexFileOpen.Checked)
            {
                openFileDialog1.FileName = "";
                openFileDialog1.Title = "Open file";
                openFileDialog1.DefaultExt = "txt";
                openFileDialog1.Filter = "HEX files|*.hex|Text files|*.txt|All files|*.*";
                openFileDialog1.ShowDialog();
            }
            else
            {
                openFileDialog1.FileName = "";
                openFileDialog1.Title = "Open file";
                openFileDialog1.DefaultExt = "bin";
                openFileDialog1.Filter = "BIN files|*.bin|PRN files|*.prn|All files|*.*";
                openFileDialog1.ShowDialog();
            }
        }

        private void CheckBox_hexFileOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_hexFileOpen.Checked)
            {
                radioButton_byString.Enabled = false;
                if (radioButton_byString.Checked) radioButton_byByte.Checked = true;
                checkBox_hexFileOpen.Text = "binary data";
            }
            else
            {
                radioButton_byString.Enabled = true;
                checkBox_hexFileOpen.Text = "hex text data";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.checkBox_hexCommand = checkBox_hexCommand.Checked;
            Settings.Default.textBox_command = textBox_command.Text;
            Settings.Default.checkBox_hexParam = checkBox_hexParam.Checked;
            Settings.Default.textBox_param = textBox_param.Text;
            Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox_hexCommand.Checked = Settings.Default.checkBox_hexCommand;
            textBox_command.Text = Settings.Default.textBox_command;
            checkBox_hexParam.Checked = Settings.Default.checkBox_hexParam;
            textBox_param.Text = Settings.Default.textBox_param;
            timer1.Interval = Settings.Default.USBReadInterval;

            _logger = new TextLogger.TextLogger(this)
            {
                Channels = _directions,
                FilterZeroChar = false
            };
            textBox_terminal.DataBindings.Add("Text", _logger, "Text", false, DataSourceUpdateMode.OnPropertyChanged);

            _logger.LineTimeLimit = Settings.Default.LineBreakTimeout;
            _logger.LineLimit = Settings.Default.LogLinesLimit;
            _logger.AutoSave = checkBox_saveInput.Checked;
            _logger.LogFileName = textBox_saveTo.Text;

            _logger.DefaultTextFormat = checkBox_hexTerminal.Checked
                ? TextLogger.TextLogger.TextFormat.Hex
                : TextLogger.TextLogger.TextFormat.AutoReplaceHex;

            _logger.DefaultTimeFormat =
                checkBox_saveTime.Checked
                    ? TextLogger.TextLogger.TimeFormat.LongTime
                    : TextLogger.TextLogger.TimeFormat.None;

            _logger.DefaultDateFormat =
                checkBox_saveTime.Checked
                    ? TextLogger.TextLogger.DateFormat.ShortDate
                    : TextLogger.TextLogger.DateFormat.None;

            _logger.AutoScroll = checkBox_autoscroll.Checked;

            CheckBox_autoscroll_CheckedChanged(null, EventArgs.Empty);
        }

        private void RadioButton_stream_CheckedChanged(object sender, EventArgs e)
        {
            textBox_strDelay.Enabled = !radioButton_stream.Checked;
        }

        private void TextBox_fileName_TextChanged(object sender, EventArgs e)
        {
            if (textBox_fileName.Text != "" && button_closeport.Enabled) button_sendFile.Enabled = true;
            else button_sendFile.Enabled = false;
        }

        private void CheckBox_printer_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_printer.Checked)
            {
                checkBox_scanner.Checked = false;
                m_pGuid = new Guid(Settings.Default.GUID_PRINT);
                RefreshUSB();
            }

            if (checkBox_printer.Checked == false && checkBox_scanner.Checked == false)
            {
                checkBox_scanner.Checked = true;
                m_pGuid = new Guid(Settings.Default.GUID_SCAN);
                RefreshUSB();
            }
        }

        private void CheckBox_scanner_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_scanner.Checked)
            {
                checkBox_printer.Checked = false;
                m_pGuid = new Guid(Settings.Default.GUID_SCAN);
                RefreshUSB();
            }

            if (checkBox_printer.Checked == false && checkBox_scanner.Checked == false)
            {
                checkBox_printer.Checked = true;
                m_pGuid = new Guid(Settings.Default.GUID_PRINT);
                RefreshUSB();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            ReadUSB();
        }

        private void CheckBox_autoscroll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_autoscroll.Checked)
            {
                _logger.AutoScroll = true;
                textBox_terminal.TextChanged += TextBox_terminal_TextChanged;
            }
            else
            {
                _logger.AutoScroll = false;
                textBox_terminal.TextChanged -= TextBox_terminal_TextChanged;
            }
        }

        private void TextBox_terminal_TextChanged(object sender, EventArgs e)
        {
            if (checkBox_autoscroll.Checked)
            {
                textBox_terminal.SelectionStart = textBox_terminal.Text.Length;
                textBox_terminal.ScrollToCaret();
            }
        }

        private void TextBox_saveTo_Leave(object sender, EventArgs e)
        {
            _logger.LogFileName = textBox_saveTo.Text;
        }

        private void CheckBox_hexTerminal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_hexTerminal.Checked)
                _logger.DefaultTextFormat = TextLogger.TextLogger.TextFormat.Hex;
            else
                _logger.DefaultTextFormat = TextLogger.TextLogger.TextFormat.AutoReplaceHex;
        }

        private void CheckBox_saveTime_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_saveTime.Checked)
            {
                _logger.DefaultDateFormat = TextLogger.TextLogger.DateFormat.ShortDate;
                _logger.DefaultTimeFormat = TextLogger.TextLogger.TimeFormat.LongTime;
            }
            else
            {
                _logger.DefaultDateFormat = TextLogger.TextLogger.DateFormat.None;
                _logger.DefaultTimeFormat = TextLogger.TextLogger.TimeFormat.None;
            }
        }
    }
}
