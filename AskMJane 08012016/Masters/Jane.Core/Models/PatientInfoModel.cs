using System;

namespace Jane.Core.Models
{
    public class PatientInfoModel :BaseModel
    {

        public string MedicalCardNumber { get; set; }
        public DateTime? MedicalCardExpirationDate { get; set; }
        public bool IsValidMedicalCard { get; set; }
        public DateTime? MedicalCardValidationDate { get; set; }
        public string RecommendationImageUrl { get; set; }
        public string DriversLicenseNumber { get; set; }
        public string DriversLicenseImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public decimal PreferredWeight { get; set; }
        public ApproalStatusModel ApprovalStatus { get; set; }
    }

    public enum ApproalStatusModel
    {
        NOTAPPLIED = 1,
        APPLIED = 2,
        REJECTED = 3,
        ACCEPTED = 4,
        OTHER = 5
    }

}
