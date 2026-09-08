using System.Collections.Generic;
using System.Linq;
using BitDuc.EnhancedTimeline.Animator;
using Mirror;
using UnityEngine;

namespace BitDuc.EnhancedTimeline.Network
{
    public class SmoothNetworkAnimator : NetworkBehaviour, AnimatorService
    {
        public UnityEngine.Animator animator;
        public bool clientAuthority = true;

        readonly SyncDictionary<int, float> floatParameters = new();
        readonly SyncDictionary<int, int> intParameters = new();
        readonly SyncDictionary<int, bool> boolParameters = new();
        Estimator[] estimators;

        void Awake()
        {
            floatParameters.OnSet += OnFloatChange;

            var now = NetworkTime.time;
            var floats = ParametersOfType(AnimatorControllerParameterType.Float);

            estimators = CreateEstimator(floats, now);

            foreach (var parameter in floats)
                floatParameters.Add(parameter.nameHash, parameter.defaultFloat);
        }

        void Update()
        {
            if (HasClientAuthority || HasServerAuthority)
                return;

            foreach (var estimator in estimators)
            {
                var estimation = estimator.Approximate(Time.deltaTime);
                animator.SetFloat(estimator.Hash, estimation);
            }

            foreach (var parameter in intParameters)
                animator.SetInteger(parameter.Key, parameter.Value);
 
            foreach (var parameter in boolParameters)
                animator.SetBool(parameter.Key, parameter.Value);
        }

        public void SetFloat(int hash, float value)
        {
            if (HasClientAuthority)
            {
                animator.SetFloat(hash, value);
                SetFloatOnServer(hash, value);
            }
            else if (HasServerAuthority)
            {
                animator.SetFloat(hash, value);
                SetFloatParameter(hash, value);
            }
        }

        public void SetInteger(int hash, int value)
        {
            if (HasClientAuthority)
            {
                animator.SetInteger(hash, value);
                SetIntegerOnServer(hash, value);
            }
            else if (HasServerAuthority)
            {
                animator.SetInteger(hash, value);
                SetIntegerParameter(hash, value);
            }
        }

        public void SetBool(int hash, bool value)
        {
            if (HasClientAuthority)
            {
                animator.SetBool(hash, value);
                SetBoolOnServer(hash, value);
            }
            else if (HasServerAuthority)
            {
                animator.SetBool(hash, value);
                SetBoolParameter(hash, value);
            }
        }

        public void Trigger(int hash)
        {
            if (HasClientAuthority)
            {
                animator.SetTrigger(hash);
                TriggerOnServer(hash);
            }
            else if (HasServerAuthority) 
            {
                animator.SetTrigger(hash);
                TriggerOnClients(hash);
            }
        }

        [Command(channel = Channels.Unreliable)]
        void SetFloatOnServer(int hash, float value)
        {
            if (!CheckAuthority(nameof(SetFloatOnServer)))
                return;

            floatParameters[hash] = value;
        }

        [Command(channel = Channels.Unreliable)]
        void SetIntegerOnServer(int hash, int value)
        {
            if (!CheckAuthority(nameof(SetIntegerOnServer)))
                return;

            SetIntegerParameter(hash, value);
        }

        [Command(channel = Channels.Unreliable)]
        void SetBoolOnServer(int hash, bool value)
        {
            if (!CheckAuthority(nameof(SetBoolOnServer)))
                return;

            boolParameters[hash] = value;
        }

        [Command]
        void TriggerOnServer(int hash)
        {
            if (!CheckAuthority(nameof(TriggerOnServer)))
                return;

            TriggerOnClients(hash);
        }

        [ClientRpc]
        void TriggerOnClients(int hash)
        {
            animator.SetTrigger(hash);
        }
        
        void SetFloatParameter(int hash, float value) =>
            floatParameters[hash] = value;

        void SetIntegerParameter(int hash, int value) =>
            intParameters[hash] = value;

        void SetBoolParameter(int hash, bool value) =>
            boolParameters[hash] = value;

        AnimatorControllerParameter[] ParametersOfType(AnimatorControllerParameterType type) =>
            animator.parameters
                .Where(parameter => parameter.type == type)
                .ToArray();

        void OnFloatChange(int hash, float value)
        {
            estimators.First(estimator => estimator.Hash == hash)
                .NextValue(value, Time.time);
        }

        bool HasClientAuthority => clientAuthority && isOwned;

        bool HasServerAuthority => !clientAuthority && isServer;

        bool CheckAuthority(string methodName)
        {
            if (clientAuthority)
                return true;

            Debug.LogWarning($"Attempted to call {methodName} from a client on an authoritative server.");
            return false;
        }

        static Estimator[] CreateEstimator(IEnumerable<AnimatorControllerParameter> floats, double now) =>
            floats
                .Select(parameter => new Estimator(parameter.nameHash, parameter.defaultFloat, now))
                .ToArray();

        static bool OpIsAdd<T>(SyncIDictionary<int, T>.Operation op) =>
            op == SyncIDictionary<int, T>.Operation.OP_ADD;

        static bool OpIsSet<T>(SyncIDictionary<int, T>.Operation op) =>
            op == SyncIDictionary<int, T>.Operation.OP_SET;
    }
}
