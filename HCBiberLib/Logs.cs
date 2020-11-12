using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberLib
{
    class Logs
    {
    }

    /// <summary>
    /// Son Hataları Log Tutumak Icin Kullanılır
    /// </summary>
    public class LogTable
    {
        DataTable _LogData;

        public LogTable()
        {
            _LogData = new DataTable("LogDataTable");

            _LogData.Columns.Add("Zaman");

            _LogData.Columns.Add("Mesaj");
        }

        public string LogYaz
        {
            set
            {
                _LogData.Rows.Add(DateTime.Now.ToLongTimeString(), value);
            }
        }

        public DataTable GetLogList
        {
            get
            {
                return _LogData;
            }
        }

        public void Clear()
        {
            _LogData.Rows.Clear();
        }


    }

    public class KW_LogMonitor : AHCLastError
    {
        DataTable Dt_Log = new DataTable();

        public string LogFilePath = @"C:\HCBiber\KW_LogMonitor.Log";

        public int MaxErrorRow = 10;

        Boolean _TabloOlsun = true;
        public Boolean TabloOlsun
        {
            set { _TabloOlsun = value; }

            get { return _TabloOlsun; }
        }

        public KW_LogMonitor()
        {
            // Default Deger ile
            try
            {
                if (!Directory.Exists(@"C:\HCBiber")) Directory.CreateDirectory(@"C:\HCBiber");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public KW_LogMonitor(string oLogFilePath)
        {
            if (!Directory.Exists(@"C:\HCBiber")) Directory.CreateDirectory(@"C:\HCBiber");

            LogFilePath = oLogFilePath;
        }

        public void LogWrite(string LogMesaj)
        {
            LogWrite(LogMesaj, LogFilePath);
        }


        /// <summary>
        /// Kendi İç Hataları Loglama işlemi
        /// </summary>
        /// <param name="LogMesaj"></param>
        /// <param name="FilePath"></param>
        private void ErrorWrite(string LogMesaj, string HataMesaj, String FilePath)
        {
            try
            {
                StreamWriter oDosya = new StreamWriter(@"C:\HCBiber\LogMonitor_Hata.TXT", true);

                oDosya.WriteLine("[" + DateTime.Now.ToShortDateString() + " "
                                     + DateTime.Now.ToLongTimeString() + "]"
                                     + "Hata Mesajı " + HataMesaj + Environment.NewLine
                                     + "Mesaj : " + LogMesaj
                                     + "Log Dosyası : " + FilePath);
                oDosya.Flush();

                oDosya.Close();

                LogWriteTable(LogMesaj);
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Disk üzerine Log Atar
        /// </summary>
        /// <param name="LogMesaj"></param>
        /// <param name="FilePath"></param>
        public void LogWrite(string LogMesaj, string FilePath)
        {
            try
            {

                StreamWriter oDosya = new StreamWriter(FilePath, true);

                oDosya.WriteLine("[" + DateTime.Now.ToShortDateString() + " "
                                     + DateTime.Now.ToLongTimeString() + "] "
                                     + LogMesaj);
                oDosya.Flush();

                oDosya.Close();

                LogWriteTable(LogMesaj);
            }
            catch (Exception ex)
            {
                ErrorWrite(LogMesaj, ex.Message, FilePath);
            }
        }

        private void TabloCiz()
        {

            Dt_Log.Columns.Add("LogZamani");

            Dt_Log.Columns.Add("LogMesaj");

        }

        public void LogTableClear()
        {
            Dt_Log.Clear();
        }

        /// <summary>
        /// ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/dv_csref/html/122d1e3b-1604-4add-b6b4-b84530a77b6b.htm
        /// </summary>
        /// <returns></returns>
        public object[] Get_ColumnsName()
        {
            try
            {
                object[] sColumns = new object[Dt_Log.Columns.Count];

                for (int i = 0; i < Dt_Log.Columns.Count; i++)
                {
                    sColumns[i] = Dt_Log.Columns[i].ColumnName;
                }

                return sColumns;
            }
            catch (Exception Ex)
            {
                object[] Sonuc = { "Hata", Ex.Message };

                return Sonuc;
            }

        }

        public DataTable LogTable()
        {
            return Dt_Log;
        }

        private void LogWriteTable(string LogMesaj)
        {
            if (!_TabloOlsun) { return; }; // Tablo Yoksa Devam etme

            try
            {
                if (Dt_Log.Columns.Count < 1) { TabloCiz(); }

                object[] Satir = { DateTime.Now.ToLongTimeString(), LogMesaj };

                Dt_Log.Rows.Add(Satir);

                if (Dt_Log.Rows.Count > MaxErrorRow)
                {
                    Dt_Log.Rows.RemoveAt(1);
                }

            }
            catch(Exception Ex)
            {
                base._LastErrorWrite = Ex.Message;
            }

        }
    }


}
