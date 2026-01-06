using ISEMES.Models;
using Microsoft.EntityFrameworkCore;

namespace ISEMES.Repositories
{
    public class InventoryDBContext : DbContext
    {
        public InventoryDBContext(DbContextOptions<InventoryDBContext> options) : base(options)
        {

        }
        public virtual DbSet<ReceiptDetail> ListReceiptDetails { get; set; }
        public virtual DbSet<DeviceDetails> ListDeviceDetails { get; set; }
        public virtual DbSet<HardwareDetails> ListHardwareDetails { get; set; }
        public virtual DbSet<Address> ListAddress { get; set; }

        public async Task<List<Address>> GetAddressAsync()
        {
            return await ListAddress.FromSqlRaw("EXEC proc_inv_ListAddress").ToListAsync();
        }

    }
}



