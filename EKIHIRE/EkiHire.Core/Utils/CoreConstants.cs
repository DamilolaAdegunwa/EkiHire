using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EkiHire.Core.Utils
{
    public abstract class CoreConstants
    {
        public const string DefaultAccount = "administrator@Travel.com";
        public const string WebBookingAccount = "web@EkiHire.com";
        public const string IosBookingAccount = "ios@EkiHire.com";
        public const string AndroidBookingAccount = "android@EkiHire.com";
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";

        public class Roles
        {
            public const string Admin = "Administrator";
            //Todo: remove once role creation is available
            public const string T = "Ticketer";
            public const string OM = "Operations Manager";
            public const string BM = "Booking Manager";
            public const string A = "Auditor";
            public const string CC = "Customer Care";
            public const string AC = "Accountant";
            public const string TM = "Terminal Manager";
        }

        public class Url
        {
            public const string PasswordResetEmail = "messaging/emailtemplates/password-resetcode-email.html";
            public const string AccountActivationEmail = "messaging/emailtemplates/account-email.html";
            public const string BookingSuccessEmail = "messaging/emailtemplates/confirm-email.html";
            public const string BookingAndReturnSuccessEmail = "messaging/emailtemplates/confirm-return-email.html";
            public const string ActivationCodeEmail = "messaging/emailtemplates/activation-code-email.html";
            public const string BookingUnSuccessEmail = "messaging/emailtemplates/failed-email.html";
            public const string RescheduleSuccessEmail = "messaging/emailtemplates/reschedule-success.html";
            public const string AdminHireBookingEmail = "messaging/emailtemplates/hirebooking-admin.html";
            public const string CustomerHireBookingEmail = "messaging/emailtemplates/hirebooking.html";
        }

        public class AdOptions
        {
            public class HousesAndApartmentsForSale
            {
                public string[] Bedrooms { get; set; } = new string[8] { "1", "2", "3", "4", "5", "6", "7", "Others" };
                public string[] Bathrooms { get; set; } = new string[5] { "1", "2", "3", "4", "Others" };
                public string[] Type { get; set; } = new string[4] { "Apartments/Flats", "Commercial Proper", "Garage/Parking", "Others" };
                public string[] Furniture { get; set; } = new string[3] { "Yes", "No", "Partly" };
                public string[] ParkingSpace { get; set; } = new string[4] { "1", "2", "3", "4" };
                public string[] NewProperty { get; set; } = new string[3] { "Yes", "No", "Renovated" };
                public string[] PropertyDocuments { get; set; } = new string[4] { "C of O", "Governor Consent", "Survey", "Others" };
                public string[] LandScape { get; set; } = new string[4] { "Ocean View", "Dry Land", "Low Density", "Others" };
                public string[] YourRole { get; set; } = new string[4] { "Landlord", "Agent", "Lawyer", "Surveyor" };
            }
            public class LandForSale
            {
                public string[] Type { get; set; } = new string[4] { "Commercial Land", "Mixed Used", "Residential Land", "Others" };
                public string[] PropertyDocuments { get; set; } = new string[4] { "C of O", "Governor Consent", "Survey", "Others" };
                public string[] LandScape { get; set; } = new string[4] { "Ocean View", "Dry Land", "Low Density", "Others" };
                public string[] YourRole { get; set; } = new string[4] { "Landlord", "Agent", "Lawyer", "Surveyor" };
            }
            public class ShortLet
            {
                public string[] Type { get; set; } = new string[7] { "Flats", "Duplex", "House", "Mini Flats", "Apartments", "Bungalow", "Others..." };
                public string[] PropertyDocuments { get; set; } = new string[4] { "C of O", "Governor Consent", "Survey", "Others" };
                public string[] LandScape { get; set; } = new string[4] { "Ocean View", "Dry Land", "Low Density", "Others" };
                public string[] YourRole { get; set; } = new string[4] { "Landlord", "Agent", "Lawyer", "Surveyor" };
            }
            public class Job
            {
                public string[] Type { get; set; } = new string[4] { "Full-Time", "Part-Time", "Contract", "Internship" };
                public string[] Experience { get; set; } = new string[4] { "1", "2", "3", "4" };
                public string[] Education { get; set; } = new string[10] { "All", "Phd", "M.sc", "MBA", "B.sc", "HND", "OND", "NCE", "Diploma", "SSCE" };
                public string[] YourRole { get; set; } = new string[9] { "CEO", "COO/CTO", "HR", "Manger", "Digital Marketer", "Company Lawyer", "Secretary", "Staff", "Others..." };

            }
            public class Automobile
            {
                public Automobile()
                {
                    YearOfManufacture.Reverse();
                }
                public int[] YearOfManufacture { get; set; } = Enumerable.Range(1990, DateTime.Now.Year - 1990 + 1).ToArray();
                public string[] Color { get; set; } = new string[] { "Black", "Blue", "Wine", "Gold", "Others..." };
                public string[] Conditions { get; set; } = new string[] { "Brand New", "Foreign Used", "Nigerian Used" };
                public string[] Transmission { get; set; } = new string[] { "Automatic", "Manual" };
                public string[] Registered { get; set; } = new string[] { "Yes", "No" };
                public string[] Fuel { get; set; } = new string[] { "Petrol", "Diesel" };
                public string[] CylinderNumber { get; set; } = new string[] {"1","2","3","4","5","6"};
                public string[] YourRole { get; set; } = new string[] { "Owner", "Dealer", "Lawyer", "Agent" };
            }
        }
    }
}