using System;
using System.Security.Cryptography;

namespace AutoPsy.Logic
{
    public static class Hashing
    {
        // Статическая вспомогательная библиотека для хэширования пароля и последующего его хранения

        /// <summary>
        /// Методя для хэширования введенного пользователем пароля
        /// </summary>
        /// <param name="password">Созданный пользователем пароль</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string HashPassword(string password)
        {
            byte[] salt;        // При хэшировании используется соль для повышения безопасности
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        /// <summary>
        /// Метод для верификации введенного пользователем пароля с хэшированным
        /// </summary>
        /// <param name="hashedPassword">Хэшированный пароль, хранимый в базе данных</param>
        /// <param name="password">Пароль, введенный пользователем</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            var src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            var dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            var buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (var bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        private static bool ByteArraysEqual(byte[] src, byte[] dst)
        {
            for (var i = 0; i < src.Length; i++)
                if (src[i] != dst[i]) return false;
            return true;
        }
    }
}
