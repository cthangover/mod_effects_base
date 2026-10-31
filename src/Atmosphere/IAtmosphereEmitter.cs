namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    using Godot;

    /// <summary>Advance once per host frame, draw without changing state, release owned assets when the host leaves.</summary>
    public interface IAtmosphereEmitter : IDisposable
    {
        void Advance(double delta, AtmosphereContext context);
        void Draw(Node2D canvas, AtmosphereContext context);
    }
}
