using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace BitDuc.Support.Demo.Mirror
{
    public class MirrorDemoMenu : DemoMenu
    {
        [SerializeField] Button connect;
        [SerializeField] TMP_InputField hostToConnect;
        [SerializeField] Button host;
        [SerializeField] Button serve;

        NetworkManager network;

        void Awake()
        {
            network = FindFirstObjectByType<NetworkManager>();
            connect.onClick.AddListener(Connect);
            host.onClick.AddListener(Host);
            serve.onClick.AddListener(Serve);
            Setup();
        }

        void Connect()
        {
            network.networkAddress = hostToConnect.text;
            network.StartClient();
        }

        void Host()
        {
            Debug.Log("Starting host.");
            network.StartHost();
            Debug.Log("Host started.");
        }

        void Serve()
        {
            Debug.Log("Starting server.");
            network.StartServer();
            Debug.Log("Server started.");
        }
    }
}
