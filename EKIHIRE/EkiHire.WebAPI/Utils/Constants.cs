using EkiHire.Core.Utils;

namespace EkiHire.WebAPI.Utils
{
    public class WebConstants : CoreConstants
    {
        public const string ConnectionStringName = "Database";

        public const int DefaultPageSize = int.MaxValue;

        public class Sections
        {
            internal const string Smtp = "Smtp";
            internal const string AuthJwtBearer = "Authentication:JwtBearer";
            internal const string Booking = "Booking";
            internal const string App = "App";
            internal const string Paystack = "Payment:PayStack";
        }
    }
    public class ErrorContants : CoreConstants
    {
        public const string InvalidData = "You entered an invalid data!";
    }
}