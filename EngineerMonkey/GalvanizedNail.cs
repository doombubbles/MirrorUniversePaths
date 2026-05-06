using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace MirrorUniversePaths.EngineerMonkey;

public class GalvanizedNail : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.EngineerMonkey, 0, 0, 4).GetDescendant<ProjectileModel>().display;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        node.GetRenderer<SpriteRenderer>().sprite = GetSprite(Name);
    }
}