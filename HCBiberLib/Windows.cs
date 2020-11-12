using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberLib
{
    public class Windows
    {
        public static string LogonUserName
        {
            get
            {
                WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();

                return windowsIdentity.Name;

            }
        }

        public static string ComputerName
        {
            get
            {
                return Dns.GetHostName();
            }
        }

    }

    public class KW_Enviroment
    {

        string[] oEnviroments;

        public KW_Enviroment(string[] EnViromets)
        {
            oEnviroments = EnViromets;
        }

        public KW_Enviroment()
        {
            oEnviroments = Environment.GetCommandLineArgs();
        }

        public string[] GetEnviroments
        {
            get
            {
                return oEnviroments;
            }
        }

        public string[] GetFieldValue(string FieldName)
        {
            try
            {

                string[] Cevre = oEnviroments; //Environment.GetCommandLineArgs();

                if (Cevre.Length > 1)
                {
                    // arayalım
                    int j = 1;
                    foreach (string item in Cevre)
                    {
                        //
                        //MessageBox.Show(string.Format("{0} Sirada : [ {1} ] ", j , item));

                        if (item.Split('=').Length > 1)
                        {
                            string[] Islem = item.Split('=');

                            if (Islem[0].ToString() == FieldName)
                            {
                                return Islem;
                            }
                        }

                        j++;
                    }
                }

                return new string[] { "HATA", FieldName + " Bulunamadi " };

            }
            catch (Exception Ex)
            {

                return new string[] { "EXCEPT", Ex.Message };

            }
        }
    }


}
