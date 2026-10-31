namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    using System.Collections.Generic;
    using Godot;

    /// <summary>The host supplies coordinates, named regions and projection; effects do not depend on maps or a renderer.</summary>
    public class AtmosphereContext
    {
        public Rect2 Bounds { get; set; }
        /// <summary>Optional cells restricting particle spawning, without multiplying the profile particle budget.</summary>
        public IReadOnlyList<Vector2> SpawnCells { get; set; }
        public Func<Vector3, Vector2> Project { get; set; }
        public IReadOnlyDictionary<string, IReadOnlyList<Vector2>> Regions { get; set; }
    }
}
