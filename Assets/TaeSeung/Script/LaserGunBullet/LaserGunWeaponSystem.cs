using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//weapon���� �ֻ��� Ŭ����
public abstract class LaserGunWeaponSystem : MonoBehaviour
{
    [SerializeField]
    protected LaserTeamType Team;
    [SerializeField]
    protected float _emissionstrength;
    protected Color _materialcolor;


    private void Awake()
    {
        TakeTeamInfo();
        getTeamColor();
    }

    protected Color getTeamColor()
    {
        if (Team == LaserTeamType.Red) _materialcolor = Color.red;
        else if (Team == LaserTeamType.Blue) _materialcolor = Color.blue;
        else if (Team == LaserTeamType.Green) _materialcolor = Color.green;

        return _materialcolor;
    }
    protected abstract void SetObjectTeamColor(Color color, float emissionstrength);


    //������ �ܺ� ������ ���� �� ������ �޾ƿ� �� �ִ� �Լ�
    private void TakeTeamInfo() { }


}


public abstract class LaserGunWeaponShootingSystem : LaserGunWeaponSystem
{
    [SerializeField]
    protected Guninfo _guninfo;

    //���� ���� źâ ������ ����
    protected float _currentbulletcount;
    //cooltime ����
    protected float _cooltimeinterval = 0f;
    //��� ������ �ƴ��� Ȯ��
    protected bool _isshoot = true;


    protected void Start()
    {
       //�׽�Ʈ�� ������ �ʱ�ȭ
       _currentbulletcount = _guninfo.maxgauge;
    }

    protected abstract void BulletFire();
    protected abstract void TakeLightEnergy();

}