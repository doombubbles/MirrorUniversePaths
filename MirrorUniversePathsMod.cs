global using BTD_Mod_Helper.Extensions;
using MelonLoader;
using BTD_Mod_Helper;
using MirrorUniversePaths;

[assembly: MelonInfo(typeof(MirrorUniversePathsMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MirrorUniversePaths;

public class MirrorUniversePathsMod : BloonsTD6Mod;