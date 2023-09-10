using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunShootingManager : LaserGunWeaponSystem
{
    [SerializeField]
    private Guninfo _guninfo;
    [SerializeField]
    private LaserTeamType team;

    private float currentbulletcount;
    private float _cooltimeinterval = 0f;
    private bool isshoot = true;


    private void Start()
    {
        currentbulletcount = _guninfo.maxgauge;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isshoot && currentbulletcount != 0){
            bulletfire();

            currentbulletcount -= _guninfo.usinggauge;
            if (currentbulletcount < 0) currentbulletcount = 0;

            _cooltimeinterval = 0;
            isshoot = false;
        }
    }

    private void FixedUpdate()
    {
        if (_cooltimeinterval >= _guninfo.Cooltime) isshoot = true;
        else _cooltimeinterval += Time.deltaTime;
    }

    void bulletfire()
    {
        GameObject newbullet = Instantiate(_guninfo.usingbullet);
        newbullet.transform.position = _guninfo.firepoint.position;
        newbullet.transform.rotation = _guninfo.firepoint.rotation;

        _guninfo.gunanimation.Play(0);
        _guninfo.shootingsound.Play(0);
    }

}
