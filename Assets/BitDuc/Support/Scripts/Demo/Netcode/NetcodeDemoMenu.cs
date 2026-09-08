using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace BitDuc.Support.Demo.Netcode
{
    public class NetcodeDemoMenu : DemoMenu
    {
        [SerializeField] Button connect;
        [SerializeField] TMP_InputField hostToConnect;
        [SerializeField] Button host;
        [SerializeField] Button serve;
        [SerializeField] SceneAsset onlineScene;

        NetworkManager network;
        UnityTransport transport;

        void Awake()
        {
            network = FindFirstObjectByType<NetworkManager>();
            transport = network.GetComponent<UnityTransport>();
            connect.onClick.AddListener(Connect);
            host.onClick.AddListener(() => network.StartCoroutine(Host()));
            serve.onClick.AddListener(Serve);
            Setup();
        }

        void Connect()
        {
            var (address, port) = GetAddressAndPort();
            var connectionData = transport.ConnectionData;
            connectionData.Address = address;
            connectionData.Port = port ?? connectionData.Port;

            transport.ConnectionData = connectionData;

            Debug.Log("Starting client.");
            SceneManager.LoadScene(onlineScene.name);
            network.StartClient();
            Debug.Log("Client started.");
        }

        IEnumerator Host()
        {
            Debug.Log("Starting host.");

            var loading = SceneManager.LoadSceneAsync(onlineScene.name);
            while (!loading.isDone)
                yield return null;

            network.StartHost();
            Debug.Log("Host started.");
        }

        void Serve()
        {
            Debug.Log("Starting server.");
            SceneManager.LoadScene(onlineScene.name);
            network.StartServer();
            Debug.Log("Server started.");
        }
 
        (string, ushort?) GetAddressAndPort()
        {
            var addressAndPort = hostToConnect.text;
            var split = addressAndPort.Split(':');

            var address = split[0];
            var port = split.Length > 1 ? split[1] : null;

            if (port == null)
                return (address, null);
            
            return ushort.TryParse(port, out var ushortPort) ?
                (address, ushortPort) :
                throw new Exception($"Invalid port {port}.");
        }
    }
}
