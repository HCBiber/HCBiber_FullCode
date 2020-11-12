using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberLib
{
    /// <summary>
    /// Yazılım Paketi İçin 
    /// </summary>
    public class HCBiberUtils
    {

        /// <summary>
        /// DLL Version
        /// </summary>
        public static string Version = "2020.11.12"; 
 

        #region Error Msg

        private static string _LastErrorMsg = "";
        private static string _LastErrorWrite
        {
            set
            {
                _LastErrorMsg = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " -> " + value;
            }
        }

        public static string LastErrorMsg
        {
            get
            {
                return _LastErrorMsg;
            }
        }

        #endregion

        #region Dizin İşlemleri
        /// <summary>
        /// Boolean DizinYoksaOlustur : True İse Olmasada Dizin Açar
        /// </summary>
        /// <param name="DizinAdresi"></param>
        /// <param name="DizinYoksaOlustur"></param>
        /// <returns></returns>
        public static Boolean DizinVarmi(string DizinAdresi, Boolean DizinYoksaOlustur)
        {
            try
            {
                _LastErrorWrite = "[DizinVarmi] Başlatıldı ";

                if (Directory.Exists(DizinAdresi)) return true;

                bool HataliIslem = false;

                if (DizinYoksaOlustur)
                {
                    string[] KacBolum = DizinAdresi.Split('\\');

                    string DizinAdi = " ";

                    foreach (string item in KacBolum)
                    {
                        if (!HataliIslem) // Hatalı İşlem Yoksa Devam
                        {
                            DizinAdi += item;

                            if (!DizinOlustur(DizinAdi))
                            {
                                HataliIslem = true;// _LastError = "[DizinVarmi] " + Ex.Message;
                            }
                        }
                    }


                }

                return (HataliIslem ? false : true);
            }
            catch (Exception Ex)
            {

                _LastErrorWrite = "[DizinVarmi] " + Ex.Message;

                return false;
            }
        }

        public static Boolean DizinOlustur(string DizinAdresi)
        {
            try
            {
                if (Directory.Exists(DizinAdresi)) return true;

                Directory.CreateDirectory(DizinAdresi);

                return true;
            }
            catch (Exception Ex)
            {
                _LastErrorWrite = "[DizinOlustur] " + Ex.Message;

                return false;
            }
        }

        #endregion

        #region Zamandan Dosya İsimleri Olsutur

        public static string ZamandanDosyaIsmiYap(DateTime Zaman)
        {
            return Zaman.Year.ToString() + "_" + (100 + Zaman.Month).ToString().Substring(1, 2) + "_" + (100 + Zaman.Day).ToString().Substring(1, 2);
        }

        public static string ZamandanDosyaIsmiYapSaatli(DateTime Zaman)
        {
            return Zaman.Year.ToString() + "_" + (100 + Zaman.Month).ToString().Substring(1, 2) + "_" + (100 + Zaman.Day).ToString().Substring(1, 2)
                   + "_" + (100 + Zaman.Hour).ToString().Substring(1, 2) + "_" + (100 + Zaman.Minute).ToString().Substring(1, 2)
                   + "_" + (100 + Zaman.Second).ToString().Substring(1, 2);
        }

        #endregion

        public static string BoslukSil(string Kaynak)
        {
            try
            {
                string[] TmpStr = Kaynak.Split(' ');

                string RetTemp = "";

                foreach (string item in TmpStr)
                {
                    RetTemp += item;
                }

                return RetTemp;

            }
            catch (Exception Ex)
            {

                return Kaynak;
            }
        }

        /// <summary>
        /// Örnek : HÜSEYİN ÖZÇAKIR
        ///       Donusum : HUSEYIN OZCAKIR
        /// </summary>
        /// <param name="TurkceKaraketerIcerenMetin"></param>
        /// <returns></returns>
        public static string Get_TurkcedenLatinAlfabesine(string TurkceKaraketerIcerenMetin)
        {
            try
            {
                Encoding iso = Encoding.GetEncoding("Cyrillic");

                Encoding utf8 = Encoding.UTF8;

                byte[] utfBytes = utf8.GetBytes(TurkceKaraketerIcerenMetin);

                byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);

                string msg = iso.GetString(isoBytes);

                TurkceKaraketerIcerenMetin = msg;

                return TurkceKaraketerIcerenMetin;

            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }

        #region Rondam Sayi Uret

        static byte[] _RndTemp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        static public int GetRandomRumber
        {
            get
            {
                System.Security.Cryptography.RNGCryptoServiceProvider prov = new System.Security.Cryptography.RNGCryptoServiceProvider();

                prov.GetBytes(_RndTemp);

                return _RndTemp[0];
            }

        }

        static public long GetRandomRumberLong
        {
            get
            {
                System.Security.Cryptography.RNGCryptoServiceProvider prov = new System.Security.Cryptography.RNGCryptoServiceProvider();

                prov.GetBytes(_RndTemp);

                return long.Parse(_RndTemp[0].ToString() + _RndTemp[1].ToString() + _RndTemp[2].ToString());
            }

        }

        static public byte[] GetRandomByte
        {
            get
            {
                int i = GetRandomRumber;

                return _RndTemp;
            }
        }

        #endregion

        public static void Sleep(int MiliSecondWaitTime) //, Boolean WindowsNefesAlsinMi)
        {

            System.Threading.Thread.Sleep(MiliSecondWaitTime);

        }


        public static string DizidenMetinYap(object[] Dizi, string Karaketr, int basla, int Bitis)
        {
            try
            {
                _LastErrorWrite = "";

                string TempStr = "";
                int Uz = Dizi.Length;

                if (Uz < Bitis)
                {
                    Bitis = Uz;
                }

                for (int i = basla; i < Bitis; i++)
                {
                    TempStr += Dizi[i].ToString() + Karaketr;
                }

                return TempStr;
            }
            catch (Exception Ex)
            {
                _LastErrorWrite = "DizidenMetinYap : " + Ex.Message;

                return "";
            }
        }



    }
}
