using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class MasterData
    {
        public List<Root> Root { get; set; }
    }
    public class Root
    {
        public List<CustomerType> CustomerType { get; set; }
        public List<ReceiptLocation> ReceiptLocation { get; set; }
        public List<GoodsType> GoodsType { get; set; }
        public List<DeliveryMode> DeliveryMode { get; set; }
        public List<CourierDetail> CourierDetails { get; set; }
        public List<Country> Country { get; set; }
        public List<LotCategory> LotCategory { get; set; }
    }

    public class CustomerType
    {
        public int CustomerTypeID { get; set; }
        public string CustomerTypeName { get; set; }
    }

    public class LotCategory
    {
        public int LotCategoryID { get; set; }
        public string LotCategoryName { get; set; }
    }

    public class DeviceTypes
    {
        [Key]
        public int DeviceTypeID { get; set; }
        public string DeviceTypeName { get; set; }
    }
    public class ReceiptLocation
    {
        public int ReceivingFacilityID { get; set; }
        public string ReceivingFacilityName { get; set; }
    }

    public class GoodsType
    {
        public int GoodsTypeID { get; set; }
        public string GoodsTypeName { get; set; }
    }

    public class DeliveryMode
    {
        public int DeliveryModeID { get; set; }
        public string DeliveryModeName { get; set; }
    }

    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class Vendor
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
    }
    public class Employee
    {
        public int EmployeeId { get; set; }
    }

    public class CourierDetail
    {
        public int CourierDetailID { get; set; }
        public string CourierName { get; set; }
    }

    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
    public class EmployeeMaster
    {
        public int employeeID { get; set; }
        public string employeeName { get; set; }
    }
    public class TFSCustomer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string? Email { get; set; } = string.Empty;
        public bool IsCreditHold { get; set; } 
        public string? PaymentTerm { get; set; } = string.Empty;
        public bool IsTBR { get; set; } 
        public bool ShowInphiFamilyID { get; set; } 
        public bool CanAddVerifyStep { get; set; } 
    }
    public class TFSDeviceFamily
    {
        public int DeviceFamilyId { get; set; }
        public string DeviceFamilyName { get; set; } = string.Empty;
        public string? CustomerDeviceFamily { get; set; } = string.Empty;
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
    public class TFSDevice
    {
        public int DeviceId { get; set; }
        public string Device { get; set; } = string.Empty;
        public int? DeviceFamilyId { get; set; }
        public string? DeviceFamily { get; set; } = string.Empty;
        public string? CustomerDevice { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
    public class TFSDeviceAlias
    {
        public int AliasId { get; set; }
        public string AliasName { get; set; } = string.Empty;
        public int? DeviceId { get; set; }
        public string? Device { get; set; } = string.Empty;
        public int? DeviceFamilyId { get; set; }
    }
    public class LotOwner
    {
        public int OwnerId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; } = string.Empty;
    }
}

