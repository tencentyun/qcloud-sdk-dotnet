using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace COSXML.Utils
{
    public sealed class DigestUtils
    {
        public static string GetSha1ToHexString(string content, Encoding encode)
        {

            return GetSha1ToHexString(encode.GetBytes(content));
        }

        public static string GetSha1ToHexString(byte[] content)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] result = sha1.ComputeHash(content);
            sha1.Clear();
            var hexStr = new StringBuilder();

            foreach (byte b in result)
            {
                // to lower
                // to lower
                hexStr.Append(b.ToString("x2"));
            }

            return hexStr.ToString();
        }

        // public static string GetSha1ToHexString(Stream inputStream)
        // {
        //     SHA1 sha1 = new SHA1CryptoServiceProvider();

        //     byte[] result = sha1.ComputeHash(inputStream);
        //     sha1.Clear();
        //     var hexStr = new StringBuilder();

        //     foreach (byte b in result)
        //     {
        //         // to lower
        //         // to lower
        //         hexStr.Append(b.ToString("x2"));
        //     }

        //     return hexStr.ToString();
        // }

        public static string GetHamcSha1ToHexString(string content, Encoding contentEncoding, string key, Encoding keyEncoding)
        {
            HMACSHA1 hmacSha1 = new HMACSHA1(keyEncoding.GetBytes(key));

            byte[] result = hmacSha1.ComputeHash(contentEncoding.GetBytes(content));
            hmacSha1.Clear();
            var hexStr = new StringBuilder();

            foreach (byte b in result)
            {
                // to lower
                // to lower
                hexStr.Append(b.ToString("x2"));
            }

            return hexStr.ToString();

        }

        public static string GetHamcSha1ToBase64(string content, Encoding contentEncoding, string key, Encoding keyEncoding)
        {
            HMACSHA1 hmacSha1 = new HMACSHA1(keyEncoding.GetBytes(key));

            byte[] result = hmacSha1.ComputeHash(contentEncoding.GetBytes(content));
            hmacSha1.Clear();

            return Convert.ToBase64String(result);
        }

        public static string GetMd5ToBase64(string content, Encoding encoding)
        {

            return GetMd5ToBase64(encoding.GetBytes(content));
        }

        public static string GetMd5ToBase64(byte[] content)
        {
            MD5 md5 = MD5.Create();

            byte[] result = md5.ComputeHash(content);
            md5.Clear();

            return Convert.ToBase64String(result);
        }

        public static string GetMd5ToBase64(Stream inStream)
        {
            MD5 md5 = MD5.Create();

            byte[] result = md5.ComputeHash(inStream);
            md5.Clear();

            return Convert.ToBase64String(result);
        }

        public static string GetMd5ToBase64(Stream inStream, long size)
        {
            int bufferSize = 1024 * 16;

            byte[] buffer = new byte[bufferSize];
            int readLength = 0;
            long total = 0L;
            byte[] data = new byte[bufferSize];
            MD5 md5 = MD5.Create();
            int count = (int)(size - total);

            while ((readLength = inStream.Read(buffer, 0, count > buffer.Length ? buffer.Length : count)) > 0)
            {
                md5.TransformBlock(buffer, 0, readLength, data, 0);
                total += readLength;
                count = (int)(size - total);

                if (count <= 0)
                {
                    break;
                }
            }

            md5.TransformFinalBlock(buffer, 0, 0);
            string result = Convert.ToBase64String(md5.Hash);
            md5.Clear();

            return result;
        }

        public static string GetBase64(string content, Encoding encoding)
        {
            byte[] result = encoding.GetBytes(content);

            return Convert.ToBase64String(result);
        }
    }
}
