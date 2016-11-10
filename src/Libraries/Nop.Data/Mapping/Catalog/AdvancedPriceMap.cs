using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class AdvancedPriceMap : NopEntityTypeConfiguration<AdvancedPrice>
    {
        public AdvancedPriceMap()
        {
            this.ToTable("AdvancedPrice");
            this.HasKey(tp => tp.Id);
            this.Property(tp => tp.Price).HasPrecision(18, 4);

            this.HasRequired(tp => tp.Product)
                .WithMany(p => p.AdvancedPrices)
                .HasForeignKey(tp => tp.ProductId);

            this.HasOptional(tp => tp.CustomerRole)
                .WithMany()
                .HasForeignKey(tp => tp.CustomerRoleId)
                .WillCascadeOnDelete(true);
        }
    }
}