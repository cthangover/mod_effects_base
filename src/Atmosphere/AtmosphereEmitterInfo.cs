namespace Cthangover.Mods.Effects.Atmosphere
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>Authorable particle settings; custom emitter factories retain their own additional JSON fields.</summary>
    public class AtmosphereEmitterInfo
    {
        public string Type { get; set; } = "particles";
        public string Shape { get; set; } = "flake";
        public int Count { get; set; } = 64;
        public int Seed { get; set; } = 1;
        public string Color { get; set; } = "#ffffffb0";
        public float[] Velocity { get; set; } = new[] { 0.03f, 0.02f, -0.4f };
        public float[] Height { get; set; } = new[] { 0.1f, 5f };
        public float[] Size { get; set; } = new[] { 0.7f, 1.5f };
        public float Sway { get; set; } = 0.2f;
        public float Pulse { get; set; }
        public float Trail { get; set; } = 0.2f;
        public float CycleSeconds { get; set; } = 3.333333f;
        public float RadiusMin { get; set; } = 0.08f;
        public float RadiusMax { get; set; } = 0.38f;
        public int Segments { get; set; } = 12;
        public string Region { get; set; } = "water";
        [JsonExtensionData] public Dictionary<string, JsonElement> ExtraFields { get; set; }
    }
}
