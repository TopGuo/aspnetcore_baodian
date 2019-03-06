using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TB.AspNetCore.Domain.DataProtection
{
    public class Base256Encoders
    {
        public static readonly UTF8Encoding SecureUtf8Encoding = new UTF8Encoding(false, true);

        public const string Base256Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ";

        private const string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        public static string EncryptToBase64(string plainInput, string confusionCode)
        {
            if (string.IsNullOrEmpty(plainInput))
            {
                return string.Empty;
            }
            return Base64Encoders.Base64Encode(EncryptToBase256(SecureUtf8Encoding.GetBytes(plainInput), confusionCode));
        }

        public static string EncryptToBase256(string plainInput, string confusionCode)
        {
            if (string.IsNullOrEmpty(plainInput))
            {
                return string.Empty;
            }
            byte[] bytes = SecureUtf8Encoding.GetBytes(plainInput);
            char[] array = new char[bytes.Length];
            EncryptToBase256(bytes, confusionCode, array);
            string result = new string(array, 0, array.Length);
            array = new char[0];
            return result;
        }

        public static byte[] EncryptToBase256(byte[] plainBytes, string confusionCode)
        {
            char[] output = null;
            return EncryptToBase256(plainBytes, confusionCode, output);
        }

        public static byte[] EncryptToBase256(byte[] plainBytes, string confusionCode, char[] output)
        {
            if (plainBytes != null && plainBytes.Length != 0)
            {
                CheckIsBase256Code(confusionCode);
                byte[] array = new byte[plainBytes.Length];
                if (output != null && output.Length != plainBytes.Length)
                {
                    output = new char[plainBytes.Length];
                }
                for (int i = 0; i < plainBytes.Length; i++)
                {
                    int num = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ".IndexOf(confusionCode[plainBytes[i]]);
                    if (output != null)
                    {
                        output[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ"[num];
                    }
                    array[i] = (byte)num;
                }
                return array;
            }
            return new byte[0];
        }

        public static string DecryptFromBase64(string encryptText, string confusionCode)
        {
            if (string.IsNullOrEmpty(encryptText))
            {
                return string.Empty;
            }
            CheckIsBase64(encryptText);
            byte[] bytes = DecryptFromBase256(Base64Encoders.Base64Decode(encryptText), confusionCode);
            return SecureUtf8Encoding.GetString(bytes);
        }

        public static string DecryptFromBase256(string encryptText, string confusionCode)
        {
            if (string.IsNullOrEmpty(encryptText))
            {
                return string.Empty;
            }
            CheckIsBase256(encryptText);
            byte[] array = new byte[encryptText.Length];
            for (int i = 0; i < array.Length; i++)
            {
                int num = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ".IndexOf(encryptText[i]);
                array[i] = (byte)num;
            }
            byte[] bytes = DecryptFromBase256(array, confusionCode);
            return SecureUtf8Encoding.GetString(bytes);
        }

        public static byte[] DecryptFromBase256(byte[] encryptBytes, string confusionCode)
        {
            if (encryptBytes != null && encryptBytes.Length != 0)
            {
                CheckIsBase256Code(confusionCode);
                byte[] array = new byte[encryptBytes.Length];
                for (int i = 0; i < encryptBytes.Length; i++)
                {
                    char value = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ"[encryptBytes[i]];
                    array[i] = (byte)confusionCode.IndexOf(value);
                    if (array[i] < 1)
                    {
                        throw new ArgumentOutOfRangeException("Unsupport");
                    }
                }
                return array;
            }
            return new byte[0];
        }

        /// <summary>
        /// Check whether the the confusion string codes is valid Base256
        /// </summary>
        /// <param name="confusionCode"></param>
        protected static void CheckIsBase256Code(string confusionCode)
        {
            if (string.IsNullOrEmpty(confusionCode))
            {
                throw new ArgumentNullException("confusionCode");
            }
            if (confusionCode.Length == 256)
            {
                return;
            }
            throw new ArgumentException($"Invalid confusion code Length: parameter => confusionCode, length => {confusionCode.Length}");
        }

        protected static void CheckIsBase256(string code)
        {
            if (code.IndexOf("=") <= -1 && code.IndexOf("+") <= -1 && code.IndexOf("/") <= -1)
            {
                return;
            }
            throw new NotSupportedException("Invalid base256 code.");
        }

        protected static void CheckIsBase64(string code)
        {
            if (!code.Any((char t) => t > 'Ā'))
            {
                return;
            }
            throw new NotSupportedException("Invalid base64 code.");
        }

        /// <summary>
        /// Create new confusion string codes
        /// </summary>
        /// <returns></returns>
        public static string CreateConfusionCodes()
        {
            List<int> list = (from t in Enumerable.Range(0, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ".Length)
                              orderby Guid.NewGuid()
                              select t).ToList();
            new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ".Length);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ".Length; i++)
            {
                int index = list[i];
                stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789†‡ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſ"[index]);
            }
            return stringBuilder.ToString();
        }
    }
}
