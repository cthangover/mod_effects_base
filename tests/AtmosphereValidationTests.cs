namespace Cthangover.Mods.Effects.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Cthangover.Core.Mods.Testing;
    using Cthangover.Core.Utils;
    using Cthangover.Mods.Effects.Atmosphere;

    /// <summary>Author data survives save/load, and unreasonable particle budgets fail before effects are allocated.</summary>
    public class AtmosphereValidationTests
    {
        [ModTest("atmosphere/profile_json_roundtrip")]
        static void Roundtrip(ModTestContext ctx)
        {
            var profile = new AtmosphereProfile { ID = "author/ash", Name = "author/ash/name", Emitters = new() { new AtmosphereEmitterInfo {
                Type = "author/ash", Count = 17, Shape = "leaf", Seed = 73, Color = "#cc998866", Velocity = new[] { 1f, 2f, -3f },
                Height = new[] { .2f, 4f }, Size = new[] { .5f, 2f }, Sway = .3f, Pulse = .6f, Trail = .7f, Region = "vents", CycleSeconds = 7, RadiusMin = .2f, RadiusMax = .9f, Segments = 36,
                ExtraFields = new() { ["Heat"] = JsonSerializer.SerializeToElement(42) }
            } } };
            var restored = JsonSerializer.Deserialize<AtmosphereProfile>(JsonSerializer.Serialize(profile), JsonOptionsProvider.ReadOptions);
            AtmosphereValidation.Validate(restored);
            var emitter = restored.Emitters[0];
            ModAssert.Equal("author/ash",restored.ID,"profile id"); ModAssert.Equal("author/ash/name",restored.Name,"localized name");
            ModAssert.Equal("author/ash",emitter.Type,"custom emitter type"); ModAssert.Equal(17,emitter.Count,"budget");
            ModAssert.Equal("leaf",emitter.Shape,"shape"); ModAssert.Equal(73,emitter.Seed,"seed"); ModAssert.Equal("#cc998866",emitter.Color,"color");
            ModAssert.True(emitter.Velocity[0]==1 && emitter.Velocity[1]==2 && emitter.Velocity[2]==-3,"world-space velocity");
            ModAssert.True(emitter.Height[0]==.2f && emitter.Height[1]==4 && emitter.Size[0]==.5f && emitter.Size[1]==2,"height and size ranges");
            ModAssert.True(emitter.Sway==.3f && emitter.Pulse==.6f && emitter.Trail==.7f,"animation parameters");
            ModAssert.True(emitter.CycleSeconds == 7 && emitter.RadiusMin == .2f && emitter.RadiusMax == .9f && emitter.Segments == 36, "ripple controls survive JSON");
            ModAssert.Equal("vents",emitter.Region,"named host region"); ModAssert.Equal(42,emitter.ExtraFields["Heat"].GetInt32(),"custom factory fields survive");
        }
        [ModTest("atmosphere/invalid_ranges_and_global_budget")]
        static void Invalid(ModTestContext ctx)
        {
            ModAssert.True(Rejected(new AtmosphereEmitterInfo { Count = -1 }),"negative counts rejected");
            ModAssert.True(Rejected(new AtmosphereEmitterInfo { Count = 513 }),"per-emitter cap");
            ModAssert.True(Rejected(new AtmosphereEmitterInfo { Velocity = new[] { float.NaN,0f,0f } }),"NaN velocity rejected");
            ModAssert.True(Rejected(new AtmosphereEmitterInfo { Height = new[] { 1f,1f } }),"empty height rejected");
            ModAssert.True(Rejected(new AtmosphereEmitterInfo { Size = new[] { 0f,2f } }),"invisible size rejected");
            ModAssert.True(Rejected(new AtmosphereEmitterInfo { CycleSeconds = 0 }), "zero duration rejected");
            ModAssert.True(Rejected(new AtmosphereEmitterInfo { Segments = 129 }), "segment budget enforced");
            var profile = new AtmosphereProfile { ID = "too_many", Emitters = new List<AtmosphereEmitterInfo>() };
            for(var i=0;i<5;i++)profile.Emitters.Add(new AtmosphereEmitterInfo { Count = 512 });
            var rejected=false;
            try { AtmosphereValidation.Validate(profile); } catch(ArgumentException){rejected=true;}
            ModAssert.True(rejected,"profile cap applies across emitters");
        }
        private static bool Rejected(AtmosphereEmitterInfo info)
        {
            try { AtmosphereValidation.Validate(info); return false; } catch(ArgumentException){return true;}
        }
    }
}
