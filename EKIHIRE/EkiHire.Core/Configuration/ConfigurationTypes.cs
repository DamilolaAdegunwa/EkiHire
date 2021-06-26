using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Configuration
{
    public class AppConfig : ISettings
    {
        public string AppEmail { get; set; }
        public double ReviewsWeight { get; set; }
        public double LikesWeight { get; set; }
        public double SearchWeight { get; set; }
        public double DaysSincePostWeight { get; set; }
        public string CLOUDINARY_CLOUD_NAME { get; set; }
        public string CLOUDINARY_API_KEY { get; set; }
        public string CLOUDINARY_API_SECRET { get; set; }
        public string CLOUDINARY_API_ENVIRONMENT_VARIABLE { get; set; }
    }

    //public class BookingConfig : ISettings
    //{
    //    public string CampTripEndDate { get; set; }
    //    public string TerminalKey { get; set; }
    //    public string BookingCountReciever { get; set; }
    //    public string HHEx { get; set; }
    //    public string HHSEx { get; set; }
    //    public string HExHSEx { get; set; }
    //}

    public class PaymentConfig : ISettings
    {
        public class Paystack
        {
            public string Secret { get; set; }
        }
    }
}
