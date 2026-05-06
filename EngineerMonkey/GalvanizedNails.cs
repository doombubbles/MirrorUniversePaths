using System.Linq;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using PathsPlusPlus;

namespace MirrorUniversePaths.EngineerMonkey;

public class GalvanizedNails : UpgradePlusPlus<EngineerNailPath>
{
    public override int Cost => 3600;
    public override int Tier => 4;

    public override string Description =>
        "Nails deal significantly increased damage, can pop all Bloon types, and give increased cash per pop.";

    public override string DetailedDescription =>
        "Nail damage increased by +2, pops all Bloon types, cash per pop is 2x, Deconstruction damage bonus increased by 4";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var damageIncrease = 2;

        if (towerModel.HasBehavior<AttackModel>("BloonTrap", out var bloonTrap))
        {
            var eatBloon = bloonTrap.GetDescendant<EatBloonModel>();

            bloonTrap.AddBehavior(CashIncreaseModel.Create(new()
            {
                multiplier = eatBloon.rbeCashMultiplier
            }));

            towerModel.RemoveBehavior(bloonTrap);
        }

        foreach (var nail in EngineerNailPath.Nails(towerModel))
        {
            var damageModel = nail.GetDamageModel();
            damageModel.immuneBloonProperties = damageModel.immuneBloonPropertiesOriginal = BloonProperties.None;
            damageModel.damage += damageIncrease;

            nail.SetDisplay(CreatePrefabReference<GalvanizedNail>());

            if (!towerModel.appliedUpgrades.Contains(UpgradeType.Deconstruction)) continue;

            foreach (var damageModifier in nail.GetBehaviors<DamageModifierForTagModel>().Where(damageModifier =>
                         damageModifier.tags.Contains(BloonTag.Moabs) ||
                         damageModifier.tags.Contains(BloonTag.Fortified)))
            {

                damageModifier.damageAddative += damageIncrease * 2;
            }
        }


        towerModel.RemoveBehavior(bloonTrap);
    }
}