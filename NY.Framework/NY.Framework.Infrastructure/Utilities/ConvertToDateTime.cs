using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NY.Framework.Infrastructure.Utilities
{
    public static class ConvertToDateTime
    {
       public static DateTime ToDateTime(string datetime)
        {
            DateTime dt = DateTime.ParseExact(datetime, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dt = dt.AddHours(00);
            dt = dt.AddMinutes(00);
            dt = dt.AddSeconds(00);
            return dt;
        }
    }
}
