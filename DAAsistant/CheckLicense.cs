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
			Log($"[POST] [REQUEST] {apiUrl} | Data: {value}");
			
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string input = streamReader.ReadToEnd();
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			dynamic val = javaScriptSerializer.DeserializeObject(input);
			dynamic val2 = val["result"];
			string logResponse = $"[POST] [RESPONSE] {apiUrl} | Status: {Convert.ToString(val2["status"])} | Message: {Convert.ToString(val2["message"])}";
			Log(logResponse);
			return string.Format("{0};{1};{2}", "success", Convert.ToString(val2["status"]), Convert.ToString(val2["message"]));
		}
		catch (Exception ex)
		{
			Log($"[POST] [ERROR] {apiUrl} Error: {ex.GetBaseException().Message}");
			return string.Format("{0};{1};{2}", "error", "false", ex.GetBaseException().Message);
		}
	}

	private static string getCPUId()
	{
		return string.Empty;
	}

	private static readonly string LogDirectory = @"C:\AppLogs\";

	/// <summary>
	/// Konstruktor statis untuk memastikan folder log otomatis dibuat saat library pertama kali diakses.
	/// </summary>
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
			// Gagal membuat direktori log (masalah hak akses, dll)
			System.Diagnostics.Debug.WriteLine("Gagal membuat folder log: " + ex.Message);
		}
	}


	/// <summary>
	/// Fungsi utilitas internal dan thread-safe untuk menulis log ke dalam file harian.
	/// </summary>
	private static void Log(string message)
	{
		lock (typeof(CheckLicense)) // Mencegah tabrakan tulisan jika dipanggil bersamaan (Thread-Safe)
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

public class NewTrade
{
	[return: MarshalAs(UnmanagedType.LPWStr)]
	public static string CreateNewTrade([In][MarshalAs(UnmanagedType.LPWStr)] string apiUrl, [In][MarshalAs(UnmanagedType.LPWStr)] string accountNumber, [In][MarshalAs(UnmanagedType.LPWStr)] string accountName, [In][MarshalAs(UnmanagedType.LPWStr)] string accountCompany, [In][MarshalAs(UnmanagedType.LPWStr)] string accountBalance, [In][MarshalAs(UnmanagedType.LPWStr)] string symbol, [In][MarshalAs(UnmanagedType.LPWStr)] string entryPrice, [In][MarshalAs(UnmanagedType.LPWStr)] string stoplossPrice, [In][MarshalAs(UnmanagedType.LPWStr)] string targetProfitPrice, [In][MarshalAs(UnmanagedType.LPWStr)] string positionSize, [In][MarshalAs(UnmanagedType.LPWStr)] string orderId, [In][MarshalAs(UnmanagedType.I4)] int timeframe, [In][MarshalAs(UnmanagedType.Bool)] bool isDemo, [In][MarshalAs(UnmanagedType.LPWStr)] string accountCurrency, [In][MarshalAs(UnmanagedType.I4)] int accountLeverage, [In][MarshalAs(UnmanagedType.LPWStr)] string lotSize, [In][MarshalAs(UnmanagedType.I4)] int tradeType)
	{
		try
		{
			string requestUriString = $"{apiUrl}";
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string value = "{ \"AccountNumber\": \"" + accountNumber + "\",\"AccountName\":\"" + accountName + "\",\"AccountCompany\":\"" + accountCompany + "\",\"AccountBalance\":\"" + accountBalance + "\",\"Symbol\":\"" + symbol + "\",\"EntryPrice\":\"" + entryPrice + "\",\"StoplossPrice\":\"" + stoplossPrice + "\",\"TargetProfitPrice\":\"" + targetProfitPrice + "\",\"PositionSize\":\"" + positionSize + "\",\"Timeframe\":\"" + timeframe + "\",\"IsDemo\":\"" + isDemo + "\",\"AccountCurrency\":\"" + accountCurrency + "\",\"AccountLeverage\":\"" + accountLeverage + "\",\"LotSize\":\"" + lotSize + "\",\"TradeType\":\"" + tradeType + "\",\"OrderId\":\"" + orderId + "\"}";
				streamWriter.Write(value);
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string text = streamReader.ReadToEnd();
			return "success";
		}
		catch
		{
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
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string value = "{ \"AccountNumber\": \"" + accountNumber + "\",\"OrderId\":\"" + orderId + "\",\"ProfitAndLoss\":\"" + profitAndLoss + "\"}";
				streamWriter.Write(value);
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string text = streamReader.ReadToEnd();
			return "success";
		}
		catch
		{
			return "error";
		}
	}

	[return: MarshalAs(UnmanagedType.LPWStr)]
	public static string GetActiveOrders([In][MarshalAs(UnmanagedType.LPWStr)] string apiUrl, [In][MarshalAs(UnmanagedType.LPWStr)] string accountNumber)
	{
		ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
		string result = "";
		try
		{
			string requestUriString = $"{apiUrl}?accountNumber={accountNumber}";
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			if (httpWebResponse.StatusCode == HttpStatusCode.NotFound || httpWebResponse.StatusCode == HttpStatusCode.InternalServerError)
			{
				throw new Exception("error");
			}
			using Stream stream = httpWebResponse.GetResponseStream();
			using StreamReader streamReader = new StreamReader(stream);
			result = streamReader.ReadToEnd();
		}
		catch
		{
			return "error";
		}
		return result;
	}

		// Tentukan path absolut folder log di sini agar tidak mengikuti lokasi client/DLL
        private static readonly string LogDirectory = @"C:\AppLogs\";

        /// <summary>
        /// Konstruktor statis untuk memastikan folder log otomatis dibuat saat library pertama kali diakses.
        /// </summary>
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
                // Gagal membuat direktori log (masalah hak akses, dll)
                System.Diagnostics.Debug.WriteLine("Gagal membuat folder log: " + ex.Message);
            }
        }


        /// <summary>
        /// Fungsi utilitas internal dan thread-safe untuk menulis log ke dalam file harian.
        /// </summary>
        private static void Log(string message)
        {
            lock (typeof(NewTrade)) // Mencegah tabrakan tulisan jika dipanggil bersamaan (Thread-Safe)
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