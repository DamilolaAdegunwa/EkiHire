using System.Net;

namespace EkiHire.WebAPI.Utils
{
    public static class HttpHelpers
    {
        public static string GetStatusCodeValue(this HttpStatusCode code) {
            return ((int)code).ToString();
        }
    }
}