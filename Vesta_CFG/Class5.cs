using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vesta_CFG
{
    class Class5
    {
        public static byte[] smethod_0(string string_0)
        {
            char[] array = string_0.ToCharArray();
            int upperBound = array.GetUpperBound(0);
            checked
            {
                byte[] array2 = new byte[upperBound + 1];
                int upperBound2 = array.GetUpperBound(0);
                for (int i = 0; i <= upperBound2; i++)
                {
                    array2[i] = (byte)Strings.Asc(array[i]);
                }
                SHA512Managed sha512Managed = new SHA512Managed();
                byte[] array3 = sha512Managed.ComputeHash(array2);
                byte[] array4 = new byte[16];
                int num = 32;
                do
                {
                    array4[num - 32] = array3[num];
                    num++;
                }
                while (num <= 47);
                return array4;
            }
        }

        // Token: 0x06000046 RID: 70 RVA: 0x00004758 File Offset: 0x00002958
        public static byte[] smethod_1(string string_0)
        {
            char[] array = string_0.ToCharArray();
            int upperBound = array.GetUpperBound(0);
            checked
            {
                byte[] array2 = new byte[upperBound + 1];
                int upperBound2 = array.GetUpperBound(0);
                for (int i = 0; i <= upperBound2; i++)
                {
                    array2[i] = (byte)Strings.Asc(array[i]);
                }
                SHA512Managed sha512Managed = new SHA512Managed();
                byte[] array3 = sha512Managed.ComputeHash(array2);
                byte[] array4 = new byte[32];
                int num = 0;
                do
                {
                    array4[num] = array3[num];
                    num++;
                }
                while (num <= 31);
                return array4;
            }
        }
        public enum Enum0
        {

        }
        public static void smethod_2(Stream object_0, string string_0, string string_1, Enum0 enum0_0, object object_1)
        {
            if (!Directory.Exists(Path.GetDirectoryName(string_1)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(string_1));
            }
            FileStream output_stream = new FileStream(string_1, FileMode.Create, FileAccess.Write);
            byte[] buffer = new byte[4097];
            long num = 0L;
            long length = object_0.Length;
            CryptoStream cryptoStream = null;
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            if (enum0_0 != (Enum0)1)
            {
                if (enum0_0 == (Enum0)2)
                {
                    cryptoStream = new CryptoStream(output_stream, rijndaelManaged.CreateDecryptor(smethod_1(string_0), smethod_0(string_0)), CryptoStreamMode.Write);
                }
            }
            else
            {
                cryptoStream = new CryptoStream(output_stream, rijndaelManaged.CreateEncryptor(smethod_1(string_0), smethod_0(string_0)), CryptoStreamMode.Write);
            }
            object_0.Position = 0L;
            checked
            {
                while (num < length)
                {
                    int num2 = object_0.Read(buffer, 0, 4096);
                    cryptoStream.Write(buffer, 0, num2);
                    num += unchecked((long)num2);
                    if (object_1 != null)
                    {
                        NewLateBinding.LateSet(object_1, null, "EditValue", new object[]
                        {
                        (int)Math.Round(unchecked((double)num / (double)length * 100.0))
                        }, null, null);
                    }
                    Application.DoEvents();
                }
            }
            object_0.Close();
            cryptoStream?.Close();
            output_stream.Close();
        }
        public static MemoryStream smethod_3(string string_0)
        {
            BinaryReader binaryReader = new BinaryReader(File.OpenRead(string_0));
            byte[] array = binaryReader.ReadBytes(checked((int)binaryReader.BaseStream.Length));
            MemoryStream memoryStream = new MemoryStream(array, 0, array.Length);
            memoryStream.Write(array, 0, array.Length);
            binaryReader.Close();
            return memoryStream;
        }
        public static void smethod_4(string string_0, Stream object_0)
        {
            using (FileStream fileStream = new FileStream(string_0, FileMode.Create, FileAccess.Write))
            {
                object_0.CopyTo(fileStream);
            }
            object_0.Close();
        }
        public static MemoryStream OwiicddWE(Stream object_0, string string_0, Class5.Enum0 enum0_0, object object_1)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Padding = PaddingMode.Zeros;
            byte[] buffer = new byte[4097];
            long num = 0L;
            long length = object_0.Length;
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(Class5.smethod_1(string_0), Class5.smethod_0(string_0)), CryptoStreamMode.Write);
            object_0.Position = 0L;
            checked
            {
                while (num < length)
                {
                    int num2 = object_0.Read(buffer, 0, 4096);
                    cryptoStream.Write(buffer, 0, num2);
                    num += unchecked((long)num2);
                    if (object_1 != null)
                    {
                        NewLateBinding.LateSet(object_1, null, "EditValue", new object[]
                        {
                        (int)Math.Round(unchecked((double)num / (double)length * 100.0))
                        }, null, null);
                    }
                    Application.DoEvents();
                }
                object_0.Close();
                return memoryStream;
            }
        }
    }
}
