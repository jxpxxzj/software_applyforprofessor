namespace Webdisk.Backend.Helper
{
    /// <summary>
    /// 文本辅助类
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// 判断是否匹配目标格式, 支持使用 * 和 ? 作为通配符.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pattern"></param>
        /// <param name="ignoreCase"></param>
        /// <returns>是否匹配</returns>
        /// <seealso cref="http://blog.csdn.net/wumuzi520/article/details/7378371"/>
        public static bool IsMatch(string text, string pattern, bool ignoreCase = true)
        {
            string pText, pPattern;
            if (ignoreCase)
            {
                pText = text.ToLower();
                pPattern = pattern.ToLower();
            }
            else
            {
                pText = text;
                pPattern = pattern;
            }
            var nStr = pText.Length;
            var nPatt = pPattern.Length;
            var pTable = new int[nStr + 1, nPatt + 1];
            if (pPattern[0] == '*')
            {
                for (var i = 0; i <= nPatt; ++i)
                {
                    pTable[0, i] = 1;
                }
            }
            pTable[0, 0] = 1;

            for (var j = 1; j <= nPatt; ++j)
            {
                for (var i = 1; i <= nStr; ++i)
                {
                    if ((pPattern[j - 1] == '?' && pText[i - 1] != '\0') || pPattern[j - 1] == pText[i - 1])
                    {
                        pTable[i, j] = pTable[i - 1, j - 1];
                    }
                    else if (pPattern[j - 1] == '*')
                    {
                        if (pTable[i, j - 1] == 1 || pTable[i - 1, j] == 1 || pTable[i - 1, j - 1] == 1)
                            pTable[i, j] = 1;
                    }
                }
            }

            var ret = pTable[nStr, nPatt] == 1 ? true : false;
            return ret;
        }
    }
}