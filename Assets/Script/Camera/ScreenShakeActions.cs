using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class defines actions triggered by various events in the game, such as shooting, grenade explosions, and sword hits.
public class ScreenShakeActions : MonoBehaviour
{
    private void Start()
    {
        // Subscribe to events triggered by shooting, grenade explosions, and sword hits
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
    }

    private void SwordAction_OnAnySwordHit(object sender, System.EventArgs e)
    {
        ScreenShake.Instance.Shake(2f);
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, System.EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
