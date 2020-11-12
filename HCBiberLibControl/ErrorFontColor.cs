using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCBiberLibControl
{
    public class ErrorFontColor
    {
        public Font oFont {set;get;}

        public Color oBackColor { set; get; }

        public Color oForeColor { set; get; }

        public ErrorFontColor()
        {
            //
        }

        public ErrorFontColor(Font FontX, Color BackColorX, Color ForeColorX)
        {
            oFont = FontX;

            oBackColor = BackColorX;

            oForeColor = ForeColorX;
        }
    }
}
