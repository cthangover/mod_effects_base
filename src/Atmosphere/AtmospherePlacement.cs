namespace Cthangover.Mods.Effects.Atmosphere
{
    using System.Collections.Generic;
    /// <summary>A profile applied to a map or selected cells. Null Cells covers the whole map; an empty list draws nothing.</summary>
    public class AtmospherePlacement
    {
        public string Profile { get; set; }
        public List<int[]> Cells { get; set; }
    }
}
