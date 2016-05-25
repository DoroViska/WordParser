using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{
    public static class WordsParser
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double FastPow(int num, int exp)
        {
            double result = 1.0;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= num;
                exp >>= 1;
                num *= num;
            }
            return result;
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string WordStringValue(ref string inpu, int wordIndex)
        {
            return WordStringValue(ref inpu, wordIndex, WordLen(ref inpu, wordIndex));
        }
        public static string WordStringValue(ref string inpu, int wordIndex, int input_length)
        {
            StringBuilder bl = new StringBuilder(input_length);


            for (int i = 0; i < input_length; i++)
            {
                if (wordIndex + i + 1 > inpu.Length)
                    return null;
                bl.Append(inpu[wordIndex + i]);
            }
      
            return bl.ToString();
        }
       // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WordIntValue(ref string inpu, int wordIndex)
        {
            return WordIntValue(ref inpu, wordIndex, WordLen(ref inpu, wordIndex));
        }
        public static int WordIntValue(ref string inpu, int wordIndex, int input_length)
        {
            int rez = 0;
            int ms = 1;

            for (int a = 0; a < input_length; a++)
            {
                int dec_idx = wordIndex + input_length - 1 - a;
                if (inpu[dec_idx] == '-')
                {
                    rez = rez * -1;
                    continue;
                }

                if (char.IsDigit(inpu[dec_idx]))
                {
                    int nmul = inpu[dec_idx] - '0';
                    rez += nmul * ms;

                }
                ms = ms * 10;
            }
            return rez;
        }

       // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float WordFloatValue(ref string inpu, int wordIndex)
        {
            return WordFloatValue(ref inpu, wordIndex, WordLen(ref inpu, wordIndex));
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float WordFloatValue(ref string inpu, int wordIndex, int input_length)
        {
            return (float)WordDoubleValue(ref inpu, wordIndex, input_length);
        }


        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double WordDoubleValue(ref string inpu, int wordIndex)
        {
            return WordDoubleValue(ref inpu, wordIndex, WordLen(ref inpu ,wordIndex));
        }

        public static double WordDoubleValue(ref string inpu, int wordIndex, int input_length)
        {
            double n = 0;
            int decimalPosition = input_length;
            bool separatorFound = false;
            bool negative = inpu[wordIndex + 0] == '-';
            for (int k = (negative ? 1 : 0); k < input_length; k++)
            {
                char c = inpu[wordIndex + k];

                if (c == '.' || c == ',')
                {
                    if (separatorFound)
                        return Double.NaN;
                    decimalPosition = k + 1;
                    separatorFound = true;
                }
                else
                {
                    if (!char.IsDigit(c))
                        return Double.NaN;
                    n = (n * 10) + (c - '0');
                }
            }
            return ((negative ? -1 : 1) * n) / FastPow(10, input_length - decimalPosition);
        }

        /// <summary>
        /// Получает первое слово от начала сканирования
        /// </summary>
        /// <param name="parse"></param>
        /// <param name="scanoffset"></param>
        /// <param name="under_line"></param>
        /// <returns></returns>
        public static int WordIndex(ref string parse, int scanoffset, char under_line = '/')
        {
            int lenrez = 0;
            while (true)
            {
                if ((scanoffset + lenrez + 1) > parse.Length)
                    break;
                char s = parse[scanoffset + lenrez];
                if (s != ' ' && s != '\n' && s != '\r' && s != '\t' && s != under_line)
                    break;
                lenrez++;
            }
            return scanoffset + lenrez;
        }

        /// <summary>
        /// Даёт длину слова
        /// </summary>
        /// <param name="parse"></param>
        /// <param name="wordIndex"></param>
        /// <param name="under_line"></param>
        /// <returns></returns>
        public static int WordLen(ref string parse, int wordIndex, char under_line = '/')
        {
            int lenrez = 0;
            while (true)
            {
                if ((wordIndex + lenrez + 1) > parse.Length)
                    break;
                char s = parse[wordIndex + lenrez];
                if (s == ' ' || s == '\n' || s == '\r' || s == '\t' || s == under_line)
                    break;
                lenrez++;
            }
            return lenrez;
        }





        /// <summary>
        ///  Сравнивает 2 слова
        /// </summary>
        /// <param name="parse"></param>
        /// <param name="wordIndex0"></param>
        /// <param name="parse1"></param>
        /// <param name="wordIndex1"></param>
        /// <returns></returns>
       // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WordCmp(ref string parse, int wordIndex0, ref string parse1, int wordIndex1)
        {
            return WordCmp(ref parse, wordIndex0, WordLen(ref parse, wordIndex0), ref parse1, wordIndex1, WordLen(ref parse1, wordIndex1));
        }

        /// <summary>
        /// Сравнивает 2 слова
        /// </summary>
        /// <param name="parse"></param>
        /// <param name="wordIndex0"></param>
        /// <param name="w0len"></param>
        /// <param name="parse1"></param>
        /// <param name="wordIndex1"></param>
        /// <param name="w1len"></param>
        /// <returns></returns>
        public static bool WordCmp(ref string parse, int wordIndex0, int w0len, ref string parse1, int wordIndex1, int w1len)
        {
            if (w0len != w1len)
                return false;
            for (int i = 0; i < w0len; i++)
            {
                if (parse[wordIndex0 + i] != parse1[wordIndex1 + i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Даёт сдедующие слово после указанного
        /// </summary>
        /// <param name="parse"></param>
        /// <param name="wordIndex"></param>
        /// <returns></returns>
       // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WordNext(ref string parse, int wordIndex)
        {         
            return WordsParser.WordIndex(ref parse, wordIndex + WordsParser.WordLen(ref parse, wordIndex));
        }

    }
}
