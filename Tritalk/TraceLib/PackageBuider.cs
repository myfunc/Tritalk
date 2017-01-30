using System;
using System.Collections.Generic;
using System.Text;

namespace Tritalk.Libs
{
    /* Конструктор пакетов.
     * Создает пакет по протоколу
     * {
     * 4 bytes: int size; в байтах
     * n bytes: input bytes; 
     * }
     * */
    static public class PackageBuider
    {
        const int m_bytes_for_size = 4;
        public static byte[] AddSizeToByteArray(byte[] bytes)
        {
            int package_size = bytes.Length;
            byte[] package = new byte[m_bytes_for_size + package_size];
            byte[] package_size_in_bytes = BitConverter.GetBytes(package_size);
            //Array.Reverse(package_size_in_bytes);
            for (int i = 0; i < m_bytes_for_size; i++)
            {
                package[i] = package_size_in_bytes[i];
            }
            for (int i = 0; i < package_size; i++)
            {
                package[i + m_bytes_for_size] = bytes[i];
            }
            return package;
        }

        public static int GetSizeFromPackage(byte[] bytes)
        {
            try
            {
                byte[] byte_size = new byte[4];
                Array.Copy(bytes, byte_size, 4);
                Array.Reverse(byte_size);
                return BitConverter.ToInt32(byte_size, 0);
            }
            catch
            {
                return -1;
            }
        }
    }
}
