using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public static class RandomHelper
    {
        private static readonly Random baseRandom = new Random();

        /// <summary>
        /// Từ 0 -> max -1
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int max)
        {
            var random = new Random(baseRandom.Next());
            var r = random.Next(max);
            return r;
        }

        /// <summary>
        /// min < x < max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            var random = new Random(baseRandom.Next());
            var r = random.Next(min, max);
            return r;
        }


        private static char getRandomOfArray(char[] array)
        {
            return array[RandomHelper.Next(array.Length)];
        }

        public static char GetRandomAlphNums()
        {
            return getRandomOfArray(StringExtensions.AlphNums);
        }

        /// <summary>
        /// Lấy ngẫu nhiên ký tự A-Z,a-z
        /// </summary>
        public static char GetRandomLetter()
        {
            return getRandomOfArray(StringExtensions.Letters);
        }

        public static char GetRandomLowerLetter()
        {
            return getRandomOfArray(StringExtensions.LowerLetters);
        }

        public static char GetRandomUpperLetter()
        {
            return getRandomOfArray(StringExtensions.UpperLetters);
        }

        public static char GetRandomNumber()
        {
            return getRandomOfArray(StringExtensions.Numbers);
        }

        public static char GetRandomSpecialLetter()
        {
            return getRandomOfArray(StringExtensions.SpecialLetters);
        }

        public static string GetRandomASCIIString(int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(GetRandomAlphNums());
            return sb.ToString();
        }

    }
}
