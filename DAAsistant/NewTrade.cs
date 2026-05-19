using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace DAAsistant;

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
}
