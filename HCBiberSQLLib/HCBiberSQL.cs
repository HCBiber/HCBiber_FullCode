using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberSQLLib
{
    /// <summary>
    /// HCBiber SQL Bağlantı İşlemlerim
    /// SQL Connection String Example : Server=myServerName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;
    /// </summary>
    public class HCBiberSQL
    {

        /// <summary>
        /// Hata Mesajı Yazılırken
        /// </summary>
        public _ErrorEventPacked onErrorMsgRecive;

        SqlConnection SQLCnn;

        public int oCommandTimeOut = 0;

        /// <summary>
        /// SP lerinizde @Haberles Out Parametresi varsa @Haberles Kalsım
        /// Ancak Bu Şekilde Aldığınız Farklı İsimde Mesaj Varsa Onu Yazınız
        /// Bu Değişken SP Çalıştıktan sonraki Mesajı Çıkartır
        /// </summary>
        public string oHaberlesName = "@Haberles";

        HCBiberSQLError _LastErrorMessages;

        public HCBiberSQLError LastErrorMessage
        {
            get
            {
                return _LastErrorMessages;
            }
        }

        public DataTable LastErrorMessagesTable
        {
            get
            {
                return _LastErrorMessages.LastErrorTable;
            }
        }

        /// <summary>
        /// Kaynak : https://www.connectionstrings.com/sql-server-2019/
        ///          https://docs.microsoft.com/tr-tr/dotnet/api/system.data.sqlclient.sqlconnectionstringbuilder.applicationname?view=dotnet-plat-ext-5.0
        /// </summary>
        /// <param name="ServerName"></param>
        /// <param name="DataBase"></param>
        /// <param name="User"></param>
        /// <param name="PassWord"></param>
        /// <param name="ApplicationName"></param>
        /// <returns></returns>
        public string Get_TemplateCnnStringCreate(string ServerName , string DataBase, string User, string PassWord , string ApplicationName )
        {
            return string.Format("Server={0};Database={1};User Id={2};Password={3};;Application Name={4}", ServerName, DataBase, User, PassWord, ApplicationName);
        }

        string _CnnString;

        /// <summary>
        /// SQL Bağlantı Bilgisi Gönderimi Sağlar
        /// </summary>
        public string CnnString
        {
            set
            {
                SQLCnn.Close();

                _CnnString = value;

                SQLCnn = new SqlConnection(_CnnString);
            }

            get
            {
                return SQLCnn.ConnectionString;
            }
        }


        #region Constructor
        /// <summary>
        /// SQL Connection
        /// </summary>
        public HCBiberSQL()
        {
            //
            initiAlize();


            SQLCnn = new SqlConnection();

            _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { ModulName = "SQL Initialize",  HCBiberSqlErrorType = eHCBiberSqlErrorType.Info, SPName = "Create", HataMesaji = "Yapı Hazırlandı", ReturnValue = 0 };

        }

        public HCBiberSQL(string ConnectionString)
        {
            //
            initiAlize();

            SQLCnn = new SqlConnection(ConnectionString);

            _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { ModulName = "SQL Initialize 2", HCBiberSqlErrorType = eHCBiberSqlErrorType.Info, SPName = "Create", HataMesaji = "Yapı Hazırlandı", ReturnValue = 0 };

        }

        private void initiAlize()
        {
            _LastErrorMessages = new HCBiberSQLError();

            _LastErrorMessages.onErrorMsgRecive += _ErrorEventPacked;
        }


        private void _ErrorEventPacked(HCBiberSQLErrorMsgPacked ErrorMessage)
        {
            if (onErrorMsgRecive != null)
            {
                onErrorMsgRecive(ErrorMessage);
            }
        }

        #endregion

        #region Open

        public Boolean Open()
        {
            try
            {
                SQLCnn.Open();

                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() {ModulName="SQL Open" , HCBiberSqlErrorType = eHCBiberSqlErrorType.Info, SPName = "Connection Open", HataMesaji = "Bağlantı Açıldı", ReturnValue = 0 };

                return true;
            }
            catch (Exception Ex)
            {
                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { ModulName = "SQL Open", HCBiberSqlErrorType = eHCBiberSqlErrorType.TryExcept, SPName = "Connection Open", HataMesaji = Ex.Message, ReturnValue = 0 };

                return false;
            }
        }

        public Boolean Open(string oConnectionString)
        {
            try
            {
                CnnString = oConnectionString;

                SQLCnn.Open();

                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { HCBiberSqlErrorType = eHCBiberSqlErrorType.Info, SPName = "Connection Open", HataMesaji = "Bağlantı Açıldı", ReturnValue = 0 };

                return true;
            }
            catch (Exception Ex)
            {
                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { HCBiberSqlErrorType = eHCBiberSqlErrorType.TryExcept, SPName = "Connection Open", HataMesaji = Ex.Message, ReturnValue = 0 };

                return false;
            }
        }

        /// <summary>
        /// SQL Bağlantısını Kapatır
        /// </summary>
        /// <returns></returns>
        public Boolean Close()
        {
            try
            {

                SQLCnn.Close();

                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { ModulName = "SQL Close", HCBiberSqlErrorType = eHCBiberSqlErrorType.Info, SPName = "Connection Close", HataMesaji = "Bağlantı Kapatıldı", ReturnValue = 0 };

                return true;
            }
            catch (Exception Ex)
            {
                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { ModulName = "SQL Close", HCBiberSqlErrorType = eHCBiberSqlErrorType.TryExcept, SPName = "Connection Close", HataMesaji = Ex.Message, ReturnValue = 0 };

                return false;
            }

        }

        #endregion


        public SqlCommand GetSqlCommand(string SpName)
        {
            return GetSqlCommand(SpName, null);
        }

        public SqlCommand GetSqlCommand(string SpName,  object[] SetParams)
        {
            try
            {
                GC.Collect();

                if (SQLCnn.State == ConnectionState.Closed)
                {
                    if (!this.Open())
                    {
                        return null;
                    }

                }

                SqlCommand CmdTemp = new SqlCommand(SpName, SQLCnn);

                CmdTemp.CommandTimeout = oCommandTimeOut;

                CmdTemp.CommandType = CommandType.StoredProcedure;

                SqlCommandBuilder.DeriveParameters(CmdTemp);

                return CmdTemp;

            }
            catch (Exception Ex)
            {
                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { ModulName = "SQL GetSqlCommand", HCBiberSqlErrorType = eHCBiberSqlErrorType.TryExcept, SPName = SpName, HataMesaji = Ex.Message, ReturnValue = 0 };

                return null;
            }
        }

        #region Get Data ve DataSet

        object[] _RetParams = null;

        #region GetDataTable

        public DataTable GetDataTable(string StoredProcedureName)
        {
            _RetParams = null;

            return GetDataTable(StoredProcedureName, null, out _RetParams );
        }
        public DataTable GetDataTable(string StoredProcedureName, object[] SetParams, out object[] rRetParams)
        {
            try
            {
                GC.Collect();

                if (SQLCnn.State == ConnectionState.Closed)
                {
                    if (!this.Open())
                    {
                        rRetParams = null;

                        return null;
                    }
                    
                }

                SqlCommand CmdTemp = new SqlCommand(StoredProcedureName, SQLCnn);

                CmdTemp.CommandTimeout = oCommandTimeOut;

                CmdTemp.CommandType = CommandType.StoredProcedure;

                SqlCommandBuilder.DeriveParameters(CmdTemp);

                if (SetParams != null)
                {
                    for (int i = 0; i < SetParams.Length; i++)
                    {
                        CmdTemp.Parameters[i].Value = SetParams[i];
                    }
                }

                DataTable DtSonuc = new DataTable(string.Format("dt{0}_Tablo", DateTime.Now.Hour));

                SqlDataAdapter DAP = new SqlDataAdapter(CmdTemp);

                DAP.Fill(DtSonuc);

                rRetParams = new object[] { CmdTemp.Parameters["@Return_Value"].Value, CmdTemp.Parameters["@Return_Value"] == null ? "YOK" : CmdTemp.Parameters[oHaberlesName].Value };

                return DtSonuc.Copy();
            }
            catch (Exception Ex)
            {
                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { ModulName = "SQL GetDataTable", HCBiberSqlErrorType = eHCBiberSqlErrorType.TryExcept, SPName = StoredProcedureName, HataMesaji = Ex.Message, ReturnValue = 0 };

                rRetParams = null;

                return null;
            }
        }

        #endregion

        #region GetDataSet

        public DataSet GetDataSet(string StoredProcedureName)
        {
            _RetParams = null;

            return GetDataSet(StoredProcedureName, null, out _RetParams);
        }
        public DataSet GetDataSet(string StoredProcedureName, object[] SetParams, out object[] rRetParams)
        {
            try
            {
                GC.Collect();

                if (SQLCnn.State == ConnectionState.Closed)
                {
                    if (!this.Open())
                    {
                        rRetParams = null;

                        return null;
                    }

                }

                SqlCommand CmdTemp = new SqlCommand(StoredProcedureName, SQLCnn);

                CmdTemp.CommandTimeout = oCommandTimeOut;

                CmdTemp.CommandType = CommandType.StoredProcedure;

                SqlCommandBuilder.DeriveParameters(CmdTemp);

                if (SetParams != null)
                {
                    for (int i = 0; i < SetParams.Length; i++)
                    {
                        CmdTemp.Parameters[i].Value = SetParams[i];
                    }
                }

                DataSet DsSonuc = new DataSet(string.Format("ds{0}_Tablo_{1}", 10 + DateTime.Now.Hour, 10 + DateTime.Now.Minute));

                SqlDataAdapter DAP = new SqlDataAdapter(CmdTemp);

                DAP.Fill(DsSonuc);

                rRetParams = new object[] { CmdTemp.Parameters["@Return_Value"].Value, CmdTemp.Parameters["@Return_Value"] == null ? "YOK" : CmdTemp.Parameters[oHaberlesName].Value };

                return DsSonuc.Copy();
            }
            catch (Exception Ex)
            {
                _LastErrorMessages.WriteMsgError = new HCBiberSQLErrorMsgPacked() { ModulName = "SQL GetDataSet" ,HCBiberSqlErrorType = eHCBiberSqlErrorType.TryExcept, SPName = StoredProcedureName, HataMesaji = Ex.Message, ReturnValue = 0 };

                rRetParams = null;

                return null;
            }
        }

        #endregion

        #endregion



    }
}
