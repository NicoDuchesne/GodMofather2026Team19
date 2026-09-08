using System.Collections.Generic;
using System.Linq;
using BitDuc.EnhancedTimeline.Observable;
using BitDuc.EnhancedTimeline.Timeline;
using Mirror;
using R3;
using UnityEngine;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Network
{
    [RequireComponent(typeof(EnhancedTimelinePlayer))]
    public class NetworkTimelinePlayer : NetworkBehaviour, TimelinePlayer
    {
        public bool clientAuthority = true;

        /**
         * Timelines to synchronize over the network, this property should include every timeline to be played by this
         * player.
         */
        [SerializeField] PlayableAsset[] synchronizedTimelines;

        EnhancedTimelinePlayer player;
        Dictionary<PlayableAsset, int> idsBySynchronizedTimelines;

        public IEnumerable<PlayableAsset> CurrentlyPlaying => player.CurrentlyPlaying;

        void Awake()
        {
            player = GetComponent<EnhancedTimelinePlayer>();
            idsBySynchronizedTimelines = BuildIdsByTimeline();
        }

        /**
         * Adds a timeline to synchronize over the network. This method should be called on both ends.
         *
         * @param timeline The timeline to add.
         */
        public void AddSynchronizedTimeline(PlayableAsset timeline)
        {
            synchronizedTimelines = synchronizedTimelines.Append(timeline).ToArray();
            idsBySynchronizedTimelines = BuildIdsByTimeline();
        }

        /**
         * Remove a timeline to synchronize over the network. This method should be called on both ends.
         *
         * @param timeline The timeline to remove.
         */
        public void RemoveSynchronizedTimeline(PlayableAsset timeline)
        {
            synchronizedTimelines = synchronizedTimelines.Where(t => t != timeline).ToArray();
            idsBySynchronizedTimelines = BuildIdsByTimeline();
        }

        public Observable<TimelineEvent> Play(PlayableAsset timeline)
        {
            if (HasClientAuthority)
            {
                PlayOnServer(idsBySynchronizedTimelines[timeline]);
                return player.Play(timeline);
            }

            if (HasServerAuthority)
            {
                PlayOnClients(idsBySynchronizedTimelines[timeline]);
                return player.Play(timeline);
            }

            return R3.Observable.Empty<TimelineEvent>();
        }

        public void Pause(PlayableAsset timeline)
        {
            if (HasClientAuthority)
            {
                PauseOnServer(idsBySynchronizedTimelines[timeline]);
                player.Pause(timeline);
            }
            else if (HasServerAuthority)
            {
                PauseOnClients(idsBySynchronizedTimelines[timeline]);
                player.Pause(timeline);
            }
        }

        public void Resume(PlayableAsset timeline)
        {
            if (HasClientAuthority)
            {
                ResumeOnServer(idsBySynchronizedTimelines[timeline]);
                player.Resume(timeline);
            }
            else if (HasServerAuthority)
            {
                ResumeOnClients(idsBySynchronizedTimelines[timeline]);
                player.Resume(timeline);
            }
        }
        public void PauseAll()
        {
            if (HasClientAuthority)
            {
                PauseAllOnServer();
                player.PauseAll();
            }
            else if (HasServerAuthority)
            {
                PauseAllOnClients();
                player.PauseAll();
            }
        }

        public void ResumeAll()
        {
            if (HasClientAuthority)
            {
                ResumeAllOnServer();
                player.ResumeAll();
            }
            else if (HasServerAuthority)
            {
                ResumeAllOnClients();
                player.ResumeAll();
            }
        }

        public void Stop(PlayableAsset timeline)
        {
            if (HasClientAuthority)
            {
                StopOnServer(idsBySynchronizedTimelines[timeline]);
                player.Stop(timeline);
            }
            else if (HasServerAuthority)
            {
                StopOnClients(idsBySynchronizedTimelines[timeline]);
                player.Stop(timeline);
            }
        }

        public void BreakLoop(PlayableAsset timeline)
        {
            if (HasClientAuthority)
            {
                BreakLoopOnServer(idsBySynchronizedTimelines[timeline]);
                player.BreakLoop(timeline);
            }
            else if (HasServerAuthority)
            {
                BreakLoopOnClients(idsBySynchronizedTimelines[timeline]);
                player.BreakLoop(timeline);
            }
        }

        public Observable<T> Listen<T>() where T : TimelineEvent =>
            player.Listen<T>();

        [Command]
        void PlayOnServer(int timelineId)
        {
            if (!CheckAuthority(nameof(PlayOnServer)))
                return;

            foreach (var target in NetworkServer.connections.Values)
                if (target != connectionToClient)
                    PlayOnClients(target, timelineId);
        }

        [ClientRpc]
        void PlayOnClients(int timelineId)
        {
            player.Play(synchronizedTimelines[timelineId]);
        }

        [TargetRpc]
        void PlayOnClients(NetworkConnection _, int timelineId)
        {
            player.Play(synchronizedTimelines[timelineId]);
        }

        [Command]
        void PauseOnServer(int timelineId)
        {
            if (!CheckAuthority(nameof(PauseOnServer)))
                return;

            foreach (var target in NetworkServer.connections.Values)
                if (target != connectionToClient)
                    PauseOnClients(target, timelineId);
        }

        [ClientRpc]
        void PauseOnClients(int timelineId)
        {
            player.Pause(synchronizedTimelines[timelineId]);
        }

        [TargetRpc]
        void PauseOnClients(NetworkConnection _, int timelineId)
        {
            player.Pause(synchronizedTimelines[timelineId]);
        }

        [Command]
        void ResumeOnServer(int timelineId)
        {
            if (!CheckAuthority(nameof(ResumeOnServer)))
                return;

            foreach (var target in NetworkServer.connections.Values)
                if (target != connectionToClient)
                    ResumeOnClients(target, timelineId);
        }

        [ClientRpc]
        void ResumeOnClients(int timelineId)
        {
            player.Resume(synchronizedTimelines[timelineId]);
        }

        [TargetRpc]
        void ResumeOnClients(NetworkConnection _, int timelineId)
        {
            player.Resume(synchronizedTimelines[timelineId]);
        }

        [Command]
        void PauseAllOnServer()
        {
            if (!CheckAuthority(nameof(PauseAllOnServer)))
                return;

            foreach (var target in NetworkServer.connections.Values)
                if (target != connectionToClient)
                    PauseAllOnClients(target);
        }

        [ClientRpc]
        void PauseAllOnClients()
        {
            player.PauseAll();
        }

        [TargetRpc]
        void PauseAllOnClients(NetworkConnection _)
        {
            player.PauseAll();
        }

        [Command]
        void ResumeAllOnServer()
        {
            if (!CheckAuthority(nameof(ResumeAllOnServer)))
                return;

            foreach (var target in NetworkServer.connections.Values)
                if (target != connectionToClient)
                    ResumeAllOnClients(target);
        }

        [ClientRpc]
        void ResumeAllOnClients()
        {
            player.ResumeAll();
        }

        [TargetRpc]
        void ResumeAllOnClients(NetworkConnection _)
        {
            player.ResumeAll();
        }

        [Command]
        void StopOnServer(int timelineId)
        {
            if (!CheckAuthority(nameof(StopOnServer)))
                return;

            foreach (var target in NetworkServer.connections.Values)
                if (target != connectionToClient)
                    StopOnClients(target, timelineId);
        }

        [ClientRpc]
        void StopOnClients(int timelineId)
        {
            player.Stop(synchronizedTimelines[timelineId]);
        }

        [TargetRpc]
        void StopOnClients(NetworkConnection _, int timelineId)
        {
            player.Stop(synchronizedTimelines[timelineId]);
        }

        [Command]
        void BreakLoopOnServer(int timelineId)
        {
            if (!CheckAuthority(nameof(BreakLoopOnServer)))
                return;

            foreach (var target in NetworkServer.connections.Values)
                if (target != connectionToClient)
                    BreakLoopOnClients(target, timelineId);
        }

        [ClientRpc]
        void BreakLoopOnClients(int timelineId)
        {
            player.BreakLoop(synchronizedTimelines[timelineId]);
        }

        [TargetRpc]
        void BreakLoopOnClients(NetworkConnection _, int timelineId)
        {
            player.BreakLoop(synchronizedTimelines[timelineId]);
        }

        bool HasClientAuthority => clientAuthority && isOwned;

        bool HasServerAuthority => !clientAuthority && isServer;

        Dictionary<PlayableAsset, int> BuildIdsByTimeline() =>
            synchronizedTimelines
                .Select((timeline, index) => (timeline, index))
                .ToDictionary(
                    pair => pair.timeline,
                    pair => pair.index
                );

        bool CheckAuthority(string methodName)
        {
            if (clientAuthority)
                return true;

            Debug.LogWarning($"Attempted to call {methodName} from a client on an authoritative server.");
            return false;
        }
    }
}
