using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using UnityEngine;

namespace MirrorUniversePaths.SniperMonkey;

public class ExecuteMOAB : UpgradePlusPlus<SniperNonMaimPath>
{
    public override int Cost => 32000;
    public override int Tier => 5;

    public override string DisplayName => "Execute MOAB";
    public override string Description => "Deals 10x damage to MOAB-class Bloons that are below 25% health.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(proj =>
        {
            if (!proj.HasBehavior<DamageModel>()) return;
            proj.hasDamageModifiers = true;


            proj.AddBehavior(DamageModifierForTagModel.Create(new()
            {
                tag = BloonTag.Moabs,
                damageMultiplier = 10,
                name = "EXECUTE_25"
            }));
        });

        towerModel.SetDisplay<ExecuteMOABDisplay>();
    }
}

[HarmonyPatch(typeof(DamageModifierForTagModel), nameof(DamageModifierForTagModel.DoesApplyBonus))]
internal static class DamageModifierForTagModel_DoesApplyBonus
{
    [HarmonyPostfix]
    internal static void Postfix(DamageModifierForTagModel __instance, Bloon bloon, ref bool __result)
    {
        if (__instance.name.Contains("EXECUTE") && int.TryParse(__instance.name.Split('_')[^1], out var percent) &&
            bloon.Health * 100 > bloon.bloonModel.maxHealth * percent)
        {
            __result = false;
        }
    }
}

public class ExecuteMOABDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.SniperMonkey, 5, 0, 0).display;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        var meshRenderer = node.GetMeshRenderer(1);
        meshRenderer.AdjustHSV(0, -1, 0, new Color(68 / 255f, 89 / 255f, 33 / 255f), .25f);
        meshRenderer.AdjustHSV(0, -1, 0, new Color(41 / 255f, 51 / 255f, 16 / 255f), .25f);
        meshRenderer.AdjustHSV(0, -1, 0, new Color(107 / 255f, 127 / 255f, 41 / 255f), .25f);
        meshRenderer.AdjustHSV(0, -1, 0, new Color(165 / 255f, 193 / 255f, 13 / 255f), .25f);

        meshRenderer.SetOutlineColor(new Color(62 / 255f, 62 / 255f, 62 / 255f));
    }
}