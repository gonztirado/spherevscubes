using UnityEngine;
using System.Collections;

public class IAGunController : GunController
{
    [Header("IASettings")]
    public bool enableIA;
    

    protected override void CheckFire()
    {
        if(enableIA)
            TryShoot(1);
        else
            base.CheckFire();
    }
}