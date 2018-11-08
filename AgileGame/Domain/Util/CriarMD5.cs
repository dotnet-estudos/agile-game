using System;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Util
{
    public static class CriarMd5
    {

        /// <summary>
        /// Criptografa string em MD5
        /// </summary>
        /// <param name="senha"></param>
        /// <returns></returns>
        public static string Criptografar(string senha)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return RetonarHash(md5Hash, senha);
            }
        }

        /// <summary>
        /// Compara 2 hash md5
        /// </summary>
        /// <param name="senhaBanco"></param>
        /// <param name="senhaMd5"></param>
        /// <returns></returns>
        public static bool ComparaMD5(string senhaBanco, string senhaMd5)
        {
            var senha = Criptografar(senhaBanco);
            return VerificarHash(senhaMd5, senha);
        }

        /// <summary>
        /// Converte String em hash md5
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string RetonarHash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Verifica Hash
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        private static bool VerificarHash(string input, string hash)
        {
            StringComparer compara = StringComparer.OrdinalIgnoreCase;

            return (0 == compara.Compare(input, hash));
        }
    }
}
