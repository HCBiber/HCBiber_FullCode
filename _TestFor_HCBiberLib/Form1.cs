using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HCBiberLib;
using HCBiberSQLLib;

namespace _TestFor_HCBiberLib
{
    public partial class Form1 : Form
    {

        KW_LogMonitor oLog;
        public Form1()
        {
            InitializeComponent();

            oLog = new KW_LogMonitor();

            oLog.TabloOlsun = true;

            dtgrdKWLogTable.DataSource = oLog.LogTable();
        }

        void _ErrorEventPacked(HCBiberSQLErrorMsgPacked ErrorMessage)
        {
            listBox1.Items.Add("<" + ErrorMessage.HataMesaji + ">");
        }

        private void hcBtn_BiberSqlTest2_Click(object sender, EventArgs e)
        {
           
            
            try
            {
                oLog.LogWrite("Deneme Başladı");

                HCBiberSQL SQl = new HCBiberSQL("Server=localhost;Database=TestHC;User Id=TestUser;Password=TestUser;");

                SQl.onErrorMsgRecive += _ErrorEventPacked;

                DataTable DtSonuc = SQl.GetDataTable("Sp_Demo");

                oLog.LogWrite("Constructor Bitti");

                dtgrdW_Error.DataSource = SQl.LastErrorMessagesTable;

                if (DtSonuc == null )
                {
                    foreach (DataRow item in SQl.LastErrorMessagesTable.Rows)
                    {
                        listBox1.Items.Add( string.Format("{0} : {1} : {2} : {3}  : {4} - ModulaName : {5} ", item[0].ToString(), item[1].ToString(), item[2].ToString(), item[3].ToString(),  item[4].ToString(),  item[5].ToString()));

                        label1.Text = item[3].ToString().ToLowerInvariant();
                    }


                    listBox1.Items.Add(SQl.CnnString);

                    return;
                }

                dtgrdVeri.DataSource = DtSonuc;

                oLog.LogWrite("İşlem Başarıyla Bitti");

                oLog.LogWrite("İşlem Başarıyla Bitti. \n Bir Sonraki Satır Kontrolü", "C:\\HCBiber\\Deneme2_log.Txt");

               

            }
            catch (Exception Ex)
            {

                listBox1.Items.Add(Ex.Message);

                oLog.LogWrite(string.Format(" TryExceptHatasi :\n {0}" , Ex.Message));

            }


        }
    }
}
