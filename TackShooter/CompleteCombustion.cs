using System;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;

namespace MirrorUniversePaths.TackShooter;

public class CompleteCombustion : UpgradePlusPlus<TackFireNonRingPath>
{
    public override int Cost => 45000;
    public override int Tier => 5;

    public override string Portrait => VanillaSprites.TackShooter500;

    public override string Description => "Impossibly hot tacks roast Bloons with blazing efficiency.";

    public override string DetailedDescription =>
        "Same meteor attack as base 5xx. Tack damage increased to 8 with +4 to MOABs, ranged increased by 11.5, pierce increased by 2x then +3, attack speed is tripled.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var infernoRing = GameModel.Current.GetTower(TowerType.TackShooter, Math.Clamp(tier, 4, 5),
            Math.Clamp(towerModel.tiers[1], 0, 2), Math.Clamp(towerModel.tiers[2], 0, 2));

        towerModel.range = infernoRing.range;
        towerModel.GetAttackModel().range = infernoRing.GetAttackModel().range;

        towerModel.GetWeapon().projectile.pierce *= 2;
        towerModel.GetWeapon().projectile.pierce += 3;
        towerModel.GetAttackModel().weapons[0]!.Rate /= 3f;

        towerModel.AddBehavior(infernoRing.GetAttackModel("Meteor").Duplicate());
    }
}