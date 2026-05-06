using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using PathsPlusPlus;

namespace MirrorUniversePaths.SuperMonkey;

public class VengefulAvatar : UpgradePlusPlus<VengefulNonTemplePath>
{
    public override int Cost => 100000;
    public override int Tier => 4;

    public override string Portrait => VanillaSprites.SunAvatarTurret555;

    public override string Description => "Walks the path of vengeance to enact untold ruin onto Bloons.";
    public override string DetailedDescription => "Projectile damage increased to 13, can hit Purple Bloons, range increased by +15";

    // Real stats of a mini Vengeful Sun Avatar without buffs
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.SetDisplay<VengefulAvatarDisplay>();
        towerModel.range += 15;
        towerModel.GetAttackModel().range += 15;

        var projectile = towerModel.GetWeapon().projectile;
        projectile.SetDisplay("e4abb3d789fca4747a7459d21c835472");

        var damageModel = projectile.GetDamageModel();
        damageModel.immuneBloonProperties &= ~BloonProperties.Purple;
        damageModel.immuneBloonPropertiesOriginal &= ~BloonProperties.Purple;
        damageModel.damage = 13;

        towerModel.GetBehavior<CreateSoundOnAttachedModel>().SetName(Name);
    }
}

public class VengefulAvatarDisplay : ModDisplay
{
    public override string BaseDisplay => "36d3f05381187cf4da168f4676076159";

    public override float Scale => 1.4f / .8f;
}