using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;

namespace MirrorUniversePaths.DartlingGunner;

public class DartlingCannonPath : PathPlusPlus
{
    public override string Tower => TowerType.DartlingGunner;
    public override int ExtendVanillaPath => Top;
    public override bool UseUpgradedTowerModels => false;

    public override string DisplayName => "Cannon";
    public override string Description => "Continues shooting upgraded laser projectiles rather than switching to solid beams.";
}