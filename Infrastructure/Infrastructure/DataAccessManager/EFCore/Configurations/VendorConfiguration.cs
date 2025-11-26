using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class VendorConfiguration : BaseEntityConfiguration<Vendor>
{
    public override void Configure(EntityTypeBuilder<Vendor> builder)
    {
        base.Configure(builder);

        // Basic
        builder.Property(x => x.Name).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Number).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.TRN).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);

        // Address hierarchy (ids)
        builder.Property(x => x.CountryId).HasMaxLength(IdConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.GovernorateId).HasMaxLength(IdConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.CityId).HasMaxLength(IdConsts.MaxLength).IsRequired(false);

        // Address details
        builder.Property(x => x.BuildingNumber).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Floor).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.FlatNumber).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Street).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.PostalCode).HasMaxLength(NameConsts.MaxLength).IsRequired(false);

        // Communication
        builder.Property(x => x.Mobile).HasMaxLength(NameConsts.MaxLength).IsRequired(false);

        // Grouping
        builder.Property(x => x.VendorGroupId).HasMaxLength(IdConsts.MaxLength).IsRequired(false);

        // Indexes
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Number);
    }
}

