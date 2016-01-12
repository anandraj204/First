using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jane.Core.Models
{
    public class DispensaryModel : BaseModel
    {
        public DispensaryModel()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string HoursAndInfo { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public bool HasDelivery { get; set; }
        public bool HasPickup { get; set; }
        public bool HasScheduledDelivery { get; set; }
        public bool HasStorefront { get; set; }
        public bool IsCaregiver { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsHidden { get; set; }
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }
        public string Guid { get; set; }
        public JObject Photos { get; set; }
        public List<HoursOfOperation> HoursOfOperation { get; set; }
        [ScriptIgnore]
        public string HoursOfOperationString { get; set; }
        public JObject DeliveryZones { get; set; }
        public string DeliveryZipCodes { get; set; }
        public string ScheduledDeliveryZipCodes { get; set; }
        public string ApprovalZipCodes { get; set; }
        [ScriptIgnore]
        public IEnumerable<string> DeliveryZipCodesCollection { get; set; }
        [ScriptIgnore]
        public IEnumerable<string> ApprovalZipCodesCollection { get; set; }
        public string OnfleetMerchantId { get; set; }

        public string Type { get; set; }
        public AddressModel Address { get; set; }
        public ICollection<DispensaryProductModel> DispensaryProducts { get; set; }
        public ICollection<UserModel> Patients { get; set; } 
    }

    public class HoursOfOperation
    {
        public string Day { get; set; }
        public string Start { get; set; }
        public string Stop { get; set; }
    }

    public class HoursToStringConverter : ITypeConverter<List<HoursOfOperation>, string>
    {
        public string Convert(ResolutionContext context)
        {
            return JsonConvert.SerializeObject(context.SourceValue);
        }
    }
    public class StringToHoursConverter : ITypeConverter<string, List<HoursOfOperation>>
    {
        public List<HoursOfOperation> Convert(ResolutionContext context)
        {
            return JsonConvert.DeserializeObject<List<HoursOfOperation>>(context.SourceValue.ToString());
        }
    }


    public class StringToStringArrayConverter : ITypeConverter<string, string[]>
    {
        public string[] Convert(ResolutionContext context)
        {
            string s = (string) context.SourceValue;

            if (String.IsNullOrEmpty(s))
                return new string[0];

            return s.Split(',');
        }
    }

    public class StringArrayToStringConverter : ITypeConverter<string[], string>
    {
        public string Convert(ResolutionContext context)
        {
            return string.Join(",", context.SourceValue);
        }
    }

    public class ThinDispensaryModel :BaseModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string HoursAndInfo { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public bool HasDelivery { get; set; }
        public bool HasPickup { get; set; }
        public bool HasScheduledDelivery { get; set; }
        public bool HasStorefront { get; set; }
        public bool IsCaregiver { get; set; }
        public bool IsPrivate { get; set; }
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }
        public string Photos { get; set; }
        public List<HoursOfOperation> HoursOfOperation { get; set; }
        public JObject DeliveryZones { get; set; }
        public string DeliveryZipCodes { get; set; }
        public string ScheduledDeliveryZipCodes { get; set; }
        public string ApprovalZipCodes { get; set; }
        public AddressModel Address { get; set; }
    }

}
