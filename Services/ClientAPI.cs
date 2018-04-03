// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Content;
using Newtonsoft.Json.Linq;
using System;

namespace CarSto.Services
{
    public static class ClientAPI
    {
        public static Context Context;

        public static async Task<Tuple<bool, string>> PostAsync(string url, object model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var response = await HttpService.HttpPostAsync(url, json, DataPreferences.Instance.Token);
                if (response == null)
                {
                    return new Tuple<bool, string>(true, "Response is NULL");
                }
                var responseModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                if ((bool)responseModel["error"])
                {
                    return new Tuple<bool, string>(true, responseModel["message"].ToString());
                }
                return new Tuple<bool, string>(false, responseModel["model"].ToString());
            }
            catch (JsonSerializationException ex)
            {
                return new Tuple<bool, string>(true, ex.ToString());
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(true, ex.ToString());
            }
        }

        public static async Task<Tuple<bool, string>> PutAsync(string url, object model)
        {
            try
            {
                string json = string.Empty;
                if (model != null)
                {
                    if (IsValidJson<string>(model.ToString()))
                        json = model.ToString();
                    else json = JsonConvert.SerializeObject(model);
                }
                var response = await HttpService.HttpPutAsync(url, json, DataPreferences.Instance.Token);
                if (response == null)
                {
                    return new Tuple<bool, string>(true, "Response is NULL");
                }
                var responseModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                if ((bool)responseModel["error"])
                {                    
                    return new Tuple<bool, string>(true, responseModel["message"].ToString());
                }
                return new Tuple<bool, string>(false, responseModel["model"].ToString());
            }
            catch (JsonSerializationException ex)
            {
                return new Tuple<bool, string>(true, ex.ToString());
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(true, ex.ToString());
            }
        }

        public static async Task<Tuple<bool, string>> GetAsync(string url)
        {
            try
            {
                var response = await HttpService.HttpGetAsync(url, DataPreferences.Instance.Token);
                if (response == null)
                {
                    return new Tuple<bool, string>(true, "Response is NULL");
                }
                var responseModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                if ((bool)responseModel["error"])
                {
                    return new Tuple<bool, string>(true, responseModel["message"].ToString());
                }
                return new Tuple<bool, string>(false, responseModel["model"].ToString());
            }
            catch (JsonSerializationException ex)
            {
                return new Tuple<bool, string>(true, ex.ToString());
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(true, ex.ToString());
            }
        }

        public static async Task<Tuple<bool, string>> DeleteAsync(string url)
        {
            try
            {
                var response = await HttpService.HttpDeleteAsync(url, DataPreferences.Instance.Token);
                if (response == null)
                {
                    return new Tuple<bool, string>(true, "Response is NULL");
                }
                var responseModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                if ((bool)responseModel["error"])
                {
                    return new Tuple<bool, string>(true, responseModel["message"].ToString());
                }
                return new Tuple<bool, string>(false, responseModel["model"].ToString());
            }
            catch (JsonSerializationException ex)
            {
                return new Tuple<bool, string>(true, ex.ToString());
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(true, ex.ToString());
            }
        }

        public static bool IsValidJson<T>(this string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<T>(strInput);
                    return true;
                }
                catch // not valid
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}