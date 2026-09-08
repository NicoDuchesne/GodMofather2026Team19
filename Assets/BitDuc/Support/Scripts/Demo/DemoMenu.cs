using UnityEngine;
using UnityEngine.UI;

namespace BitDuc.Support.Demo
{
    public enum Authority { Client, Server }

    public class DemoMenu : MonoBehaviour
    {
        public static Authority Authority = Authority.Client;

        [SerializeField] Toggle clientAuthorityToggle;
        [SerializeField] Toggle serverAuthorityToggle;
        [SerializeField] Authority authority;

        protected void Setup()
        {
            Authority = authority;

            if (!clientAuthorityToggle || !serverAuthorityToggle)
                return;

            clientAuthorityToggle.isOn = authority == Authority.Client;
            serverAuthorityToggle.isOn = authority == Authority.Server;
            clientAuthorityToggle.onValueChanged.AddListener(SetAuthority);
        }

        static void SetAuthority(bool newClientAuthority) =>
            Authority = newClientAuthority ? Authority.Client : Authority.Server;
    }
}
