namespace Wpf.Ui
{
     

    /// <summary>
    /// Extension methods for language and culture management.
    /// </summary>
    public static class Language
    {
        static Dictionary<string, string> langDict = new Dictionary<string, string>(); 
        static HashSet<string> languages = new HashSet<string>();
        public static string ToLanguage(this string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            // already exists
            if(languages.Contains(value))
            {
                return value;
            }

            if(langDict.TryGetValue(value,out var lang))
            {
                return lang;
            }
            else
            {
                langDict[value] = value;
            }
            return value;
        }


        // 设置指定key的语言
        public static void SetLanguage(this string value, string key)
        {
            if(string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return;
            }
            langDict[key] = value;
            if(!languages.Contains(value))
            {
                languages.Add(value);
            }
        }

        
    }
}