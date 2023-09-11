using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//무기에 대한 정보
public class LaserGunShootingManager : LaserGunWeaponShootingSystem
{
    [SerializeField]
    private List<MeshRenderer> L_Gunmeshrenderer;

    private Ray _ray;

    private new void Start()
    {
        base.Start();
        SetObjectTeamColor(_materialcolor, _emissionstrength);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _isshoot && _currentbulletcount != 0){
            BulletFire();
        }
        else if (Input.GetMouseButton(1)){
            _ray.direction = _guninfo.firepoint.forward;
            _ray.origin = _guninfo.firepoint.position;

            RaycastHit[] hits = Physics.RaycastAll(_ray, 250, LayerMask.GetMask("Light"));
            ObjectEmissionManager Emissionmanager = hits[0].transform.GetComponent<ObjectEmissionManager>();
            print(Emissionmanager.getGuage());
            
            TakeLightEnergy();
        }
    }

    private void FixedUpdate()
    {
        if (_cooltimeinterval >= _guninfo.Cooltime) _isshoot = true;
        else _cooltimeinterval += Time.deltaTime;
    }

    protected override void BulletFire()
    {
        GameObject newbullet = Instantiate(_guninfo.usingbullet);
        newbullet.transform.position = _guninfo.firepoint.position;
        newbullet.transform.rotation = _guninfo.firepoint.rotation;

        _guninfo.gunanimation.Play(0);
        _guninfo.shootingsound.Play(0);

        _currentbulletcount -= _guninfo.usinggauge;
        if (_currentbulletcount < 0) _currentbulletcount = 0;

        _cooltimeinterval = 0;
        _isshoot = false;
    }

    protected override void TakeLightEnergy()
    {
        
    }


    protected override void SetObjectTeamColor(Color color, float emissionstrength)
    {
        for (int i = 0; i < L_Gunmeshrenderer.Count; i++) {
            L_Gunmeshrenderer[i].material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionstrength));

        }
    }



}
