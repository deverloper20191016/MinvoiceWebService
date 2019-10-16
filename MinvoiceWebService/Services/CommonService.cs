using System;
using System.Globalization;

namespace MinvoiceWebService.Services
{
    public class CommonService
    {
        public static bool CheckDate(string date)
        {
            string[] formats = { "yyyy-MM-dd" };
            DateTime parsedDate;
            var isValidFormat = DateTime.TryParseExact(date, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDate);

            return isValidFormat;
        }
    }
}