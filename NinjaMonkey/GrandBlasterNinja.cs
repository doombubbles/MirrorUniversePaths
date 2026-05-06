using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using UnityEngine;

namespace MirrorUniversePaths.NinjaMonkey;

public class GrandBlasterNinja : UpgradePlusPlus<NinjaBombPath>
{
    public override int Cost => 40000;
    public override int Tier => 5;

    public override string DisplayName => "Grand-Blaster Ninja";
    public override string Description => "Explosions now stun MOAB-class Bloons and deal way more damage.";

    public override string DetailedDescription =>
        "Applies the base xx5 effects of flash bombs stunning MOABs for .325s, flash bombs having 10 base damage, and caltrops having 5 base damage with +5 to ceramic. Flash bomb bonus MOAB damage is now +19.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        // damage bonuses already covered by Boomjitsu applying to the Tier 5 TowerModel

        if (tier == Tier)
        {
            GetInstance<GrandBlasterNinjaDisplay>().Apply(towerModel);
        }
    }
}

public class GrandBlasterNinjaDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.NinjaMonkey, 0, 0, 5).display;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        node.RemoveBone("Dart");

        node.GetMeshRenderer().AdjustHSV(-157, .88f, .32f, new Color(247 / 255f, 56 / 255f, 0), .5f);
        node.GetMeshRenderer().AdjustHSV(-157, .88f, .32f, new Color(189 / 255f, 42 / 255f, 0), .5f);
    }
}