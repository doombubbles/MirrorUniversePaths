using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Unity.Display;
using PathsPlusPlus;

namespace MirrorUniversePaths.SuperMonkey;

public class VengefulGod : UpgradePlusPlus<VengefulNonTemplePath>
{
    public override int Cost => 500000;
    public override int Tier => 5;

    public override string Portrait => VanillaSprites.SuperMonkey555;

    public override string Description => "There can be only one.";
    public override string DetailedDescription =>
        "Attack speed is doubled, shoots 5 Vengeful True Sun God projectiles at 80% scale that have 30 damage and 30 pierce, range increased by +15.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.SetDisplay<VengefulGodDisplay>();

        var weaponModel = towerModel.GetWeapon();
        weaponModel.projectile.SetDisplay<VengefulGodBlastDisplay>();
        weaponModel.SetEject(new Vector3(2, 5, 42));

        weaponModel.emission.Cast<RandomArcEmissionModel>().Count = 5;
        weaponModel.Rate /= 2f;

        weaponModel.projectile.pierce += 24;
        weaponModel.projectile.radius = 8;
        weaponModel.projectile.scale = .8f;

        var damageModel = weaponModel.projectile.GetDamageModel();
        damageModel.damage += 17;
        damageModel.immuneBloonProperties = damageModel.immuneBloonPropertiesOriginal = BloonProperties.None;
    }
}

public class VengefulGodDisplay : ModDisplay
{
    public override string BaseDisplay => "45d40b8795fd36c4eadb7856f96a180c";

    public override float Scale => .75f;
}

public class VengefulGodBlastDisplay : ModDisplay
{
    public override string BaseDisplay => "4fb0baaa656410f4ba1f2fd07b37eda4";

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var offsetTowardsCamera in node.gameObject.GetComponentsInChildren<OffsetTowardsCamera>(true))
        {
            offsetTowardsCamera.offset = 0;
            offsetTowardsCamera.Start();
        }
    }
}