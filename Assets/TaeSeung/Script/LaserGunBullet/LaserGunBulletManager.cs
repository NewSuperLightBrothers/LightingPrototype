using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserGunBulletManager : LaserGunManager
{ 
    private void FixedUpdate()
    {
        LaserBulletFire();
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (Mathf.Pow(2, other.transform.gameObject.layer) == LayerMask.GetMask("Mirror")) LaserBulletReflection(); 
        else if(Mathf.Pow(2, other.transform.gameObject.layer) == LayerMask.GetMask("Player")) LaserBulletToPlayer(other);
    
    }


    protected override void LaserBulletToPlayer(Collider other)
    {
        other.GetComponent<TestPlayer>().TestHP -= _laserinfo.dmg;
    }

    protected override void LaserBulletFire()
    { 
        if (Mathf.Abs(Vector3.Distance(_startposition, transform.position)) <= _laserinfo.distance) 
            transform.Translate(Vector3.forward * _laserinfo.speed);

        else LaserBulletDestroy();

        if (_rayhitposdistance >= 0)
        {    
            if(Vector3.Distance(_startposition, transform.position) - _rayhitposdistance > 0.1)
            {
                transform.position = _rayhitpos;
            }
        }
    }
    
    protected override void LaserBulletDestroy()
    {
        _laserinfo.usinglaserParticle[0].particleInstantiate(this.transform.position,this.transform.rotation);
        Destroy(this.gameObject);
    }

    protected override void LaserBulletReflection()
    {
        _laserinfo.distance -= Vector3.Distance(_startposition, _rayhitpos);
        _startposition = _rayhitpos;

        Vector3 forward = _bulletforwardvector.normalized;
        Vector3 collisionnormal = _rayoppositenormal;
        transform.forward = Vector3.Reflect(forward, collisionnormal).normalized;
        _bulletforwardvector = transform.forward;

        _ray.direction = _bulletforwardvector;
        _ray.origin = _startposition;

        _laserinfo.usinglaserParticle[1].particleInstantiate(_rayhitpos, this.transform.rotation);
        MakeMirrorRayhitInfo(_ray, 500);
    }



}
