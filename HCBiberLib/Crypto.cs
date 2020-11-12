using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberLib
{
    public class Crypto
    {

        private string Anahtar =  "Huseyin3521@HCBiber";

        static string _LastError = "";

        public Crypto(string oAnahtar)
        {
            _LastError = "";

            Anahtar = oAnahtar;

        }

        public string Sifrele(string Veri)
        {
            try
            {

                string tmp_Sifre = "";
                string tmp_Veri = Veri;
                int Uzunluk = Veri.Length;
                int Sira = 0;
                int SifreMod = 0;
                int KodY = 0;
                int KodX = 0;
                char Harf = '0';

                for (Sira = 0; Sira < Uzunluk; Sira++)
                {

                    KodY = tmp_Veri[Sira];
                    KodX = 58 - Anahtar[SifreMod];
                    // Harf = (char)(KodY ^ KodX);
                    Harf = (char)(KodY + KodX);
                    //Harf = (char)(Harf + 60);
                    tmp_Sifre = tmp_Sifre.Trim() + Harf.ToString().Trim();

                    SifreMod++;

                    if (SifreMod > Anahtar.Length - 1)
                    {
                        SifreMod = 0;
                    }
                }


                return tmp_Sifre;
            }
            catch (Exception Ex)
            {
                _LastError = "Sifrele:" + Ex.Message;

                return "";
            }

        }

        public string Coz(string Veri)
        {
            try
            {
                string tmp_Sifre = "";
                string tmp_Veri = Veri;
                int Uzunluk = Veri.Length;
                int Sira = 0;
                int SifreMod = 0;
                int KodY = 0;
                int KodX = 0;
                char Harf = '0';

                for (Sira = 0; Sira < Uzunluk; Sira++)
                {
                    KodY = tmp_Veri[Sira];
                    KodX = 58 - Anahtar[SifreMod];
                    //Harf = (char)((KodY ^ KodX));
                    Harf = (char)(KodY - KodX);
                    tmp_Sifre = tmp_Sifre + Harf.ToString();

                    SifreMod++;
                    if (SifreMod > Anahtar.Length - 1)
                    {
                        SifreMod = 0;
                    }
                }

                return tmp_Sifre;
            }
            catch (Exception Ex)
            {
                _LastError = "SifreyiCoz:" + Ex.Message;

                return "";
            }
        }

        public string LastError
        {
            get
            {
                return _LastError;
            }
        }
        public string Decrypt(string text)
        {
            
            byte[] plaintextbytes = Convert.FromBase64String(text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Anahtar));

                using (TripleDESCryptoServiceProvider triples = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = triples.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
                    return UTF8Encoding.UTF8.GetString(results);
                }
            }

        }

        public string Encrypt(string text)
        {
             
            byte[] plaintextbytes = UTF8Encoding.UTF8.GetBytes(text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Anahtar));
                using (TripleDESCryptoServiceProvider triples = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = triples.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
                    return Convert.ToBase64String(results);
                }
            }
        }




    }
}
