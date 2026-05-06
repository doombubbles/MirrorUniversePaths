using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using PathsPlusPlus;

namespace MirrorUniversePaths.MonkeySub;

public class PeriscopeTargeting : UpgradePlusPlus<SubNonSubmergePath>
{
    public override int Cost => 700;
    public override int Tier => 3;

    public override string Description => "Allows Monkey Sub to detect and deal extra damage to Camo Bloons.";
    public override string DetailedDescription =>
        "Grants camo detection, all projectiles deal bonus camo damage equal to their base damage.";

    public override string Portrait => VanillaSprites.MonkeySub300;

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.RemoveBehavior<SubmergeModel>();
        towerModel.RemoveBehavior<SubmergeEffectModel>();
        towerModel.RemoveBehavior<LinkProjectileRadiusToTowerRangeModel>();
        towerModel.RemoveBehavior<AttackModel>("Submerge");
        towerModel.GetAttackModel().RemoveBehavior<SubmergedTargetModel>();
    }

    public override void LateApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        towerModel.GetDescendants<ProjectileModel>().ForEach(model =>
        {
            if (!model.HasBehavior(out DamageModel damageModel)) return;

            model.AddBehavior(DamageModifierForTagModel.Create(new()
            {
                tag = BloonTag.Camo,
                damageAddative = damageModel.damage,
                damageMultiplier = 1
            }));
            model.hasDamageModifiers = true;
        });
    }
}