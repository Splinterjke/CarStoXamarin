// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ModernHttpClient;

namespace CarSto.Services
{
	public static class HttpService
	{
		// CardPort or carstoDev
		public static string URL = "http://cp.carsto.ru/";
        
		// Http клиент с нативным обработчиком (OkHttp: Android)
		public static HttpClient Client;

		#region Http Methods
		public static async Task<string> HttpPostAsync(string _url, string _bodyData, string _token)
		{
			Client = new HttpClient(new NativeMessageHandler()) { Timeout = TimeSpan.FromSeconds(25) };
			if (!string.IsNullOrEmpty(_token))
				Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
			StringContent content = null;
			if (!string.IsNullOrEmpty(_bodyData))
				content = new StringContent(_bodyData, Encoding.UTF8, "text/json");
			try
			{
				var response = await Client.PostAsync($"{URL}{_url}", content).ConfigureAwait(false);
				if (response == null || !response.IsSuccessStatusCode)
					return null;
				Client.Dispose();
				return response.Content.ReadAsStringAsync().Result;
			}
			catch
			{
                Client.Dispose();
                return null;
			}
		}

		public static async Task<string> HttpGetAsync(string _url, string _token)
		{
			Client = new HttpClient(new NativeMessageHandler()) { Timeout = TimeSpan.FromSeconds(25) };
			if (!string.IsNullOrEmpty(_token))
				Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            try
			{
				var response = await Client.GetAsync($"{URL}{_url}").ConfigureAwait(false);
				if (response == null || !response.IsSuccessStatusCode)
					return null;
				Client.Dispose();
                return response.Content.ReadAsStringAsync().Result;

            }
			catch (Exception ex)
			{
                Client.Dispose();
                return null;
			}
		}

        public static async Task<string[]> HttpGetListAsync(string[] _urls, string _token)
        {
            Client = new HttpClient(new NativeMessageHandler()) { Timeout = TimeSpan.FromSeconds(25) };
            if (!string.IsNullOrEmpty(_token))
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            try
            {
                var result = new string[_urls.Length];
                for (int i = 0; i < _urls.Length; i++)
                {
                    var response = await Client.GetAsync($"{URL}{_urls[i]}").ConfigureAwait(false);
                    if (response == null || !response.IsSuccessStatusCode)
                        continue;
                    result[i] = response.Content.ReadAsStringAsync().Result;
                }
                Client.Dispose();
                return result;
            }
            catch
            {
                Client.Dispose();
                return null;
            }
        }

        public static async Task<string> HttpPutAsync(string _url, string _bodyData, string _token)
		{
			Client = new HttpClient(new NativeMessageHandler()) { Timeout = TimeSpan.FromSeconds(25) };
			if (!string.IsNullOrEmpty(_token))
				Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
			System.Net.Http.StringContent content = null;
			if (!string.IsNullOrEmpty(_bodyData))
				content = new System.Net.Http.StringContent(_bodyData, System.Text.Encoding.UTF8, "application/json");
			try
			{
				var response = await Client.PutAsync($"{URL}{_url}", content).ConfigureAwait(false);
				if (response == null || !response.IsSuccessStatusCode)
					return null;
				Client.Dispose();
				return response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception ex)
			{
                Client.Dispose();
                return null;
			}
		}

		public static async Task<string> HttpDeleteAsync(string _url, string _token)
		{
			Client = new HttpClient(new NativeMessageHandler()) { Timeout = TimeSpan.FromSeconds(25) };
			if (!string.IsNullOrEmpty(_token))
				Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
			try
			{
				var response = await Client.DeleteAsync($"{URL}{_url}").ConfigureAwait(false);
				if (response == null || !response.IsSuccessStatusCode)
					return null;
				Client.Dispose();
				return response.Content.ReadAsStringAsync().Result;
			}
			catch
			{
                Client.Dispose();
                return null;
			}
		}
		#endregion
	}
}