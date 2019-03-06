using System;

using System.Globalization;

namespace TB.AspNetCore.Domain.DataProtection
{
    /// <summary>
    /// base64 编码器
    /// 提供给base256 编码器使用
    /// modify by 鸟窝
    /// </summary>
    public class Base64Encoders
    {
        private static readonly byte[] EmptyBytes = new byte[0];

        /// <summary>
        /// Decodes a base64url-encoded string. 解码base64字符串
        /// </summary>
        /// <param name="input">The base64url-encoded input to decode.输入base64编码的字符串</param>
        /// <returns>The base64url-decoded form of the input.输出base64解码后的字符串</returns>
        /// <remarks>
        /// The input must not contain any whitespace or padding characters.输入不能包含任何空字符和填充字符
        /// Throws <see cref="T:System.FormatException" /> if the input is malformed. 否则抛出formatException异常
        /// </remarks>
        public static byte[] Base64Decode(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            return Base64Decode(input, 0, input.Length);
        }

        /// <summary>
        /// Decodes a base64url-encoded substring of a given string.解码给定字符串的base64url编码的子字符串
        /// </summary>
        /// <param name="input">A string containing the base64url-encoded input to decode.输入要解码的包含base64编码的字符串</param>
        /// <param name="offset">The position in <paramref name="input" /> at which decoding should begin.输入字符串要解码开始的位置</param>
        /// <param name="count">The number of characters in <paramref name="input" /> to decode.要解码字符串的个数</param>
        /// <returns>The base64url-decoded form of the input.输入的字符串base64解码后的形式</returns>
        /// <remarks>
        /// The input must not contain any whitespace or padding characters.
        /// Throws <see cref="T:System.FormatException" /> if the input is malformed.
        /// </remarks>
        public static byte[] Base64Decode(string input, int offset, int count)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            ValidateParameters(input.Length, "input", offset, count);
            if (count == 0)
            {
                return EmptyBytes;
            }
            char[] buffer = new char[GetArraySizeRequiredToDecode(count)];
            return Base64Decode(input, offset, buffer, 0, count);
        }

        /// <summary>
        /// Decodes a base64url-encoded <paramref name="input" /> into a <c>byte[]</c>.将base64编码的字符串解码为byte[]形式
        /// </summary>
        /// <param name="input">A string containing the base64url-encoded input to decode.输入包含base64编码的需要解码的字符串</param>
        /// <param name="offset">The position in <paramref name="input" /> at which decoding should begin.开始位置</param>
        /// <param name="buffer">
        /// Scratch buffer to hold the <see cref="T:System.Char" />s to decode. Array must be large enough to hold        
        /// <paramref name="bufferOffset" /> and <paramref name="count" /> characters as well as Base64 padding
        /// characters. Content is not preserved.
        /// </param>
        /// <param name="bufferOffset">
        /// The offset into <paramref name="buffer" /> at which to begin writing the <see cref="T:System.Char" />s to decode.
        /// </param>
        /// <paramref name ="bufferOffset"/>和<paramref name ="count"/>字符以及Base64填充字符。 内容未保留。
        /// <param name="count">The number of characters in <paramref name="input" /> to decode.</param>
        /// <returns>The base64url-decoded form of the <paramref name="input" />.</returns>
        /// <remarks>
        /// The input must not contain any whitespace or padding characters.
        /// Throws <see cref="T:System.FormatException" /> if the input is malformed.
        /// </remarks>
        public static byte[] Base64Decode(string input, int offset, char[] buffer, int bufferOffset, int count)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            ValidateParameters(input.Length, "input", offset, count);
            if (bufferOffset < 0)
            {
                throw new ArgumentOutOfRangeException("bufferOffset");
            }
            if (count == 0)
            {
                return EmptyBytes;
            }
            int num = GetNumBase64PaddingCharsToAddForDecode(count);
            int num2 = checked(count + num);
            if (buffer.Length - bufferOffset < num2)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid {0}, {1} or {2} length.", "count", "bufferOffset", "input"), "count");
            }
            int num3 = bufferOffset;
            int num4 = offset;
            while (num3 - bufferOffset < count)
            {
                char c = input[num4];
                switch (c)
                {
                    case '-':
                        buffer[num3] = '+';
                        break;
                    case '_':
                        buffer[num3] = '/';
                        break;
                    default:
                        buffer[num3] = c;
                        break;
                }
                num3++;
                num4++;
            }
            while (num > 0)
            {
                buffer[num3] = '=';
                num3++;
                num--;
            }
            return Convert.FromBase64CharArray(buffer, bufferOffset, num2);
        }

        /// <summary>
        /// Gets the minimum <c>char[]</c> size required for decoding of <paramref name="count" /> characters
        /// with the <see cref="!:Base64UrlDecode(string, int, char[], int, int)" /> method.
        /// </summary>
        /// <param name="count">The number of characters to decode.</param>
        /// <returns>
        /// The minimum <c>char[]</c> size required for decoding  of <paramref name="count" /> characters.
        /// </returns>
        public static int GetArraySizeRequiredToDecode(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (count == 0)
            {
                return 0;
            }
            int numBase64PaddingCharsToAddForDecode = GetNumBase64PaddingCharsToAddForDecode(count);
            return checked(count + numBase64PaddingCharsToAddForDecode);
        }

        /// <summary>
        /// Encodes <paramref name="input" /> using base64url encoding.
        /// </summary>
        /// <param name="input">The binary input to encode.</param>
        /// <returns>The base64url-encoded form of <paramref name="input" />.</returns>
        public static string Base64Encode(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            return Base64Encode(input, 0, input.Length);
        }

        /// <summary>
        /// Encodes <paramref name="input" /> using base64url encoding.
        /// </summary>
        /// <param name="input">The binary input to encode.</param>
        /// <param name="offset">The offset into <paramref name="input" /> at which to begin encoding.</param>
        /// <param name="count">The number of bytes from <paramref name="input" /> to encode.</param>
        /// <returns>The base64url-encoded form of <paramref name="input" />.</returns>
        public static string Base64Encode(byte[] input, int offset, int count)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            ValidateParameters(input.Length, "input", offset, count);
            if (count == 0)
            {
                return string.Empty;
            }
            char[] array = new char[GetArraySizeRequiredToEncode(count)];
            int length = Base64Encode(input, offset, array, 0, count);
            return new string(array, 0, length);
        }

        /// <summary>
        /// Encodes <paramref name="input" /> using base64url encoding.
        /// </summary>
        /// <param name="input">The binary input to encode.</param>
        /// <param name="offset">The offset into <paramref name="input" /> at which to begin encoding.</param>
        /// <param name="output">
        /// Buffer to receive the base64url-encoded form of <paramref name="input" />. Array must be large enough to
        /// hold <paramref name="outputOffset" /> characters and the full base64-encoded form of
        /// <paramref name="input" />, including padding characters.
        /// </param>
        /// <param name="outputOffset">
        /// The offset into <paramref name="output" /> at which to begin writing the base64url-encoded form of
        /// <paramref name="input" />.
        /// </param>
        /// <param name="count">The number of <c>byte</c>s from <paramref name="input" /> to encode.</param>
        /// <returns>
        /// The number of characters written to <paramref name="output" />, less any padding characters.
        /// </returns>
        public static int Base64Encode(byte[] input, int offset, char[] output, int outputOffset, int count)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            ValidateParameters(input.Length, "input", offset, count);
            if (outputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("outputOffset");
            }
            int arraySizeRequiredToEncode = GetArraySizeRequiredToEncode(count);
            if (output.Length - outputOffset < arraySizeRequiredToEncode)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid {0}, {1} or {2} length.", "count", "outputOffset", "output"), "count");
            }
            if (count == 0)
            {
                return 0;
            }
            int num = Convert.ToBase64CharArray(input, offset, count, output, outputOffset);
            for (int i = outputOffset; i - outputOffset < num; i++)
            {
                switch (output[i])
                {
                    case '+':
                        output[i] = '-';
                        break;
                    case '/':
                        output[i] = '_';
                        break;
                    case '=':
                        return i - outputOffset;
                }
            }
            return num;
        }

        /// <summary>
        /// Get the minimum output <c>char[]</c> size required for encoding <paramref name="count" />
        /// <see cref="T:System.Byte" />s with the <see cref="M:Microsoft.AspNetCore.DataProtection.Base64Encoders.Base64Encode(System.Byte[],System.Int32,System.Char[],System.Int32,System.Int32)" /> method.
        /// </summary>
        /// <param name="count">The number of characters to encode.</param>
        /// <returns>
        /// The minimum output <c>char[]</c> size required for encoding <paramref name="count" /> <see cref="T:System.Byte" />s.
        /// </returns>
        public static int GetArraySizeRequiredToEncode(int count)
        {
            return checked(unchecked(checked(count + 2) / 3) * 4);
        }

        /// <summary>
        /// 获取Base64填充字符串数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static int GetNumBase64PaddingCharsInString(string str)
        {
            if (str[str.Length - 1] == '=')
            {
                if (str[str.Length - 2] == '=')
                {
                    return 2;
                }
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 获取Base64填充字符以添加解码数
        /// </summary>
        /// <param name="inputLength"></param>
        /// <returns></returns>
        private static int GetNumBase64PaddingCharsToAddForDecode(int inputLength)
        {
            switch (inputLength % 4)
            {
                case 0:
                    return 0;
                case 2:
                    return 2;
                case 3:
                    return 1;
                default:
                    throw new FormatException("TODO: Malformed input.");
            }
        }
        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="bufferLength">大字符串长度</param>
        /// <param name="inputName">输入名称</param>
        /// <param name="offset">偏移量/开始截取的位置</param>
        /// <param name="count">截取数量</param>
        private static void ValidateParameters(int bufferLength, string inputName, int offset, int count)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (bufferLength - offset >= count)
            {
                return;
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid {0}, {1} or {2} length.", "count", "offset", inputName), "count");
        }
    }
}
