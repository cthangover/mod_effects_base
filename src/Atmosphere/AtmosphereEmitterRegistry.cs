namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    using System.Collections.Generic;

    /// <summary>Factories are removed on mod unload; duplicate type names are diagnosed rather than chosen by accident.</summary>
    public static class AtmosphereEmitterRegistry
    {
        private static readonly Dictionary<string, IAtmosphereEmitterFactory> factories = new(StringComparer.Ordinal);
        public static IEnumerable<string> Ids => factories.Keys;
        public static void Register(IAtmosphereEmitterFactory factory)
        {
            if (!factories.TryAdd(factory.Id, factory))
                throw new InvalidOperationException("Duplicate atmosphere emitter type: " + factory.Id);
        }
        public static void Unregister(IAtmosphereEmitterFactory factory)
        {
            if (factories.TryGetValue(factory.Id, out var current) && ReferenceEquals(current, factory)) factories.Remove(factory.Id);
        }
        public static IAtmosphereEmitter Create(AtmosphereEmitterInfo info)
        {
            AtmosphereValidation.Validate(info);
            if (!factories.TryGetValue(info.Type, out var factory))
                throw new ArgumentException("Unknown atmosphere emitter type: " + info.Type);
            return factory.Create(info);
        }
    }
}
