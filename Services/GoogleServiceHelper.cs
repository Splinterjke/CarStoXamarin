using Android.Content;
using Android.Gms.Common;
using System;

namespace CarSto.Services
{
    public class GoogleServiceHelper
    {
        private static GoogleServiceHelper instance;
        private GoogleServiceHelper() { }
        public static GoogleServiceHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GoogleServiceHelper();
                }
                return instance;
            }
        }

        public Tuple<bool, string> IsPlayServicesAvailable(Context context)
        {

            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    return Tuple.Create(false, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                    return Tuple.Create(false, "This device is not supported");
            }
            else
                return Tuple.Create(true, string.Empty);
        }
    }
}