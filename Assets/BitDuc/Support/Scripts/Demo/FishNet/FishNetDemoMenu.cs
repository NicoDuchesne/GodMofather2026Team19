using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Transporting;
using TMPro;
using BitDuc.Support.Demo;

namespace BitDuc.Support.Demo.FishNet
{
    public class FishNetDemoMenu : DemoMenu
    {
        [SerializeField] Button connect;
        [SerializeField] TMP_InputField hostToConnect;
        [SerializeField] Button host;
        [SerializeField] Button serve;
        [SerializeField] SceneAsset onlineScene;

        NetworkManager network;

        void Awake()
        {
            network = FindFirstObjectByType<NetworkManager>();
            connect.onClick.AddListener(Connect);
            host.onClick.AddListener(Host);
            serve.onClick.AddListener(Serve);
            network.ServerManager.OnServerConnectionState += OnServerConnectionState;
            Setup();
        }

        void OnDestroy()
        {
            if (network != null)
                network.ServerManager.OnServerConnectionState -= OnServerConnectionState;
        }

        void OnServerConnectionState(ServerConnectionStateArgs args)
        {
            if (args.ConnectionState == LocalConnectionState.Started)
                network.SceneManager.LoadGlobalScenes(new SceneLoadData(onlineScene.name) { ReplaceScenes = ReplaceOption.All });
        }

        void Connect()
        {
            network.TransportManager.Transport.SetClientAddress(hostToConnect.text);
            network.ClientManager.StartConnection();
            network.SceneManager.LoadGlobalScenes(new SceneLoadData(onlineScene.name) { ReplaceScenes = ReplaceOption.All });
        }

        void Host()
        {
            Debug.Log("Starting host.");
            network.ServerManager.StartConnection();
            network.ClientManager.StartConnection();
            Debug.Log("Host started.");
        }

        void Serve()
        {
            Debug.Log("Starting server.");
            network.ServerManager.StartConnection();
            Debug.Log("Server started.");
        }
    }
}
