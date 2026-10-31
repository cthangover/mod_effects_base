namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    using System.Linq;

    /// <summary>Bound particle budgets and finite settings before allocating effects, including author supplied custom emitters.</summary>
    public static class AtmosphereValidation
    {
        public static void Validate(AtmosphereProfile profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.ID)) throw new ArgumentException("An atmosphere needs an Id.");
            long count = 0;
            foreach (var emitter in profile.Emitters ?? new System.Collections.Generic.List<AtmosphereEmitterInfo>())
            { Validate(emitter); count += emitter.Count; }
            if (count > 2048) throw new ArgumentException("Atmosphere particle budget exceeds 2048: " + profile.ID);
        }
        public static void Validate(AtmosphereEmitterInfo info)
        {
            if (info == null || string.IsNullOrWhiteSpace(info.Type)) throw new ArgumentException("An emitter needs a Type.");
            if (!float.IsFinite(info.CycleSeconds) || info.CycleSeconds <= 0 || !float.IsFinite(info.RadiusMin) || !float.IsFinite(info.RadiusMax) || info.RadiusMin < 0 || info.RadiusMax < info.RadiusMin || info.RadiusMax > 32 || info.Segments < 3 || info.Segments > 128)
                throw new ArgumentException("Invalid ripple cycle, radius or segment count.");
            if (info.Count < 0 || info.Count > 512) throw new ArgumentException("Emitter Count must be in 0..512.");
            if (info.Velocity?.Length != 3 || info.Velocity.Any(v => !float.IsFinite(v))) throw new ArgumentException("Velocity needs three finite world coordinates.");
            if (info.Height?.Length != 2 || info.Height.Any(v => !float.IsFinite(v)) || info.Height[0] < 0 || info.Height[1] <= info.Height[0]) throw new ArgumentException("Height needs increasing nonnegative bounds.");
            if (info.Size?.Length != 2 || info.Size.Any(v => !float.IsFinite(v)) || info.Size[0] <= 0 || info.Size[1] < info.Size[0] || info.Size[1] > 32) throw new ArgumentException("Size needs increasing positive pixel bounds up to 32.");
            if (!float.IsFinite(info.Sway) || !float.IsFinite(info.Pulse) || !float.IsFinite(info.Trail) || info.Trail < 0) throw new ArgumentException("Sway, Pulse and Trail must be finite; Trail cannot be negative.");
        }
    }
}
