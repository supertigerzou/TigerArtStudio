using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json;
using ifunction.WebChatApi.Contract;

namespace ifunction.WebChatApi
{
    /// <summary>
    /// Extensions for common type and common actions
    /// </summary>
    public static class Extensions
    {
        #region Format regex

        /// <summary>
        /// The cell phone regex
        /// </summary>
        private static Regex cellPhoneRegex = new Regex(@"^((((\+)[0-9]{2,4}(\s)?)?)([1-9][0-9]{5,10}))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The email regex
        /// </summary>
        private static Regex emailRegex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region DateTime in milliseconds

        /// <summary>
        /// The zero time
        /// </summary>
        private static DateTime zeroTime = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Automatics the milliseconds.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.Int64.</returns>
        public static long ToMilliseconds(this DateTime dateTimeObject)
        {
            return (long)((dateTimeObject - zeroTime).TotalMilliseconds);
        }

        /// <summary>
        /// Automatics the date time.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToDateTime(this long milliseconds)
        {
            return zeroTime.AddMilliseconds(milliseconds);
        }

        #endregion

        #region Format Constants

        /// <summary>
        /// The date time format for commonly use. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01 12:01:02.
        /// </summary>
        public const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// The date time format for commonly log use. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01 12:01:02.027.
        /// </summary>
        public const string dateTimeLogFormat = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// The date time format for commonly use. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01.
        /// </summary>
        public const string dateFormat = "yyyy-MM-dd";

        /// <summary>
        /// The date time format for commonly use. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012/12/01.
        /// </summary>
        public const string westenDateFormat = "yyyy/MM/dd";

        #endregion

        #region Extensions for all objects

        /// <summary>
        /// Creates the XML node.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>XElement.</returns>
        public static XElement CreateXmlNode(this object anyObject, string nodeName = null)
        {
            return XElement.Parse(string.Format("<{0}></{0}>", !string.IsNullOrWhiteSpace(nodeName) ? nodeName : "item"));
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string GetStringValue(this object anyObject)
        {
            return anyObject != null ? anyObject.ToString() : string.Empty;
        }

        /// <summary>
        /// Gets object value for the specified object.
        /// If the object is null, then return "&lt;null&gt;". Otherwise, return ToString() result.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>System.String.</returns>
        public static string GetObjectValue(this object anyObject, object obj)
        {
            return obj == null ? "<null>" : obj.ToString();
        }

        /// <summary>
        /// Checks the null object.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <exception cref="NullObjectException"></exception>
        public static void CheckNullObject(this object anyObject, object obj, string objectIdentity)
        {
            if (obj == null)
            {
                throw new NullObjectException(objectIdentity);
            }
        }

        /// <summary>
        /// Checks the null object.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <exception cref="NullObjectException"></exception>
        public static void CheckNullObject(this object anyObject, string objectIdentity)
        {
            CheckNullObject(anyObject, anyObject, objectIdentity);
        }

        #endregion

        #region Extensions for string

        /// <summary>
        /// Gets string value for specified string object.
        /// If the object is null, then return string.Empty. Otherwise, return value.
        /// </summary>
        /// <param name="stringObj">The string obj.</param>
        /// <returns>System.String.</returns>
        public static string GetStringValue(this string stringObj)
        {
            return stringObj == null ? string.Empty : stringObj;
        }

        /// <summary>
        /// Determines whether the specified string obj is number.
        /// </summary>
        /// <param name="stringObj">The string obj.</param>
        /// <param name="min">The min.</param>
        /// <returns><c>true</c> if the specified string obj is number; otherwise, <c>false</c>.</returns>
        public static bool IsNumber(this string stringObj, decimal min = decimal.MinValue)
        {
            decimal result;
            return !string.IsNullOrWhiteSpace(stringObj) && decimal.TryParse(stringObj, out result) && result >= min;
        }

        /// <summary>
        /// Determines whether the specified string obj is index.
        /// </summary>
        /// <param name="stringObj">The string obj.</param>
        /// <returns><c>true</c> if the specified string obj is index; otherwise, <c>false</c>.</returns>
        public static bool IsIndex(this string stringObj)
        {
            return IsNumber(stringObj, 0);
        }

        #endregion

        #region Extension for DB operation

        /// <summary>
        /// To double.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Double.</returns>
        public static double DBToDouble(this object data, double defaultValue = 0)
        {
            double result;
            if (data == null || data == DBNull.Value || !double.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// DBs to float.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static float DBToFloat(this object data, float defaultValue = 0)
        {
            float result;
            if (data == null || data == DBNull.Value || !float.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int32.</returns>
        public static int DBToInt32(this object data, int defaultValue = 0)
        {
            int result;
            if (data == null || data == DBNull.Value || !int.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// To the long.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int64.</returns>
        public static long DBToLong(this object data, long defaultValue = 0)
        {
            long result;
            if (data == null || data == DBNull.Value || !long.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// To the nullable int32.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable{System.Int32}.</returns>
        public static int? DBToNullableInt32(this object data, int? defaultValue = null)
        {
            int result;
            return (data == null || data == DBNull.Value || !int.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// DBs to date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? DBToDateTime(this object data)
        {
            DateTime? result = null;

            if (data != null && data != DBNull.Value)
            {
                result = data as DateTime?;
            }

            if (result != null)
            {
                result = DateTime.SpecifyKind(result.Value, DateTimeKind.Utc);
            }

            return result;
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>DateTime.</returns>
        public static DateTime DBToDateTime(this object data, DateTime defaultValue)
        {
            DateTime? result = DBToDateTime(data);
            return result == null ? defaultValue : result.Value;
        }

        /// <summary>
        /// To GUID.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable{Guid}.</returns>
        public static Guid? DBToGuid(this object data, Guid? defaultValue = null)
        {
            Guid result;
            return (data == null || data == DBNull.Value || !Guid.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// To Bool?.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool? DBToBoolean(this object data, bool? defaultValue = null)
        {
            bool result;
            return (data == null || data == DBNull.Value || !bool.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// DBs to boolean.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DBToBoolean(this object data)
        {
            return DBToBoolean(data, false).Value;
        }

        /// <summary>
        /// To string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public static string DBToString(this object data)
        {
            return (data == null || data == DBNull.Value) ? string.Empty : data.ToString();
        }

        #endregion

        #region String format verification

        /// <summary>
        /// Determines whether [is email address] [the specified string object].
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns><c>true</c> if [is email address] [the specified string object]; otherwise, <c>false</c>.</returns>
        public static bool IsEmailAddress(this string stringObject)
        {
            return !string.IsNullOrWhiteSpace(stringObject) && emailRegex.IsMatch(stringObject);
        }


        /// <summary>
        /// Determines whether [is cell phone] [the specified string object].
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns><c>true</c> if [is cell phone] [the specified string object]; otherwise, <c>false</c>.</returns>
        public static bool IsCellPhone(this string stringObject)
        {
            return !string.IsNullOrWhiteSpace(stringObject) && cellPhoneRegex.IsMatch(stringObject);
        }

        #endregion

        #region String Convert Extensions

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>Int32.</returns>
        public static Int32 ToInt32(this string stringObject)
        {
            Int32 result = 0;
            Int32.TryParse(stringObject, out result);
            return result;
        }

        /// <summary>
        /// Automatics the long.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>System.Int64.</returns>
        public static long ToLong(this string stringObject)
        {
            long result = 0;
            long.TryParse(stringObject, out result);
            return result;
        }

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>Double.</returns>
        public static Double ToDouble(this string stringObject)
        {
            Double result = 0;
            Double.TryParse(stringObject, out result);
            return result;
        }

        /// <summary>
        /// To the decimal.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ToDecimal(this string stringObject)
        {
            Decimal result = 0;
            Decimal.TryParse(stringObject, out result);
            return result;
        }

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultDateTime">The default date time.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? ToDouble(this string stringObject, DateTime? defaultDateTime = null)
        {
            DateTime output;
            return DateTime.TryParseExact(stringObject, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out output) ?
                output
                : defaultDateTime;
        }

        /// <summary>
        /// To the grid.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultGuid">The default GUID.</param>
        /// <returns>System.Nullable{Guid}.</returns>
        public static Guid? ToGrid(this string stringObject, Guid? defaultGuid = null)
        {
            Guid output;
            return Guid.TryParse(stringObject, out output) ?
                output
                : defaultGuid;
        }

        /// <summary>
        /// Froms the string to date time.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultDateTime">The default date time.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? FromStringToDateTime(this string stringObject, DateTime? defaultDateTime = null)
        {
            DateTime output;
            return DateTime.TryParseExact(stringObject, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out output) ?
                output
                : defaultDateTime;
        }

        /// <summary>
        /// Froms the string to date.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultDate">The default date.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? FromStringToDate(this string stringObject, DateTime? defaultDate = null)
        {
            DateTime output;
            return DateTime.TryParseExact(stringObject, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out output) ?
                output
                : defaultDate;
        }

        #endregion

        #region DateTime Extensions

        /// <summary>
        /// Automatics the UTC.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="currentTimeZoneOffset">The current time zone offset.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToUtc(this DateTime dateTimeObject, TimeSpan currentTimeZoneOffset = default(TimeSpan))
        {
            if (dateTimeObject.Kind == DateTimeKind.Unspecified)
            {
                dateTimeObject = (new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond, DateTimeKind.Utc)) - currentTimeZoneOffset;
            }

            if (dateTimeObject.Kind == DateTimeKind.Local)
            {
                dateTimeObject = dateTimeObject.ToUniversalTime();
            }

            return dateTimeObject;
        }

        /// <summary>
        /// Automatics the UTC.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="currentTimeZoneOffsetInMinute">The current time zone offset information minute.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToUtc(this DateTime dateTimeObject, int currentTimeZoneOffsetInMinute = 0)
        {
            return ToUtc(dateTimeObject, new TimeSpan(0, currentTimeZoneOffsetInMinute, 0));
        }

        /// <summary>
        /// Automatics the different time zone.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="targetTimeZoneOffset">The target time zone offset.</param>
        /// <param name="currentTimeZoneOffset">The current time zone offset.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToDifferentTimeZone(this DateTime dateTimeObject, TimeSpan targetTimeZoneOffset, TimeSpan currentTimeZoneOffset = default(TimeSpan))
        {
            var utc = dateTimeObject.ToUtc(currentTimeZoneOffset);
            return (new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, utc.Millisecond, DateTimeKind.Local)) + targetTimeZoneOffset;
        }

        /// <summary>
        /// Automatics the different time zone.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="targetTimeZoneOffsetInMinute">The target time zone offset information minute.</param>
        /// <param name="currentTimeZoneOffsetInMinute">The current time zone offset information minute.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToDifferentTimeZone(this DateTime dateTimeObject, int targetTimeZoneOffsetInMinute, int currentTimeZoneOffsetInMinute = 0)
        {
            var utc = dateTimeObject.ToUtc(currentTimeZoneOffsetInMinute);
            return (new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, utc.Millisecond, DateTimeKind.Local)) + new TimeSpan(0, targetTimeZoneOffsetInMinute, 0);
        }

        /// <summary>
        /// To the date time string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToDateTimeString(this DateTime dateTimeObject)
        {
            return dateTimeObject.ToString(dateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the date time string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToDateTimeString(this DateTime? dateTimeObject)
        {
            return dateTimeObject == null ? string.Empty : dateTimeObject.Value.ToDateTimeString();
        }

        /// <summary>
        /// To the date string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToDateString(this DateTime dateTimeObject)
        {
            return dateTimeObject.ToString(dateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the date string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToDateString(this DateTime? dateTimeObject)
        {
            return dateTimeObject == null ? string.Empty : dateTimeObject.Value.ToDateString();
        }

        /// <summary>
        /// To the log stamp string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToLogStampString(this DateTime dateTimeObject)
        {
            return dateTimeObject.ToString(dateTimeLogFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the log stamp string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToLogStampString(this DateTime? dateTimeObject)
        {
            return dateTimeObject == null ? string.Empty : dateTimeObject.Value.ToLogStampString();
        }

        /// <summary>
        /// Gets the first day of month.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime dateTimeObject)
        {
            return new DateTime(dateTimeObject.Year,
                dateTimeObject.Month,
                1,
                dateTimeObject.Hour,
                dateTimeObject.Minute,
                dateTimeObject.Second);
        }

        /// <summary>
        /// Gets the last day of month.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTimeObject)
        {
            return new DateTime(dateTimeObject.Year,
                dateTimeObject.Month,
                1,
                dateTimeObject.Hour,
                dateTimeObject.Minute,
                dateTimeObject.Second).AddMonths(1).AddDays(-1);
        }

        #endregion

        #region XElement Extensiosn

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified element has elements; otherwise, <c>false</c>.</returns>
        public static bool HasElements(this XElement element, string name = null)
        {
            return element != null
                && (!string.IsNullOrEmpty(name) ? element.Elements(name).Count() > 0 : element.Elements().Count() > 0);
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>System.String.</returns>
        public static string GetAttributeValue(this XElement xElement, string attribute)
        {
            string result = string.Empty;

            if (xElement != null && !string.IsNullOrWhiteSpace(attribute))
            {
                var attr = xElement.Attribute(attribute);
                if (attr != null)
                {
                    result = attr.Value;
                }
            }

            return result;
        }

        #endregion

        #region Dictionary Extensions

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="keyValueCollection">The key value collection.</param>
        public static void Merge<T, V>(this Dictionary<T, V> instance, ICollection<KeyValuePair<T, V>> keyValueCollection)
        {
            if (instance != null && keyValueCollection != null)
            {
                if (keyValueCollection.Count > 0)
                {
                    foreach (var one in keyValueCollection)
                    {
                        T key = one.Key;
                        V value = one.Value;

                        if (key != null)
                        {
                            instance.Merge(key, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="keyValueCollection">The key value collection.</param>
        public static void Load<T, V>(this Dictionary<T, V> instance, ICollection<KeyValuePair<T, V>> keyValueCollection)
        {
            if (instance != null)
            {
                instance.Clear();
                instance.Merge(keyValueCollection);
            }
        }

        /// <summary>
        /// To the key value collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>List{KeyValuePair{``0``1}}.</returns>
        public static List<KeyValuePair<T, V>> ToKeyValueCollection<T, V>(this Dictionary<T, V> instance)
        {
            List<KeyValuePair<T, V>> result = null;

            if (instance != null)
            {
                result = new List<KeyValuePair<T, V>>();

                foreach (var key in instance.Keys)
                {
                    result.Add(new KeyValuePair<T, V>(key, instance[key]));
                }
            }

            return result;
        }

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>Dictionary{``0``1}.</returns>
        public static Dictionary<T, V> ToDictionary<T, V>(this List<KeyValuePair<T, V>> instance)
        {
            Dictionary<T, V> result = null;

            if (instance != null)
            {
                result = new Dictionary<T, V>();
                result.Merge(instance);
            }

            return result;
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void Merge<T, V>(this Dictionary<T, V> instance, T key, V value)
        {
            if (instance != null && key != null)
            {
                if (instance.ContainsKey(key))
                {
                    instance[key] = value;
                }
                else
                {
                    instance.Add(key, value);
                }
            }
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>XElement.</returns>
        public static XElement ToXml<T, V>(this Dictionary<T, V> instance)
        {
            if (instance != null)
            {
                SerializableDictionary<T, V> tmp = new SerializableDictionary<T, V>(instance);

                return tmp.ToXml();
            }

            return null;
        }

        /// <summary>
        /// To the XML string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>System.String.</returns>
        public static string ToXmlString<T, V>(this Dictionary<T, V> instance)
        {
            return ToXml(instance).DBToString();
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>Dictionary{``0``1}.</returns>
        public static Dictionary<T, V> LoadFromXml<T, V>(this Dictionary<T, V> instance, string xml)
        {
            if (instance != null && !string.IsNullOrWhiteSpace(xml))
            {
                SerializableDictionary<T, V> tmp = new SerializableDictionary<T, V>();
                tmp.LoadFromXml(XElement.Parse(xml));

                return new Dictionary<T, V>(tmp);
            }

            return null;
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="clearExisting">if set to <c>true</c> [clear existing].</param>
        public static void LoadFromXml<T, V>(this Dictionary<T, V> instance, XElement xml, bool clearExisting = false)
        {
            if (instance != null && xml != null)
            {
                SerializableDictionary<T, V> tmp = new SerializableDictionary<T, V>();
                tmp.LoadFromXml(xml);

                if (clearExisting)
                {
                    instance.Clear();
                }

                foreach (var key in tmp.Keys)
                {
                    instance.Merge(key, tmp[key]);
                }
            }
        }

        #endregion

        #region IEnumerable, ICollection, IList

        /// <summary>
        /// Determines whether the specified instance has item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if the specified instance has item; otherwise, <c>false</c>.</returns>
        public static bool HasItem<T>(this IEnumerable<T> instance)
        {
            return instance != null && instance.Count() > 0;
        }

        /// <summary>
        /// Joins the items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="sperator">The sperator.</param>
        /// <returns>System.String.</returns>
        public static string Join<T>(this IEnumerable<T> instance, string sperator)
        {
            return string.Join<T>(sperator, instance);
        }

        #endregion

        #region Bytes

        /// <summary>
        /// Reads the stream to bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] ReadStreamToBytes(this Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }


        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The <see cref="Byte" />  array of stream.</returns>
        /// <exception cref="OperationFailureException">StreamToBytes</exception>
        public static byte[] ToBytes(this Stream stream)
        {
            byte[] bytes = null;

            try
            {
                if (stream != null && stream.Length > 0)
                {
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                }
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("StreamToBytes", ex);
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }

            return bytes;
        }

        /// <summary>
        /// To the stream.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The <see cref="Stream" />  for byte array.</returns>
        /// <exception cref="OperationFailureException">BytesToStream</exception>
        public static Stream ToStream(this byte[] bytes)
        {
            Stream stream = null;

            try
            {
                if (bytes != null)
                {
                    stream = new MemoryStream(bytes);
                }
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("BytesToStream", ex);
            }

            return stream;
        }

        #endregion

        #region IO

        /// <summary>
        /// The dot
        /// </summary>
        const string dot = ".";

        /// <summary>
        /// Combines the extension.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="pureFileName">Name of the pure file.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>System.String.</returns>
        public static string CombineExtension(this object anyObject, string pureFileName, string extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                extension = dot + extension.Replace(dot, string.Empty);
            }

            return pureFileName.GetStringValue() + extension;
        }


        /// <summary>
        /// Reads the file content lines.
        /// This method would not impact the conflict for reading and writing.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String[][].</returns>
        /// <exception cref="OperationFailureException">GetFileContentLines</exception>
        public static string[] ReadFileContentLines(this object anyObject, string path, Encoding encoding)
        {
            Stream stream = null;

            StreamReader streamReader = null;
            List<string> lines = new List<string>();

            try
            {
                stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(stream, encoding); string stringLine = string.Empty;

                do
                {
                    stringLine = streamReader.ReadLine();
                    lines.Add(stringLine);
                }
                while (stringLine != null);

                if (lines.Count > 0)
                {
                    lines.RemoveAt(lines.Count - 1);
                }
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("GetFileContentLines", ex, path);
            }
            finally
            {
                streamReader.Close();
                stream.Close();
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Reads the file content lines.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="path">The path.</param>
        /// <returns>System.String[][].</returns>
        public static string[] ReadFileContentLines(this object anyObject, string path)
        {
            return ReadFileContentLines(anyObject, path, Encoding.UTF8);
        }

        /// <summary>
        /// Reads the file contents.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">ReadFileContens</exception>
        public static string ReadFileContents(this object anyObject, string path, Encoding encoding)
        {
            Stream stream = null;
            StreamReader streamReader = null;

            try
            {
                stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(stream, encoding);
                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("ReadFileContents", ex, path);
            }
            finally
            {
                streamReader.Close();
                stream.Close();
            }
        }

        /// <summary>
        /// Reads the file contents.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        public static string ReadFileContents(this object anyObject, string path)
        {
            return ReadFileContents(anyObject, path);
        }

        /// <summary>
        /// Reads the file bytes.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="path">The path.</param>
        /// <returns>System.Byte[][].</returns>
        /// <exception cref="OperationFailureException">ReadFileBytes</exception>
        public static byte[] ReadFileBytes(this object anyObject, string path)
        {
            Stream stream = null;
            StreamReader streamReader = null;

            try
            {
                stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return stream.ReadFileBytes(path);
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("ReadFileBytes", ex, path);
            }
            finally
            {
                streamReader.Close();
                stream.Close();
            }
        }

        #endregion

        #region Encodings

        /// <summary>
        /// To the URL path encoded text.
        /// </summary>
        /// <param name="originalText">The originalText.</param>
        /// <returns>System.String.</returns>
        public static string ToUrlPathEncodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.UrlPathEncode(originalText);
            }

            return originalText;
        }

        /// <summary>
        /// To the URL encoded text.
        /// </summary>
        /// <param name="originalText">The originalText.</param>
        /// <returns>System.String.</returns>
        public static string ToUrlEncodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.UrlEncode(originalText, Encoding.UTF8);
            }

            return originalText;
        }

        /// <summary>
        /// To the URL decoded text.
        /// </summary>
        /// <param name="originalText">The originalText.</param>
        /// <returns>System.String.</returns>
        public static string ToUrlDecodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.UrlDecode(originalText, Encoding.UTF8);
            }

            return originalText;
        }

        /// <summary>
        /// To the HTML encoded text.
        /// </summary>
        /// <param name="originalText">The originalText.</param>
        /// <returns>System.String.</returns>
        public static string ToHtmlEncodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.HtmlEncode(originalText);
            }

            return originalText;
        }

        /// <summary>
        /// To the HTML decoded text.
        /// </summary>
        /// <param name="originalText">The originalText.</param>
        /// <returns>System.String.</returns>
        public static string ToHtmlDecodedText(this string originalText)
        {
            if (originalText != null)
            {
                originalText = HttpUtility.HtmlDecode(originalText);
            }

            return originalText;
        }

        #endregion

        #region Regex

        /// <summary>
        /// Gets the regex match value.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="content">The content.</param>
        /// <param name="variable">The variable.</param>
        /// <returns>System.String.</returns>
        public static string GetRegexMatchValue(this Regex regex, string content, string variable)
        {
            string result = string.Empty;

            if (regex != null && !string.IsNullOrWhiteSpace(content) && variable != null)
            {
                var match = regex.Match(content);
                if (match.Success)
                {
                    result = match.Result("${" + variable + "}");
                }
            }

            return result;
        }


        /// <summary>
        /// Gets the regex match values.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="content">The content.</param>
        /// <param name="variable">The variable.</param>
        /// <returns>System.String[][].</returns>
        public static string[] GetRegexMatchValues(this Regex regex, string content, string variable)
        {
            List<string> result = new List<string>();

            if (regex != null && !string.IsNullOrWhiteSpace(content) && variable != null)
            {
                var matches = regex.Matches(content);
                if (matches.Count > 0)
                {
                    foreach (Match one in matches)
                    {
                        string value = one.Result("${" + variable + "}");
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            result.Add(value);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Fills the regex match value.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="content">The content.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>Dictionary{System.StringSystem.String}.</returns>
        public static Dictionary<string, string> GetRegexMatchValues(this Regex regex, string content, Dictionary<string, string> variables)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (regex != null && !string.IsNullOrWhiteSpace(content) && variables != null)
            {
                var match = regex.Match(content);
                if (match.Success)
                {
                    foreach (var key in variables.Keys)
                    {
                        result.Add(key, match.Result("${" + key + "}"));
                    }
                }
            }

            return result;
        }

        #endregion

        #region Json

        /// <summary>
        /// Jsons to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson">The STR json.</param>
        /// <returns>``0.</returns>
        public static T JsonToObject<T>(this string strJson) where T : new()
        {
            T t = new T();

            if (!string.IsNullOrWhiteSpace(strJson))
            {
                t = JsonConvert.DeserializeObject<T>(strJson);
            }

            return t;
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string ToJson(this object anyObject)
        {
            string result = string.Empty;

            if (anyObject != null)
            {
                result = JsonConvert.SerializeObject(anyObject);
            }

            return result;
        }

        #endregion

        #region Compression

        /// <summary>
        /// Compresses the specified bytes object.
        /// </summary>
        /// <param name="bytesObject">The bytes object.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">Compress</exception>
        public static string CompressBytesToString(this  byte[] bytesObject)
        {
            if (bytesObject != null && bytesObject.Length > 0)
            {
                try
                {
                    var memoryStream = new MemoryStream();
                    using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                    {
                        gZipStream.Write(bytesObject, 0, bytesObject.Length);
                    }

                    memoryStream.Position = 0;

                    var compressedData = new byte[memoryStream.Length];
                    memoryStream.Read(compressedData, 0, compressedData.Length);

                    var gZipBuffer = new byte[compressedData.Length + 4];
                    Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                    Buffer.BlockCopy(BitConverter.GetBytes(bytesObject.Length), 0, gZipBuffer, 0, 4);
                    return Convert.ToBase64String(gZipBuffer);
                }
                catch (Exception ex)
                {
                    throw new OperationFailureException("Compress", ex, bytesObject);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Compresses the specified string object.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string Compress(this string stringObject, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] buffer = string.IsNullOrWhiteSpace(stringObject) ? null : encoding.GetBytes(stringObject);
            return CompressBytesToString(buffer);
        }

        /// <summary>
        /// Compresses the object automatic string.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string CompressObjectToString(this object anyObject)
        {
            if (anyObject != null)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                byte[] bytes = null;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, anyObject);
                    bytes = memoryStream.ToArray();
                }

                return bytes.CompressBytesToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Decompresses the specified string object.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string Decompress(this string stringObject, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = DecompressStringToBytes(stringObject);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Decompresses the string automatic object.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>System.Object.</returns>
        public static object DecompressStringToObject(this string stringObject)
        {
            object result = null;

            var bytes = stringObject.DecompressStringToBytes();

            if (bytes != null && bytes.Length > 0)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    result = binaryFormatter.Deserialize(memoryStream);
                }
            }

            return result;
        }

        /// <summary>
        /// Decompresses the string automatic bytes.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>System.Byte[][].</returns>
        /// <exception cref="OperationFailureException">Decompress</exception>
        public static byte[] DecompressStringToBytes(this string stringObject)
        {
            if (!string.IsNullOrWhiteSpace(stringObject))
            {
                try
                {
                    byte[] gZipBuffer = Convert.FromBase64String(stringObject);
                    using (var memoryStream = new MemoryStream())
                    {
                        int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                        memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                        var buffer = new byte[dataLength];

                        memoryStream.Position = 0;
                        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                        {
                            gZipStream.Read(buffer, 0, buffer.Length);
                        }

                        return gZipBuffer;
                    }
                }
                catch (Exception ex)
                {
                    throw new OperationFailureException("Decompress", ex, stringObject);
                }
            }

            return new byte[] { };
        }

        #endregion

        #region Encrption

        /// <summary>
        /// Encrypts to SHA1.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string EncryptToSHA1(this string input)
        {
            return EncryptToSHA1(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// Encrypts to SH a1.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public static string EncryptToSHA1(this byte[] data)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                var hash_byte = sha1.ComputeHash(data);
                string result = System.BitConverter.ToString(hash_byte);
                return result.Replace("-", "").ToUpperInvariant();
            }
        }

        #endregion

        #region Xml

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="xElement">The executable element.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>System.String.</returns>
        public static string GetValue(this XElement xElement, string nodeName)
        {
            string result = string.Empty;

            if (xElement != null && !string.IsNullOrWhiteSpace(nodeName))
            {
                var row = xElement.Element(nodeName);

                if (row != null)
                {
                    result = row.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="value">The value.</param>
        /// <param name="omitCDATA">if set to <c>true</c> [omit cdata].</param>
        public static void SetValue(this XElement container, string nodeName, string value, bool omitCDATA = false)
        {
            if (container != null && !string.IsNullOrWhiteSpace(nodeName))
            {
                XElement row = container.CreateXmlNode(nodeName);

                if (!omitCDATA)
                {
                    row.Add(new XCData(value));
                }
                else
                {
                    row.Value = value;
                }

                container.Add(row);
            }
        }


        #endregion

        #region Http

        /// <summary>
        /// Gets the post data from HTTP web request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] GetPostDataFromHttpWebRequest(this HttpRequest httpRequest)
        {
            byte[] data = null;

            if (httpRequest != null)
            {
                MemoryStream ms = new MemoryStream();
                httpRequest.InputStream.CopyTo(ms);
                data = ms.ToArray();
            }

            return data;
        }

        #endregion
    }
}
