using System;

namespace EkiHire.Core.Exceptions
{
    [Serializable]
    public class EkiHireGenericException : Exception
    {
        public string ErrorCode { get; set; }

        public EkiHireGenericException(string message) : base(message)
        { }

        public EkiHireGenericException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}