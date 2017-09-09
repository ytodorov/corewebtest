using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace DimoPdfToExcelWeb.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Превръша съответния string в long. Ако не успее превръщането се връща резултат 0.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this string value)
        {
            long result;
            long.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Превръша съответния string в int. Ако не успее превръщането се връща резултат 0.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            int result;
            int.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Превръша съответния string в double. Ако не успее превръщането се връща резултат 0.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            double result;
            double.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Превръша съответния string в decimal. Ако не успее превръщането се връща резултат 0.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            decimal result;
            decimal.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Превръша съответния string в DateTime. Ако не успее превръщането се връща резултат DateTime.Now.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            DateTime result;
            if (!DateTime.TryParse(value, out result))
            {
                result = DateTime.Now;
            }

            return result;
        }

        /// <summary>
        /// Превръша съответния string в bool. Ако не успее превръщането се връща резултат false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string value)
        {
            bool result;
            bool.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Encodes the input value to a Base64 string using the default encoding.
        /// </summary>
        /// <param name = "value">The input value.</param>
        /// <returns>The Base 64 encoded string</returns>
        public static string EncodeBase64Safe(this string value)
        {
            //value = value.Replace("+", "_PLUS_").Replace("/", "_BACKSLASH_").Replace("=", "_EQUALS_");
            return value.EncodeBase64Safe(null);
        }

        /// <summary>
        /// 	Encodes the input value to a Base64 string using the supplied encoding.
        /// </summary>
        /// <param name = "value">The input value.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <returns>The Base 64 encoded string</returns>
        public static string EncodeBase64Safe(this string value, Encoding encoding)
        {
            value = value.Replace("+", "_PLUS_").Replace("/", "_BACKSLASH_").Replace("=", "_EQUALS_");
            encoding = (encoding ?? Encoding.UTF8);
            var bytes = encoding.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 	Decodes a Base 64 encoded value to a string using the default encoding.
        /// </summary>
        /// <param name = "encodedValue">The Base 64 encoded value.</param>
        /// <returns>The decoded string</returns>
        public static string DecodeBase64Safe(this string encodedValue)
        {
            //encodedValue = encodedValue.Replace("_PLUS_", "+").Replace("_BACKSLASH_", "/").Replace("_EQUALS_", "=");
            return encodedValue.DecodeBase64Safe(null);
        }

        /// <summary>
        /// 	Decodes a Base 64 encoded value to a string using the supplied encoding.
        /// </summary>
        /// <param name = "encodedValue">The Base 64 encoded value.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <returns>The decoded string</returns>
        public static string DecodeBase64Safe(this string encodedValue, Encoding encoding)
        {
            encoding = (encoding ?? Encoding.UTF8);
            var bytes = Convert.FromBase64String(encodedValue);
            var res = encoding.GetString(bytes);
            res = res.Replace("_PLUS_", "+").Replace("_BACKSLASH_", "/").Replace("_EQUALS_", "=");
            return res;
        }


        public static string Compress(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }

                ms.Position = 0;
                using (MemoryStream outStream = new MemoryStream())
                {
                    byte[] compressed = new byte[ms.Length];
                    ms.Read(compressed, 0, compressed.Length);

                    byte[] gzBuffer = new byte[compressed.Length + 4];
                    Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                    Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
                    string result = Convert.ToBase64String(gzBuffer);
                    return result;
                }
            }
        }

        public static string Decompress(this string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                string result = Encoding.UTF8.GetString(buffer);
                return result;
            }

        }

        public static bool IsValidEmailAddress(this string email)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(email))
            {
                result = Regex.IsMatch(email,
                    @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
                    RegexOptions.IgnoreCase);
            }
            return result;
        }

        public static bool IsValidWebsiteUrl(this string website)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(website))
            {
                result = Regex.IsMatch(website,
                    @"^((http|https|www):\/\/)?([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*(\.\w+))",
                RegexOptions.IgnoreCase);
            }
            return result;
        }

        public static string TryMakeValidTelephoneNumber(this string numberToFix)
        {
            if (string.IsNullOrWhiteSpace(numberToFix))
            {
                return numberToFix;
            }
            StringBuilder resultSb = new StringBuilder();
            List<char> validChars = new List<char>() { '+', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            
            foreach (char c in numberToFix)
            {
                if (validChars.Contains(c))
                {
                    resultSb.Append(c);
                }
            }

            string validTelephone = resultSb.ToString();
            return validTelephone;
        }

        public static bool IsValidTelephoneAndFaxNumber(this string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return true;
            }
            if (number.StartsWith("+"))
            {
                number = number.Remove(0, 1);

                var chars = number.ToCharArray();

                for (int c = 0; c < chars.Length; c++)
                {

                    if (!char.IsDigit(chars[c]))
                    {
                        return false;
                    }


                }
            }
            else
            {
                var chars = number.ToCharArray();

                for (int c = 0; c < chars.Length; c++)
                {

                    if (!char.IsDigit(chars[c]))
                    {
                        return false;
                    }


                }
            }
            return true;
        }
    

        public static bool IsValidBulstat(this string bulstat)
        {
            long bulstatAsInt = 0;
            if (long.TryParse(bulstat, out bulstatAsInt) == false)
            {
                return false;
            }
            if (bulstat.Length == 9)
            {
                var ninthDigit = int.Parse(bulstat[8].ToString());
                var remainder = GetEightDigitRemainder(bulstat.Substring(0, 8), 0);
                if ((remainder != 10) && (remainder != ninthDigit))
                {
                    return false;
                }
                if (remainder == 10)
                {
                    if (ninthDigit == 0)
                    {
                        return true;
                    }
                    remainder = GetEightDigitRemainder(bulstat.Substring(0, 8), 2);
                    remainder %= 10;
                    if (remainder != ninthDigit)
                    {
                        return false;
                    }
                }
                return true;
            }
            if (bulstat.Length == 13)
            {
                string firstNineLetters = bulstat.Substring(0, 9);
                if (!firstNineLetters.IsValidBulstat())
                {
                    return false;
                }
                int thirteenthDigit = int.Parse(bulstat[12].ToString());
                int remainder = GetFourDigitRemainder(bulstat.Substring(8, 4), 0);
                if ((remainder != 10) && (remainder != thirteenthDigit))
                {
                    return false;
                }
                if (remainder == 10)
                {
                    if (thirteenthDigit == 0)
                    {
                        return true;
                    }
                    remainder = GetFourDigitRemainder(bulstat.Substring(8, 4), 2);
                    remainder %= 10;
                    if (remainder != thirteenthDigit)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool IsValidEGN(this string egn)
        {
            if (egn == null)
            {
                return false;
            }
            if (egn.Length == 0)
            {
                return false;
            }
            int[] digits = new int[10];
            int lastDigit = 0;
            int polinom = 0;
            bool result = false;

            // Checking Social number Lenght
            if (egn.Length != 10)
            {
                return false;
            }

            // Checking if Social number consist of only deciaml digits

            long longEgn;
            if (!long.TryParse(egn, out longEgn))
            {
                return false;
            }          

            // Checking for valid Social number Date
            int year = int.Parse(egn.Substring(0, 2));
            int month = int.Parse(egn.Substring(2, 2));
            int day = int.Parse(egn.Substring(4, 2));

            if (month >= 40)
            {
                //if (!
                //    (
                //    (1 <= 2000 + year && 2000 + year <= 9999) &&
                //    (1 <= month - 40 && month - 40 <= 12) && 
                //    ))

                try
                {
                    DateTime date = new DateTime(2000 + year, month - 40, day);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    DateTime date = new DateTime(1900 + year, month, day);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            // Checking for valid Social number check sum (last digit);
            for (int i = 0; i < 10; i++)
            {
                digits[i] = int.Parse(egn[i].ToString());
            }
            polinom = 2 * digits[8];
            for (int i = 7; i >= 0; i--)
            {
                polinom = 2 * (digits[i] + polinom);
            }
            lastDigit = polinom % 11;
            if (lastDigit == 10)
            {
                lastDigit = 0;
            }
            result = (lastDigit == digits[9]);
            return result;
        }

        public static bool IsValidLNCH(this string lnch)
        {
            int[] digits = new int[10];
            int lastDigit = 0;
            int sum = 0;
            bool result = false;
            if (lnch.Length != 10)
            {
                return false;
            }
            long helperLnch;
            if (!long.TryParse(lnch, out helperLnch))
            {
                return false;
            }        
            for (int i = 0; i < 10; i++)
            {
                digits[i] = int.Parse(lnch[i].ToString());
            }
            sum += digits[0] * 21;
            sum += digits[1] * 19;
            sum += digits[2] * 17;
            sum += digits[3] * 13;
            sum += digits[4] * 11;
            sum += digits[5] * 9;
            sum += digits[6] * 7;
            sum += digits[7] * 3;
            sum += digits[8];
            lastDigit = sum % 10;
            result = (lastDigit == digits[9]);
            return result;
        }

        private static int GetEightDigitRemainder(string digits, int addition)
        {
            int multiplicator = 1 + addition;
            int result = 0;

            foreach (char ch in digits)
            {
                result += int.Parse(ch.ToString()) * multiplicator;
                multiplicator++;
            }
            result %= 11;
            return result;
        }

        private static int GetFourDigitRemainder(string digits, int addition)
        {
            int[] multiplicator = { 2 + addition, 7 + addition, 3 + addition, 5 + addition };
            int result = 0;

            for (int digit = 0; digit < digits.Length; digit++)
            {
                result += int.Parse(digits[digit].ToString()) * multiplicator[digit];
            }
            result %= 11;
            return result;
        }

        /// <summary>
        /// Премахва последните символи от стринга до избраната дължина и след това
        /// заменя последните два символа от стринга с точки.
        /// Например: 'testing tester' -> 'testing test..'
        /// </summary>
        /// <param name="text">Низът който трябва да се промени</param>
        /// <param name="length">Дължината, до която трябва да се сведе низа</param>
        /// <returns></returns>
        public static string CutAndSetDotsAtEnd(this string text, int length)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            if (text.Length <= length)
            {
                return text;
            }
            else if (text.Length <= length + 2)
            {
                return text.Remove(text.Length - 2, 2) + "..";
            }
            return text.Substring(0, length).Remove(length - 2, 2) + "..";
        }

        /// <summary>
        /// Променя символи като /r и /n към br таг
        /// </summary>
        /// <param name="text">Низът, който ще се промени</param>
        /// <returns>Стринг със html символи за нов ред</returns>
        public static string ChangeNewLineForHtml(this string text)
        {
            var result = text;
            result = result.Replace("\r\n", "<br/>");
            result = result.Replace("\r", "<br/>");
            result = result.Replace("\n", "<br/>");

            return result;
        }

        /// <summary>
        /// Променя низа, така че да започва с главна буква, а всички останали да са малки
        /// </summary>
        /// <param name="str">Низът, който ще се промени</param>
        /// <returns>Низ, започващ с главна буква</returns>
        public static string FirstLetterToUpper(this string str)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length == 1)
            {
                return str.ToUpper();
            }

            return char.ToUpper(str[0]) + str.Substring(1).ToLower();

        }

        /// <summary>
        /// Конвертиране на имена от кирилица към латиница.
        /// </summary>
        /// <param name="str">Низът, който ще се промени</param>
        /// <returns>Низ на латиница</returns>
        public static string ConvertFromCyrillicToLatin(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                string[] latin_up = { "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Sht", "A", "'", "Y", "E", "Yu", "Ya" };
                string[] latin_low = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "sht", "a", "'", "y", "e", "yu", "ya" };
                string[] cyrillic_up = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
                string[] cyrillic_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
                for (int i = 0; i < cyrillic_low.Length; i++)
                {
                    str = str.Replace(cyrillic_up[i], latin_up[i]);
                    str = str.Replace(cyrillic_low[i], latin_low[i]);
                }
            }
            return str;
        }
    }
}
