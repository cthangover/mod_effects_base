namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    using Godot;

    /// <summary>Expanding projected rings anchored to a named host region, such as water cells.</summary>
    public class RippleAtmosphereEmitter : IAtmosphereEmitter
    {
        private readonly AtmosphereEmitterInfo info;
        private readonly Color color;
        private double elapsed;
        public RippleAtmosphereEmitter(AtmosphereEmitterInfo info)
        {
            this.info = info;
            if (!Color.HtmlIsValid(info.Color)) throw new ArgumentException("Invalid ripple color: " + info.Color);
            color = new Color(info.Color);
        }
        public void Advance(double delta, AtmosphereContext context) => elapsed += delta;
        public void Draw(Node2D canvas, AtmosphereContext context)
        {
            if (context.Regions == null || !context.Regions.TryGetValue(info.Region ?? "", out var points)) return;
            var count = Math.Min(points.Count, info.Count);
            for (var i = 0; i < count; i++)
            {
                var cycle = (float)(elapsed / info.CycleSeconds + i * 0.37 + (info.Seed - 1) * 0.618);
                var phase = cycle - MathF.Floor(cycle);
                var radius = info.RadiusMin + phase * (info.RadiusMax - info.RadiusMin);
                var index = ((long)i + (long)MathF.Floor(cycle) * count) % points.Count;
                if (index < 0) index += points.Count;
                var center = points[(int)index] + Vector2.One * 0.5f;
                var tint = color; tint.A *= 1 - phase;
                for (var edge = 0; edge < info.Segments; edge++)
                {
                    var a = center + Vector2.FromAngle(edge * MathF.Tau / info.Segments) * radius;
                    var b = center + Vector2.FromAngle((edge + 1) * MathF.Tau / info.Segments) * radius;
                    canvas.DrawLine(context.Project(new Vector3(a.X,a.Y,0)), context.Project(new Vector3(b.X,b.Y,0)), tint);
                }
            }
        }
        public void Dispose() { }
    }
}
