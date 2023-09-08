using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunBulletManager : LaserGunManager
{
    private void Update()
    {
        LaserBulletFire();
    }



    private void OnCollisionEnter(Collision collision)
    {
        LaserBulletDestroy();
    }


    protected override void LaserBulletFire()
    {
        if (Mathf.Abs(Vector3.Distance(_startposition, transform.position)) < _laserinfo.distance)
        {
            transform.Translate(Vector3.forward * _laserinfo.speed);
        }
    }
    
    protected override void LaserBulletDestroy()
    {
        _laserinfo.usinglaserParticle.particleInstantiate(this.transform.position,this.transform.rotation);
        Destroy(this.gameObject);
    }
}
