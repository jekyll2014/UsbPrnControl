using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

public partial class Accessory
{
    public static string ConvertHexToString(string hexString, int cp = 866)
    {
        hexString = hexString.Replace(" ", "");
        if (hexString.Length % 2 == 1) return "";
        byte[] StrValue = new byte[hexString.Length / 2];
        int i = 0;
        while (hexString.Length > 1)
        {
            StrValue[i] = Convert.ToByte(Convert.ToUInt32(hexString.Substring(0, 2), 16));
            hexString = hexString.Substring(2, hexString.Length - 2);
            i++;
        }
        return System.Text.Encoding.GetEncoding(cp).GetString(StrValue, 0, i);
    }

    public static byte[] ConvertHexToByteArray(string hexString)
    {
        hexString = hexString.Replace(" ", "");
        if (hexString.Length % 2 == 1) hexString = hexString + "0";
        byte[] byteValue = new byte[hexString.Length / 2];
        int i = 0;
        while (hexString.Length > 1)
        {
            byteValue[i] = Convert.ToByte(Convert.ToUInt32(hexString.Substring(0, 2), 16));
            hexString = hexString.Substring(2, hexString.Length - 2);
            i++;
        }
        return byteValue;
    }

    public static byte ConvertHexToByte(string hexString)
    {
        hexString = hexString.Trim();
        byte byteValue = 0;
        if (hexString.Length > 0 && hexString.Length < 3)
        {
            byteValue = Convert.ToByte(Convert.ToUInt32(hexString, 16));
        }
        return byteValue;
    }

    public static string ConvertHexToDec(string HexString)
    {
        string DecString;
        DecString = Convert.ToInt32(HexString, 16).ToString();
        return DecString;
    }

    public static int ConvertHexToInt(string hexString)
    {
        hexString = hexString.Replace(" ", "");
        hexString = hexString.Replace("x", "");
        hexString = hexString.Replace("h", "");
        hexString = hexString.Replace("X", "");
        hexString = hexString.Replace("H", "");
        return int.Parse(hexString, NumberStyles.HexNumber);
    }

    public static string ConvertStringToHex(string utfString, int cp = 866)
    {
        byte[] encodedBytes = System.Text.Encoding.GetEncoding(cp).GetBytes(utfString);
        string hexStr = "";
        foreach (System.Char c in encodedBytes)
        {
            hexStr += ((int)c).ToString("X2") + " ";
        }
        return hexStr;
    }

    //???? check negative values
    public static string ConvertDecToString(string decString, int cp = 866)
    {
        decString = decString.Replace(" ", "");
        if (decString.Length % 3 == 1) return "";
        byte[] StrValue = new byte[decString.Length / 3];
        int i = 0;
        while (decString.Length > 1)
        {
            if (Convert.ToUInt32(decString.Substring(0, 3), 10) < 256) StrValue[i] = Convert.ToByte(Convert.ToUInt32(decString.Substring(0, 3), 10));
            else StrValue[i] = 0xff;
            decString = decString.Substring(3, decString.Length - 3);
            i++;
        }
        return System.Text.Encoding.GetEncoding(cp).GetString(StrValue, 0, i);
    }

    public static string ConvertDecToHex(string DecString)
    {
        string HexString;
        HexString = Convert.ToInt32(DecString, 16).ToString();
        return HexString;
    }

    public static string ConvertStringToDec(string utfString, int cp = 866)
    {
        byte[] encodedBytes = System.Text.Encoding.GetEncoding(cp).GetBytes(utfString);
        string hexStr = "";
        foreach (System.Char c in encodedBytes)
        {
            hexStr += ((int)c).ToString("D3") + " ";
        }
        return hexStr;
    }

    //???? check negative values
    public static string ConvertBinToString(string binString, int cp = 866)
    {
        binString = binString.Replace(" ", "");
        if (binString.Length % 8 == 1) return "";
        byte[] StrValue = new byte[binString.Length / 8];
        int i = 0;
        while (binString.Length > 0)
        {
            StrValue[i] = Convert.ToByte(Convert.ToUInt32(binString.Substring(0, 8), 2));
            binString = binString.Substring(8, binString.Length - 8);
            i++;
        }
        return System.Text.Encoding.GetEncoding(cp).GetString(StrValue, 0, i);
    }

    public static string ConvertStringToBin(string utfString, int cp = 866)
    {
        byte[] encodedBytes = System.Text.Encoding.GetEncoding(cp).GetBytes(utfString);
        string hexStr = "";
        foreach (System.Char c in encodedBytes)
        {
            string tmpStr = Convert.ToString((byte)c, 2);
            while (tmpStr.Length < 8) tmpStr = "0" + tmpStr;
            hexStr += tmpStr + " ";
        }
        return hexStr + " ";
    }

    public static string CheckHexString(string inStr)
    {
        string outStr = "";
        if (inStr != "")
        {
            char[] str = inStr.ToCharArray(0, inStr.Length);
            string tmpStr = "";
            for (int i = 0; i < inStr.Length; i++)
            {
                if ((str[i] >= 'A' && str[i] <= 'F') || (str[i] >= 'a' && str[i] <= 'f') || (str[i] >= '0' && str[i] <= '9'))
                {
                    tmpStr += str[i].ToString();
                }
                else if (str[i] == ' ' && tmpStr.Length > 0)
                {
                    for (int i1 = 0; i1 < 2 - tmpStr.Length; i1++) outStr += "0";
                    outStr += tmpStr + " ";
                    tmpStr = "";
                }
                if (tmpStr.Length == 2)
                {
                    outStr += tmpStr + " ";
                    tmpStr = "";
                }
            }
            if (tmpStr != "")
            {
                for (int i = 0; i < 2 - tmpStr.Length; i++) outStr += "0";
                outStr += tmpStr + " ";
            }
            return outStr.ToUpperInvariant();
        }
        else return ("");
    }

    //добавить фильтрацию значений >255
    public static string CheckDecString(string inStr)
    {
        string outStr = "";
        if (inStr != "")
        {
            char[] str = inStr.ToCharArray(0, inStr.Length);
            string tmpStr = "";
            for (int i = 0; i < inStr.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                {
                    tmpStr += str[i].ToString();
                }
                else if (str[i] == ' ' && tmpStr.Length > 0)
                {
                    for (int i1 = 0; i1 < inStr.Length - tmpStr.Length; i1++) outStr += "0";
                    outStr += tmpStr + " ";
                    tmpStr = "";
                }
                if (tmpStr.Length == inStr.Length)
                {
                    outStr += tmpStr + " ";
                    tmpStr = "";
                }
            }
            if (tmpStr != "")
            {
                for (int i = 0; i < inStr.Length - tmpStr.Length; i++) outStr += "0";
                outStr += tmpStr + " ";
            }
            return outStr;
        }
        else return ("");
    }

    //добавить фильтрацию значений >255
    public static string CheckDecString(string inStr, int length)
    {
        string outStr = "";
        if (inStr != "")
        {
            char[] str = inStr.ToCharArray(0, inStr.Length);
            string tmpStr = "";
            for (int i = 0; i < inStr.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                {
                    tmpStr += str[i].ToString();
                }
                else if (str[i] == ' ' && tmpStr.Length > 0)
                {
                    for (int i1 = 0; i1 < length - tmpStr.Length; i1++) outStr += "0";
                    outStr += tmpStr + " ";
                    tmpStr = "";
                }
                if (tmpStr.Length == length)
                {
                    outStr += tmpStr + " ";
                    tmpStr = "";
                }
            }
            if (tmpStr != "")
            {
                for (int i = 0; i < length - tmpStr.Length; i++) outStr += "0";
                outStr += tmpStr + " ";
            }
            return outStr;
        }
        else return ("");
    }

    // проверить
    public static string CheckBinString(string inStr)
    {
        string outStr = "";
        if (inStr != "")
        {
            char[] str = inStr.ToCharArray(0, inStr.Length);
            string tmpStr = "";
            for (int i = 0; i < inStr.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '1')
                {
                    tmpStr += str[i].ToString();
                }
                else if (str[i] == ' ' && tmpStr.Length > 0)
                {
                    for (int i1 = 0; i1 < 8 - tmpStr.Length; i1++) outStr += "0";
                    outStr += tmpStr + " ";
                    tmpStr = "";
                }
                if (tmpStr.Length == 8)
                {
                    outStr += tmpStr + " ";
                    tmpStr = "";
                }
            }
            if (tmpStr != "")
            {
                for (int i = 0; i < 8 - tmpStr.Length; i++) outStr += "0";
                outStr += tmpStr + " ";
            }
            return outStr;
        }
        else return ("");
    }

    //определение формата строки
    public static int GetStringFormat(string inString)
    {
        int i = 0, i1 = 0;
        //bool xbin = true, xdec = true;
        List<int> l = new List<int>();
        while (i < inString.Length)
        {
            if ((inString[i] >= '0' && inString[i] <= '9') || (inString[i] >= 'a' && inString[i] <= 'f') || (inString[i] >= 'A' && inString[i] <= 'F') || inString[i] == ' ')
            {
                //if (inString[i] < '0' && inString[i] > '1' && inString[i] != ' ') xbin=false; //not BIN
                //if (inString[i] < '0' && inString[i] > '9' && inString[i] != ' ') xdec = false; //not DEC
                if (inString[i] == ' ')
                {
                    if (i1 != 0) l.Add(i1);
                    i1 = 0;
                }
                else i1++;
                i++;
            }
            else return 0; //TEXT
        }
        if (i1 != 0) l.Add(i1);
        //if (xbin == false && xdec == false) return 16;
        for (i = 0; i < l.Count - 1; i++)
        {
            if (l[i + 1] != l[i]) return 0;
        }
        if (l[0] == 2) return 16;
        else if (l[0] == 3) return 10;
        else if (l[0] == 8) return 2;
        return 0;
    }

    public static string ConvertByteArrayToHex(byte[] byteArr)
    {
        string hexStr = "";
        int i = 0;
        for (i = 0; i < byteArr.Length; i++)
        {
            hexStr += byteArr[i].ToString("X2") + " ";
        }
        return hexStr;
    }

    public static string ConvertByteArrayToHex(byte[] byteArr, int Length)
    {
        if (Length > byteArr.Length) Length = byteArr.Length;
        string hexStr = "";
        int i = 0;
        for (i = 0; i < Length; i++)
        {
            hexStr += byteArr[i].ToString("X2") + " ";
        }
        return hexStr;
    }

    public static string ConvertByteToHex(byte byteVal)
    {
        return byteVal.ToString("X2") + " ";
    }

    public static bool PrintableByteArray(byte[] str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] < 32) return false;
        }
        return true;
    }

    public static bool PrintableByte(byte b)
    {
        if (b < 32) return false;
        return true;
    }

    public static bool PrintableHex(string str)
    {
        for (int i = 0; i < str.Length; i += 3)
        {
            if (!byte.TryParse(str.Substring(i, 3), NumberStyles.HexNumber, null, out byte n)) return false;
            else if (n < 32) return false;
        }
        return true;
    }

    public static int CountSubString(string str, string subStr)
    {
        int count = 0;
        int n = 0;
        while ((n = str.IndexOf(subStr, n, StringComparison.InvariantCulture)) != -1)
        {
            n += subStr.Length;
            ++count;
        }
        return count;
    }

    public static bool GetBit(byte b, byte bitNumber)
    {
        return (b & (1 << bitNumber)) != 0;
    }

    public static byte SetBit(byte b, byte bitNumber)
    {
        b = (byte)(b | (1 << bitNumber));
        return b;
    }

    public static byte ClearBit(byte b, byte bitNumber)
    {
        b = (byte)(b & ~(1 << bitNumber));
        return b;
    }

    public static long Evaluate(string expression)  //calculate string formula
    {
        var loDataTable = new DataTable();
        var loDataColumn = new DataColumn("Eval", typeof(long), expression);
        loDataTable.Columns.Add(loDataColumn);
        loDataTable.Rows.Add(0);
        return (long)(loDataTable.Rows[0]["Eval"]);
    }

    public static void Delay_ms(long milisec)
    {
        DateTime start = DateTime.Now;

        while (DateTime.Now.Subtract(start).TotalMilliseconds < milisec)
        {
            Application.DoEvents();
            System.Threading.Thread.Sleep(1);
        }
    }

    public static byte[] BMPtoByteArray_1D_76_30(Bitmap PICTURE_BMP, int PIC_WIDTH, int PIC_HEIGHT, byte mode)
    {
        byte[] PIC_BUFFER = new byte[0];
        if (mode != 0 && mode != 1 && mode != 2 && mode != 3 && mode != 48 && mode != 49 && mode != 50 && mode != 51) return PIC_BUFFER;
        //apply image size limitations
        if (PIC_WIDTH > PICTURE_BMP.Width) PIC_WIDTH = PICTURE_BMP.Width;
        if (PIC_HEIGHT > PICTURE_BMP.Height) PIC_HEIGHT = PICTURE_BMP.Height;
        int change_step_width = 8;
        PIC_WIDTH = PIC_WIDTH - (PIC_WIDTH % change_step_width);
        if (PIC_WIDTH == 0 || PIC_HEIGHT == 0) return PIC_BUFFER;
        int maxHeight = 2304;
        if (PIC_HEIGHT > maxHeight) PIC_HEIGHT = maxHeight;

        //convert bitmap to B/W        
        if (PICTURE_BMP.PixelFormat != System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
        {
            for (int i = 0; i < PICTURE_BMP.Width; i++)
            {
                for (int j = 0; j < PICTURE_BMP.Height; j++)
                {
                    if (PICTURE_BMP.GetPixel(i, j).GetBrightness() > 0.9) PICTURE_BMP.SetPixel(i, j, Color.White);
                    else PICTURE_BMP.SetPixel(i, j, Color.Black);
                }
            }
        }

        PIC_BUFFER = new byte[(PIC_WIDTH * PIC_HEIGHT / 8) + 8];

        byte xH = (byte)(PIC_WIDTH / 8 / 256);
        byte xL = (byte)((PIC_WIDTH / 8) - xH * 256);
        byte yH = (byte)(PIC_HEIGHT / 256);
        byte yL = (byte)((PIC_HEIGHT) - yH * 256);
        PIC_BUFFER[0] = 0x1d;
        PIC_BUFFER[1] = 0x76;
        PIC_BUFFER[2] = 0x30;
        PIC_BUFFER[3] = mode;
        PIC_BUFFER[4] = xL;
        PIC_BUFFER[5] = xH;
        PIC_BUFFER[6] = yL;
        PIC_BUFFER[7] = yH;

        byte tmp_byte;
        int index = 8;
        for (int row = 0; row < PIC_HEIGHT; row++)
        {
            for (int column = 0; column < PIC_WIDTH; column += 8) //'every 8 pixel is one byte
            {
                tmp_byte = 0;
                //' NOTE: if any of RGB value is 0, then the color must be black, otherwhise if FF it means white
                //' not need to check all RGB since it is black/white only, so R is enough.
                if (PICTURE_BMP.GetPixel(column, row).R == 0) tmp_byte += 128;
                if (PICTURE_BMP.GetPixel(column + 1, row).R == 0) tmp_byte += 64;
                if (PICTURE_BMP.GetPixel(column + 2, row).R == 0) tmp_byte += 32;
                if (PICTURE_BMP.GetPixel(column + 3, row).R == 0) tmp_byte += 16;
                if (PICTURE_BMP.GetPixel(column + 4, row).R == 0) tmp_byte += 8;
                if (PICTURE_BMP.GetPixel(column + 5, row).R == 0) tmp_byte += 4;
                if (PICTURE_BMP.GetPixel(column + 6, row).R == 0) tmp_byte += 2;
                if (PICTURE_BMP.GetPixel(column + 7, row).R == 0) tmp_byte += 1;

                PIC_BUFFER[index] = tmp_byte;
                index++;
            }
        }
        return (PIC_BUFFER);
    }

    public static Bitmap ByteArrayToBMP_1D_76_30(byte[] PIC_BUFFER, out byte mode)
    {
        Bitmap PICTURE_BMP = new Bitmap(1, 1);
        mode = 0;
        if (!(PIC_BUFFER[0] == 0x1D && PIC_BUFFER[1] == 0x76 && PIC_BUFFER[2] == 0x30)) return (PICTURE_BMP); //command incorrect

        mode = PIC_BUFFER[3];
        int PIC_WIDTH = (PIC_BUFFER[4] + PIC_BUFFER[5] * 256) * 8;
        int PIC_HEIGHT = PIC_BUFFER[6] + PIC_BUFFER[7] * 256;
        int datasize = (PIC_WIDTH * PIC_HEIGHT) / 8;


        if ((PIC_BUFFER.Length - 8) < datasize)
        {
            //Command string resolution doesn't match picture data
            return (PICTURE_BMP);
        }
        PICTURE_BMP = new Bitmap(PIC_WIDTH, PIC_HEIGHT);

        byte tmp_byte = 0;
        int index = 8;
        //'create the buffer

        for (int row = 0; row < PIC_HEIGHT; row++)
        {
            for (int column = 0; column < PIC_WIDTH; column += 8) //'every 8 pixel is one byte
            {
                tmp_byte = PIC_BUFFER[index];
                if (GetBit(tmp_byte, 7) == true) PICTURE_BMP.SetPixel(column, row, Color.Black);
                else PICTURE_BMP.SetPixel(column, row, Color.White);

                if (GetBit(tmp_byte, 6) == true) PICTURE_BMP.SetPixel(column + 1, row, Color.Black);
                else PICTURE_BMP.SetPixel(column + 1, row, Color.White);

                if (GetBit(tmp_byte, 5) == true) PICTURE_BMP.SetPixel(column + 2, row, Color.Black);
                else PICTURE_BMP.SetPixel(column + 2, row, Color.White);

                if (GetBit(tmp_byte, 4) == true) PICTURE_BMP.SetPixel(column + 3, row, Color.Black);
                else PICTURE_BMP.SetPixel(column + 3, row, Color.White);

                if (GetBit(tmp_byte, 3) == true) PICTURE_BMP.SetPixel(column + 4, row, Color.Black);
                else PICTURE_BMP.SetPixel(column + 4, row, Color.White);

                if (GetBit(tmp_byte, 2) == true) PICTURE_BMP.SetPixel(column + 5, row, Color.Black);
                else PICTURE_BMP.SetPixel(column + 5, row, Color.White);

                if (GetBit(tmp_byte, 1) == true) PICTURE_BMP.SetPixel(column + 6, row, Color.Black);
                else PICTURE_BMP.SetPixel(column + 6, row, Color.White);

                if (GetBit(tmp_byte, 0) == true) PICTURE_BMP.SetPixel(column + 7, row, Color.Black);
                else PICTURE_BMP.SetPixel(column + 7, row, Color.White);
                index++;
            }
        }
        return (PICTURE_BMP);
    }

    public static byte[] BMPtoByteArray_1B_2A(Bitmap PICTURE_BMP, int PIC_WIDTH, int PIC_HEIGHT, byte mode)
    {
        byte[] PIC_BUFFER = new byte[0];
        if (!(mode == 0 || mode == 1 || mode == 32 || mode == 33)) return PIC_BUFFER;
        byte picMode;
        if (mode >= 32) picMode = 3;
        else picMode = 1;

        //convert bitmap to B/W
        if (PICTURE_BMP.PixelFormat != System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
        {
            for (int i = 0; i < PICTURE_BMP.Width; i++)
            {
                for (int j = 0; j < PICTURE_BMP.Height; j++)
                {
                    if (PICTURE_BMP.GetPixel(i, j).GetBrightness() > 0.9) PICTURE_BMP.SetPixel(i, j, Color.White);
                    else PICTURE_BMP.SetPixel(i, j, Color.Black);
                }
            }
        }
        if (PIC_WIDTH > PICTURE_BMP.Width) PIC_WIDTH = PICTURE_BMP.Width;
        if (PIC_HEIGHT > PICTURE_BMP.Height) PIC_HEIGHT = PICTURE_BMP.Height;
        int maxWidth = 1024;
        if (PIC_WIDTH > maxWidth) PIC_WIDTH = maxWidth;
        int change_step_height = 8 * picMode;
        PIC_HEIGHT = PIC_HEIGHT - (PIC_HEIGHT % change_step_height);

        //generate string length
        byte h = (byte)(PIC_WIDTH / 256);
        byte l = (byte)(PIC_WIDTH - h * 256);

        //create the buffer
        int pages = PIC_HEIGHT / change_step_height;
        PIC_BUFFER = new byte[(PIC_WIDTH * picMode * pages + pages * 6)]; //add 6 control bytes to bit fields for each part

        byte tmp_byte = 0;
        int index = 0;
        for (int row = 0; row < PIC_HEIGHT; row += 8 * picMode) //'every 8/24 pixel is one/three byte
        {
            PIC_BUFFER[index] = 0x1B;
            index++;
            PIC_BUFFER[index] = 0x2A;
            index++;
            PIC_BUFFER[index] = mode;
            index++;
            PIC_BUFFER[index] = l;
            index++;
            PIC_BUFFER[index] = h;
            index++;
            for (int column = 0; column < PIC_WIDTH; column++)
            {
                for (int i = 0; i < picMode; i++)
                {
                    tmp_byte = 0;
                    //' NOTE: if any of RGB value is 0, then the color must be black, otherwhise if FF it means white
                    //' not need to check all RGB since it is black/white only, so R is enough.
                    if (PICTURE_BMP.GetPixel(column, 8 * i + row).R == 0) tmp_byte += 128;
                    if (PICTURE_BMP.GetPixel(column, 8 * i + row + 1).R == 0) tmp_byte += 64;
                    if (PICTURE_BMP.GetPixel(column, 8 * i + row + 2).R == 0) tmp_byte += 32;
                    if (PICTURE_BMP.GetPixel(column, 8 * i + row + 3).R == 0) tmp_byte += 16;
                    if (PICTURE_BMP.GetPixel(column, 8 * i + row + 4).R == 0) tmp_byte += 8;
                    if (PICTURE_BMP.GetPixel(column, 8 * i + row + 5).R == 0) tmp_byte += 4;
                    if (PICTURE_BMP.GetPixel(column, 8 * i + row + 6).R == 0) tmp_byte += 2;
                    if (PICTURE_BMP.GetPixel(column, 8 * i + row + 7).R == 0) tmp_byte += 1;
                    PIC_BUFFER[index] = tmp_byte;
                    index++;
                }
            }
            PIC_BUFFER[index] = 0x0a;
            index++;
            /*PIC_BUFFER[index] = 0x33;
            index++;
            PIC_BUFFER[index] = 0x01;
            index++;*/
        }
        return PIC_BUFFER;
    }

    public static Bitmap ByteArrayToBMP_1B_2A(byte[] PIC_BUFFER, out byte mode)
    {
        Bitmap PICTURE_BMP = new Bitmap(1, 1);
        int PIC_WIDTH = 0;
        int PIC_HEIGHT = 0;
        List<byte> tmp_buffer = new List<byte>();
        int cmdStart = 0;
        byte picMode = 1;
        int picLength = 0;
        int picNumber = 0;
        bool flag = false;
        mode = 0;

        while (flag == false) //Start of picture crop
        {
            if (PIC_BUFFER.Length >= cmdStart + 5)
            {
                while (flag == false && !(PIC_BUFFER[cmdStart] == 0x1B && PIC_BUFFER[cmdStart + 1] == 0x2A)) //looking for a command
                {
                    if ((cmdStart + 5) < PIC_BUFFER.Length) cmdStart++;
                    else if (picNumber < 1)
                    {
                        //MessageBox.Show("1B 2A command not found");
                        return PICTURE_BMP;
                    }
                    else flag = true;
                }
                if (flag == false)
                {
                    mode = PIC_BUFFER[cmdStart + 2]; //get picture mode (1 or 3 byte height)
                    picMode = mode;
                    if (picMode == 32)
                    {
                        picMode = 3;
                    }
                    else if (picMode == 33)
                    {
                        picMode = 3;
                    }
                    else if (picMode == 0)
                    {
                        picMode = 1;
                    }
                    else if (picMode == 1)
                    {
                        picMode = 1;
                    }
                    PIC_WIDTH = PIC_BUFFER[cmdStart + 3] + PIC_BUFFER[cmdStart + 4] * 256;
                    picLength = picMode * PIC_WIDTH; //getting picture size
                }
            }
            else if (picNumber < 1)
            {
                //MessageBox.Show("1B 2A command parameters corrupted");
                return PICTURE_BMP;
            }
            else flag = true;
            if (flag == false && PIC_BUFFER.Length >= (cmdStart + 5 + picLength))
            {
                List<byte> tmp_list = new List<Byte>(PIC_BUFFER).GetRange(cmdStart + 5, picLength);
                tmp_buffer.AddRange(tmp_list);
                picNumber++;
                cmdStart += 5 + picLength;
            }
            else if (picNumber < 1)
            {
                //MessageBox.Show("1B 2A command data corrupted");
                return PICTURE_BMP;
            }
            else flag = true;
        } //end of picture crop

        //if (picNumber == 0) return PICTURE_BMP;

        PIC_HEIGHT = picMode * 8 * picNumber;
        PICTURE_BMP = new Bitmap(PIC_WIDTH, PIC_HEIGHT);

        int index = 0;

        byte tmp_byte;
        for (int nn = 0; nn < picNumber; nn++)
        {
            for (int col = 0; col < PIC_WIDTH; col++)
            {
                for (int row = 0; row < picMode; row++)
                {
                    tmp_byte = tmp_buffer[index];
                    if (GetBit(tmp_byte, 7) == true) PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8, Color.Black);
                    else PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8, Color.White);

                    if (GetBit(tmp_byte, 6) == true) PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 1, Color.Black);
                    else PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 1, Color.White);

                    if (GetBit(tmp_byte, 5) == true) PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 2, Color.Black);
                    else PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 2, Color.White);

                    if (GetBit(tmp_byte, 4) == true) PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 3, Color.Black);
                    else PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 3, Color.White);

                    if (GetBit(tmp_byte, 3) == true) PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 4, Color.Black);
                    else PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 4, Color.White);

                    if (GetBit(tmp_byte, 2) == true) PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 5, Color.Black);
                    else PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 5, Color.White);

                    if (GetBit(tmp_byte, 1) == true) PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 6, Color.Black);
                    else PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 6, Color.White);

                    if (GetBit(tmp_byte, 0) == true) PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 7, Color.Black);
                    else PICTURE_BMP.SetPixel(col, nn * 8 * picMode + row * 8 + 7, Color.White);
                    index++;
                }
            }
        }   //end converting 1st picture
        return PICTURE_BMP;
    }
}
