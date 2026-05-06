using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;

namespace MirrorUniversePaths.SniperMonkey;

public class SniperNonMaimPath : PathPlusPlus
{
    public override string Tower => TowerType.SniperMonkey;
    public override int ExtendVanillaPath => Top;
    public override bool UseUpgradedTowerModels => true;

    public override string DisplayName => "MOAB DPS";
    public override string Description => "Focuses on pure MOAB damage rather than stall/support.";
}