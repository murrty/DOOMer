namespace HashSlingingSlasher;
internal static class Extensions {
    public static string RemoveChar(this string str, char ch) {
        //StringBuilder sb = new();
        ValueStringBuilder sb = new(stackalloc char[str.Length]);
        for (int i = 0; i < str.Length; i++) {
            if (str[i] != ch) {
                sb.Append(str[i]);
            }
        }
        str = sb.ToString();
        sb.Dispose();
        return str;
    }
}