using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.PLC.Base
{
    public class Common
    {
        /// <summary>
        /// PLC Common Data Type(LEGACY)
        /// </summary>
        public struct PLC_DeviceArray
        {
            public short[] iBitValue;
            public short iWordValue;
        }
        public static byte[] GetByteFromLong(long nVal, int maxLen)
        {
            byte[] ret = new byte[maxLen];
            byte[] byVal = BitConverter.GetBytes(nVal);
            for (int i = 0; i < maxLen; i++)
            {
                ret[i] = byVal[i];
            }
            return ret;
        }
        public static bool GetBoolStr(string strbool)
        {
            strbool = strbool.Replace(" ", "").Trim().ToUpper();

            switch (strbool)
            {
                case "Y":
                case "TRUE":
                case "T":
                case "1":
                case "YES":
                case "01":
                    return true;

            }
            return false;

        }
        public static byte[] GetByte(string val)
        {
            val = Common.Reverse(val);
            int rest = val.Length % 2;
            int max = val.Length / 2 + rest;

            byte[] byRet = new byte[max];

            for (int i = 0; i < max; i++)
            {
                if (rest > 0 && i == max - 1)
                {
                    byRet[i] = byte.Parse(Reverse(val.Substring(i * 2, 1)), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    byRet[i] = byte.Parse(Reverse(val.Substring(i * 2, 2)), System.Globalization.NumberStyles.HexNumber);
                }

            }

            return byRet;
        }
        /// <summary>
        /// Clear PLC buffer 
        /// </summary>
        /// <param name="mem">PLC buffer</param>
        public static void MemSet(ref Common.PLC_DeviceArray[] mem)
        {
            try
            {
                for (int iLoop1 = 0; iLoop1 < mem.Length; ++iLoop1)
                {
                    mem[iLoop1].iWordValue = 0;


                    mem[iLoop1].iBitValue = new short[16];
                    for (int iLoop2 = 0; iLoop2 < 16; ++iLoop2)
                    {
                        mem[iLoop1].iBitValue[iLoop2] = 0;
                    }
                }
            }
            catch (Exception eLog)
            {
                throw new Exception(eLog.Message);
            }
        }

        /// <summary>
        /// Convert string to PLC DATA STRUCTURE(LEGACY)
        /// </summary>
        /// <param name="strReadBuf">original value</param>
        /// <param name="buf">converted value</param>
        /// <param name="useReverse"></param>
        /// <returns>Conversion condition</returns>
        public static bool SplitValue(string strReadBuf, ref Common.PLC_DeviceArray[] buf, bool useReverse = true)
        {
            if (strReadBuf.Length % 4 != 0)
            {   //devide by word(4byte)
                return false;
            }
            bool bRet = false;
            try
            {
                MemSet(ref buf);

                int maxSize = strReadBuf.Length / 4;

                for (int i = 0; i < maxSize; i++)
                {
                    if (buf.Length > i)
                    {
                        int nWord = 0;
                        if (useReverse)
                        {
                            nWord = int.Parse(strReadBuf.Substring(i * 4 + 2, 2) + strReadBuf.Substring(i * 4, 2), System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            nWord = int.Parse(strReadBuf.Substring(i * 4, 2) + strReadBuf.Substring(i * 4 + 2, 2), System.Globalization.NumberStyles.HexNumber);
                        }
                        buf[i].iWordValue = (short)nWord;



                        string binary = Convert.ToString(nWord, 2).PadLeft(16, '0');
                        string rBinary = Common.Reverse(binary);

                        for (int j = 0; j < 16; j++)
                        {
                            buf[i].iBitValue[j] = Convert.ToByte(rBinary.Substring(j, 1));
                        }
                    }
                }
                bRet = true;
            }
            catch
            {

            }
            return bRet;
        }
        public static string ToHexString(int by1)
        {

            byte[] bytes = BitConverter.GetBytes(by1);

            string hexString = "";
            hexString += bytes[0].ToString("X");
            return hexString;
        }
        public static string MakeBinaryString(string strValue)
        {
            try
            {
                string strBinary = "";

                for (int iLoop = strValue.Length - 1; iLoop >= 0; iLoop--)
                {
                    string strDigit = strValue.Substring(iLoop, 1);

                    int iValue = Convert.ToInt32(strDigit, 16);

                    string strBitValue = Convert.ToString(iValue, 2);

                    if (strBitValue.Length < 4)
                    {
                        strBitValue = new string('0', 4 - strBitValue.Length) + strBitValue;
                    }

                    strBinary = strBitValue + strBinary;
                }

                return strBinary;
            }
            catch
            {
                return "";
            }
        }
        public static string ByteToString(byte[] buffer, int startPOS = 0, int len = 0)
        {
            string ret = "";
            int idx = 0;
            int idx_len = 0;
            foreach (byte by in buffer)
            {
                if (len != 0 && len <= idx_len)
                {
                    break;
                }
                if (idx >= startPOS)
                {
                    ret += string.Format("{0:X2}", by);
                    idx_len++;
                }
                idx++;
            }
            return ret;
        }
        /// <summary>
        /// Couple left byte value and right byte value
        /// </summary>
        /// <param name="lByte">left value</param>
        /// <param name="rByte">right value</param>
        /// <returns>left value + right value</returns>
        public static byte[] CatByte(byte[] lByte, byte[] rByte)
        {
            byte[] ret = new byte[lByte.Length + rByte.Length];
            int idx = 0;
            for (int i = 0; i < lByte.Length; i++)
            {
                ret[idx] = lByte[i];
                idx++;
            }

            for (int i = 0; i < rByte.Length; i++)
            {
                ret[idx] = rByte[i];
                idx++;
            }
            return ret;
        }
        /// <summary>
        /// Reverse the string value
        /// </summary>
        /// <param name="s">orginal value</param>
        /// <returns>reversed value</returns>
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Data list combine with string
        /// </summary>
        /// <param name="lst">Data List</param>
        /// <param name="val">String Value</param>
        public static void CatListString(ref List<byte> lst, string val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                lst.Add((byte)Convert.ToChar(val.Substring(i, 1)));
            }
        }


    }
}
