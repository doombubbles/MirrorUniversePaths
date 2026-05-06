using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.Display.Animation;
using UnityEngine;

namespace MirrorUniversePaths.DartlingGunner;

public class PlasmaShock : ModBloonOverlay
{
    public override string BaseOverlay => "LaserShock";

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var renderer in node.genericRenderers)
        {
            if (renderer.Is<SpriteRenderer>(out var spriteRenderer))
            {
                var frameAnimator = spriteRenderer.gameObject.GetComponent<CustomSpriteFrameAnimator>();
                for (var i = 0; i < frameAnimator.frames.Count; i++)
                {
                    frameAnimator.frames[i] = GetSprite(Name + (i + 1));
                }
            }
            else if (renderer.Is<MeshRenderer>(out var meshRenderer))
            {
                meshRenderer.SetMainTexture(GetTexture(Name));
            }
        }
    }
}