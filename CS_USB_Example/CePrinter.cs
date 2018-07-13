//This document contains programming examples.
//Custom S.P.A. grants you a nonexclusive copyright license to use all programming code examples from which you can generate similar function tailored to your own specific needs.
//All sample code is provided by Custom Engineering for illustrative purposes only. These examples have not been thoroughly tested under all conditions. 
//Custom S.P.A., therefore, cannot guarantee or imply reliability, serviceability, or function of these programs.
//In no event shall Custom Engineering be liable for any direct, indirect, incidental, special, exemplary, or consequential damages (including, but not limited to, procurement of substitute goods or services; loss of use, data, or profits; or business interruption) however caused and on any theory of liability, whether in contract, strict liability, or tort 
//(including negligence or otherwise) arising in any way out of the use of this software, even if advised of the possibility of such damage.
//All programs contained herein are provided to you "as is" without any warranties of any kind. 
//The implied warranties of non-infringement, merchantability and fitness for a particular purpose are expressly disclaimed.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UsbPrnControl
{
    public class CePrinter
    {
        #region STRUCT-ENUM-VARIABLE
        public String USB_ADDRESS_NUMBER;
        public String USB_SYMBOLIC_NAME;
        public String PRINTER_PID;
        public int READ_TIMEOUT = 200;
        public int WRITE_TIMEOUT = 1000;

        private const int bytesperlong = 4;
        private const int bitsperbyte = 8;
        private const uint GENERIC_READ = 0x80000000;
        private const int FILE_SHARE_READ = 1;
        private const int OPEN_EXISTING = 3;
        private const int FILE_FLAG_OVERLAPPED = 1073741824;
        private const int GENERIC_WRITE = 0x40000000;
        private const int FILE_FLAG_NO_BUFFERING = 0x20000000;
        private const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        private const int FILE_SHARE_WRITE = 2;
        private const int WAIT_OBJECT_0 = 0x00000000;
        private const int WAIT_TIMEOUT = 0x00000102;

        private IntPtr hUsbDev_read;
        private IntPtr hUsbDev_write;
        private IntPtr hEvent_write;
        private IntPtr hEvent_read;

        //private byte[] temporary_buffer_1 = new byte[2048];
        public const int USB_PACK = 2048;

        #region DLL_IMPORTED_FUNCTIONS
        /***** CREATE FILE *****/
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName,
                                                uint dwDesiredAccess,
                                                uint dwShareMode,
                                                IntPtr lpSecurityAttributes,
                                                uint dwCreationDisposition,
                                                uint dwFlagsAndAttributes,
                                                IntPtr hTemplateFile);
        /***** CREATE EVENT *****/
        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateEvent(IntPtr lpEventAttributes,
                                                 bool bManualReset,
                                                 bool bInitialState,
                                                 string lpName);
        /***** CLOSE HANDLE *****/
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
        /***** WRITE FILE *****/
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool WriteFile(
            IntPtr hFile,
            [In] byte[] lpBuffer,
            int nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            ref System.Threading.NativeOverlapped lpOverlapped);
        /***** WAIT FOR SINGLE OBJECT *****/
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);
        /***** CANCEL IO *****/
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CancelIo(IntPtr hFile);
        /***** READ FILE *****/
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern bool ReadFile(
            IntPtr hFile,
            [Out] IntPtr lpBuffer,//[Out] byte[] lpBuffer,
            int nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead,
            ref System.Threading.NativeOverlapped lpOverlapped);
        /***** GET OVERLAPPED RESULT *****/
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        private static extern int GetOverlappedResult(
            IntPtr hFile,
            ref System.Threading.NativeOverlapped lpOverlapped,
            ref uint lpNumberOfBytesTransferred,
            bool bWait);
        #endregion
        #endregion

        /***********************************************************************
        * OPEN COMMUNICATION WITH DEVICE
        ***********************************************************************/
        public bool OpenDevice()
        {
            //READ OVERLAPPED
            hUsbDev_read = CreateFile(USB_SYMBOLIC_NAME + "\\PIPE00\\",
                    GENERIC_READ,
                    FILE_SHARE_READ,
                    IntPtr.Zero,
                    OPEN_EXISTING,
                    FILE_FLAG_OVERLAPPED,
                    IntPtr.Zero);
            //WRITE OVERLAPPED
            hUsbDev_write = CreateFile(USB_SYMBOLIC_NAME + "\\PIPE01\\",
                    GENERIC_WRITE,
                    FILE_SHARE_WRITE,
                    IntPtr.Zero,
                    OPEN_EXISTING,
                    FILE_FLAG_OVERLAPPED,
                    IntPtr.Zero);
            //EVENT HANDLER READ
            hEvent_read = CreateEvent(IntPtr.Zero, false, true, "read_event");
            //EVENT HANDLER WRITE
            hEvent_write = CreateEvent(IntPtr.Zero, false, true, "write_event");
            if ((hUsbDev_read != IntPtr.Zero) && (hUsbDev_write != IntPtr.Zero) && (hEvent_read != IntPtr.Zero) && (hEvent_write != IntPtr.Zero))
            {
                if ((hUsbDev_read.ToInt32() != -1) && (hUsbDev_write.ToInt32() != -1))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        /***********************************************************************
         * CLOSE DEVICE
         ***********************************************************************/
        public void CloseDevice()
        {
            CloseHandle(hUsbDev_read);
            CloseHandle(hUsbDev_write);
            CloseHandle(hEvent_read);
            CloseHandle(hEvent_write);
            hUsbDev_read = IntPtr.Zero;
            hUsbDev_write = IntPtr.Zero;
            hEvent_read = IntPtr.Zero;
            hEvent_write = IntPtr.Zero;
        }
        /***********************************************************************
        * WRITE GENERIC DATA BUFFER
        ***********************************************************************/
        public bool GenericWrite(byte[] BufferWrite)
        {
            if (BufferWrite.Length > USB_PACK)
            {
                bool last_error = false;
                int numpack = Convert.ToInt32(Math.Floor(Convert.ToDecimal(BufferWrite.Length / USB_PACK)));
                int remind = BufferWrite.Length - (numpack * USB_PACK);
                byte[] pack = new byte[USB_PACK];
                for (int q = 0; q < numpack; q++)
                {
                    //create pack
                    for (int y = 0; y < USB_PACK; y++)
                        pack[y] = BufferWrite[q * USB_PACK + y];
                    //send pack
                    last_error = GenericWrite_singlePack_USB(pack);
                    if (!last_error)
                        return last_error;
                }
                if (remind > 0)
                {
                    //create minipack
                    pack = new byte[remind];
                    for (int y = 0; y < remind; y++)
                        pack[y] = BufferWrite[BufferWrite.Length - remind + y];
                    //send minipack
                    last_error = GenericWrite_singlePack_USB(pack);
                    if (!last_error)
                        return last_error;
                }
                return true;
            }
            else
            {
                return GenericWrite_singlePack_USB(BufferWrite);
            }
        }

        private bool GenericWrite_singlePack_USB(byte[] pack)
        {
            uint dwRet_w = 1;
            int success = 0;
            System.Threading.NativeOverlapped ovp = new System.Threading.NativeOverlapped();
            ovp.OffsetLow = 0;
            ovp.OffsetHigh = 0;
            ovp.EventHandle = hEvent_write;

            WriteFile(hUsbDev_write, pack, pack.Length, out dwRet_w, ref ovp);
            success = WaitForSingleObject(hEvent_write, WRITE_TIMEOUT);
            if (success == WAIT_OBJECT_0)
            {
                CancelIo(hUsbDev_write);
                if (ovp.InternalHigh.ToInt32() == pack.Length)
                    return true;
                else
                    return false;
            }
            else if (success == WAIT_TIMEOUT)
            {
                CancelIo(hUsbDev_read);
                return false;
            }
            return false;
        }
        /***********************************************************************
        * READ GENERIC DATA BUFFER
        ***********************************************************************/
        public bool GenericRead(ref byte[] BufferRead)
        {
            List<byte> fifoBuffer = new List<byte>();
            int success = 0;
            bool timeout_tick = true;
            IntPtr tmp_buffer = Marshal.AllocHGlobal(USB_PACK);
            uint temp_nbyte = 1;
            while (temp_nbyte != 0)
            {
                System.Threading.NativeOverlapped ovp = new System.Threading.NativeOverlapped();
                ovp.OffsetLow = 0;
                ovp.OffsetHigh = 0;
                ovp.EventHandle = hEvent_read;

                ReadFile(hUsbDev_read, tmp_buffer, USB_PACK, out temp_nbyte, ref ovp);
                success = WaitForSingleObject(hEvent_read, READ_TIMEOUT);
                if (success == WAIT_OBJECT_0)
                {
                    //read content
                    if (GetOverlappedResult(hUsbDev_read, ref ovp, ref temp_nbyte, false) > 0)
                    {
                        byte[] bAppBuffer = new byte[temp_nbyte];
                        Marshal.Copy(tmp_buffer, bAppBuffer, 0, (int)temp_nbyte);
                        for (int t = 0; t < temp_nbyte; t++)
                            fifoBuffer.Add(bAppBuffer[t]);
                        bAppBuffer = null;
                        timeout_tick = false;
                    }
                    else
                    {
                        //error
                        Marshal.FreeHGlobal(tmp_buffer);
                        return false;
                    }
                }
                else if (success == WAIT_TIMEOUT)
                {
                    //timeout
                    CancelIo(hUsbDev_read);
                    int error = Marshal.GetLastWin32Error();
                    if (error == 5)
                        return false;
                }
                else
                {
                    this.CloseDevice();
                    Marshal.FreeHGlobal(tmp_buffer);
                    return false;
                }
            }

            //copy list into array
            BufferRead = new byte[fifoBuffer.Count];
            for (int i = 0; i < fifoBuffer.Count; i++)
                BufferRead[i] = fifoBuffer[i];
            fifoBuffer.Clear();
            Marshal.FreeHGlobal(tmp_buffer);
            if (!timeout_tick)
                return true;
            else
                return false;
        }
    }
}
