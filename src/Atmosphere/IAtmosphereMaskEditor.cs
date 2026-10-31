namespace Cthangover.Mods.Effects.Atmosphere
{
    /// <summary>
    /// Optional capability of a map editor (plan 14.G10, <c>rpg_editor</c>): the effects editor's "Open map editor" button
    /// opens the map editor and asks it to show the Effects tool with the atmosphere profile chosen, so the author paints its
    /// mask on the map at once. The editor is found through <c>ToolFactory</c>, so neither mod names the other.
    /// </summary>
    public interface IAtmosphereMaskEditor
    {
        /// <summary>Selects the effects tool and the atmosphere profile <paramref name="profileId"/> (an unknown id leaves the choice as it is).</summary>
        void ShowMaskTool(string profileId);
    }
}
