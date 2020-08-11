using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TopKala3
{
    public  static class PersianConvertor
    {
        public static string ToShamsi(this DateTime Value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(Value) + "/" + pc.GetMonth(Value).ToString("00") + "/"
                + pc.GetDayOfMonth(Value).ToString("00");
        }
    }
}
