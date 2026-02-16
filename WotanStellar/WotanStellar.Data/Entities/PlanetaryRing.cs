using System.Drawing;

namespace WotanStellar.Data.Entities;

public class PlanetaryRing
{
   public int Id { get; set; }
   public int PlanetaryRingSystemId { get; set; } // same as planet.Id
   public int RingIndex { get; set; }
   public double InnerRadius { get; set; } // in planetary radii
   public double OuterRadius { get; set; } // in planetary radii
   public double Density { get; set; }
   public double Brightness { get; set; }
   public RingType RingType { get; set; }
   public int Seed { get; set; }

   public Color RingColor
   {
      get => Color.FromArgb(RedTint, GreenTint, BlueTint);
      set
      {
         RedTint = value.R;
         GreenTint = value.G;
         BlueTint = value.B;
      }
   }
   public int RedTint { get; set; }
   public int GreenTint { get; set; }
   public int BlueTint { get; set; }

   public PlanetaryRingSystem RingSystem { get; set; } = null!;
}


public enum RingType
{
   Dense,
   Diffuse,
   Gap,
   Shepherd
}
