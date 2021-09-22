using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace MinadukiTei.Products
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Automatic_Door_Unit : UdonSharpBehaviour
    {
        // User set target object and position of destination.
        // Value of transform synchronize automatically,
        // so default position and player count is defined as synchronizing variable.
        [SerializeField] private GameObject target;
        [SerializeField] private Vector3 destination;
        [UdonSynced(UdonSyncMode.None)] private Vector3 defaultPosition;
        [UdonSynced(UdonSyncMode.None)] private int triggerPlayerCount;

        void Start()
        {
            // Initialize position and player count.
            defaultPosition = target.transform.position;
            triggerPlayerCount = 0;
        }

        private void Update()
        {
            if (triggerPlayerCount == 0)
            {
                // If nobody is here, reset position.
                if (target.transform.position == defaultPosition) return;
                target.transform.position = Vector3.MoveTowards(target.transform.position, defaultPosition, Time.deltaTime);
            }
            else
            {
                // If anyone is here, move object.
                if (target.transform.position == destination) return;
                target.transform.position = Vector3.MoveTowards(target.transform.position, destination, Time.deltaTime);
            }
        }
        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (Networking.IsOwner(player, this.gameObject))
            {
                // If player is owner, just counts up.
                CountUp();
            }
            else
            {
                // If player is not owner, owner does.
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(CountUp));
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (Networking.IsOwner(player, this.gameObject))
            {
                // If player is owner, just counts down.
                CountDown();
            }
            else
            {
                // If player is not owner, owner does.
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(CountDown));
            }
        }

        public void CountUp()
        {
            // Add player count, and request others to synchronize 
            triggerPlayerCount += 1;
            RequestSerialization();
        }

        public void CountDown()
        {
            // Subtract player count, and request others to synchronize 
            triggerPlayerCount -= 1;
            if (triggerPlayerCount < 0) triggerPlayerCount = 0;
            RequestSerialization();
        }
    }
}
