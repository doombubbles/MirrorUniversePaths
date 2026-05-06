using System;
using System.Collections.Generic;
using System.Reflection;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using PathsPlusPlus;
using UnityEngine;

namespace MirrorUniversePaths.MonkeySub;

public class BloontoniumDarts : UpgradePlusPlus<SubNonSubmergePath>
{
    public override int Cost => 2400;
    public override int Tier => 4;

    public override string Description =>
        "Darts do more damage and leave behind radioactive fallout that irradiates Bloons. Twin Guns makes radiation damage faster, Barbed Darts makes it last longer.";

    public override string DetailedDescription =>
        "Dart damage increased to 2 like base 4xx, creates Fallout projectiles on first hitting a Bloon that have the same pierce, damage, and damage interval as the base 4xx attack. Lifespan is 2s, or 5s with Barbed Darts.";

    public override string Portrait => VanillaSprites.MonkeySub400;

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        // base 4xx Sub increases dart damage already

        var paragon = Game.instance.model.GetParagonTower(TowerType.MonkeySub);

        var baseSubmergeAttack = GameModel.Current
            .GetTower(TowerType.MonkeySub, Math.Clamp(tier, 4, 5),
                Math.Clamp(towerModel.tiers[1], 0, 2), Math.Clamp(towerModel.tiers[2], 0, 2))
            .GetAttackModel("Submerge");
        var baseSubmergeWeapon = baseSubmergeAttack.weapons[1]!;

        var fallout = paragon
            .GetDescendant<FinalStrikeModel>()
            .GetDescendant<CreateProjectilesOnTrackOnExpireModel>()
            .projectile
            .Duplicate();

        fallout.pierce = baseSubmergeWeapon.projectile.pierce;
        fallout.RemoveBehavior<RefreshPierceModel>();
        fallout.GetBehavior<AgeModel>().Lifespan = towerModel.appliedUpgrades.Contains(UpgradeType.BarbedDarts) ? 5 : 2;
        fallout.GetBehavior<DamageModel>().damage = baseSubmergeWeapon.GetDescendant<DamageModel>().damage;
        fallout.RemoveBehaviors<DamageModifierModel>();
        fallout.AddBehavior(baseSubmergeWeapon.GetDescendant<AddTagToBloonModel>().Duplicate());
        foreach (var damageModifier in baseSubmergeWeapon.projectile.GetBehaviors<DamageModifierForTagModel>())
        {
            fallout.AddBehavior(damageModifier.Duplicate());
            fallout.hasDamageModifiers = true;
        }

        var clearHitBloonsModel = fallout.GetBehavior<ClearHitBloonsModel>();
        clearHitBloonsModel.interval = baseSubmergeWeapon.rate;
        clearHitBloonsModel.intervalFrames = (int) (clearHitBloonsModel.interval / 60);

        fallout.UpdateCollisionPassList();

        if (tier == Tier)
        {
            fallout.scale *= .75f;
            fallout.radius *= .75f;
        }

        var projectileModel = towerModel.GetWeapon().projectile;

        projectileModel.GetDescendants<ProjectileModel>().ForEach(proj =>
        {
            if (proj.id.Contains("Airburst"))
            {
                proj.SetDisplay<GreenAirburstDart>();
            }
        });

        projectileModel.AddBehavior(CreateProjectileOnContactModel.Create(new()
        {
            projectile = fallout,
            name = "FIRST_CONTACT",
            emission = EmissionAtClosestPathSegmentModel.Create(new()
            {
                count = 1
            })
        }));
    }
}

public class GreenAirburstDart : ModDisplay
{
    public override string BaseDisplay => "8ac212675d4760541a5f9bcccad5ccf7";

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        node.GetRenderer<ParticleSystemRenderer>().material.color = new Color(0, 1, 0.5f, 1);
    }
}

[HarmonyPatch]
internal static class CreateProjectileOnContact_Collide
{
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(CreateProjectileOnContact), nameof(CreateProjectileOnContact.Collide));
        yield return AccessTools.Method(typeof(CreateProjectileOnContact),
            nameof(CreateProjectileOnContact.CollideMap));
    }

    [HarmonyPrefix]
    internal static bool Prefix(CreateProjectileOnContact __instance)
    {
        return !(__instance.projectile != null &&
                 __instance.createProjectileOnContactModel.name.Contains("FIRST_CONTACT") &&
                 __instance.projectile.collidedWith.Count > 1);
    }
}