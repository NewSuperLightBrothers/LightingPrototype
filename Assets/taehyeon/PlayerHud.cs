using System;
using System.Collections;
using System.Collections.Generic;
using Taehyeon;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerHud : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playersName = new();

    private bool _overlaySet = false;
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playersName.Value = $"Player {OwnerClientId}";
        }
    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TMP_Text>();
        localPlayerOverlay.text = playersName.Value;
    }

    private void Update()
    {
        if (!_overlaySet && !string.IsNullOrEmpty(playersName.Value))
        {
            SetOverlay();
            _overlaySet = true;
        }
    }
}