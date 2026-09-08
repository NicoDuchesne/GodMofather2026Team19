#if MIRROR
using BitDuc.EnhancedTimeline.Demos.SinglePlayer;
using BitDuc.EnhancedTimeline.Network;
using BitDuc.Support.Demo;
using Mirror;

namespace BitDuc.EnhancedTimeline.Demos.Multiplayer
{
    public class MultiplayerFighter: NetworkBehaviour
    {
        void Start()
        {
            if (DemoMenu.Authority == Authority.Client)
                StartInClientAuthorityMode();
            else
                StartInServerAuthorityMode();
        }

        void StartInClientAuthorityMode()
        {
            SetClientAuthority();

            var fighter = GetComponent<Fighter>();

            if (!isOwned)
            {
                Destroy(fighter);
                Destroy(GetComponent<ServerFighterInput>());
            }
            else
            {
                var player = GetComponent<NetworkTimelinePlayer>();
                var animator = GetComponent<SmoothNetworkAnimator>();
                var input = GetComponent<LocalFighterInput>();
                Destroy(GetComponent<ServerFighterInput>());
                fighter.StartWith(player, animator, input);
            }
        }

        void StartInServerAuthorityMode()
        {
            SetServerAuthority();

            var fighter = GetComponent<Fighter>();

            Destroy(GetComponent<LocalFighterInput>());

            if (isServer)
            {
                var player = GetComponent<NetworkTimelinePlayer>();
                var animator = GetComponent<SmoothNetworkAnimator>();
                var input = GetComponent<ServerFighterInput>();
                fighter.StartWith(player, animator, input);
            }
            else
            {
                Destroy(fighter);
            }

            if (!isServer && !isLocalPlayer)
            {
                Destroy(GetComponent<ServerFighterInput>());
            }
        }

        void SetClientAuthority()
        {
            GetComponent<NetworkTransformBase>().syncDirection = SyncDirection.ClientToServer;
            GetComponent<NetworkTimelinePlayer>().clientAuthority = true;
            GetComponent<SmoothNetworkAnimator>().clientAuthority = true;
        }

        void SetServerAuthority()
        {
            GetComponent<NetworkTransformBase>().syncDirection = SyncDirection.ServerToClient;
            GetComponent<NetworkTimelinePlayer>().clientAuthority = false;
            GetComponent<SmoothNetworkAnimator>().clientAuthority = false;
        }
    }
}
#endif
