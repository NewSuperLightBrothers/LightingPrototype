using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEmissionManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private MeshRenderer _meshrenderer;
    [SerializeField]
    private float _gauge;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public float getGuage() => _gauge;
    public void SetGauge(float gauge) => _gauge = gauge;


}
