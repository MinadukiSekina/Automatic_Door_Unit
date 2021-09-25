
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace MinadukiTei.Products
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Auto_Mover : UdonSharpBehaviour
    {
        private Animator animator;
        // Synced variables and onValueChange event looks like property.
        [UdonSynced(UdonSyncMode.None), FieldChangeCallback(nameof(TriggerPlayerCount))] private int triggerPlayerCount;
        public int TriggerPlayerCount
        {
            get => triggerPlayerCount;
            set
            {
                triggerPlayerCount = value;
                if(triggerPlayerCount == 0)
                {
                    animator.SetBool("Open", false);
                }
                else
                {
                    animator.SetBool("Open", true);
                }
            }
        }

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Call from sensor unit's onPlayerTriggerEnter.
        public void PlayerEnter()
        {
            if (Networking.LocalPlayer.IsOwner(this.gameObject))
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

        // Call from sensor unit's onPlayerTriggerExit.
        public void PlayerExit()
        {
            if (Networking.LocalPlayer.IsOwner(this.gameObject))
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
            TriggerPlayerCount += 1;
            RequestSerialization();
        }

        public void CountDown()
        {
            // Subtract player count, and request others to synchronize 
            TriggerPlayerCount -= 1;
            if (triggerPlayerCount < 0) triggerPlayerCount = 0;
            RequestSerialization();
        }
    }

}
