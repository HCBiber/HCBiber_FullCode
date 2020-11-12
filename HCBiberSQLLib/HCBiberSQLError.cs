using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberSQLLib
{
    public interface IHCBiberSQLError
    {
        string ModulName { set; get; }

        string SPName { set; get; }

        DateTime Zaman { set; get; }

        string HataMesaji { set; get; }

        int ReturnValue { set; get; }

        eHCBiberSqlErrorType HCBiberSqlErrorType { set; get; }
    }

    public class HCBiberSQLErrorMsgPacked : IHCBiberSQLError
    {
        public string ModulName { get; set; }
        public string SPName { get; set; } //{ get { return _Mesaj.SPName; } set { _Mesaj.SPName = value; } }
        public DateTime Zaman { get; set; } //{ get { return _Mesaj.Zaman; } set { _Mesaj.Zaman = value; } }
        public string HataMesaji { get; set; } //{ get { return _Mesaj.HataMesaji; } set { _Mesaj.HataMesaji = value; } }
        public int ReturnValue { get; set; } //{ get { return _Mesaj.ReturnValue; } set { _Mesaj.ReturnValue = value; } }

        /// <summary>
        /// Mesaj Tipleri
        /// </summary>
        public eHCBiberSqlErrorType HCBiberSqlErrorType { set; get; }

}

    public class HCBiberSQLError 
    {

        public _ErrorEventPacked onErrorMsgRecive;

        private int _LastErrorPoint = 0, _LastError_Max = 25;

        HCBiberSQLErrorMsgPacked[] _Mesaj; //= new HCBiberSQLErrorMsgPacked[_LastError_Max + 1];

        /// <summary>
        /// SQL Hata Mesajları Takip İçin
        /// </summary>
        public HCBiberSQLError()
        {
            _LastErrorPoint = 0;

            _Mesaj = new HCBiberSQLErrorMsgPacked[_LastError_Max + 1];
        }


        public HCBiberSQLError(HCBiberSQLErrorMsgPacked oMesaj)
        {
            _LastErrorPoint = 0;

            _Mesaj = new HCBiberSQLErrorMsgPacked[_LastError_Max + 1];

            WriteMsgError = oMesaj;
        }

        public HCBiberSQLError(string Mesaj)
        {
            _LastErrorPoint = 0;

            _Mesaj = new HCBiberSQLErrorMsgPacked[_LastError_Max + 1];

            WriteMsgError = new HCBiberSQLErrorMsgPacked() { HCBiberSqlErrorType = eHCBiberSqlErrorType.Info, HataMesaji = Mesaj };

        }

        public HCBiberSQLError(string Mesaj , string oSpName)
        {
            _LastErrorPoint = 0;

            _Mesaj = new HCBiberSQLErrorMsgPacked[_LastError_Max + 1];

            WriteMsgError = new HCBiberSQLErrorMsgPacked() { HCBiberSqlErrorType = eHCBiberSqlErrorType.Info,  HataMesaji = Mesaj, SPName = oSpName , ReturnValue = 0 };

        }

        public HCBiberSQLError(string Mesaj, int oReturnValue)
        {
            _LastErrorPoint = 0;

            _Mesaj = new HCBiberSQLErrorMsgPacked[_LastError_Max + 1];

            WriteMsgError = new HCBiberSQLErrorMsgPacked() { HCBiberSqlErrorType = eHCBiberSqlErrorType.Info, HataMesaji = Mesaj, SPName = "-", ReturnValue = oReturnValue };

        }

        public HCBiberSQLErrorMsgPacked WriteMsgError
        {
            set
            {
                if (_LastErrorPoint <= _LastError_Max)
                {
                    _Mesaj[_LastErrorPoint] = new HCBiberSQLErrorMsgPacked();
                }


                _Mesaj[_LastErrorPoint].HataMesaji = value.HataMesaji;

                _Mesaj[_LastErrorPoint].SPName = value.SPName;

                _Mesaj[_LastErrorPoint].ModulName = value.ModulName;

                _Mesaj[_LastErrorPoint].Zaman = DateTime.Now;//value.Zaman;

                _Mesaj[_LastErrorPoint].ReturnValue = value.ReturnValue;

                _Mesaj[_LastErrorPoint].HCBiberSqlErrorType = value.HCBiberSqlErrorType;

                if (onErrorMsgRecive != null)
                {
                    int SiraNoX = _LastErrorPoint;

                    onErrorMsgRecive(_Mesaj[SiraNoX]);
                }

                if (_LastErrorPoint == _LastError_Max) ShiftMesaj();

                _LastErrorPoint++;

            }
        }

        private void ShiftMesaj()
        {
            try
            {
                for (int i = 0; i < _LastError_Max; i++)
                {
                    _Mesaj[i].ModulName = _Mesaj[i + 1].ModulName;

                    _Mesaj[i].HataMesaji = _Mesaj[i + 1].HataMesaji;

                    _Mesaj[i].SPName = _Mesaj[i + 1].SPName;

                    _Mesaj[i].Zaman = _Mesaj[i + 1].Zaman;

                    _Mesaj[i].ReturnValue = _Mesaj[i + 1].ReturnValue;

                    _Mesaj[i].HCBiberSqlErrorType = _Mesaj[i + 1].HCBiberSqlErrorType;


                }

                _LastErrorPoint--;
            }
            catch (Exception Ex)
            {

                throw;
            }
        }

        public HCBiberSQLErrorMsgPacked LastErrorIndex(int Index)
        {
            if (Index < 0) Index = 0;

            if (Index > _LastError_Max) Index = _LastError_Max;

            return _Mesaj[Index];
        }

        public HCBiberSQLErrorMsgPacked LastError
        {
            get
            {
                return _LastErrorPoint < 1 ? _Mesaj[_LastErrorPoint] : _Mesaj[_LastErrorPoint - 1];
                
            }
        }


        /// <summary>
        /// Son Hata Mesajları
        /// </summary>
        public DataTable LastErrorTable
        {
            get
            {
                DataTable dtSonuc = new DataTable("LastErrorTable");

                dtSonuc.Columns.Add("Mesaj Tipi", System.Type.GetType("System.String"));

                dtSonuc.Columns.Add("Tarih", System.Type.GetType("System.DateTime"));

                dtSonuc.Columns.Add("Hata Mesaji", System.Type.GetType("System.String"));

                dtSonuc.Columns.Add("Sp Name", System.Type.GetType("System.String"));

                dtSonuc.Columns.Add("Return Value", System.Type.GetType("System.Int64"));

                dtSonuc.Columns.Add("Modul Name", System.Type.GetType("System.String"));

                for (int i = 0; i < _LastErrorPoint; i++)
                {
                       dtSonuc.Rows.Add(new object[] { HataMesajTipiAciklama(_Mesaj[i].HCBiberSqlErrorType), _Mesaj[i].Zaman , _Mesaj[i].HataMesaji , _Mesaj[i].SPName , _Mesaj[i].ReturnValue , _Mesaj[i].ModulName });
                }
                
                return dtSonuc;

            }
        }

        private string HataMesajTipiAciklama(eHCBiberSqlErrorType oMsjType)
        {
            string RetMsg = "BelirSiz";

            switch(oMsjType)
            {
              case eHCBiberSqlErrorType.TryExcept : RetMsg = "TryExcept"; break;

              case eHCBiberSqlErrorType.Info: RetMsg = "Info"; break;

             case eHCBiberSqlErrorType.Error: RetMsg = "Error"; break;

            }

            return RetMsg;
        }

    }

    /// <summary>
    /// SQL Error Mesaj Tipleri
    /// </summary>
    public enum eHCBiberSqlErrorType { Info , Error , TryExcept };
}
