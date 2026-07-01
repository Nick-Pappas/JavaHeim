/**
 * v1
 */
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JavaHeim
{
    /**
     * A sophisticated Lexical Analysis Service that tokenizes Valheim UI strings
     * and compiles them into complex Java method-chaining expressions of appropriate 
     * sophistication lexically analyzed wise-ation.
     */
    public static class JavaHeimStringManipulationService
    {
        /**
         * Cache for sanitized class names to reduce string allocation overhead so that I remain performant and efficient.
         */
        private static Dictionary<string, string> _classNameCache = new Dictionary<string, string>();

        /**
         * Transforms an ItemData object into a print statement based on item weight. Weight is all that matters often.
         * 
         * @param itemDataP The vanilla ItemData to be transformed.
         * @return A formatted Java print statement in all its glorious Javaness.
         */
        public static string GetCursedJavaString(ItemDrop.ItemData itemDataP)
        {
            if (itemDataP == null || itemDataP.m_shared == null)
            {
                return "System.out.println((Object object = new Object()).toString());";
            }

            /**
             * The Legend
             */
            string rawName = itemDataP.m_shared.m_name;
            string localizedName = SafeLocalize(rawName);
            float weight = itemDataP.GetWeight();

            string weightArg = (weight > 0f)
                ? weight.ToString("0.0", CultureInfo.InvariantCulture) + "f"
                : "/* no_weight */ true";

            return GetFormattedString(localizedName, weightArg);
        }

        /**
         * Performs lexical analysis on raw hover text to build a multi-token Java statement for blaxxun.
         * Handles keys, colors, AND actions. 
         * 
         * @param textP The raw text from the HUD.
         * @return The ultimate glorious Java statement.
         */
        public static string GetCursedJavaStringFromText(string textP)
        {
            if (string.IsNullOrEmpty(textP) || textP.Contains("System.out.println"))
            {
                return textP;
            }

            /**
             * The Legend
             */
            string localized = SafeLocalize(textP);
            StringBuilder sb = new StringBuilder();
            sb.Append("System.out.println(");

            /**
             * Extraction Phase: Extract [Key] and color
             */
            int bracketStart = localized.IndexOf('[');
            int bracketEnd = localized.IndexOf(']');
            if (bracketStart != -1 && bracketEnd > bracketStart)
            {
                string keyContent = localized.Substring(bracketStart + 1, bracketEnd - bracketStart - 1);
                sb.Append("Keyboard.getBinding(\"").Append(StripRichText(keyContent)).Append("\")");

                if (localized.Contains("color="))
                {
                    int cStart = localized.IndexOf("color=") + 6;
                    int cEnd = localized.IndexOf('>', cStart);
                    if (cEnd > cStart)
                    {
                        string color = localized.Substring(cStart, cEnd - cStart);
                        sb.Append(".setColor(\"").Append(char.ToUpper(color[0]) + color.Substring(1)).Append("\")");
                    }
                }
                sb.Append(" + ");
            }

            /**
             * Extraction Phase: Action/Object logic
             */
            string clean = StripRichText(localized);
            if (clean.Contains("\n"))
            {
                string[] lines = clean.Split('\n');
                sb.Append("Interaction.getAction(\"").Append(lines[lines.Length - 1].Trim()).Append("\") + ");
                clean = lines[0];
            }

            sb.Append(BuildInstantiation(clean.Trim(), "/* world_object */ 1.0f"));
            sb.Append(".toString());");

            string finalResult = sb.ToString();

            if (IsFarnsworthActive())
            {
                return "Good news everyone! This is a " + finalResult;
            }

            return finalResult;
        }

        private static string BuildInstantiation(string nameP, string weightArgP)
        {
            /**
             * The Legend
             */
            string className = GetOrCacheClassName(nameP);
            string varName = char.ToLower(className[0]) + className.Substring(1);

            return string.Format("({0} {1} = new {0}({2}))", className, varName, weightArgP);
        }

        private static string GetFormattedString(string localizedNameP, string weightArgP)
        {
            /**
             * The Legend
             */
            string instantiation = BuildInstantiation(localizedNameP, weightArgP);
            string result = string.Format("System.out.println({0}.toString());", instantiation);

            if (IsFarnsworthActive())
            {
                return "Good news everyone! This is a " + result;
            }

            return result;
        }

        private static string SafeLocalize(string textP)
        {
            return (Localization.instance != null) ? Localization.instance.Localize(textP) : textP;
        }

        private static bool IsFarnsworthActive()
        {
            return JavaHeimPlugin.FarnsworthMode != null && JavaHeimPlugin.FarnsworthMode.Value;
        }

        private static string StripRichText(string inputP)
        {
            /**
             * The Legend
             */
            StringBuilder sb = new StringBuilder();
            bool insideTag = false;
            for (int i = 0; i < inputP.Length; i++)
            {
                char c = inputP[i];
                if (c == '<') insideTag = true;
                else if (c == '>') insideTag = false;
                else if (!insideTag) sb.Append(c);
            }
            return sb.ToString();
        }

        private static string GetOrCacheClassName(string localizedNameP)
        {
            /**
             * The Legend
             */
            string input = StripRichText(localizedNameP);
            if (_classNameCache.TryGetValue(input, out string cached)) return cached;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (char.IsLetterOrDigit(c)) sb.Append(c);
            }

            string clean = sb.ToString();
            if (string.IsNullOrEmpty(clean)) clean = "UnknownIdentifier";

            string className = char.ToUpper(clean[0]) + clean.Substring(1);
            _classNameCache[input] = className;

            return className;
        }
    }
}