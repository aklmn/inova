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
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string value = "{ \"AccountNumber\": \"" + accountNumber + "\", \"ProductType\":\"" + productType + "\",\"CPUId\":\"" + cPUId + "\",\"Version\":\"" + version + "\"}";
				streamWriter.Write(value);
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string input = streamReader.ReadToEnd();
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			dynamic val = javaScriptSerializer.DeserializeObject(input);
			dynamic val2 = val["result"];
			return string.Format("{0};{1};{2}", "success", Convert.ToString(val2["status"]), Convert.ToString(val2["message"]));
		}
		catch (Exception ex)
		{
			return string.Format("{0};{1};{2}", "error", "false", ex.GetBaseException().Message);
		}
	}

	private static string getCPUId()
	{
		return string.Empty;
	}
}
