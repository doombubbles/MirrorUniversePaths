using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;

namespace MirrorUniversePaths.MonkeySub;

public class RbmkFallout : UpgradePlusPlus<SubNonSubmergePath>
{
    public override int Cost => 28000;
    public override int Tier => 5;

    public override string DisplayName => "RBMK Fallout";
    public override string Description =>
        "Radioactive Bloontonium Molecular Killzone. Darts and radiation tear apart Bloons at an atomic level, inflicting far more than 3.6 Roentgen.";

    public override string DetailedDescription =>
        "Dart damage increased to 5 like base 5xx, Fallout projectiles are 33% larger, and have the same pierce, damage and damage interval as the base 5xx attack.";

    public override string Portrait => VanillaSprites.MonkeySub500;

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        // BloontoniumDarts ApplyUpgrade correctly applies all changes based on 5xx Sub
    }
}