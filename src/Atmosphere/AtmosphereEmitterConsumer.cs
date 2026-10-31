namespace Cthangover.Mods.Effects.Atmosphere
{
    using Cthangover.Core.Mods.Consumers;
    public class AtmosphereEmitterConsumer : ModTypeConsumer<IAtmosphereEmitterFactory>
    {
        public override string Id => "effects_base.atmosphere_emitters";
        protected override void Add(IAtmosphereEmitterFactory instance, string modId) => AtmosphereEmitterRegistry.Register(instance);
        protected override void Remove(IAtmosphereEmitterFactory instance, string modId) => AtmosphereEmitterRegistry.Unregister(instance);
    }
}
