using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;

namespace MirrorUniversePaths.SuperMonkey;

public class VengefulNonTemplePath : PathPlusPlus
{
    public override string Tower => TowerType.SuperMonkey;
    public override int ExtendVanillaPath => Top;
    public override bool UseUpgradedTowerModels => false;

    public override string DisplayName => "Vengeful Non-Temple";
    public override string Description => "Doesn't become a Temple or enact sacrifices, but is vengeful about it.";
}