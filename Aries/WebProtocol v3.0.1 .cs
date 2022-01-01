// Rebuild at 2020/08/20
// Version 3.0.0 
// new(v3.0.0): rebuild the class;
//				to insure this class can be disposed.
// new(v3.0.1): add new methods to add or get headers and cookies;
//				add new method to initialize this object.
//

using System;
using System.IO;
using System.Net;
using System.Text;

namespace Aries
{
	/// <summary>
	/// 提供网络交互的方法
	/// </summary>
	public class WebProtocol : IDisposable
	{
		// 构造函数
		public WebProtocol()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
			request = (HttpWebRequest)WebRequest.Create(url);
			request.CookieContainer = new CookieContainer();
			Timeout = 1000;
		}
		public WebProtocol(string url)
		{
			this.url = url;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
			request = (HttpWebRequest)WebRequest.Create(url);
			request.CookieContainer = new CookieContainer();
			Timeout = 1000;
		}

		// 字段、属性
		/// <summary>
		/// 指定是否使用PC端的UA标识
		/// </summary>
		public bool computerUAsign = true;
		/// <summary>
		/// 指定是否使用随机UA标识
		/// </summary>
		public bool autoUA = true;
		/// <summary>
		/// 获取每次请求的时间间隔
		/// </summary>
		public int requestInterval
		{
			get
			{
				return rand.Next(15, 20);
			}
		}
		protected HttpWebRequest request;
		protected HttpWebResponse response;
		protected RandomUserAgent randUA = new RandomUserAgent();

		/// <summary>
		/// 获取或设置请求的目标URL链接
		/// </summary>
		public string url { get; set; }
		/// <summary>
		/// 获取或设置当前的UA标识
		/// </summary>
		public string UserAgent
		{
			get
			{
				return request.UserAgent;
			}
			set
			{
				request.UserAgent = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求的引用来源
		/// </summary>
		public string Referer
		{
			get
			{
				return request.Referer;
			}
			set
			{
				request.Referer = value;
			}
		}
		public string Host
		{
			get { return request.Host; }
			set { request.Host = value; }
		}
		/// <summary>
		/// 获取或设置当前请求接受的数据格式
		/// </summary>
		public string Accept
		{
			get
			{
				return request.Accept;
			}
			set
			{
				request.Accept = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求的Cookies缓存信息
		/// </summary>
		public CookieContainer Cookies
		{
			get
			{
				return request.CookieContainer;
			}
			private set
			{
				Cookies = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求的Connection值
		/// </summary>
		public string Connection
		{
			get
			{
				return request.Connection;
			}
			set
			{
				request.Connection = value;
			}
		}
		public bool KeepAlive
		{
			get
			{
				return request.KeepAlive;
			}
			set
			{
				request.KeepAlive = value;
			}
		}
		/// <summary>
		/// 获取或设置当前的请求方法
		/// </summary>
		public string Method
		{
			get
			{
				return request.Method;
			}
			set
			{
				request.Method = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求的普通请求报头
		/// </summary>
		public WebHeaderCollection Headers
		{
			get
			{
				return request.Headers;
			}
			set
			{
				request.Headers = value;
			}
		}
		/// <summary>
		/// 获取或设置当前请求允许等待的最多响应时长
		/// </summary>
		public int Timeout
		{
			get
			{
				return request.Timeout;
			}
			set
			{
				request.Timeout = value;
			}
		}
		private Random rand = new Random(DateTime.Now.GetHashCode());
		private bool disposedValue;

		/// <summary>
		/// 获取请求返回的响应流
		/// </summary>
		/// <returns>数据流：<seealso cref="Stream"/></returns>
		public Stream Content
		{
			get
			{
				using (MemoryStream ms = new MemoryStream() { Position = 0 })
				{
					GetContentStream().CopyTo(ms);
					return ms;
				}					
			}
		}
		/// <summary>
		/// 获取请求返回的字符串报文
		/// </summary>
		/// <returns>数据报文：<seealso cref="string"/></returns>
		public string ContentDocument
		{
			get
			{
				using (StreamReader reader = new StreamReader(GetContentStream()))
				{ return reader.ReadToEnd(); }
			}
		}
		/// <summary>
		/// 获取请求返回的响应字节数组
		/// </summary>
		/// <returns>字节流数组：<seealso cref="byte[]"/></returns>
		public byte[] ContentBytes
		{
			get
			{
				using (MemoryStream ms = (MemoryStream)Content)
				{ return ms.ToArray(); }
			}
		}

		// 用于设置属性的方法
		/// <summary>
		/// 指示启用客户端UserAgent
		/// </summary>
		public void EnableComputerUserAgent()
		{ computerUAsign = true; }
		/// <summary>
		/// 指示启用移动端UserAgent
		/// </summary>
		public void EnableMobileUserAgent()
		{ computerUAsign = false; }
		/// <summary>
		/// 指示启用自动更换UserAgent
		/// </summary>
		public void EnableAutoUserAgent()
		{ autoUA = true; }
		/// <summary>
		/// 指示禁用自动更换UserAgent
		/// </summary>
		public void DisableAutoUserAgent()
		{ autoUA = false; }
		/// <summary>
		/// 指示启用GET请求方法
		/// </summary>
		public void EnableGetMethod()
		{ Method = "GET"; }
		/// <summary>
		/// 自动配置请求信息
		/// </summary>
		public void Initialize()
		{
			EnableAutoUserAgent();
			EnableComputerUserAgent();
			EnableGetMethod();
			Timeout = 5000;
			Referer = url;
			Host = new Uri(url).Host;
			Headers = new WebHeaderCollection();
			Cookies = new CookieContainer();
		}

		// 函数
		/// <summary>
		/// 将字符串转义为URI字符串
		/// </summary>
		/// <param name="input">待转换的字符串</param>
		/// <returns>转义后的字符串：<seealso cref="string"/></returns>
		public static string EscapeDataString(string input)
		{
			return Uri.EscapeDataString(input);
		}
		/// <summary>
		/// 将URI字符串反转义为文本字符串
		/// </summary>
		/// <param name="input">待反转换的字符串</param>
		/// <returns>反转义后的字符串：<seealso cref="string"/></returns>
		public static string UnescapeDataString(string input)
		{
			return Uri.UnescapeDataString(input);
		}
		/// <summary>
		/// 将输入的字符串发送到服务端
		/// </summary>
		/// <param name="content">待发送的字符串</param>
		public void Post(string content)
		{
			Post(Encoding.UTF8.GetBytes(content));
		}
		/// <summary>
		/// 将输入的流数据发送到服务端
		/// </summary>
		/// <param name="content">待发送的流数据</param>
		public void Post(Stream content)
		{			
			using (MemoryStream ms = (MemoryStream)content)
			{ Post(ms.ToArray()); }
		}
		/// <summary>
		/// 将输入的字节数据发送到服务端
		/// </summary>
		/// <param name="content">待发送的字节数据</param>
		public void Post(byte[] bytes)
		{
			request.Method = "POST";
			request.ContentLength = bytes.Length;
			var requestStream = request.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
		}
		/// <summary>
		/// 添加一个Cookie键值对到request对象
		/// </summary>
		/// <param name="name">键</param>
		/// <param name="value">值</param>
		/// <param name="domain">该cookie的作用域</param>
		public void AddCookie(string name, string value, string domain)
		{
			string host = new Uri(url).Host;
			Cookie cookieItem = new Cookie(name, value);
			if (Cookies is null)
			{ Cookies = new CookieContainer(); }
			Cookies.SetCookies(new Uri(host), name);
			Cookies.Add(cookieItem);
		}
		/// <summary>
		/// 从Cookies字符串中批量添加Cookie
		/// </summary>
		/// <param name="cookiesString">包含了Cookies的字符串</param>
		/// <param name="domain">每个Cookie的作用域</param>
		public void AddCookies(string cookiesString, string domain)
		{
			foreach (string item in cookiesString.Split(';'))
			{ 
				string[] parts = item.Split('=');
				AddCookie(parts[0], parts[1], domain);
			}
		}
		/// <summary>
		/// 获取有关指定URI的Cookies
		/// </summary>
		/// <param name="uri">指定的URI</param>
		/// <returns><see cref="CookieCollection"/></returns>
		public CookieCollection GetCookies(Uri uri)
		{
			if (Cookies is null)
			{ return null; }
			return Cookies.GetCookies(uri);
		}
		/// <summary>
		/// 获取实例对应的的Cookies
		/// </summary>
		/// <returns><see cref="CookieCollection"/></returns>
		public CookieCollection GetCookies()
		{ return GetCookies(new Uri(new Uri(url).Host)); }
		/// <summary>
		/// 添加一个Header键值对到request对象
		/// </summary>
		/// <param name="name">键</param>
		/// <param name="value">值</param>
		public void AddHeader(string name, string value)
		{
			if (Headers is null )
			{ Headers = new WebHeaderCollection(); }
			Headers.Add(name, value);
		}
		/// <summary>
		/// 从Headers字符串中批量添加Header
		/// </summary>
		/// <param name="headersString">包含了Headers的字符串</param>
		public void AddHeaders(string headersString)
		{
			foreach (string item in headersString.Split('&'))
			{
				string[] parts = item.Split('=');
				AddHeader(parts[0], parts[1]);
			}
		}
		/// <summary>
		/// 将新的Headers加入已有Headers
		/// </summary>
		/// <param name="headers">新的Headers</param>
		public void AddHeaders(WebHeaderCollection headers)
		{ Headers.Add(headers); }

		/// <summary>
		/// 获取请求返回的响应流
		/// </summary>
		/// <returns>数据流：<seealso cref="Stream"/></returns>
		private Stream GetContentStream()
		{
			for(int i = 1; i <= 6; i ++)
			{
				if (autoUA) { UserAgent = computerUAsign ? randUA.nextComputerUA : randUA.nextCellphoneUA;}
				try 
				{
				 	response = GetResponse();
					break;
				}
				catch (Exception)
				{
					System.Threading.Thread.Sleep(requestInterval);
					if(! (i == 6)) { continue; };
					throw;
				}
			}
			return response.GetResponseStream();
		}
		/// <summary>
		/// 返回 1970年1月1日 到现在的协调世界时(UTC)毫秒数的差值
		/// </summary>
		/// <returns>间隔毫秒数<see cref="long"/></returns>
		public static long GetTime()
		{
			var origin = DateTime.Parse("1970/01/01").ToUniversalTime().AddHours(8.0);
			long diff = DateTime.Now.ToUniversalTime().Ticks - origin.Ticks;
			return diff;
		}
		/// <summary>
		/// 如果response对象为实例则返回response，否则从request对象中加载response响应，并覆盖response对象
		/// </summary>
		/// <returns>存放响应的对象：<see cref="HttpWebResponse"/></returns>
		public HttpWebResponse GetResponse()
		{
			if (response is null)
			{ response = (HttpWebResponse)request.GetResponse(); }
			return response;
		}
		public HttpWebRequest GetRequest()
        { return request; }

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				response.Dispose();
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~WebProtocol()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
