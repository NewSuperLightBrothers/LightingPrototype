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
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkObjectPool.Instance.InitializePool();
        };
    }

    public void SpawnObjects()
    {
        if(!IsServer) return;

        for (int i = 0; i < maxObjectInstanceCount; i++)
        {
            // GameObject go = Instantiate(_objectPrefab, new Vector3(Random.Range(-10, 10), 10.0f, Random.Range(-10, 10)), Quaternion.identity);
            GameObject go = NetworkObjectPool.Instance.GetNetworkObject(_objectPrefab).gameObject;
            go.transform.position = new Vector3(Random.Range(-10, 10), 10.0f, Random.Range(-10, 10));
            go.GetComponent<NetworkObject>().Spawn();
            
        }
    }
}
