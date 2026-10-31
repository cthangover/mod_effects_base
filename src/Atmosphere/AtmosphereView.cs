namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cthangover.Core.Mods.Resolvers;
    using Cthangover.Core.Utils;
    using Godot;

    /// <summary>Continuous effect host usable by tile maps or any scene that supplies a projection and world bounds.</summary>
    public partial class AtmosphereView : Node2D
    {
        private AtmosphereProfile profile;
        private AtmosphereContext context;
        private readonly List<IAtmosphereEmitter> emitters = new();
        private string requestedId;
        public override void _EnterTree() => AtmosphereCatalog.Changed += ReloadProfile;
        private void ReloadProfile()
        {
            if (context == null) return;
            AtmosphereCatalog.Collect().TryGetValue(requestedId ?? "", out var candidate);
            ApplyProfile(candidate, context);
        }
        public string ProfileId => profile?.ID;
        public int EmitterCount => emitters.Count;

        /// <summary>Keep animation state across geometry edits; a profile change releases the previous effects.</summary>
        public void Configure(string id, AtmosphereContext next)
        {
            requestedId = id;
            var profiles = AtmosphereCatalog.Collect();
            profiles.TryGetValue(id ?? "", out var candidate);
            ApplyProfile(candidate, next);
            if (candidate == null && !string.IsNullOrEmpty(id))
                GameLogger.Log("EFFECTS", "Unknown atmosphere profile: " + id, LogLevel.Error);
        }

        /// <summary>Preview an unsaved profile directly. A fresh profile object restarts emitters; geometry-only changes preserve time.</summary>
        private void ApplyProfile(AtmosphereProfile candidate, AtmosphereContext next)
        {
            context = next;
            var id = candidate?.ID;
            if (ReferenceEquals(profile, candidate) && (candidate != null || string.IsNullOrEmpty(id))) return;
            ReleaseEmitters();
            profile = candidate;
            if (candidate == null)
            {
                return;
            }
            try { AtmosphereValidation.Validate(profile); }
            catch (Exception ex)
            {
                GameLogger.Log("EFFECTS", ex.Message, LogLevel.Error);
                return;
            }
            foreach (var info in profile.Emitters ?? new List<AtmosphereEmitterInfo>())
            {
                try { emitters.Add(AtmosphereEmitterRegistry.Create(info)); }
                catch (Exception ex) { GameLogger.Log("EFFECTS", $"Atmosphere '{id}': {ex.Message}", LogLevel.Error); }
            }
        }
        public void Advance(double delta)
        {
            if (context?.Project == null) return;
            foreach (var emitter in emitters) emitter.Advance(Math.Max(0, delta), context);
            QueueRedraw();
        }
        public override void _Draw()
        {
            if (context?.Project == null || context.Bounds.Size.X <= 0 || context.Bounds.Size.Y <= 0) return;
            foreach (var emitter in emitters) emitter.Draw(this, context);
        }
        public void Release() { ReleaseEmitters(); profile = null; context = null; }
        public override void _ExitTree() { AtmosphereCatalog.Changed -= ReloadProfile; Release(); }
        private void ReleaseEmitters()
        {
            foreach (var emitter in emitters) emitter.Dispose();
            emitters.Clear();
        }
    }
}
