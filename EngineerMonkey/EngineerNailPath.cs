using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using PathsPlusPlus;

namespace MirrorUniversePaths.EngineerMonkey;

public class EngineerNailPath : PathPlusPlus
{
    public override string Tower => TowerType.EngineerMonkey;
    public override int ExtendVanillaPath => Bottom;
    public override bool UseUpgradedTowerModels => true;

    public override string DisplayName => "Nail Gun";
    public override string Description => "Empowers Nail attacks instead of using Traps.";

    public static IEnumerable<AttackModel> Attacks(TowerModel towerModel)
    {
        yield return towerModel.GetBehavior<AttackModel>();

        if (towerModel.HasBehavior<AttackModel>("Spawner", out var sentrySpawner))
        {
            foreach (var sentry in sentrySpawner.GetDescendants<TowerModel>().AsIEnumerable())
            {
                yield return sentry.GetAttackModel();
            }
        }
    }

    public static IEnumerable<ProjectileModel> Nails(TowerModel towerModel) => Attacks(towerModel)
        .SelectMany(attack => attack.GetDescendants<ProjectileModel>().AsIEnumerable()
            .Where(projectileModel => projectileModel.HasBehavior<DamageModel>() &&
                                      projectileModel.HasBehavior<TravelStraitModel>()));

}