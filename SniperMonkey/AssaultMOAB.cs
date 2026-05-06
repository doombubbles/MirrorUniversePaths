using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using UnityEngine;

namespace MirrorUniversePaths.SniperMonkey;

public class AssaultMOAB : UpgradePlusPlus<SniperNonMaimPath>
{
    public override int Cost => 6300;
    public override int Tier => 4;

    public override string DisplayName => "Assault MOAB";
    public override string Description =>
        "Deadly Precision's +50 Ceramic damage bonus now also applies to MOAB-class Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(proj =>
        {
            if (!proj.HasBehavior<DamageModel>()) return;

            proj.RemoveBehaviors<SlowMaimMoabModel>();

            foreach (var damageModifier in proj.GetBehaviors<DamageModifierForTagModel>()
                         .Where(damageModifier => damageModifier.tag == BloonTag.Ceramic))
            {
                damageModifier.tag += "," + BloonTag.Moabs;
                damageModifier.tags = damageModifier.tag.CommaSeperatedListToStringArray();
            }
        });

        if (tier == Tier && GetInstances<AssaultMOABDisplay>()
                .FirstOrDefault(d => d.Bot == towerModel.tiers[2] && d.Mid == towerModel.tiers[1]) is { } display)
        {
            display.Apply(towerModel);
        }
    }
}

public class AssaultMOABDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.SniperMonkey, 4, Mid, Bot).display;

    public int Mid { get; init; }
    public int Bot { get; init; }

    public override string Name => $"{base.Name}4{Mid}{Bot}";

    public override IEnumerable<ModContent> Load()
    {
        yield return this;
        yield return new AssaultMOABDisplay { Mid = 1 };
        yield return new AssaultMOABDisplay { Mid = 2 };
        yield return new AssaultMOABDisplay { Bot = 1 };
        yield return new AssaultMOABDisplay { Bot = 2 };
    }

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        var meshRenderer = node.GetMeshRenderer();
        meshRenderer.AdjustHSV(0, -1, 0, new Color(68 / 255f, 89 / 255f, 33 / 255f), .25f);
        meshRenderer.AdjustHSV(0, -1, 0, new Color(41 / 255f, 51 / 255f, 16 / 255f), .25f);
        meshRenderer.AdjustHSV(0, -1, 0, new Color(107 / 255f, 127 / 255f, 41 / 255f), .25f);
        meshRenderer.AdjustHSV(0, -1, 0, new Color(165 / 255f, 193 / 255f, 13 / 255f), .25f);

        meshRenderer.SetOutlineColor(new Color(62 / 255f, 62 / 255f, 62 / 255f));
    }
}