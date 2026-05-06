using System;
using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.Display.Animation;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using UnityEngine;
using Math = System.Math;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace MirrorUniversePaths.SuperMonkey;

public class SunDemigod : UpgradePlusPlus<SunNonTemplePath>
{
    public override int Cost => 50000;
    public override int Tier => 4;

    public override string Description => "Ascends further along the path of solar divinity.";
    public override string DetailedDescription =>
        "Attack speed is halved, shoots 3 Sun Temple projectiles at 80% scale that have 5 damage and 20 pierce, range increased by +15.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var weaponModel = towerModel.GetWeapon();

        var temple = Game.instance.model.GetTower(TowerType.SuperMonkey, Math.Clamp(tier, 4, 5),
            Math.Clamp(towerModel.tiers[1], 0, 2), Math.Clamp(towerModel.tiers[2], 0, 2));
        if (tier == Tier)
        {
            weaponModel.Rate *= 2f; // change to base temple rate?

            if (GetInstances<SunDemigodDisplay>()
                    .FirstOrDefault(d => d.Bot == towerModel.tiers[2] && d.Mid == towerModel.tiers[1]) is { } display)
            {
                display.Apply(towerModel);
            }
        }

        towerModel.range = temple.range;
        towerModel.GetAttackModel().range = temple.GetAttackModel().range;


        var proj = weaponModel.projectile = temple.GetWeapon().projectile.Duplicate();
        proj.scale = .8f;
        proj.radius *= .8f;

        proj.SetDisplay<TempleBlastDisplay>();

        towerModel.GetBehavior<CreateSoundOnAttachedModel>().SetName(Name);
    }
}

public class SunDemigodDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.SuperMonkey, 3, Mid, Bot).display;

    public int Mid { get; init; }
    public int Bot { get; init; }

    public override string Name => $"{base.Name}4{Mid}{Bot}";

    public override IEnumerable<ModContent> Load()
    {
        yield return this;
        yield return new SunDemigodDisplay { Mid = 1 };
        yield return new SunDemigodDisplay { Mid = 2 };
        yield return new SunDemigodDisplay { Bot = 1 };
        yield return new SunDemigodDisplay { Bot = 2 };
    }

    public override void ModifyDisplayNodeAsync(UnityDisplayNode node, Action onComplete)
    {
        node.GetMeshRenderer().AdjustHSV(0, -.5f, 0, new Color(255 / 255f, 219 / 255f, 0 / 255f), .1f);

        UseNode(Game.instance.model.GetTower(TowerType.SuperMonkey, 4, Mid, Bot)
            .GetAttackModel().GetBehavior<DisplayModel>().display.AssetGUID, head =>
        {
            var newHead = Object.Instantiate(head.transform.GetChild(0), node.GetBone("SuperMonkeyRig:MonkeyJnt_Head"),
                false);

            newHead.gameObject.RemoveComponent<Animator>();
            newHead.gameObject.RemoveComponent<PlayableAnimator>();
            newHead.gameObject.RemoveComponent<MonkeyAnimationController>();

            newHead.localPosition = new Vector3(10, 0, 0.15f);
            newHead.localRotation = Quaternion.Euler(0, 180, 270);
            newHead.localScale = new Vector3(.6f, .6f, .6f);

            newHead.GetComponentInChildren<SkinnedMeshRenderer>()
                .AdjustHSV(0, -.5f, 0, new Color(255 / 255f, 219 / 255f, 0 / 255f), .1f);

            onComplete();
        });
    }
}

public class TempleBlastDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.SuperMonkey, 4, 0, 0).GetWeapon().projectile.display;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var offsetTowardsCamera in node.gameObject.GetComponentsInChildren<OffsetTowardsCamera>(true))
        {
            offsetTowardsCamera.offset = 0;
            offsetTowardsCamera.Start();
        }
    }
}