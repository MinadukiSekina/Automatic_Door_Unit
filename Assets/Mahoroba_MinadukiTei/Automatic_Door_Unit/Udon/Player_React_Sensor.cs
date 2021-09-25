
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace MinadukiTei.Products
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Player_React_Sensor : UdonSharpBehaviour
    {
        // target Behaviour and methods.
        [SerializeField] private UdonBehaviour otherBehaviour;
        [SerializeField] private string enteredMethodName;
        [SerializeField] private string exitedMethodName;

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            // Only when designated behaviour and method exist. 
            if (otherBehaviour == null) return;
            if (string.IsNullOrWhiteSpace(enteredMethodName)) return;

            // Call method locally.
            otherBehaviour.SendCustomEvent(enteredMethodName);
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            // Only when designated behaviour and method exist. 
            if (otherBehaviour == null) return;
            if (string.IsNullOrWhiteSpace(exitedMethodName)) return;

            // Call method locally.
            otherBehaviour.SendCustomEvent(exitedMethodName);
        }
    }

}
