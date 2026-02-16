using Microsoft.EntityFrameworkCore;
using WotanStellar.Data.Entities;

namespace WotanStellar.Data;

public class StellarContext : DbContext
{
    public DbSet<StarSystem> StarSystems { get; set; }
    public DbSet<Star> Stars { get; set; }
    public DbSet<Planet> Planets { get; set; }
    public DbSet<Moon> Moons { get; set; }

    public StellarContext(DbContextOptions<StellarContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.Entity<StarSystem>(entity =>
        {
            entity.ToTable("star_systems");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("system_name");
            entity.Property(e => e.SystemType).HasColumnName("system_type");
            entity.Property(e => e.SourceSystemId).HasColumnName("source_system_id");
            entity.Property(e => e.GalacticX).HasColumnName("galactic_x");
            entity.Property(e => e.GalacticY).HasColumnName("galactic_y");
            entity.Property(e => e.GalacticZ).HasColumnName("galactic_z");
            entity.Property(e => e.Position).HasColumnName("position").HasColumnType("geometry(PointZ)");
            entity.Property(e => e.Mass).HasColumnName("star_mass");
            entity.Property(e => e.StarSpectralTypeSize).HasColumnName("star_spectral_type_size");
            entity.Property(e => e.DistanceSolLy).HasColumnName("distance_sol_ly");
            entity.Property(e => e.StarRadius).HasColumnName("star_radius");
            entity.Property(e => e.StarLuminosity).HasColumnName("star_luminosity");
            entity.HasMany<Star>(s => s.Stars)
                .WithOne(s => s.StarSystem)
                .HasForeignKey(s => s.StarSystemId)
                .HasPrincipalKey(ss => ss.Id);
        });

        modelBuilder.Entity<Star>(entity =>
        {
            entity.ToTable("stars");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StarSystemId).HasColumnName("star_system_id");
            entity.Property(e => e.Name).HasColumnName("star_name");
            entity.Property(e => e.StarType).HasColumnName("star_type");
            entity.Property(e => e.SourceSystemId).HasColumnName("source_system_id");
            entity.Property(e => e.GalacticX).HasColumnName("galactic_x");
            entity.Property(e => e.GalacticY).HasColumnName("galactic_y");
            entity.Property(e => e.GalacticZ).HasColumnName("galactic_z");
            entity.Property(e => e.Position).HasColumnName("position").HasColumnType("geometry(PointZ)");
            entity.Property(e => e.Mass).HasColumnName("star_mass");
            entity.Property(e => e.StarSpectralTypeSize).HasColumnName("star_spectral_type_size");
            entity.Property(e => e.DistanceSolLy).HasColumnName("distance_sol_ly");
            entity.Property(e => e.Radius).HasColumnName("star_radius");
            entity.Property(e => e.Luminosity).HasColumnName("star_luminosity");

            entity.HasOne<StarSystem>(s => s.StarSystem)
                .WithMany(ss => ss.Stars)
                .HasForeignKey(s => s.StarSystemId)
                .HasPrincipalKey(ss => ss.Id);
        });

        modelBuilder.Entity<Planet>(entity =>
        {
            entity.ToTable("planets");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StarId).HasColumnName("star_id");
            entity.Property(e => e.Name).HasColumnName("planet_name");
            entity.Property(e => e.PlanetType)
                .HasColumnName("planet_type")
                .HasConversion<string>();
            entity.Property(e => e.OrbitalRadiusAu).HasColumnName("orbital_radius_au");
            entity.Property(e => e.OrbitalPeriodDays).HasColumnName("orbital_period_days");
            entity.Property(e => e.MassEarthMasses).HasColumnName("mass_earth_masses");
            entity.Property(e => e.RadiusEarthRadii).HasColumnName("radius_earth_radii");
            entity.Property(e => e.SurfaceTemperatureK).HasColumnName("surface_temperature_k");
            entity.Property(e => e.AtmosphereType).HasColumnName("atmosphere_type");
            entity.Property(e => e.SurfaceType).HasColumnName("surface_type");
            entity.Property(e => e.Seed).HasColumnName("seed");

            entity.HasOne(p => p.Star)
                .WithMany(s => s.Planets)
                .HasForeignKey(p => p.StarId);
        });

        modelBuilder.Entity<Moon>(entity =>
        {
            entity.ToTable("moons");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlanetId).HasColumnName("planet_id");
            entity.Property(e => e.MoonName).HasColumnName("moon_name");
            entity.Property(e => e.MoonType).HasColumnName("moon_type");
            entity.Property(e => e.OrbitalRadiusKm).HasColumnName("orbital_radius_km");
            entity.Property(e => e.OrbitalPeriodDays).HasColumnName("orbital_period_days");
            entity.Property(e => e.MassEarthMasses).HasColumnName("mass_earth_masses");
            entity.Property(e => e.RadiusEarthRadii).HasColumnName("radius_earth_radii");
            entity.Property(e => e.SurfaceType).HasColumnName("surface_type");
            entity.Property(e => e.Seed).HasColumnName("seed");

            entity.HasOne(m => m.Planet)
                .WithMany(p => p.Moons)
                .HasForeignKey(m => m.PlanetId);
        });

        modelBuilder.Entity<PlanetaryRingSystem>(entity =>
        {
            entity.ToTable("planetary_ring_systems");
            entity.Property(e => e.PlanetId).HasColumnName("planet_id");
            entity.Property(e => e.InnerEdgeRadius).HasColumnName("inner_edge_radius");
            entity.Property(e => e.OuterEdgeRadius).HasColumnName("outer_edge_radius");
            entity.Property(e => e.PrimaryComposition)
                .HasColumnName("primary_composition")
                .HasConversion<string>();
            entity.Property(e => e.TotalMassKg).HasColumnName("total_mass");
            entity.Property(e => e.Seed).HasColumnName("seed");

            entity.HasOne(r => r.Planet)
                .WithOne(p => p.RingSystem)
                .HasForeignKey<PlanetaryRingSystem>(r => r.PlanetId);
        });

        modelBuilder.Entity<PlanetaryRing>(entity =>
        {
            entity.ToTable("planetary_rings");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RingIndex).HasColumnName("ring_index");
            entity.Property(e => e.InnerRadius).HasColumnName("inner_radius");
            entity.Property(e => e.OuterRadius).HasColumnName("outer_radius");
            entity.Property(e => e.Density).HasColumnName("density");
            entity.Property(e => e.Brightness).HasColumnName("brightness");
            entity.Property(e => e.RingType)
                .HasColumnName("ring_type")
                .HasConversion<string>();
            entity.Property(e => e.RedTint).HasColumnName("tint_color_r");
            entity.Property(e => e.GreenTint).HasColumnName("tint_color_g");
            entity.Property(e => e.BlueTint).HasColumnName("tint_color_b");
            entity.Property(e => e.Seed).HasColumnName("seed");

            entity.HasOne(r => r.RingSystem)
                .WithMany(rs => rs.Rings)
                .HasForeignKey(r => r.PlanetId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
