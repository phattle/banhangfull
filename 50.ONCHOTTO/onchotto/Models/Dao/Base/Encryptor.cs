using System;
using System.Security.Cryptography;

namespace OnChotto.Models.Dao
{
    internal class Encryptor
    {
        private const string DEFAULT_KEY = "SystemPassword";


        internal static string Encrypt(string strMessage)
        {

            return Encrypt(strMessage, DEFAULT_KEY);

        }


        internal static string Encrypt(string strMessage, string strPassphrase)
        {
            string strResult = "";

            strResult = EncryptString(strMessage, strPassphrase);

            return strResult;
        }


        internal static string Decrypt(string strMessage)
        {

            return Decrypt(strMessage, DEFAULT_KEY);

        }


        internal static string Decrypt(string strMessage, string strPassphrase)
        {
            string strResult = "";

            strResult = DecryptString(strMessage, strPassphrase);

            return strResult;
        }


        private static string EncryptString(string Message, string Passphrase)
        {

            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();



            // Step 1. We hash the passphrase using MD5

            // We use the MD5 hash generator as the result is a 128 bit byte array

            // which is a valid length for the TripleDES encoder we use below


            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));



            // Step 2. Create a new TripleDESCryptoServiceProvider object

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();



            // Step 3. Setup the encoder

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;



            // Step 4. Convert the input string to a byte[]

            byte[] DataToEncrypt = UTF8.GetBytes(Message);



            // Step 5. Attempt to encrypt the string

            try
            {

                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();

                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

            }

            finally
            {

                // Clear the TripleDes and Hashprovider services of any sensitive information

                TDESAlgorithm.Clear();

                HashProvider.Clear();

            }



            // Step 6. Return the encrypted string as a base64 encoded string

            return Convert.ToBase64String(Results);

        }


        private static string DecryptString(string Message, string Passphrase)
        {

            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();



            // Step 1. We hash the passphrase using MD5

            // We use the MD5 hash generator as the result is a 128 bit byte array

            // which is a valid length for the TripleDES encoder we use below


            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));



            // Step 2. Create a new TripleDESCryptoServiceProvider object

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();


            // Step 3. Setup the decoder

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;



            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt;
            try
            {
                DataToDecrypt = Convert.FromBase64String(Message);
            }
            catch //(Exception ex)
            {
                return Message;
            }



            // Step 5. Attempt to decrypt the string

            try
            {

                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();

                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                TDESAlgorithm.Clear();

                HashProvider.Clear();
            }
            catch //(Exception ex)
            {
                TDESAlgorithm.Clear();

                HashProvider.Clear();
                return "";
            }




            // Step 6. Return the decrypted string in UTF8 format

            return UTF8.GetString(Results);

        }
 
    }
}
