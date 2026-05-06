using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using UnityEngine;

namespace MirrorUniversePaths.DartlingGunner;

public class PlasmaCannon : UpgradePlusPlus<DartlingCannonPath>
{
    public override int Tier => 4;
    public override int Cost => 10000;

    public override string Description =>
        "Upgrades to Plasma beams which have extra pierce and even more bonus MOAB damage. Can pop Lead Bloons.";

    public override string DetailedDescription =>
        "Pierce increased by 5x, projectile scaled 1.25x, can pop leads, bonus MOAB damage increased to +10, laser shock lifespan to 5s";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var laser = towerModel.GetAttackModel().weapons[0]!.projectile;
        GetInstance<Plasma>().Apply(laser);

        laser.pierce *= 5;
        laser.radius++;
        laser.scale = 1.25f;

        var damage = laser.GetDamageModel();
        damage.immuneBloonProperties &= ~BloonProperties.Lead;
        damage.immuneBloonPropertiesOriginal &= ~BloonProperties.Lead;

        laser.GetBehavior<DamageModifierForTagModel>().damageAddative = 10;

        var shock = laser.GetBehavior<AddBehaviorToBloonModel>();
        shock.ApplyOverlay<PlasmaShock>();
        shock.mutationId = "LaserShock3";
        shock.lifespan = 5;
        shock.lifespanFrames = (int) (shock.lifespan * 60);

        var damageOverTime = shock.GetBehavior<DamageOverTimeModel>();
        damageOverTime.immuneBloonProperties &= ~BloonProperties.Lead;
        damageOverTime.immuneBloonPropertiesOriginal &= ~BloonProperties.Lead;

        if (tier == Tier && GetInstances<PlasmaCannonDisplay>()
                .FirstOrDefault(d => d.Bot == towerModel.tiers[2] && d.Mid == towerModel.tiers[1]) is { } display)
        {
            display.Apply(towerModel);
        }
    }
}

public class PlasmaCannonDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.DartlingGunner, 4, Mid, Bot).display;

    public int Mid { get; init; }
    public int Bot { get; init; }

    public override string Name => $"{base.Name}4{Mid}{Bot}";

    public override IEnumerable<ModContent> Load()
    {
        yield return this;
        yield return new PlasmaCannonDisplay { Mid = 1 };
        yield return new PlasmaCannonDisplay { Mid = 2 };
        yield return new PlasmaCannonDisplay { Bot = 1 };
        yield return new PlasmaCannonDisplay { Bot = 2 };
    }

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        var meshRenderer = node.GetMeshRenderer();
        meshRenderer.AdjustHSV(-80, -.46f, 0, new Color(206 / 255f, 0 / 255f, 0), .5f);

        meshRenderer.SetOutlineColor(new Color(48 / 255f, 0 / 255f, 91 / 255f));
    }
}