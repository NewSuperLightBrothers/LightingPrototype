using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Laserinfo
{
    public LineRenderer bulletlinerenderer; 
    public List<LaserParticleSystem> usinglaserParticle;
    public LaserTeamType Team;
    public float speed;
    public float distance;
    public float dmg;
}


[Serializable]
public struct Guninfo
{
    public float maxgauge;
    public float usinggauge;
    public float Cooltime;
    public Animator gunanimation;
    public GameObject usingbullet;
    public Transform firepoint;
    public AudioSource shootingsound;
}