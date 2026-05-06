using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using PathsPlusPlus;
using Math = System.Math;
using Vector3 = UnityEngine.Vector3;

namespace MirrorUniversePaths.SuperMonkey;

public class SunGod : UpgradePlusPlus<SunNonTemplePath>
{
    public override int Cost => 100000;
    public override int Tier => 5;

    public override string Icon => VanillaSprites.TrueSonGodUpgradeIcon;
    public override string? Portrait => VanillaSprites.SuperMonkey500;

    public override string Description => "Tremble before the AWESOME power of the Sun God!! *trueness not guaranteed*";
    public override string DetailedDescription =>
        "Attack speed is doubled, shoots 5 True Sun God projectiles at 80% scale that have 15 damage and 20 pierce.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.SetDisplay<SunGodDisplay>();

        var weaponModel = towerModel.GetWeapon();

        weaponModel.projectile.SetDisplay<GodBlastDisplay>();
        weaponModel.SetEject(new Vector3(2, 5, 42));
        weaponModel.GetDescendant<RandomArcEmissionModel>().Count = 5;
    }
}

public class SunGodDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.SuperMonkey, 5).GetAttackModel().GetBehavior<DisplayModel>().display;

    public override float Scale => .75f;
}

public class GodBlastDisplay : ModDisplay
{
    public override PrefabReference BaseDisplayReference =>
        Game.instance.model.GetTower(TowerType.SuperMonkey, 5, 0, 0).GetWeapon().projectile.display;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var offsetTowardsCamera in node.gameObject.GetComponentsInChildren<OffsetTowardsCamera>(true))
        {
            offsetTowardsCamera.offset = 0;
            offsetTowardsCamera.Start();
        }
    }
}