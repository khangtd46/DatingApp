using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatingApp.Data
{
	public class AppDBContext : DbContext
	{
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            builder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter>()
                .HaveColumnType("date");
        }
        public DbSet<SystemUser> Users { get; set; }
    }
}
public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    /// <summary>
    /// Creates a new instance of this converter.
    /// </summary>
    public DateOnlyConverter() : base(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
    { }
}

/// <summary>
/// Converts <see cref="DateOnly?" /> to <see cref="DateTime?"/> and vice versa.
/// </summary>
public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
{
    /// <summary>
    /// Creates a new instance of this converter.
    /// </summary>
    public NullableDateOnlyConverter() : base(
        d => d == null
            ? null
            : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
        d => d == null
            ? null
            : new DateOnly?(DateOnly.FromDateTime(d.Value)))
    { }
}