using System;
using System.Collections;
using System.Collections.Generic;
using Taehyeon;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayersManager : NetworkSingleton<PlayersManager>
{
    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>();
    
    public int PlayersInGame
    {
        get => _playersInGame.Value;
        set => _playersInGame.Value = value;
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        { 
            if (IsServer)
            {
                Debug.Log($"{id} connected");
                _playersInGame.Value++;
            }
        };
        
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"{id} disconnected");
                _playersInGame.Value--;
            }
        };
    }
    
    
}


