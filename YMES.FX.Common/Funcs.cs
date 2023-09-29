using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMES.FX.Common
{
    public class Funcs
    {
        public static int GetO2I(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                try
                {
                    if (obj is string)
                    {
                        obj = obj.ToString().Replace(",", "");
                    }
                    if (string.IsNullOrEmpty(obj.ToString()))
                    {
                        return 0;
                    }
                    return Convert.ToInt32(obj);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static double GetO2D(object obj)
        {
            if (obj == null)
            {
                return 0.0;
            }

            else if (obj.ToString() == "")

            {

                return 0.0;
            }
            else
            {
                try
                {
                    return Convert.ToDouble(obj);
                }
                catch
                {
                    return 0.0;
                }
            }
        }
        public static string GetO2S(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                try
                {
                    return Convert.ToString(obj);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public static byte[] StreamToByte(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

    }
}
