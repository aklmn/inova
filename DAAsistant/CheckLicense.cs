using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Web.Script.Serialization;

namespace DAAsistant;

public class CheckLicense
{
	[return: MarshalAs(UnmanagedType.LPWStr)]
	public static string CheckLicenseAPI([In][MarshalAs(UnmanagedType.LPWStr)] string apiUrl, [In][MarshalAs(UnmanagedType.LPWStr)] string accountNumber, [In][MarshalAs(UnmanagedType.LPWStr)] string productType, [In][MarshalAs(UnmanagedType.LPWStr)] string version)
	{
		try
		{
			string requestUriString = $"{apiUrl}";
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			string cPUId = getCPUId();
			string value = "{ \"AccountNumber\": \"" + accountNumber + "\", \"ProductType\":\"" + productType + "\",\"CPUId\":\"" + cPUId + "\",\"Version\":\"" + version + "\"}";
			
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				streamWriter.Write(value);
			}
			Log($"[CheckLicenseAPI] [POST REQUEST] Url: {apiUrl} | Data: {value}");
			
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string input = streamReader.ReadToEnd();
			
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			dynamic val = javaScriptSerializer.DeserializeObject(input);
			dynamic val2 = val["result"];
			
			string logResponse = $"[CheckLicenseAPI] [POST RESPONSE] Url: {apiUrl} | Status: {Convert.ToString(val2["status"])} | Message: {Convert.ToString(val2["message"])}";
			Log(logResponse);
			
			return string.Format("{0};{1};{2}", "success", Convert.ToString(val2["status"]), Convert.ToString(val2["message"]));
		}
		catch (Exception ex)
		{
			Log($"[CheckLicenseAPI] [POST ERROR] Url: {apiUrl} | Error: {ex.GetBaseException().Message}");
			return string.Format("{0};{1};{2}", "error", "false", ex.GetBaseException().Message);
		}
	}

	private static string getCPUId()
	{
		return string.Empty;
	}

	private static readonly string LogDirectory = @"C:\AppLogs\";

	static CheckLicense()
	{
		try
		{
			if (!Directory.Exists(LogDirectory))
			{
				Directory.CreateDirectory(LogDirectory);
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine("Gagal membuat folder log: " + ex.Message);
		}
	}

	private static void Log(string message)
	{
		lock (typeof(CheckLicense))
		{
			try
			{
				string fileName = $"Log_{DateTime.Now:yyyyMMdd}.txt";
				string fullPath = Path.Combine(LogDirectory, fileName);
				string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";

				using (var writer = new StreamWriter(fullPath, append: true, encoding: System.Text.Encoding.UTF8))
				{
					writer.WriteLine(logLine);
				}
			}
			catch
			{
				// Mengabaikan error agar tidak crash
			}
		}
	}
}

public class NewTrade
{
	[return: MarshalAs(UnmanagedType.LPWStr)]
	public static string CreateNewTrade([In][MarshalAs(UnmanagedType.LPWStr)] string apiUrl, [In][MarshalAs(UnmanagedType.LPWStr)] string accountNumber, [In][MarshalAs(UnmanagedType.LPWStr)] string accountName, [In][MarshalAs(UnmanagedType.LPWStr)] string accountCompany, [In][MarshalAs(UnmanagedType.LPWStr)] string accountBalance, [In][MarshalAs(UnmanagedType.LPWStr)] string symbol, [In][MarshalAs(UnmanagedType.LPWStr)] string entryPrice, [In][MarshalAs(UnmanagedType.LPWStr )] string StoplossPrice, [In][MarshalAs(UnmanagedType.LPWStr)] string stoplossPrice, [In][MarshalAs(UnmanagedType.LPWStr)] string targetProfitPrice, [In][MarshalAs(UnmanagedType.LPWStr)] string positionSize, [In][MarshalAs(UnmanagedType.LPWStr)] string orderId, [In][MarshalAs(UnmanagedType.I4)] int timeframe, [In][MarshalAs(UnmanagedType.Bool)] bool isDemo, [In][MarshalAs(UnmanagedType.LPWStr)] string accountCurrency, [In][MarshalAs(UnmanagedType.I4)] int accountLeverage, [In][MarshalAs(UnmanagedType.LPWStr)] string lotSize, [In][MarshalAs(UnmanagedType.I4)] int tradeType)
	{
		try
		{
			string requestUriString = $"{apiUrl}";
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			
			string value = "{ \"AccountNumber\": \"" + accountNumber + "\",\"AccountName\":\"" + accountName + "\",\"AccountCompany\":\"" + accountCompany + "\",\"AccountBalance\":\"" + accountBalance + "\",\"Symbol\":\"" + symbol + "\",\"EntryPrice\":\"" + entryPrice + "\",\"StoplossPrice\":\"" + stoplossPrice + "\",\"TargetProfitPrice\":\"" + targetProfitPrice + "\",\"PositionSize\":\"" + positionSize + "\",\"Timeframe\":\"" + timeframe + "\",\"IsDemo\":\"" + isDemo + "\",\"AccountCurrency\":\"" + accountCurrency + "\",\"AccountLeverage\":\"" + accountLeverage + "\",\"LotSize\":\"" + lotSize + "\",\"TradeType\":\"" + tradeType + "\",\"OrderId\":\"" + orderId + "\"}";
			
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				streamWriter.Write(value);
			}
			Log($"[CreateNewTrade] [POST REQUEST] Url: {apiUrl} | Data: {value}");

			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string text = streamReader.ReadToEnd();
			
			Log($"[CreateNewTrade] [POST RESPONSE] Url: {apiUrl} | Response: {text}");
			return "success";
		}
		catch (Exception ex)
		{
			Log($"[CreateNewTrade] [POST ERROR] Url: {apiUrl} | Error: {ex.GetBaseException().Message}");
			return "error";
		}
	}

	[return: MarshalAs(UnmanagedType.LPWStr)]
	public static string UpdateTrade([In][MarshalAs(UnmanagedType.LPWStr)] string apiUrl, [In][MarshalAs(UnmanagedType.LPWStr)] string accountNumber, [In][MarshalAs(UnmanagedType.LPWStr)] string orderId, [In][MarshalAs(UnmanagedType.LPWStr)] string profitAndLoss)
	{
		try
		{
			string requestUriString = $"{apiUrl}";
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "PUT";
			
			string value = "{ \"AccountNumber\": \"" + accountNumber + "\",\"OrderId\":\"" + orderId + "\",\"ProfitAndLoss\":\"" + profitAndLoss + "\"}";
			
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				streamWriter.Write(value);
			}
			Log($"[UpdateTrade] [PUT REQUEST] Url: {apiUrl} | Data: {value}");

			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string text = streamReader.ReadToEnd();
			
			Log($"[UpdateTrade] [PUT RESPONSE] Url: {apiUrl} | Response: {text}");
			return "success";
		}
		catch (Exception ex)
		{
			Log($"[UpdateTrade] [PUT ERROR] Url: {apiUrl} | Error: {ex.GetBaseException().Message}");
			return "error";
		}
	}

	[return: MarshalAs(UnmanagedType.LPWStr)]
	public static string GetActiveOrders([In][MarshalAs(UnmanagedType.LPWStr)] string apiUrl, [In][MarshalAs(UnmanagedType.LPWStr)] string accountNumber)
	{
		string requestUriString = $"{apiUrl}?accountNumber={accountNumber}";
		Log($"[GetActiveOrders] [GET REQUEST] Url: {requestUriString}");
		
		ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
		string result = "";
		
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			
			using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			if (httpWebResponse.StatusCode == HttpStatusCode.NotFound || httpWebResponse.StatusCode == HttpStatusCode.InternalServerError)
			{
				throw new Exception($"Server returned status code: {httpWebResponse.StatusCode}");
			}
			
			using Stream stream = httpWebResponse.GetResponseStream();
			using StreamReader streamReader = new StreamReader(stream);
			result = streamReader.ReadToEnd();
			
			Log($"[GetActiveOrders] [GET RESPONSE] Url: {apiUrl} | Data Length: {result.Length} chars");
		}
		catch (Exception ex)
		{
			Log($"[GetActiveOrders] [GET ERROR] Url: {requestUriString} | Error: {ex.GetBaseException().Message}");
			return "error";
		}
		return result;
	}

	private static readonly string LogDirectory = @"C:\AppLogs\";

	static NewTrade()
	{
		try
		{
			if (!Directory.Exists(LogDirectory))
			{
				Directory.CreateDirectory(LogDirectory);
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine("Gagal membuat folder log: " + ex.Message);
		}
	}

	private static void Log(string message)
	{
		lock (typeof(NewTrade))
		{
			try
			{
				string fileName = $"Log_{DateTime.Now:yyyyMMdd}.txt";
				string fullPath = Path.Combine(LogDirectory, fileName);
				string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";

				using (var writer = new StreamWriter(fullPath, append: true, encoding: System.Text.Encoding.UTF8))
				{
					writer.WriteLine(logLine);
				}
			}
			catch
			{
				// Mengabaikan error log agar tidak membuat aplikasi client crash
			}
		}
	}
}
