using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserParticleSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> l_ParticlePrefab;
    public List<ParticleSystem> l_particlesystems;

    public void particleplay()
    {
        for (int i = 0; i < l_particlesystems.Count; i++) {
            l_particlesystems[i].Play();
         }
    }

    public void particlestop()
    {
        for (int i = 0; i < l_particlesystems.Count; i++)
        {
            l_particlesystems[i].Stop();
        }
    }

    public bool isparticleStart()
    {
        return l_particlesystems[0].isPlaying;
    }

    public void particleInstantiate()
    {

    }

}
