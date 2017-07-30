﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Laplace.Framework.Cryptography.DES
{
    public static  class Des
    {
        //默认密钥向量  
        private static byte[] Keys = { 0x21, 0x43, 0x65, 0x87, 0x09, 0xBA, 0xDC, 0xFE };
        //private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }; 

        public static string EncryptDes(string encryptString)
        {
            return EncryptDes(encryptString, string.Empty);
        }
        /// DES加密字符串          
        /// 待加密的字符串  
        /// 加密密钥,要求为8位  
        /// 加密成功返回加密后的字符串，失败返回源串   
        public static string EncryptDes(string encryptString, string encryptKey)
        {
            try
            {
                encryptKey = encryptKey + "www.litecms.cn";
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        public static string DecryptDes(string decryptString)
        {
            return DecryptDes(decryptString, string.Empty);
        }
        ///   
        /// DES解密字符串          
        /// 待解密的字符串  
        /// 解密密钥,要求为8位,和加密密钥相同  
        /// 解密成功返回解密后的字符串，失败返源串  
        public static string DecryptDes(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = decryptKey + "www.litecms.cn";
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }  
    }
}
