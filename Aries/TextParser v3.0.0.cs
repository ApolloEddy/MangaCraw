// Rebuild at 2020/08/20
// Version 3.0.0 

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Aries
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// 使用正则表达式从源字符串匹配单个实例
		/// </summary>
		/// <param name="pattern">正则表达式模式</param>
		/// <returns>匹配到的字符串实例：<seealso cref="string"/></returns>
		public static string regMatchOne(this string str, string pattern)
		{
			var reg = new Regex(pattern);
			return reg.Match(str).Value;
		}
		/// <summary>
		/// 使用正则表达式从源字符串匹配所有的实例
		/// </summary>
		/// <param name="pattern">正则表达式模式</param>
		/// <returns>匹配到的字符串列表：<seealso cref="string[]"/></returns>
		public static string[] regMatch(this string str, string pattern)
		{
			var reg = new Regex(pattern);
			var ret = new List<string>();
			foreach (Match item in reg.Matches(str))
			{
				ret.Add(item.Value);
			}
			return ret.ToArray();
		}
		/// <summary>
		/// 根据前后字符串标识从字符串中提取单个实例
		/// </summary>
		/// <param name="start">起始字符串</param>
		/// <param name="end">结束字符串</param>
		/// <returns>匹配到的字符串实例：<seealso cref="string"/></returns>
		public static string MatchOne(this string str, string start, string end)
		{
			var reg = new Regex($"{start}(?<ret>(.+?)){end}");
			return reg.Match(str).Groups["ret"].Value;
		}
		/// <summary>
		/// 根据前后字符串标识从字符串中提取所有实例
		/// </summary>
		/// <param name="start">起始字符串</param>
		/// <param name="end">结束字符串</param>
		/// <returns>提取到的所有字符串实例：<see cref="List{String}"/></returns>
		public static string[] Extract(this string str, string start, string end)
		{
			var reg = new Regex($"{start}(?<ret>(.+?)){end}");
			var ret = new List<string>();
			string cache;
			foreach (Match item in reg.Matches(str))
			{
				cache = item.Groups["ret"].Value;
				ret.Add(cache);
			}
			return ret.ToArray();
		}
		/// <summary>
		/// 依次从源字符串中移除指定的字符串
		/// </summary>
		/// <param name="targets">所有待移除的字符串</param>
		/// <returns>新的字符串实例：<seealso cref="string"/></returns>
		public static string Remove(this string str, params string[] targets)
		{
			var builder = new StringBuilder(str);
			foreach (string item in targets)
			{
				builder.Replace(item, string.Empty);
			}
			return builder.ToString();
		}
		/// <summary>
		/// 移除字符串中所有符合正则 (<see cref="Regex"/>) 的字符串
		/// </summary>
		/// <param name="pattern">正则表达式 (<see cref="Regex"/>) 模式</param>
		/// <returns><see cref="string"/></returns>
		public static string RemoveReg(this string str, string pattern)
        {
			string result = str;
            foreach (var temp in regMatch(str, pattern))
            {
				result = result.Replace(temp, "");
            }
			str = result;
			return result;
        }
		/// <summary>
		/// 去除源字符串的所有换行符('\n')、回车符('\r')、换页符('\f')
		/// </summary>
		/// <param name="input"></param>
		/// <returns>新的字符串实例：<seealso cref="string"/></returns>
		public static string RemoveNewlineChars(this string str)
		{
			return Remove(str, "\n", "\r", "\f");
		}
		/// <summary>
		/// 将字符串转义为URI字符串
		/// </summary>
		/// <returns>转义后的字符串：<seealso cref="string"/></returns>
		public static string EscapeDataString(this string str)
		{
			return Uri.EscapeDataString(str);
		}
		/// <summary>
		/// 将URI字符串反转义为文本字符串
		/// </summary>
		/// <returns>反转义后的字符串：<seealso cref="string"/></returns>
		public static string UnescapeDataString(this string str)
		{
			return Uri.UnescapeDataString(str);
		}
		/// <summary>
		/// 改变当前字符串实例的编码
		/// </summary>
		/// <param name="encoding">需要转化为的编码</param>
		/// <returns><see cref="Encoding"/></returns>
		public static string ChangeEncoding(this string str, Encoding encoding)
		{
			using (MemoryStream ms = new MemoryStream(ToBytes(str, encoding)))
			{
				StreamReader reader = new StreamReader(ms);
				str = reader.ReadToEnd();
				reader.Dispose();
				return str;
			}
		}
		/// <summary>
		/// 改变当前字符串实例的编码
		/// </summary>
		/// <param name="encoding">需要转化为的编码名称</param>
		/// <returns><see cref="Encoding"/></returns>
		public static string ChangeEncoding(this string str, string encodingName)
		{
			str = ChangeEncoding(str, Encoding.GetEncoding(encodingName));
			return str;
		}
		/// <summary>
		/// 将此字符串实例转化为 <see cref="byte[]"/> 字节数组类型
		/// </summary>
		/// <returns>转化后的 <see cref="byte[]"/> 字节数组</returns>
		public static byte[] ToBytes(this string str)
		{ return Encoding.UTF8.GetBytes(str); }
		/// <summary>
		/// 将此字符串实例转化为指定编码的 <see cref="byte[]"/> 字节数组类型
		/// </summary>
		/// <param name="encoding">指定要转化为的编码</param>
		/// <returns>转化后的 <see cref="byte[]"/> 字节数组</returns>
		public static byte[] ToBytes(this string str, Encoding encoding)
		{ return encoding.GetBytes(str); }
		/// <summary>
		/// 将此字符串实例转化为指定编码的 <see cref="byte[]"/> 字节数组类型
		/// </summary>
		/// <param name="encodingName">指定编码的名称</param>
		/// <returns>转化后的 <see cref="byte[]"/> 字节数组</returns>
		public static byte[] ToBytes(this string str, string encodingName)
		{ return Encoding.GetEncoding(encodingName).GetBytes(str); }
		/// <summary>
		/// 将此字符串实例转化为Base64编码的字符串
		/// </summary>
		/// <returns>Base64编码的字符串</returns>
		public static string ToBase64(this string str)
		{ return Convert.ToBase64String(ToBytes(str)); }
		/// <summary>
		/// 将此字符串实例从指定编码格式转化为Base64编码的字符串
		/// </summary>
		/// <param name="encoding">指定的编码格式</param>
		/// <returns>Base64编码的字符串</returns>
		public static string ToBase64(this string str, Encoding encoding)
		{ return Convert.ToBase64String(ToBytes(str, encoding)); }
		/// <summary>
		/// 将此字符串实例从指定编码格式转化为Base64编码的字符串
		/// </summary>
		/// <param name="encodingName">指定的编码格式名称</param>
		/// <returns>Base64编码的字符串</returns>
		public static string ToBase64(this string str, string encodingName)
		{ return Convert.ToBase64String(ToBytes(str, encodingName)); }
		/// <summary>
		/// 返回此实例所表示的整型值
		/// </summary>
		/// <returns><see cref="int"/></returns>
		public static int ToInt(this string str)
		{ return int.Parse(str); }
		/// <summary>
		/// 返回此实例所表示的长整型值
		/// </summary>
		/// <returns><see cref="long"/></returns>
		public static long ToLong(this string str)
		{ return long.Parse(str); }
		/// <summary>
		/// 返回此实例所表示的单精度浮点值
		/// </summary>
		/// <returns><see cref="float"/></returns>
		public static float ToFloat(this string str)
		{ return float.Parse(str); }
		/// <summary>
		/// 返回此实例所表示的双精度浮点值
		/// </summary>
		/// <returns><see cref="double"/></returns>
		public static double ToDouble(this string str)
		{ return double.Parse(str); }
	}
}
