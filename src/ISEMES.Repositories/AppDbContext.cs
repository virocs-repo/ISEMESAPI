using ISEMES.Models;
using Microsoft.EntityFrameworkCore;

namespace ISEMES.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
        }

        public virtual DbSet<ReceiptDetail> ListReceiptDetails { get; set; }
        public virtual DbSet<DeviceDetails> ListDeviceDetails { get; set; }
        public virtual DbSet<DeviceTypes> ListDeviceTypes { get; set; }
        public virtual DbSet<HardwareDetails> ListHardwareDetails { get; set; }
        public virtual DbSet<Address> ListAddress { get; set; }
        public virtual DbSet<Shipment> ListShipment { get; set; }
        public virtual DbSet<ShipmentCategory> ListShipmentCategory { get; set; }
        public virtual DbSet<ShipmentType> ListShipmentType { get; set; }
        public virtual DbSet<ShipmentLineItem> ListShipmentDetails { get; set; }
        public virtual DbSet<InventoryCheckinCheckout> ListInventoryCheckinCheckoutStatuses { get; set; }
        public virtual DbSet<CustOrder> CustomerOrders { get; set; }
        public virtual DbSet<MiscellaneousGoods> ListMiscellaneousGoods { get; set; }
        public virtual DbSet<ReceiptEmployee> ListReceiptEmployee { get; set; }
        public virtual DbSet<HardwareTypeDetails> ListHardwareTypeDetails { get; set; }
        public virtual DbSet<ShipmentInventory> GetShipmentInventory { get; set; }
        public virtual DbSet<Attachment> ListReceiptAttachment { get; set; }
        public virtual DbSet<AnotherShipment> ListOtherInventoryShipment { get; set; }
    }
}



