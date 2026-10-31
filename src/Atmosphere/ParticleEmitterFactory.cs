namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    public class ParticleEmitterFactory : IAtmosphereEmitterFactory
    {
        public string Id => "particles";
        public IAtmosphereEmitter Create(AtmosphereEmitterInfo info)
        {
            if (info.Shape is not ("flake" or "leaf" or "glow" or "dust" or "streak"))
                throw new ArgumentException("Unknown particle shape: " + info.Shape);
            return new ParticleAtmosphereEmitter(info);
        }
    }
}
