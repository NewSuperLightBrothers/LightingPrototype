using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//무기들에 대한 전반적인 기능 정의를 하는 부분

public abstract class LaserGunManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("레이저 정보")]
    [SerializeField]
    protected Laserinfo _laserinfo;
    protected Vector3 _startposition;

    [SerializeField]
    private float _emissionstrength;
    private Color _materialcolor;


    protected abstract void LaserBulletDestroy();
    protected abstract void LaserBulletFire();


    private void Awake()
    {
        if (_laserinfo.Team == LaserTeamType.Red) _materialcolor = Color.red;
        else if (_laserinfo.Team == LaserTeamType.Blue) _materialcolor = Color.blue;
        else if (_laserinfo.Team == LaserTeamType.Green) _materialcolor = Color.green;
    }


    private void Start()
    {
        _startposition = transform.position;
        _laserinfo.bulletlinerenderer.material.SetColor("_EmissionColor", _materialcolor * Mathf.Pow(2, _emissionstrength));
    }

}
