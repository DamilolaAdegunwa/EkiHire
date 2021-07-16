using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.Entities.Enums
{
    public enum AdsStatus
    {
        ACTIVE = 0,
        INREVIEW = 1,
        DECLINED = 2
    }
    public enum AdClass
    {
        Normal = 0,
        //Pro = 1,
        Premium = 1
    }
    public enum Rating
    {
        BAD = 1,
        POOR = 2,
        FAIR = 3,
        GOOD = 4,
        EXCELLENT = 5
    }
    public enum PropertyType
    {
        text = 0,
        option = 1,
        color = 2,
        money = 3,
        number = 4,
        description = 5
    }
    public enum JobType
    {
        FullTime = 0,
        PartTime = 1,
        Contract = 2
    }
    public enum EmploymentStatus
    {
        Unemployed = 0, Employed = 1, Self_Employed = 2
    }
    public enum TransactionType
    {
        DebitCard = 0
    }
    public enum TransactionStatus
    {
        Pending = 0,
        Success = 1,
        Failed = 2
    }
    public enum NotificationType
    {
        Welcome = 0,
        AdApproval = 1,
        AdDenial = 2,
        Chat = 3
    }
}
