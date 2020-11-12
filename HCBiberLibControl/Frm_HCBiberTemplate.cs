using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCBiberLibControl
{
    public partial class Frm_HCBiberTemplate : Form
    {

        /// <summary>
        /// WriteMsg İşlemleri ile , Oluşturulan event
        /// Format : void eventMsgreceive(string Msj, string MsjOrj, eMsgType msgType);
        /// </summary>
        /// <returns></returns>
        public eventMsgreceive onMsgReceive;  // (string Msj, string MsjOrj, eMsgType msgType);

        public Frm_HCBiberTemplate()
        {
            InitializeComponent();

            MsgError = new ErrorFontColor(this.Font, this.BackColor, this.ForeColor);

            MsgInfo = new ErrorFontColor(this.Font, this.BackColor, this.ForeColor);

        }

        public int oPanelHeight
        {
            set
            {
                panel1.Height = value;
            }

            get
            {
                return panel1.Height;
            }
        }

        #region Error Color And Font

        ErrorFontColor MsgError;
        /// <summary>
        /// Hata Mesajı İçin Font
        /// </summary>
        public Font oErrorFont
        {
            set
            {
                MsgError.oFont = value;
            }

            get
            {
                return MsgError.oFont;
            }
        }

        public Color oErrorFontBackColor
        {
            set
            {
                MsgError.oBackColor = value;
            }

            get
            {
                return MsgError.oBackColor;
            }
        }

        public Color oErrorFontForeColor
        {
            set
            {
                MsgError.oForeColor = value;
            }

            get
            {
                return MsgError.oForeColor;
            }
        }

        #endregion

        
        #region Info Color And Font

        ErrorFontColor MsgInfo;
        public Font oErrorInfo
        {
            set
            {
                MsgInfo.oFont = value;
            }

            get
            {
                return MsgInfo.oFont;
            }
        }

        public Color oErrorInfoBackColor
        {
            set
            {
                MsgInfo.oBackColor = value;
            }

            get
            {
                return MsgInfo.oBackColor;
            }
        }

        public Color oErrorInfoForeColor
        {
            set
            {
                MsgInfo.oForeColor = value;
            }

            get
            {
                return MsgInfo.oForeColor;
            }
        }

        #endregion

        /// <summary>
        /// Hatayı Yansıtma
        /// </summary>
        public string WriteMsgError
        {
            set
            {
                olblMsj.Font = MsgError.oFont;

                olblMsj.BackColor = MsgError.oBackColor;

                olblMsj.ForeColor = MsgError.oForeColor;

                olblMsj.Text = string.Format("{0} : {1}", DateTime.Now, value);

                if(onMsgReceive != null)
                {
                    onMsgReceive(olblMsj.Text, value, eMsgType.Error);
                }

                Application.DoEvents();
            }
        }

        /// <summary>
        /// Hatayı Yansıtma
        /// </summary>
        public string WriteMsgInfo
        {
            set
            {
                olblMsj.Font = MsgInfo.oFont;

                olblMsj.BackColor = MsgInfo.oBackColor;

                olblMsj.ForeColor = MsgInfo.oForeColor;

                olblMsj.Text = string.Format("{0} : {1}", DateTime.Now, value);

                if (onMsgReceive != null)
                {
                    onMsgReceive(olblMsj.Text, value, eMsgType.Info);
                }

                Application.DoEvents();
            }
        }



    }

    public enum eMsgType { Info, Error }

    public delegate void eventMsgreceive(string Msj, string MsjOrj, eMsgType msgType);
}
