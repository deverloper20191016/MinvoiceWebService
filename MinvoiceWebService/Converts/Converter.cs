namespace MinvoiceWebService.Converts
{
    public class Converter
    {
        private static readonly char[] Tcvnchars = {
            'µ', '¸', '¶', '·', '¹',
            '¨', '»', '¾', '¼', '½', 'Æ',
            '©', 'Ç', 'Ê', 'È', 'É', 'Ë',
            '®', 'Ì', 'Ð', 'Î', 'Ï', 'Ñ',
            'ª', 'Ò', 'Õ', 'Ó', 'Ô', 'Ö',
            '×', 'Ý', 'Ø', 'Ü', 'Þ',
            'ß', 'ã', 'á', 'â', 'ä',
            '«', 'å', 'è', 'æ', 'ç', 'é',
            '¬', 'ê', 'í', 'ë', 'ì', 'î',
            'ï', 'ó', 'ñ', 'ò', 'ô',
            '­', 'õ', 'ø', 'ö', '÷', 'ù',
            'ú', 'ý', 'û', 'ü', 'þ',
            '¡', '¢', '§', '£', '¤', '¥', '¦'
        };

        private static readonly char[] Unichars = {
            'à', 'á', 'ả', 'ã', 'ạ',
            'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
            'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
            'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
            'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
            'ì', 'í', 'ỉ', 'ĩ', 'ị',
            'ò', 'ó', 'ỏ', 'õ', 'ọ',
            'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
            'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
            'ù', 'ú', 'ủ', 'ũ', 'ụ',
            'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
            'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
            'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'
        };

        private static readonly char[] ConvertTable;

        static Converter()
        {
            ConvertTable = new char[256];
            for (int i = 0; i < 256; i++)
                ConvertTable[i] = (char)i;
            for (int i = 0; i < Tcvnchars.Length; i++)
                ConvertTable[Tcvnchars[i]] = Unichars[i];
        }

        public static string Tcvn3ToUnicode(string value)
        {
            char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                if (chars[i] < (char)256)
                    chars[i] = ConvertTable[chars[i]];
            return new string(chars);
        }

        /// <summary>
        /// Convert Font sang Unicode
        /// </summary>
        /// <param name="typeFont">Loại font convert sang unicode(0: Unicode, 1: TCVN3, ...)</param>
        /// <param name="value">Chuỗi convert</param>
        /// <returns></returns>
        public static string ConvertToFont(int typeFont, string value)
        {
            var result = "";
            switch (typeFont)
            {
                case 1:
                    result = Tcvn3ToUnicode(value);
                    break;
                case 0:
                    result = value;
                    break;
            }

            return result;
        }
    }
}