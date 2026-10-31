namespace Cthangover.Mods.Effects.Atmosphere
{
    /// <summary>A mod adds an emitter type by implementing this interface; the normal mod pipeline discovers it.</summary>
    public interface IAtmosphereEmitterFactory
    {
        string Id { get; }
        IAtmosphereEmitter Create(AtmosphereEmitterInfo info);
    }
}
