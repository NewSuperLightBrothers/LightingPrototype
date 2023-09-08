using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Laserinfo
{
    public LineRenderer bulletlinerenderer; 
    public LaserParticleSystem usinglaserParticle;
    public LaserTeamType Team;
    public float speed;
    public float distance;
}
