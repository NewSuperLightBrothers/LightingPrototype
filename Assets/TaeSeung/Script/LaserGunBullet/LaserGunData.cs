using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ź�� ����
[Serializable]
public struct Laserinfo
{
    public LineRenderer bulletlinerenderer;                 //���η�����
    public List<LaserParticleSystem> usinglaserParticle;    //ź�� ���� ��ƼŬ
    public float speed;                                     //ź�� ���ǵ� (�ٰŸ��� ��� �ֵθ��� �ӵ�)
    public float distance;                                  //ź���� ���ư��� �Ÿ� (�ٰŸ��� ��� ���� �Ÿ�)
    public float dmg;                                       //ź�� ������
}

//���� 
[Serializable]
public struct Guninfo
{ 
    public float maxgauge;                                   //�ִ� �� ������ ������
    public float usinggauge;                                 //�ѹ� �򶧸��� ���Ǵ� ������
    public float Cooltime;                                   //���� ������ ��Ÿ��
    public Animator gunanimation;                            //���� �ִϸ��̼�
    public GameObject usingbullet;                           //�� ���Ⱑ ����ϴ� ���� ���ݹ��
    public Transform firepoint;                              //(���Ÿ��� �ش�) ź�� �߻� ��ġ
    public AudioSource shootingsound;                        //���ݽ� ����Ǵ� �Ҹ�
}