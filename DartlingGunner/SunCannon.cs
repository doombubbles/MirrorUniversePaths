using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using UnityEngine;

namespace MirrorUniversePaths.DartlingGunner;

public class SunCannon : UpgradePlusPlus<DartlingCannonPath>
{
    public override int Tier => 5;
    public override int Cost => 50000;

    public override string Description =>
        "Now shoots 3 beams at once of pure solar energy, with massive pierce and damage.";

    public override string DetailedDescription =>
        "Shoots 3 projectiles at a time, pierce increased a further 10x, scale now 1.5x total, damage is 30 total with no MOAB bonus, shock life span 30s dealing 20 damage per tick.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var attackModel = towerModel.GetAttackModel();
        var laser = attackModel.weapons[0]!.projectile;
        GetInstance<Sun>().Apply(laser);
        attackModel.GetDescendant<RandomEmissionModel>().count = 3;

        attackModel.offsetZ += 5;
        attackModel.GetBehavior<RotateToPointerModel>().weaponEjectZ += 5;

        laser.pierce *= 10;
        laser.radius++;
        laser.scale = 1.5f;

        var damageModel = laser.GetDamageModel();
        damageModel.damage = 30;
        damageModel.immuneBloonProperties = damageModel.immuneBloonPropertiesOriginal = BloonProperties.None;
        laser.RemoveBehavior<DamageModifierForTagModel>();

        var shock = laser.GetBehavior<AddBehaviorToBloonModel>();
        shock.ApplyOverlay<SunShock>();
        shock.lifespan = 30;
        shock.lifespanFrames = (int) (shock.lifespan * 60);
        shock.GetBehavior<DamageOverTimeModel>().damage = 20;

        if (tier == Tier)
        {
            towerModel.display = CreatePrefabReference<SunCannonLegsDisplay>();
            towerModel.RemoveBehavior<DisplayModel>();
            towerModel.AddBehavior(DisplayModel.Create(new()
            {
                display = towerModel.display,
                category = DisplayCategory.Tower,
                layer = -1,
                ignoreRotation = true
            }));

            attackModel.AddBehavior(DisplayModel.Create(new()
            {
                name = "AttackDisplay",
                display = CreatePrefabReference<SunCannonDisplay>(),
                layer = -1,
                category = DisplayCategory.Tower
            }));
        }
    }
}

public class SunCannonDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference => Game.instance.model
        .GetTower(TowerType.DartlingGunner, 4, 0, 0)
        .display;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var meshRenderer in node.GetMeshRenderers())
        {
            meshRenderer.AdjustHSV(60, 0, .2f, new Color(206 / 255f, 0 / 255f, 0), .3f);
            meshRenderer.AdjustHSV(-15, 0, .2f, new Color(140 / 255f, 190 / 255f, 0), .75f);
        }

        node.GetBone("LOD_1").localPosition = new Vector3(0, 5, -10);
        node.GetBone("GunRig_Dartling").localScale = new Vector3(2, 1, 2);
    }
}

public class SunCannonLegsDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference => Game.instance.model
        .GetTower(TowerType.DartlingGunner, 5, 0, 0)
        .display;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var meshRenderer in node.GetMeshRenderers())
        {
            meshRenderer.ReplaceColor(new Color(231 / 255f, 229 / 255f, 231 / 255f),
                new Color(255 / 255f, 255 / 255f, 210 / 255f));
            meshRenderer.ReplaceColor(new Color(206 / 255f, 205 / 255f, 206 / 255f),
                new Color(253 / 255f, 255 / 255f, 99 / 255f));
            meshRenderer.ReplaceColor(new Color(178 / 255f, 178 / 255f, 178 / 255f),
                new Color(247 / 255f, 216 / 255f, 90 / 255f));
            meshRenderer.ReplaceColor(new Color(145 / 255f, 146 / 255f, 143 / 255f),
                new Color(239 / 255f, 189 / 255f, 35 / 255f));
            meshRenderer.ReplaceColor(new Color(123 / 255f, 123 / 255f, 123 / 255f),
                new Color(189 / 255f, 119 / 255f, 0 / 255f));
            meshRenderer.ReplaceColor(new Color(107 / 255f, 107 / 255f, 107 / 255f),
                new Color(189 / 255f, 97 / 255f, 0 / 255f));
            meshRenderer.ReplaceColor(new Color(93 / 255f, 93 / 255f, 93 / 255f),
                new Color(156 / 255f, 77 / 255f, 0 / 255f));
        }
    }
}