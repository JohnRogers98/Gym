using Gym.Domain.FormAuthContext.ValueObjects;
using Gym.Domain.UserContext;
using System.Security.Cryptography;

namespace Gym.Infrastructure.Entities.Repositories.Users
{
    internal class PasswordGenerator : IPasswordGenerator
    {
        public Password Generate()
        {
            var passwordChars = new Char[8];

            passwordChars[0] = this.GetRandomAlphabetChar();
            passwordChars[1] = this.GetRandomNumericChar();

            for (int i = 2; i < passwordChars.Length; i++)
            {
                passwordChars[i] = this.GetRandomAlphaNumericChar();
            }

            var shuffledChars = this.Shuffle(passwordChars);

            return Password.From(new String(shuffledChars)).Data!;
        }

        private const String ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const String DIGITS = "0123456789";
        private const String ALPHANUMERIC = ALPHABET + DIGITS;

        private Char GetRandomAlphabetChar() => ALPHABET[RandomNumberGenerator.GetInt32(ALPHABET.Length)];

        private Char GetRandomNumericChar() => DIGITS[RandomNumberGenerator.GetInt32(DIGITS.Length)];

        private Char GetRandomAlphaNumericChar() => ALPHANUMERIC[RandomNumberGenerator.GetInt32(ALPHANUMERIC.Length)];

        /// <summary>
        /// Fisher-Yates shuffle
        /// </summary>
        /// <param name="inputChars"></param>
        /// <returns></returns>
        private Char[] Shuffle(ReadOnlySpan<Char> inputChars)
        {
            Char[] shuffleChars = inputChars.ToArray();

            for (Int32 i = shuffleChars.Length - 1; i > 0; i--)
            {
                Int32 j = RandomNumberGenerator.GetInt32(i + 1);
                (shuffleChars[i], shuffleChars[j]) = (shuffleChars[j], shuffleChars[i]);
            }

            return shuffleChars;
        }

    }
}
