namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    using System.Collections.Generic;
    using Cthangover.Core.Mods.Resolvers;
    /// <summary>Unsaved authoring previews overlay resolved profiles. Removing an override restores the game's data.</summary>
    public static class AtmosphereCatalog
    {
        private static readonly Dictionary<string, AtmosphereProfile> previews = new(StringComparer.Ordinal);
        public static event Action Changed;
        public static Dictionary<string, AtmosphereProfile> Collect()
        {
            var result = new Dictionary<string, AtmosphereProfile>(Entities.Collect<AtmosphereProfile>(), StringComparer.Ordinal);
            foreach (var pair in previews) result[pair.Key] = pair.Value;
            return result;
        }
        public static void Preview(AtmosphereProfile profile) { AtmosphereValidation.Validate(profile); previews[profile.ID] = profile; Changed?.Invoke(); }
        public static void RemovePreview(string id) { if (id != null && previews.Remove(id)) Changed?.Invoke(); }
        public static void Refresh() => Changed?.Invoke();
    }
}
