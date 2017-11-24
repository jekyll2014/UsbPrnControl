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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UsbPrnControl
{
    public partial class Form1 : Form
    {
        private CeUSBPrinter.CePrinter Selected_Printer = null;
        private byte[] PRINTER_ANSWER = new byte[0];

        public List<CeUSBPrinter.CePrinter> CONNECTED_PRINTER_LIST = new List<CeUSBPrinter.CePrinter>();
        private Guid m_pGuid = new Guid(Properties.Settings.Default.GUID_PRINT);
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
            public Guid ClassGuid;
            public int DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public Int32 cbSize;
            public Guid interfaceClassGuid;
            public Int32 flags;
            private IntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public Int32 cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String DevicePath;
        }
        #endregion

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool SetupDiEnumDeviceInterfaces(
            IntPtr deviceInfoSet,
            IntPtr deviceInfoData,
            ref Guid interfaceClassGuid,
            Int32 memberIndex,
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
           UInt32 BufferLen,
           int ulFlags
        );
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern int CM_Get_Device_ID_Size(
            ref UInt32 pulLen,
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
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            out UInt32 requiredSize,
            IntPtr deviceInfoData
        );
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(
           IntPtr hDevInfo,
           ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
           IntPtr deviceInterfaceDetailData,
           UInt32 deviceInterfaceDetailDataSize,
           out UInt32 requiredSize,
           IntPtr deviceInfoData
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
           String pipeName,
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
        private List<String> GetSymbolycLinkName()
        {
            List<String> Symb = new List<String>();

            int NumberDevices = 0;
            IntPtr hardwareDeviceInfo;
            SP_DEVICE_INTERFACE_DATA deviceInfoData = new SP_DEVICE_INTERFACE_DATA();
            IntPtr devData = IntPtr.Zero;
            int i = 0;
            bool done = false;
            String outNameBuf = "";

            hardwareDeviceInfo = SetupDiGetClassDevs(ref m_pGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_INTERFACEDEVICE);
            NumberDevices = 4;
            deviceInfoData.cbSize = Marshal.SizeOf(deviceInfoData);
            while (!done)
            {
                NumberDevices *= 2;
                for (; i < NumberDevices; i++)
                {
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
            }
            NumberDevices = i;
            SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
            return Symb;
        }
        //*************************************************************
        //* Open One USB Device
        //*************************************************************
        private static SafeFileHandle OpenOneDevice(IntPtr HardwareDeviceInfo, SP_DEVICE_INTERFACE_DATA DeviceInfoData, ref String devName)
        {
            SP_DEVICE_INTERFACE_DETAIL_DATA functionClassDeviceData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
            IntPtr devinfo = IntPtr.Zero;
            UInt32 predictedLength = 0;
            UInt32 requiredLength = 0;
            SafeFileHandle hOut;
            SetupDiGetDeviceInterfaceDetail(HardwareDeviceInfo, ref DeviceInfoData, IntPtr.Zero, 0, out requiredLength, IntPtr.Zero);
            predictedLength = requiredLength;
            functionClassDeviceData.DevicePath = "";
            requiredLength = 0;
            if (IntPtr.Size == 8) // 64-bit
            {
                functionClassDeviceData.cbSize = 8;
            }
            else // 32-bit
            {
                functionClassDeviceData.cbSize = 4 + 1;
            }

            if (!SetupDiGetDeviceInterfaceDetail(HardwareDeviceInfo, ref DeviceInfoData, ref functionClassDeviceData, predictedLength, out requiredLength, IntPtr.Zero))
            {
                if (!SetupDiGetDeviceInterfaceDetail(HardwareDeviceInfo, ref DeviceInfoData, ref functionClassDeviceData, predictedLength, out requiredLength, IntPtr.Zero))
                    return new SafeFileHandle(new IntPtr(-1), true);
            }
            devName = functionClassDeviceData.DevicePath;
            hOut = CreateFile(functionClassDeviceData.DevicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            return hOut;
        }
        /*****************************************************
        * ENMERATES DEVICES
        * & FILL THE LIST "CONNECTED_PRINTER_LIST"
        *****************************************************/
        public int GetDeviceList()
        {
            if (CONNECTED_PRINTER_LIST.Count > 0)
                CONNECTED_PRINTER_LIST.Clear();

            Enum_USB_device(ref CONNECTED_PRINTER_LIST);

            return CONNECTED_PRINTER_LIST.Count;
        }

        private void Enum_USB_device(ref List<CeUSBPrinter.CePrinter> Printer_USB_list)
        {
            IntPtr hDevInfoSet;
            SP_DEVINFO_DATA DeviceInfoData;
            int confRet = 0;

            //get all USB Symbolic links
            List<String> Symb_Link = new List<String>();
            Symb_Link = GetSymbolycLinkName();

            //devs -> handle to device information set
            hDevInfoSet = SetupDiGetClassDevs(ref m_pGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_INTERFACEDEVICE);
            for (int i = 0; ; i++)
            {
                //SetupDiEnumDeviceInterfaces enumerates the device interfaces that 
                //are contained in a device information set
                SP_DEVICE_INTERFACE_DATA dia = new SP_DEVICE_INTERFACE_DATA();
                dia.cbSize = Marshal.SizeOf(dia);
                if (!SetupDiEnumDeviceInterfaces(hDevInfoSet, IntPtr.Zero, ref m_pGuid, i, ref dia))
                    break;

                //SetupDiEnumDeviceInfo returns a SP_DEVINFO_DATA structure
                DeviceInfoData = new SP_DEVINFO_DATA();
                DeviceInfoData.cbSize = Marshal.SizeOf(DeviceInfoData);
                if (!SetupDiEnumDeviceInfo(hDevInfoSet, i, ref DeviceInfoData))
                    break;

                UInt32 len = 0;
                //CM_Get_Device_ID_Size returns size for CM_Get_Device_ID
                confRet = CM_Get_Device_ID_Size(ref len, DeviceInfoData.DevInst, 0);
                char[] t = new char[len];

                //CM_Get_Device_ID returns crah[] with the registry key path of the device
                confRet = CM_Get_Device_ID(DeviceInfoData.DevInst, t, len, 0);
                if (confRet == CR_SUCCESS)
                {
                    String str = "";
                    for (int f = 0; f < t.Length; f++)
                        str += t[f].ToString();

                    CeUSBPrinter.CePrinter temp_dev = new CeUSBPrinter.CePrinter();
                    if (str.Contains("MI"))
                    {
                        int old_devInst = DeviceInfoData.DevInst;
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
                        for (int f = 0; f < t.Length; f++)
                            str += t[f].ToString();
                    }
                    //get USB ADDRESS NUMBER from str
                    temp_dev.USB_ADDRESS_NUMBER = str[str.Length - 1].ToString();
                    //get PID from registry key path
                    if (str.Contains("PID"))
                    {
                        for (int a = 0; a < str.Length - 6; a++)
                        {
                            try
                            {
                                if (str[a].ToString().ToUpper().Equals("P"))
                                {
                                    if (str[a + 1].ToString().ToUpper().Equals("I"))
                                    {
                                        if (str[a + 2].ToString().ToUpper().Equals("D"))
                                        {
                                            temp_dev.PRINTER_PID = str[a + 4].ToString() + str[a + 5].ToString() + str[a + 6].ToString() + str[a + 7].ToString();
                                        }
                                    }
                                }
                            }
                            catch
                            { }
                        }
                    }
                    //assign USB symbolic link
                    for (int q = 0; q < Symb_Link.Count; q++)
                    {
                        if (Symb_Link[q].ToUpper().Contains("PID_" + temp_dev.PRINTER_PID))
                        {
                            //printer founded!!!
                            temp_dev.USB_SYMBOLIC_NAME = Symb_Link[q];
                            Symb_Link[q] = "";
                            break;
                        }
                    }
                    Printer_USB_list.Add(temp_dev);
                }
            }
        }
        #endregion

        int SendComing = 0;
        int txtOutState = 0;
        long oldTicks = DateTime.Now.Ticks, limitTick = 0;
        public const byte Port1DataIn = 11;
        public const byte Port1DataOut = 12;
        public const byte Port1Error = 15;

        private object threadLock = new object();
        public void collectBuffer(string tmpBuffer, int state, string time)
        {
            if (tmpBuffer != "")
            {
                lock (threadLock)
                {
                    if (!(txtOutState == state && (DateTime.Now.Ticks - oldTicks) < limitTick && state != Port1DataOut))
                    {
                        if (state == Port1DataIn)
                        {
                            tmpBuffer = "<< " + tmpBuffer;
                        }
                        else if (state == Port1DataOut)
                        {
                            tmpBuffer = ">> " + tmpBuffer;
                        }
                        else if (state == Port1Error)
                        {
                            tmpBuffer = "!! " + tmpBuffer;
                        }

                        if (checkBox_saveTime.Checked == true) tmpBuffer = time + " " + tmpBuffer;
                        tmpBuffer = "\r\n" + tmpBuffer;
                        txtOutState = state;
                    }
                    if ((checkBox_saveInput.Checked == true && state == Port1DataIn) || (checkBox_saveOutput.Checked == true && state == Port1DataOut))
                    {
                        try
                        {
                            File.AppendAllText(textBox_saveTo.Text, tmpBuffer, Encoding.GetEncoding(UsbPrnControl.Properties.Settings.Default.CodePage));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("\r\nError opening file " + textBox_saveTo.Text + ": " + ex.Message);
                        }
                    }
                    textBox_terminal.SelectionStart = textBox_terminal.TextLength;
                    textBox_terminal.SelectedText = tmpBuffer;
                    oldTicks = DateTime.Now.Ticks;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            RefreshUSB();
            ToolTipTerminal.SetToolTip(textBox_terminal, "Press left mouse button to read datas from USB manually");
        }

        private void button_REFRESH_Click(object sender, EventArgs e)
        {
            RefreshUSB();
        }

        private void button_OPEN_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < CONNECTED_PRINTER_LIST.Count; i++)
            {
                if (CONNECTED_PRINTER_LIST[i].USB_SYMBOLIC_NAME.Equals(comboBox_Printer.Text))
                {
                    Selected_Printer = CONNECTED_PRINTER_LIST[i];
                    Selected_Printer.READ_TIMEOUT = 300;
                    Selected_Printer.WRITE_TIMEOUT = 500;
                    if (Selected_Printer.OpenDevice())
                    {
                        button_Refresh.Enabled = false;
                        button_Open.Enabled = false;
                        comboBox_Printer.Enabled = false;
                        button_closeport.Enabled = true;
                        button_Send.Enabled = true;
                        checkBox_printer.Enabled = false;
                        checkBox_scanner.Enabled = false;
                        //button_sendFile.Enabled = true;
                        textBox_fileName_TextChanged(this, EventArgs.Empty);
                    }
                    else
                    {
                        collectBuffer("Port open failure", Port1Error, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                    }
                    return;
                }
            }
        }

        private void button_CLOSE_Click(object sender, EventArgs e)
        {
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

        private void button_WRITE_Click(object sender, EventArgs e)
        {
            if (Selected_Printer != null && textBox_command.Text + textBox_param.Text != "")
            {
                string outStr;
                if (checkBox_hexCommand.Checked) outStr = textBox_command.Text;
                else outStr = Accessory.ConvertStringToHex(textBox_command.Text);
                if (checkBox_hexParam.Checked) outStr += textBox_param.Text;
                else outStr += Accessory.ConvertStringToHex(textBox_param.Text);

                if (checkBox_hexTerminal.Checked) collectBuffer(outStr, Port1DataOut, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                else collectBuffer(Accessory.ConvertHexToString(outStr), Port1DataOut, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                textBox_command.AutoCompleteCustomSource.Add(textBox_command.Text);
                textBox_param.AutoCompleteCustomSource.Add(textBox_param.Text);
                if (Selected_Printer.GenericWrite(Accessory.ConvertHexToByteArray(outStr))) ReadUSB();
                else collectBuffer("Write failure", Port1Error, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
            }
        }

        private void ReadUSB()
        {
            if (Selected_Printer != null)
            {
                if (Selected_Printer.GenericRead(ref PRINTER_ANSWER))
                {
                    /*if (checkBox_saveInput.Checked)
                    {
                        if (checkBox_hexTerminal.Checked) File.AppendAllText(textBox_saveTo.Text, Accessory.ConvertByteArrayToHex(PRINTER_ANSWER, PRINTER_ANSWER.Length), Encoding.GetEncoding(UsbPrnControl.Properties.Settings.Default.CodePage));
                        else File.AppendAllText(textBox_saveTo.Text, Encoding.GetEncoding(UsbPrnControl.Properties.Settings.Default.CodePage).GetString(PRINTER_ANSWER), Encoding.GetEncoding(UsbPrnControl.Properties.Settings.Default.CodePage));
                    }*/
                    if (checkBox_hexTerminal.Checked) collectBuffer(Accessory.ConvertByteArrayToHex(PRINTER_ANSWER, PRINTER_ANSWER.Length), Port1DataIn, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                    else collectBuffer(Encoding.GetEncoding(Properties.Settings.Default.CodePage).GetString(PRINTER_ANSWER), Port1DataIn, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                }
            }
        }

        private void RefreshUSB()
        {
            comboBox_Printer.Items.Clear();
            GetDeviceList();
            for (int i = 0; i < CONNECTED_PRINTER_LIST.Count; i++)
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

        private void checkBox_hexCommand_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_hexCommand.Checked) textBox_command.Text = Accessory.ConvertStringToHex(textBox_command.Text);
            else textBox_command.Text = Accessory.ConvertHexToString(textBox_command.Text);
        }

        private void textBox_command_Leave(object sender, EventArgs e)
        {
            if (checkBox_hexCommand.Checked) textBox_command.Text = Accessory.checkHexString(textBox_command.Text);
        }

        private void textBox_param_Leave(object sender, EventArgs e)
        {
            if (checkBox_hexParam.Checked) textBox_param.Text = Accessory.checkHexString(textBox_param.Text);
        }

        private void checkBox_hexParam_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_hexParam.Checked) textBox_param.Text = Accessory.ConvertStringToHex(textBox_param.Text);
            else textBox_param.Text = Accessory.ConvertHexToString(textBox_param.Text);
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            textBox_terminal.Clear();
        }

        private void textBox_terminal_Click(object sender, EventArgs e)
        {
            ReadUSB();
        }

        private void checkBox_saveTo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_saveInput.Checked || checkBox_saveOutput.Checked) textBox_saveTo.Enabled = false;
            else textBox_saveTo.Enabled = true;
        }

        private async void button_sendFile_ClickAsync(object sender, EventArgs e)
        {
            if (SendComing > 0)
            {
                SendComing++;
            }
            else if (SendComing == 0)
            {
                UInt16 repeat = 1, delay = 1, strDelay = 1;

                if (textBox_fileName.Text != "" && textBox_sendNum.Text != "" && UInt16.TryParse(textBox_sendNum.Text, out repeat) && UInt16.TryParse(textBox_delay.Text, out delay) && UInt16.TryParse(textBox_strDelay.Text, out strDelay))
                {
                    SendComing = 1;
                    button_Send.Enabled = false;
                    button_closeport.Enabled = false;
                    button_openFile.Enabled = false;
                    button_sendFile.Text = "Stop";
                    textBox_fileName.Enabled = false;
                    textBox_sendNum.Enabled = false;
                    textBox_delay.Enabled = false;
                    textBox_strDelay.Enabled = false;
                    for (int n = 0; n < repeat; n++)
                    {
                        string outStr = "";
                        string outErr = "";
                        long length = 0;
                        if (repeat > 1) collectBuffer(" Send cycle " + (n + 1).ToString() + "/" + repeat.ToString() + ">> ", 0, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                        try
                        {
                            length = new FileInfo(textBox_fileName.Text).Length;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("\r\nError opening file " + textBox_fileName.Text + ": " + ex.Message);
                        }

                        if (!checkBox_hexFileOpen.Checked)  //binary data read
                        {
                            if (radioButton_byByte.Checked) //byte-by-byte
                            {
                                byte[] tmpBuffer = new byte[length];
                                try
                                {
                                    tmpBuffer = File.ReadAllBytes(textBox_fileName.Text);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\r\nError reading file " + textBox_fileName.Text + ": " + ex.Message);
                                }
                                for (int m = 0; m < tmpBuffer.Length; m++)
                                {
                                    byte[] outByte = { tmpBuffer[m] };
                                    if (checkBox_hexTerminal.Checked) outStr = Accessory.ConvertByteArrayToHex(tmpBuffer, tmpBuffer.Length);
                                    //else outStr = ConvertHexToString(ConvertByteArrToHex(tmpBuffer, tmpBuffer.Length));
                                    else outStr = tmpBuffer.ToString();
                                    collectBuffer(outStr, Port1DataOut, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                    if (Selected_Printer.GenericWrite(outByte))
                                    {
                                        progressBar1.Value = (n * tmpBuffer.Length + m) * 100 / (repeat * tmpBuffer.Length);
                                        await TaskEx.Delay(strDelay);
                                        ReadUSB();
                                    }
                                    else
                                    {
                                        collectBuffer("Write Failure", Port1Error, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                    }

                                    if (SendComing > 1) m = tmpBuffer.Length;
                                }
                            }
                            else //stream
                            {
                                byte[] tmpBuffer = new byte[length];
                                try
                                {
                                    tmpBuffer = File.ReadAllBytes(textBox_fileName.Text);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\r\nError reading file " + textBox_fileName.Text + ": " + ex.Message);
                                }
                                if (checkBox_hexTerminal.Checked) outStr = Accessory.ConvertByteArrayToHex(tmpBuffer, tmpBuffer.Length);
                                //else outStr += ConvertHexToString(ConvertByteArrToHex(tmpBuffer, tmpBuffer.Length));
                                else outStr = tmpBuffer.ToString();
                                collectBuffer(outStr, Port1DataOut, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                if (Selected_Printer.GenericWrite(tmpBuffer)) ReadUSB();
                                else outErr = "Write Failure";
                                collectBuffer(outErr, Port1Error, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                progressBar1.Value = (n * 100) / (repeat * tmpBuffer.Length);
                            }
                        }
                        else  //hex text read
                        {
                            if (radioButton_byString.Checked) //String-by-string
                            {
                                String[] tmpBuffer = { };
                                try
                                {
                                    length = new FileInfo(textBox_fileName.Text).Length;
                                    tmpBuffer = File.ReadAllText(textBox_fileName.Text).Replace("\n", "").Split('\r');
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\r\nError reading file " + textBox_fileName.Text + ": " + ex.Message);
                                }
                                for (int m = 0; m < tmpBuffer.Length; m++)
                                {
                                    tmpBuffer[m] = Accessory.checkHexString(tmpBuffer[m]);
                                    collectBuffer(outStr, Port1DataOut, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                    if (Selected_Printer.GenericWrite(Accessory.ConvertHexToByteArray(tmpBuffer[m])))
                                    {
                                        if (checkBox_hexTerminal.Checked) outStr = tmpBuffer[m];
                                        else outStr = Accessory.ConvertHexToString(tmpBuffer[m]);
                                        await TaskEx.Delay(strDelay);
                                        ReadUSB();
                                    }
                                    else  //??????????????
                                    {
                                        outErr = "Write failure";
                                    }

                                    if (SendComing > 1) m = tmpBuffer.Length;
                                    collectBuffer(outErr, Port1Error, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                    progressBar1.Value = (n * tmpBuffer.Length + m) * 100 / (repeat * tmpBuffer.Length);
                                }
                            }
                            else if (radioButton_byByte.Checked) //byte-by-byte
                            {
                                String tmpBuffer = "";
                                try
                                {
                                    length = new FileInfo(textBox_fileName.Text).Length;
                                    tmpBuffer = File.ReadAllText(textBox_fileName.Text);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\r\nError reading file " + textBox_fileName.Text + ": " + ex.Message);
                                }
                                tmpBuffer = Accessory.checkHexString(tmpBuffer);
                                for (int m = 0; m < tmpBuffer.Length; m += 3)
                                {
                                    collectBuffer(outStr, Port1DataOut, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                    if (Selected_Printer.GenericWrite(Accessory.ConvertHexToByteArray(tmpBuffer.Substring(m, 3))))
                                    {
                                        if (checkBox_hexTerminal.Checked) outStr = tmpBuffer.Substring(m, 3);
                                        else outStr = Accessory.ConvertHexToString(tmpBuffer.Substring(m, 3));
                                        await TaskEx.Delay(strDelay);
                                        ReadUSB();
                                    }
                                    else
                                    {
                                        outErr += "Write Failure\r\n";
                                    }

                                    if (SendComing > 1) m = tmpBuffer.Length;
                                    collectBuffer(outErr, Port1Error, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                    progressBar1.Value = (n * tmpBuffer.Length + m) * 100 / (repeat * tmpBuffer.Length);
                                }
                            }
                            else //stream
                            {
                                string tmpBuffer = "";
                                try
                                {
                                    length = new FileInfo(textBox_fileName.Text).Length;
                                    tmpBuffer = Accessory.checkHexString(File.ReadAllText(textBox_fileName.Text));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("\r\nError reading file " + textBox_fileName.Text + ": " + ex.Message);
                                }
                                collectBuffer(outStr, Port1DataOut, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                if (Selected_Printer.GenericWrite(Accessory.ConvertHexToByteArray(tmpBuffer))) ReadUSB();
                                else collectBuffer("Write Failure\r\n", Port1Error, DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString("D3"));
                                if (checkBox_hexTerminal.Checked) outStr = tmpBuffer;
                                else outStr = Accessory.ConvertHexToString(tmpBuffer);
                                progressBar1.Value = (n * 100) / (repeat * tmpBuffer.Length);
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
            }
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            textBox_fileName.Text = openFileDialog1.FileName;
        }

        private void button_openFile_Click(object sender, EventArgs e)
        {
            if (checkBox_hexFileOpen.Checked == true)
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

        private void checkBox_hexFileOpen_CheckedChanged(object sender, EventArgs e)
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
            Properties.Settings.Default.checkBox_hexCommand = checkBox_hexCommand.Checked;
            Properties.Settings.Default.textBox_command = textBox_command.Text;
            Properties.Settings.Default.checkBox_hexParam = checkBox_hexParam.Checked;
            Properties.Settings.Default.textBox_param = textBox_param.Text;
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox_hexCommand.Checked = Properties.Settings.Default.checkBox_hexCommand;
            textBox_command.Text = Properties.Settings.Default.textBox_command;
            checkBox_hexParam.Checked = Properties.Settings.Default.checkBox_hexParam;
            textBox_param.Text = Properties.Settings.Default.textBox_param;
        }

        private void radioButton_stream_CheckedChanged(object sender, EventArgs e)
        {
            textBox_strDelay.Enabled = !radioButton_stream.Checked;
        }

        private void textBox_fileName_TextChanged(object sender, EventArgs e)
        {
            if (textBox_fileName.Text != "" && button_closeport.Enabled == true) button_sendFile.Enabled = true;
            else button_sendFile.Enabled = false;
        }

        private void checkBox_printer_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_printer.Checked == true)
            {
                checkBox_scanner.Checked = false;
                m_pGuid = new Guid(Properties.Settings.Default.GUID_PRINT);
                RefreshUSB();
            }
        }

        private void checkBox_scanner_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_scanner.Checked == true)
            {
                checkBox_printer.Checked = false;
                m_pGuid = new Guid(Properties.Settings.Default.GUID_SCAN);
                RefreshUSB();
            }
        }
    }
}
