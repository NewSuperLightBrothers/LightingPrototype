using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaserGunManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private LaserParticleSystem _particleBeam;
    [SerializeField]
    private LaserParticleSystem _particleBullet;


    void Start()
    {
        _particleBeam.particlestop();
        _particleBullet.particlestop();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            _particleBeam.particlestop();
            _particleBullet.particleplay();
            _particleBullet.transform.position = this.transform.position;
        }


    }
}
