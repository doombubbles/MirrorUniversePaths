using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;

namespace MirrorUniversePaths.NinjaMonkey;

public class NinjaBombPath : PathPlusPlus
{
    public override string Tower => TowerType.NinjaMonkey;
    public override int ExtendVanillaPath => Bottom;
    public override bool UseUpgradedTowerModels => true;

    public override string DisplayName => "Bomb";
    public override string Description => "Amplifies Flash Bombs instead of getting Sticky Bombs.";
}