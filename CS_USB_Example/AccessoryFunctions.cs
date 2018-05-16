using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
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
        return Encoding.GetEncoding(cp).GetString(StrValue, 0, i);
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
        byte[] encodedBytes = Encoding.GetEncoding(cp).GetBytes(utfString);
        StringBuilder hexStr = new StringBuilder();
        foreach (System.Char c in encodedBytes)
        {
            hexStr.Append(((int)c).ToString("X2"));
            hexStr.Append(" ");
        }
        return hexStr.ToString();
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
        return Encoding.GetEncoding(cp).GetString(StrValue, 0, i);
    }

    public static string ConvertDecToHex(string DecString)
    {
        string HexString;
        HexString = Convert.ToInt32(DecString, 16).ToString();
        return HexString;
    }

    public static string ConvertStringToDec(string utfString, int cp = 866)
    {
        byte[] encodedBytes = Encoding.GetEncoding(cp).GetBytes(utfString);
        StringBuilder decStr = new StringBuilder();
        foreach (System.Char c in encodedBytes)
        {
            decStr.Append(((int)c).ToString("D3"));
            decStr.Append(" ");
        }
        return decStr.ToString();
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
        return Encoding.GetEncoding(cp).GetString(StrValue, 0, i);
    }

    public static string ConvertStringToBin(string utfString, int cp = 866)
    {
        byte[] encodedBytes = Encoding.GetEncoding(cp).GetBytes(utfString);
        StringBuilder binStr = new StringBuilder();
        foreach (System.Char c in encodedBytes)
        {
            string tmpStr = Convert.ToString((byte)c, 2);
            while (tmpStr.Length < 8) tmpStr = "0" + tmpStr;
            binStr.Append(tmpStr);
            binStr.Append(" ");
        }
        binStr.Append(" ");
        return binStr.ToString();
    }

    public static string CheckHexString(string inStr)
    {
        StringBuilder outStr = new StringBuilder();
        if (inStr != "")
        {
            char[] str = inStr.ToCharArray(0, inStr.Length);
            StringBuilder tmpStr = new StringBuilder();
            for (int i = 0; i < inStr.Length; i++)
            {
                if ((str[i] >= 'A' && str[i] <= 'F') || (str[i] >= 'a' && str[i] <= 'f') || (str[i] >= '0' && str[i] <= '9'))
                {
                    tmpStr.Append(str[i].ToString());
                }
                else if (str[i] == ' ' && tmpStr.Length > 0)
                {
                    for (int i1 = 0; i1 < 2 - tmpStr.Length; i1++) outStr.Append("0");
                    outStr.Append(tmpStr);
                    outStr.Append(" ");
                    tmpStr.Length = 0;
                }
                if (tmpStr.Length == 2)
                {
                    outStr.Append(tmpStr);
                    outStr.Append(" ");
                    tmpStr.Length = 0;
                }
            }
            if (tmpStr.Length > 0)
            {
                for (int i = 0; i < 2 - tmpStr.Length; i++) outStr.Append("0");
                outStr.Append(tmpStr);
                outStr.Append(" ");
            }
            return outStr.ToString().ToUpperInvariant();
        }
        else return ("");
    }

    //добавить фильтрацию значений >255
    public static string CheckDecString(string inStr)
    {
        StringBuilder outStr = new StringBuilder();
        if (inStr != "")
        {
            char[] str = inStr.ToCharArray(0, inStr.Length);
            StringBuilder tmpStr = new StringBuilder();
            for (int i = 0; i < inStr.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                {
                    tmpStr.Append(str[i].ToString());
                }
                else if (str[i] == ' ' && tmpStr.Length > 0)
                {
                    for (int i1 = 0; i1 < inStr.Length - tmpStr.Length; i1++) outStr.Append("0");
                    outStr.Append(tmpStr);
                    outStr.Append(" ");
                    tmpStr.Length = 0;
                }
                if (tmpStr.Length == inStr.Length)
                {
                    outStr.Append(tmpStr);
                    outStr.Append(" ");
                    tmpStr.Length = 0;
                }
            }
            if (tmpStr.Length > 0)
            {
                for (int i = 0; i < inStr.Length - tmpStr.Length; i++) outStr.Append("0");
                outStr.Append(tmpStr);
                outStr.Append(" ");
            }
            return outStr.ToString();
        }
        else return ("");
    }

    //добавить фильтрацию значений >255
    public static string CheckDecString(string inStr, int length)
    {
        StringBuilder outStr = new StringBuilder();
        if (inStr != "")
        {
            char[] str = inStr.ToCharArray(0, inStr.Length);
            StringBuilder tmpStr = new StringBuilder();
            for (int i = 0; i < inStr.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                {
                    tmpStr.Append(str[i].ToString());
                }
                else if (str[i] == ' ' && tmpStr.Length > 0)
                {
                    for (int i1 = 0; i1 < length - tmpStr.Length; i1++) outStr.Append("0");
                    outStr.Append(tmpStr);
                    outStr.Append(" ");
                    tmpStr.Length = 0;
                }
                if (tmpStr.Length == length)
                {
                    outStr.Append(tmpStr);
                    outStr.Append(" ");
                    tmpStr.Length = 0;
                }
            }
            if (tmpStr.Length > 0)
            {
                for (int i = 0; i < length - tmpStr.Length; i++) outStr.Append("0");
                outStr.Append(tmpStr);
                outStr.Append(" ");
            }
            return outStr.ToString();
        }
        else return ("");
    }

    // проверить
    public static string CheckBinString(string inStr)
    {
        StringBuilder outStr = new StringBuilder();
        if (inStr != "")
        {
            char[] str = inStr.ToCharArray(0, inStr.Length);
            StringBuilder tmpStr = new StringBuilder();
            for (int i = 0; i < inStr.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '1')
                {
                    tmpStr.Append(str[i].ToString());
                }
                else if (str[i] == ' ' && tmpStr.Length > 0)
                {
                    for (int i1 = 0; i1 < 8 - tmpStr.Length; i1++) outStr.Append("0");
                    outStr.Append(tmpStr);
                    outStr.Append(" ");
                    tmpStr.Length = 0;
                }
                if (tmpStr.Length == 8)
                {
                    outStr.Append(tmpStr);
                    outStr.Append(" ");
                    tmpStr.Length = 0;
                }
            }
            if (tmpStr.Length > 0)
            {
                for (int i = 0; i < 8 - tmpStr.Length; i++) outStr.Append("0");
                outStr.Append(tmpStr);
                outStr.Append(" ");
            }
            return outStr.ToString();
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
        StringBuilder hexStr = new StringBuilder();
        int i = 0;
        for (i = 0; i < byteArr.Length; i++)
        {
            hexStr.Append(byteArr[i].ToString("X2"));
            hexStr.Append(" ");
        }
        return hexStr.ToString();
    }

    public static string ConvertByteArrayToHex(byte[] byteArr, int Length)
    {
        if (Length > byteArr.Length) Length = byteArr.Length;
        StringBuilder hexStr = new StringBuilder();
        int i = 0;
        for (i = 0; i < Length; i++)
        {
            hexStr.Append(byteArr[i].ToString("X2"));
            hexStr.Append(" ");
        }
        return hexStr.ToString();
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

    public static long EvaluateVariables(string expression, string[] variables = null, string[] values = null)  //calculate string formula
    {
        if (variables != null)
        {
            if (variables.Length != values.Length) return 0;
            for (int i = 0; i < variables.Length; i++) expression = expression.Replace(variables[i], values[i]);
        }
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

    public bool ArrayEqual(byte[] a1, byte[] b1)
    {
        if (a1.Length != b1.Length)
        {
            return false;
        }

        for (int i = 0; i < a1.Length; i++)
        {
            if (a1[i] != b1[i])
            {
                return false;
            }
        }
        return true;
    }

}
