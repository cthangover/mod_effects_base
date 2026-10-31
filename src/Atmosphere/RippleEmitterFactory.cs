namespace Cthangover.Mods.Effects.Atmosphere
{
    public class RippleEmitterFactory : IAtmosphereEmitterFactory
    {
        public string Id => "ripples";
        public IAtmosphereEmitter Create(AtmosphereEmitterInfo info) => new RippleAtmosphereEmitter(info);
    }
}
