using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Taehyeon
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button startServerButton;
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;
        [SerializeField] private Button executePhysicsButton;
        
        [SerializeField] private TextMeshProUGUI playersInGameText;

        private bool hasServerStarted = false;
        private void Awake()
        {
            Cursor.visible = true;
            
        }

        private void Start()
        {
            startHostButton.onClick.AddListener(() =>
            {
                if (NetworkManager.Singleton.StartHost())
                {
                    Debug.Log("Host started");
                }
                else
                {
                    Debug.Log("Host failed to start");
                }
            });
            
            startServerButton.onClick.AddListener(() =>
            {
                if(NetworkManager.Singleton.StartServer())
                {
                    Debug.Log("Server started");
                }
                else
                {
                    Debug.Log("Server failed to start");
                }
            });
            
            startClientButton.onClick.AddListener(() =>
            {
                if(NetworkManager.Singleton.StartClient())
                {
                    Debug.Log("Client started");   
                }
                else
                {
                    Debug.Log("Client failed to start");
                }
            });

            NetworkManager.Singleton.OnServerStarted += () =>
            {
                hasServerStarted = true;
            };
            
            executePhysicsButton.onClick.AddListener(() =>
            {
                if (!hasServerStarted)
                {
                    Debug.Log("Server not started");
                    return;
                }
                
                SpawnerControl.Instance.SpawnObjects();
            });
        }

        private void Update()
        {
            playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";

        }
    }
}
