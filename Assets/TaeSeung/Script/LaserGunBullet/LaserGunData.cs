using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//탄알 정보
[Serializable]
public struct Laserinfo
{
    public LineRenderer bulletlinerenderer;                 //라인렌더러
    public List<LaserParticleSystem> usinglaserParticle;    //탄알 관련 파티클
    public float speed;                                     //탄알 스피드 (근거리의 경우 휘두르는 속도)
    public float distance;                                  //탄알이 날아가는 거리 (근거리의 경우 검의 거리)
    public float dmg;                                       //탄알 데미지
}

//무기 
[Serializable]
public struct Guninfo
{ 
    public float maxgauge;                                   //최대 빛 에너지 게이지
    public float usinggauge;                                 //한발 쏠때마다 사용되는 게이지
    public float Cooltime;                                   //다음 공격의 쿨타임
    public Animator gunanimation;                            //무기 애니메이션
    public GameObject usingbullet;                           //이 무기가 사용하는 실제 공격방식
    public Transform firepoint;                              //(원거리만 해당) 탄알 발사 위치
    public AudioSource shootingsound;                        //공격시 방출되는 소리
}