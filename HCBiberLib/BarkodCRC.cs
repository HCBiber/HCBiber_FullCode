using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberLib
{
    /// <summary>
    /// Girilen Bir Sayının CheckDigit Kontrolu Yapılır
    /// 123456C : 123456 Sayısı Hesaplanarak C bulunur. C mutlaka 0 ile 9 Arasındaki bir Sayıdır
    /// Yani Girilen Rakamanın Tesadüf girilmesi 10 da birdir.
    /// Hatalı Giriş Yapılan Sayıyı Kontrol eder
    /// </summary>
    public class BarkodCRC
    {
        /// <summary>
        /// Barkod Kontrolu Icin Toplam uzunluk -1 Gonderin Gelen ile aynımı bakınız
        /// </summary>
        /// <param name="oBarKodNumber"></param>
        /// <returns></returns>
        public static int GetCRC(string oBarKodNumber)
        {
            int Toplam = 0;

            int c;

            for (int Sayac = 1; Sayac <= oBarKodNumber.Length; Sayac++)
            {
                c = int.Parse(oBarKodNumber.Substring(Sayac - 1, 1));

                if ((Sayac % 2) != 0)
                    Toplam = Toplam + (c * 3);
                else
                    Toplam = Toplam + c;

            }

            if ((Toplam % 10) == 0)
            {
                return 0;
            }
            else
            {
                c = 10 - (Toplam % 10);
            }

            return c;
        }

        /// <summary>
        /// İlgili Numaranın Check Digit Kontrolunu yapar
        /// Doğru İse True Döner
        /// Yanlış ise False Döner 
        /// </summary>
        /// <param name="oBarKodNumber"></param>
        /// <returns></returns>
        public static Boolean ChkCRC(string oBarKodNumber)
        {
            string Crc_Check = oBarKodNumber.Substring(0, oBarKodNumber.Length - 1);

            int Crc_CheckCRC = Convert.ToInt32(oBarKodNumber.Substring(oBarKodNumber.Length - 1, 1)); // Son Deger

            if (GetCRC(Crc_Check) != Crc_CheckCRC)
            {
                return false;
            }

            return true;
        }

    }


}
