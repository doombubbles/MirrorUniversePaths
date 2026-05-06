using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;

namespace MirrorUniversePaths.TackShooter;

public class TackFireNonRingPath : PathPlusPlus
{
    public override string Tower => TowerType.TackShooter;
    public override int ExtendVanillaPath => Top;
    public override bool UseUpgradedTowerModels => false;

    public override string DisplayName => "Fiery Tacks";
    public override string Description => "Continues shooting tacks instead of replacing them with the ring of fire.";
}