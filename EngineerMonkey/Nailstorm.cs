using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using PathsPlusPlus;

namespace MirrorUniversePaths.EngineerMonkey;

internal class Nailstorm : UpgradePlusPlus<EngineerNailPath>
{
    public override int Cost => 45000;
    public override int Tier => 5;

    public override string Description => "Unleashes bucket loads of nails at super speed!";

    public override string DetailedDescription =>
        "Shoots 3 nails at a time in a 30° cone, attack speed increased 3x, nail pierce increased by 2x, cash per pop now 3x";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var mainAttack = towerModel.GetBehavior<AttackModel>();
        var mainWeapon = mainAttack.weapons[0]!;

        var emission = mainWeapon.emission.Cast<EmissionWithOffsetsModel>();
        emission.spreadProjectilesAcrossMarkers = false;
        emission.projectileCount = 3;
        emission.randomRotationCone = 30;

        foreach (var attackModel in EngineerNailPath.Attacks(towerModel))
        {
            attackModel.weapons[0]!.Rate /= 3;
        }

        foreach (var nail in EngineerNailPath.Nails(towerModel))
        {
            nail.pierce *= 2;
        }
    }
}