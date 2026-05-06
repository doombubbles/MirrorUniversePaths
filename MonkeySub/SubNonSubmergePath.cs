using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;

namespace MirrorUniversePaths.MonkeySub;

public class SubNonSubmergePath : PathPlusPlus
{
    public override string Tower => TowerType.MonkeySub;
    public override int ExtendVanillaPath => Top;
    public override bool UseUpgradedTowerModels => true;

    public override string DisplayName => "Non-Submerge";
    public override string Description => "Uses radioactive power for extra damage without submerging.";
}