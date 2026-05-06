using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using UnityEngine;

namespace MirrorUniversePaths.NinjaMonkey;

public class Boomjitsu : UpgradePlusPlus<NinjaBombPath>
{
    public override int Cost => 5000;
    public override int Tier => 4;

    public override string Description =>
        "All shurikens are now replaced with flash bombs which deal bonus damage to MOAB-Class Bloons. Caltrops explode too.";

    public override string DetailedDescription =>
        "Flash bombs are every attack instead of every 4th. Flash bombs deal +4 damage to MOABs. Caltrops create a flash bomb explosion on exhaust.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.RemoveBehavior<AttackModel>("StickyBomb");

        var weapon = towerModel.GetAttackModel().weapons[0]!;
        var shuriken = weapon.projectile;
        var bomb = weapon.GetBehavior<AlternateProjectileModel>().projectile;
        var explosion = bomb.GetDescendant<ProjectileModel>();

        var damageMod = shuriken.GetBehavior<DamageModifierForBloonStateModel>();

        explosion.AddBehavior(DamageModifierForTagModel.Create(new()
        {
            tag = BloonTag.Moabs,
            damageMultiplier = damageMod.damageMultiplier,
            damageAddative = damageMod.damageAdditive
        }));

        explosion.GetBehavior<SlowModel>().dontRefreshDuration = true;

        weapon.RemoveBehavior<AlternateProjectileModel>();

        weapon.SetProjectile(bomb);

        var caltrop = towerModel.GetAttackModel("Caltrops").GetDescendant<ProjectileModel>();

        caltrop.AddBehavior(CreateProjectileOnExhaustFractionModel.Create(new()
        {
            projectile = explosion.Duplicate(),
            fraction = 1,
            durationfraction = -1,
            emission = SingleEmissionModel.Create()
        }));
        caltrop.AddBehavior(CreateEffectOnExhaustFractionModel.Create(new()
        {
            effectModel = bomb.GetBehavior<CreateEffectOnContactModel>().effectModel,
            fraction = 1,
            durationFraction = -1
        }));
        var sound = bomb.GetBehavior<CreateSoundOnProjectileCollisionModel>();
        caltrop.AddBehavior(CreateSoundOnProjectileExhaustModel.Create(new()
        {
            sound1 = sound.sound1,
            sound2 = sound.sound2,
            sound3 = sound.sound3,
            sound4 = sound.sound4,
            sound5 = sound.sound5,
        }));


        if (tier == Tier && GetInstances<BoomjitsuDisplay>()
                .FirstOrDefault(d => d.Top == towerModel.tiers[0] && d.Mid == towerModel.tiers[1]) is { } display)
        {
            display.Apply(towerModel);
        }
    }
}

public class BoomjitsuDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.NinjaMonkey, Top, Mid, 4).display;

    public int Top { get; init; }
    public int Mid { get; init; }

    public override string Name => $"{base.Name}{Top}{Mid}{4}";

    public override IEnumerable<ModContent> Load()
    {
        yield return this;
        yield return new BoomjitsuDisplay { Top = 1 };
        yield return new BoomjitsuDisplay { Top = 2 };
        yield return new BoomjitsuDisplay { Mid = 1 };
        yield return new BoomjitsuDisplay { Mid = 2 };
    }

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        node.RemoveBone("NinjaMonkeyRig:Dart");

        var h = -155;
        var s = -.57f;
        var v = -.44f;

        node.GetMeshRenderer().AdjustHSV(h, s, v, new Color(255 / 255f, 56 / 255f, 0), .3f);
        node.GetMeshRenderer().AdjustHSV(h, s, v, new Color(148 / 255f, 56 / 255f, 0), .3f);
        node.GetMeshRenderer().AdjustHSV(h, s, v, new Color(82 / 255f, 56 / 255f, 0), .3f);
    }
}