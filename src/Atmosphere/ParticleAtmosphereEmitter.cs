namespace Cthangover.Mods.Effects.Atmosphere
{
    using System;
    using Godot;

    /// <summary>Deterministic cosmetic particles in world coordinates, independent of gameplay random numbers and render frequency.</summary>
    public class ParticleAtmosphereEmitter : IAtmosphereEmitter
    {
        private readonly AtmosphereEmitterInfo info;
        private readonly Color color;
        private double elapsed;
        public ParticleAtmosphereEmitter(AtmosphereEmitterInfo info)
        {
            this.info = info;
            if (!Color.HtmlIsValid(info.Color)) throw new ArgumentException("Invalid particle color: " + info.Color);
            color = new Color(info.Color);
        }
        public void Advance(double delta, AtmosphereContext context) => elapsed += delta;
        public void Draw(Node2D canvas, AtmosphereContext context)
        {
            var time = (float)elapsed;
            var bounds = context.Bounds;
            for (var i = 0; i < info.Count; i++)
            {
                var seed = i + info.Seed * 0.618f;
                var phase = seed * 2.39996f;
                var x = Wrap(seed * 4.29f + time * info.Velocity[0] + MathF.Sin(time + phase) * info.Sway, bounds.Size.X);
                var y = Wrap(seed * 2.61f + time * info.Velocity[1], bounds.Size.Y);
                var z = info.Height[0] + Wrap(seed * 1.73f + time * info.Velocity[2], info.Height[1] - info.Height[0]);
                var point = new Vector3(bounds.Position.X + x, bounds.Position.Y + y, z);
                if (context.SpawnCells != null)
                {
                    if (context.SpawnCells.Count == 0) return;
                    var cell = context.SpawnCells[i % context.SpawnCells.Count];
                    point.X = cell.X + Wrap(x, 1);
                    point.Y = cell.Y + Wrap(y, 1);
                }
                var pixel = context.Project(point);
                var size = info.Size[0] + (info.Size[1] - info.Size[0]) * (0.5f + 0.5f * MathF.Sin(phase));
                var tint = color;
                tint.A *= 1 - Math.Clamp(info.Pulse, 0, 1) * (0.5f + 0.5f * MathF.Sin(time * 2 + phase));
                switch (info.Shape)
                {
                    case "streak":
                        var tail = point - new Vector3(info.Velocity[0], info.Velocity[1], info.Velocity[2]) * info.Trail;
                        canvas.DrawLine(pixel, context.Project(tail), tint, size);
                        break;
                    case "leaf":
                        var along = Vector2.FromAngle(time + phase) * size;
                        var across = along.Orthogonal() * 0.4f;
                        canvas.DrawColoredPolygon(new[] { pixel - along, pixel + across, pixel + along, pixel - across }, tint);
                        break;
                    case "glow":
                        canvas.DrawCircle(pixel, size * 2, new Color(tint, tint.A * 0.15f));
                        canvas.DrawCircle(pixel, size, tint);
                        break;
                    default: canvas.DrawCircle(pixel, size, tint); break;
                }
            }
        }
        private static float Wrap(float value, float size) => value - MathF.Floor(value / size) * size;
        public void Dispose() { }
    }
}
