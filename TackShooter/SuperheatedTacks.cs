using System;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MirrorUniversePaths.TackShooter;

public class SuperheatedTacks : UpgradePlusPlus<TackFireNonRingPath>
{
    public override int Cost => 3500;
    public override int Tier => 4;

    public override string? Portrait => VanillaSprites.TackShooter400;

    public override string Description =>
        "Shoots superheated tacks that deal immense damage with slightly more pierce.";

    public override string DetailedDescription => "Tacks damage is increase to 5, and they have +2 pierce and +2 radius.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var ringOfFire = GameModel.Current.GetTower(TowerType.TackShooter, Math.Clamp(tier, 4, 5),
            Math.Clamp(towerModel.tiers[1], 0, 2), Math.Clamp(towerModel.tiers[2], 0, 2));

        var fireRingWeapon = ringOfFire.GetWeapon();

        var tackWeapon = towerModel.GetWeapon();
        var tackProj = tackWeapon.projectile;

        tackProj.pierce += 2;
        tackProj.radius = 2;

        var baseRingOfFire = GameModel.Current.GetTower(TowerType.TackShooter, Math.Clamp(tier, 4, 5));
        var baseRingOfFireDamage = baseRingOfFire.GetDescendant<ProjectileModel>();
        tackProj.GetBehavior<DamageModel>().damage = baseRingOfFireDamage.GetBehavior<DamageModel>().damage;
        foreach (var damageModifier in baseRingOfFireDamage.GetBehaviors<DamageModifierForTagModel>())
        {
            tackProj.AddBehavior(damageModifier.Duplicate());
            tackProj.hasDamageModifiers = true;
        }

        tackProj.ApplyDisplay<SuperHotTack>();

        if (Tier == tier)
        {
            tackWeapon.SetEject(fireRingWeapon.GetEject());
        }

        towerModel.SetDisplay(ringOfFire.display);
    }
}

public class SuperHotTack : ModDisplay
{
    public override PrefabReference BaseDisplayReference => Game.instance.model.GetTower(TowerType.TackShooter, 3, 0, 0)
        .GetDescendant<ProjectileModel>().display;

    public override void ModifyDisplayNodeAsync(UnityDisplayNode node, Action onComplete)
    {
        UseNode(Game.instance.model.GetTower(TowerType.TackShooter, 5, 0, 0)
            .GetAttackModel("Meteor").GetDescendant<ProjectileModel>().display.AssetGUID, meteor =>
        {
            var newMeteor = Object.Instantiate(meteor.transform.GetChild(0), node.transform, false);

            var offset = newMeteor.gameObject.GetComponent<OffsetTowardsCamera>();
            offset.offset = 0;
            offset.Start();
            offset.enabled = false;

            newMeteor.localPosition = new Vector3(0, 0, -5);

            newMeteor.GetChild(0).gameObject.SetActive(false);
            newMeteor.GetChild(1).gameObject.SetActive(false);

            var particleSystem = newMeteor.GetChild(2).GetComponentInChildren<ParticleSystem>();
            particleSystem.emissionRate *= 3;
            particleSystem.maxParticles *= 3;

            onComplete();
        });
    }
}