namespace Cthangover.Mods.Effects.Atmosphere
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Cthangover.Core.Factories;

    /// <summary>A continuous visual environment assembled by any content mod from reusable emitters.</summary>
    [EntityKind("atmosphere")]
    public class AtmosphereProfile : IIdentifiable
    {
        [JsonPropertyName("Id")] public string ID { get; set; }
        public string Name { get; set; }
        public List<AtmosphereEmitterInfo> Emitters { get; set; } = new();
    }
}
