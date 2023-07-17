namespace system
{
    public class @string
    {
        public static readonly string asciiLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static readonly string asciiLower = "abcdefghijklmnopqrstuvwxyz";
        public static readonly string asciiUper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static readonly string digits = "0123456789";
        public static readonly string octdigits = "01234567";
        public static readonly string hexdigits = "0123456789abcdefABCDEF";

        public static readonly string punctuation = "'!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        //public static string capitalize(string str) {
        //    return str.Length > 0 ? str.ToUpper() : str[0].ToString().ToUpper() + str[1];
        //}
        //public static string center(string str, int width, string fillChar = " ")
        //{
        //    int spaces = width - str.Length;
        //    int padLeft = spaces / 2 + str.Length;
        //    return str.PadLeft(padLeft).PadRight(width);
        //}
    }
}
