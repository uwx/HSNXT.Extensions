﻿#region --- License & Copyright Notice ---

/*
CodeBits Code Snippets
Copyright (c) 2012-2017 Jeevan James
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

#endregion

/* Documentation: https://github.com/JeevanJames/CodeBits/wiki/PasswordGenerator */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace HSNXT
{
    /// <summary>
    ///     Provides static methods to generate random passwords.
    /// </summary>
    public static class PasswordGenerator
    {
        /// <summary>
        ///     Generates a random password based on the provided criteria.
        /// </summary>
        /// <param name="length">The length of the password to generate</param>
        /// <param name="allowedCharacters">Set of allowed characters in the generated password</param>
        /// <param name="excludeCharacters">Set of disallowed characters in the generated password</param>
        /// <returns>The generated password</returns>
        public static string Generate(int length, PasswordCharacters allowedCharacters = PasswordCharacters.All,
            IEnumerable<char> excludeCharacters = null)
        {
            var password = InternalGenerate(length, allowedCharacters, excludeCharacters, () => new char[length],
                (pw, ch, index) => pw[index] = ch);
            return new string(password);
        }

        /// <summary>
        ///     Generates a random password based on the provided criteria and returns it as a <see cref="SecureString" />
        /// </summary>
        /// <param name="length">The length of the password to generate</param>
        /// <param name="allowedCharacters">Set of allowed characters in the generated password</param>
        /// <param name="excludeCharacters">Set of disallowed characters in the generated password</param>
        /// <returns>The generated password as a <see cref="SecureString" /></returns>
        public static SecureString GenerateSecure(int length,
            PasswordCharacters allowedCharacters = PasswordCharacters.All,
            IEnumerable<char> excludeCharacters = null)
        {
            var password = InternalGenerate(length, allowedCharacters, excludeCharacters, () => new SecureString(),
                (pw, ch, index) => pw.AppendChar(ch));
            password.MakeReadOnly();
            return password;
        }

        // Common method to generate the password for strings and SecureStrings.
        private static T InternalGenerate<T>(int length, PasswordCharacters allowedCharacters,
            IEnumerable<char> excludeCharacters, Func<T> initialValue, Action<T, char, int> appender)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Password length must be greater than zero");

            // Create a byte array the same length as the expected password and populate it with
            // random bytes
            var randomBytes = new byte[length];
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            randomNumberGenerator.GetBytes(randomBytes);

            // Create a string of all the characters allowed in the password
            var allowedCharactersString = GenerateAllowedCharactersString(allowedCharacters, excludeCharacters);
            var allowedCharactersCount = allowedCharactersString.Length;

            // Create the password
            var password = initialValue();
            for (var i = 0; i < length; i++)
                appender(password, allowedCharactersString[randomBytes[i] % allowedCharactersCount], i);
            return password;
        }

        private static string GenerateAllowedCharactersString(PasswordCharacters characters,
            IEnumerable<char> excludeCharacters)
        {
            var allowedCharactersString = new StringBuilder();
            foreach (var type in AllowedPasswordCharacters)
            {
                if ((characters & type.Key) != type.Key)
                    continue;
                if (excludeCharacters == null)
                    allowedCharactersString.Append(type.Value);
                else
                    allowedCharactersString.Append(type.Value.Where(c => !excludeCharacters.Contains(c)).ToArray());
            }

            return allowedCharactersString.ToString();
        }

        private static readonly Dictionary<PasswordCharacters, string> AllowedPasswordCharacters =
            new Dictionary<PasswordCharacters, string>(4)
            {
                {PasswordCharacters.LowercaseLetters, "abcdefghijklmnopqrstuvwxyz"},
                {PasswordCharacters.UppercaseLetters, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"},
                {PasswordCharacters.Numbers, "0123456789"},
                {PasswordCharacters.Punctuations, @"~`!@#$%^&*()_-+={[}]|\:;""'<,>.?/"},
                {PasswordCharacters.Space, " "}
            };
    }

    [Flags]
    public enum PasswordCharacters
    {
        LowercaseLetters = 0x01,
        UppercaseLetters = 0x02,
        Numbers = 0x04,
        Punctuations = 0x08,
        Space = 0x10,
        AllLetters = LowercaseLetters | UppercaseLetters,
        AlphaNumeric = AllLetters | Numbers,
        All = AllLetters | Numbers | Punctuations | Space
    }
}