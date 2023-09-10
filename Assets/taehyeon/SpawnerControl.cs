using System;
using System.Collections;
using System.Collections.Generic;
using Taehyeon;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerControl : NetworkSingleton<SpawnerControl>
{
    [SerializeField] private GameObject _objectPrefab;

    [SerializeField] private int maxObjectInstanceCount = 3;

    private void Awake()
    {
        
    }

    public void SpawnObjects()
    {
        if(!IsServer) return;

        for (int i = 0; i < maxObjectInstanceCount; i++)
        {
            GameObject go = Instantiate(_objectPrefab, new Vector3(Random.Range(-10, 10), 10.0f, Random.Range(-10, 10)), Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
            
        }
    }
}
