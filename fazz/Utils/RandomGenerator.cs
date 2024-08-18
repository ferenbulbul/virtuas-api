using System;
using System.Text;

namespace fazz.Utils
{
    public class RandomGenerator
    {
        private static Random random = new Random();

        private static string[] letters =
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z"
        };

        private static string[] digits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public static string GenerateRandomUsername()
        {
            StringBuilder username = new StringBuilder();
            username.Append(letters[random.Next(letters.Length)]);
            username.Append(letters[random.Next(letters.Length)]);
            username.Append(digits[random.Next(digits.Length)]);
            username.Append(digits[random.Next(digits.Length)]);
            return username.ToString();
        }

        public static string GenerateRandomPassword()
        {
            return random.Next(1000, 10000).ToString(); // Generate a random number between 1000 and 9999 (4-digit password)
        }
    }
}
