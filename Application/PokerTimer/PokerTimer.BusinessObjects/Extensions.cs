using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerTimer.BusinessObjects
{
    public static class Extensions
    {
        public static void Raise(this PropertyChangedEventHandler helper, object thing, string name)
        {
            if (helper != null)
            {
                helper(thing, new PropertyChangedEventArgs(name));
            }
        }

        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        public static bool IsNull<T>(this T? obj) where T : struct
        {
            return !obj.HasValue;
        }

        public static bool IsNotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }

        public static bool IsNotNull<T>(this T? obj) where T : struct
        {
            return obj.HasValue;
        }

        public static bool IsEmtpy(this Guid obj)
        {
            return obj == Guid.Empty;
        }

        public static bool IsNotEmtpy(this Guid obj)
        {
            return obj != Guid.Empty;
        }

        public static string GetEnumDescription<T>(this T enumerator) where T : struct, IConvertible
        {
            var fi = enumerator.GetType().GetField(enumerator.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>().ToArray();

            if (attributes != null && attributes.Length > 0)
                return attributes.First().Description;
            else
                return enumerator.ToString();
        }

        public static string GetEnumDisplayName<T>(this T enumerator) where T : struct, IConvertible
        {
            var fi = enumerator.GetType().GetField(enumerator.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DisplayNameAttribute), false).Cast<DisplayNameAttribute>().ToArray();

            if (attributes != null && attributes.Length > 0)
                return attributes.First().DisplayName;
            else
                return enumerator.ToString();
        }

        public static T ToEnum<T>(this object o) where T : struct
        {
            return (T)Enum.Parse(typeof(T), o.ToString());
        }

        public static List<int> GetValuesOf<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(e => Convert.ToInt32(e)).ToList();
        }

        public static List<T> GetValueList<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsNotNullOrEmpty(this string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        public static string RemoveDiacritics(this string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static Guid ToGuid(this string value)
        {
            return new Guid(value);
        }

        public static bool True(this bool? boolean, Action action = null, Action actionElse = null)
        {
            if (boolean.HasValue && boolean.Value)
            {
                if (action.IsNotNull())
                {
                    action();
                }
            }
            else if (actionElse.IsNotNull())
            {
                actionElse();
            }
            return boolean.HasValue && boolean.Value;
        }

        public static bool True(this bool boolean, Action action = null, Action actionElse = null)
        {
            if (boolean)
            {
                if (action.IsNotNull())
                {
                    action();
                }
            }
            else if (actionElse.IsNotNull())
            {
                actionElse();
            }
            return boolean;
        }

        public static bool False(this bool boolean, Action action, Action actionElse = null)
        {
            if (!boolean)
                action();
            else if (actionElse != null)
                actionElse();
            return boolean;
        }

        public static bool In<T>(this T enumerator, params T[] values) where T : struct, IConvertible
        {
            if (values.Length == 0)
                return false;

            return values.Any(v => enumerator.Equals(v));
        }

        public static bool In(this string enumerator, params string[] values)
        {
            if (values.Length == 0)
                return false;

            return values.Any(v => enumerator.Equals(v));
        }

        public static eGameType ToGameType(this char gametypeChar)
        {
            switch (gametypeChar)
            {
                case 'X':
                    return eGameType.NotSet;
                case 'F':
                    return eGameType.FreezeOut;
                case 'R':
                    return eGameType.RebuyUnlimited;
                case 'D':
                    return eGameType.DoubleChance;
                case 'T':
                    return eGameType.TripleChance;
                case 'C':
                    return eGameType.CashGame;
                case 'E':
                    return eGameType.FreeRoll;
                case 'L':
                    return eGameType.RebuyLimited;
                case 'A':
                    return eGameType.DoubleTrouble;

                default:
                    return eGameType.NotSet;
            }
        }

        public static T GetCopy<T>(this T S)
        {
            T newObj = Activator.CreateInstance<T>();

            foreach (PropertyInfo i in newObj.GetType().GetProperties())
            {

                //"EntitySet" is specific to link and this conditional logic is optional/can be ignored
                if (i.CanWrite && i.PropertyType.Name.Contains("EntitySet") == false)
                {
                    object value = S.GetType().GetProperty(i.Name).GetValue(S, null);
                    i.SetValue(newObj, value, null);
                }
            }

            return newObj;
        }

        public static int GetRounded(this int val)
        {
            return val - (val % 5);
        }

        public static Double GetRounded(this Double val)
        {
            return Math.Round(val - (val % 5), 0);
        }

        
        
    }
}
