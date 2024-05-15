using Terraria.ModLoader;

public class KeybindSystem : ModSystem
{
    public static ModKeybind BuildMode;
    public static ModKeybind ToggleFlight;
    //public static ModKeybind EjectSuit;
    public static ModKeybind AttackModes;

    public static void RegisterKeybindWithCategory(ref ModKeybind variableSavedTo, Mod mod, string category, string name, string defaultBinding)
    {
        variableSavedTo = KeybindLoader.RegisterKeybind(mod, name, defaultBinding);
        MarvelTerrariaUniverse.MarvelTerrariaUniverse.CategorizedModKeybinds.Add(name, category);
    }

    public override void Load()
    {
        RegisterKeybindWithCategory(ref BuildMode, Mod, "Iron Man", "ToggleBuildMode", "G");
        RegisterKeybindWithCategory(ref ToggleFlight, Mod, "Iron Man", "ToggleFlight, Cannot be Mount Key", "F");
        //RegisterKeybindWithCategory(ref EjectSuit, Mod, "Iron Man", "EjectSuit", "X");
        RegisterKeybindWithCategory(ref AttackModes, Mod, "Iron Man", "ToggleAttackModes", "Q");

    }

    public override void Unload()
    {
        BuildMode = null;
        ToggleFlight = null;
        //EjectSuit = null;
        AttackModes = null;
    }
}