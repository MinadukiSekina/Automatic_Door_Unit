
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace MinadukiTei
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Automatic_Door_Unit : UdonSharpBehaviour
    {
        // 動かす対象と目的地（グローバル座標）は指定してもらう
        [SerializeField] private GameObject target;
        [SerializeField] private Vector3 destination;
        [UdonSynced(UdonSyncMode.None)] private Vector3 defaultPosition;
        [UdonSynced(UdonSyncMode.None)] private int triggerPlayerCount;

        void Start()
        {
            // 初期化
            //if (target is null) target = new GameObject("Target");
            defaultPosition = target.transform.position;
            triggerPlayerCount = 0;
        }

        private void Update()
        {
            if(triggerPlayerCount == 0)
            {
                // 誰も触れていないなら元の位置に戻す
                if (target.transform.position == defaultPosition) return;
                target.transform.position = Vector3.MoveTowards(target.transform.position, defaultPosition, Time.deltaTime);
            }
            else
            {
                // 誰かが触れているなら目標の位置に動かす
                if (target.transform.position == destination) return;
                target.transform.position = Vector3.MoveTowards(target.transform.position, destination, Time.deltaTime);
            }
        }
        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (Networking.IsOwner(player, target))
            {
                // オブジェクトのオーナーなら純粋にカウントアップ
                CountUp();
            }
            else
            {
                // オブジェクトのオーナーでなければオブジェクトのオーナーに実行要求
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(CountUp));
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (Networking.IsOwner(player, target))
            {
                // オブジェクトのオーナーなら純粋にカウントダウン
                CountDown();
            }
            else
            {
                // オブジェクトのオーナーでなければオブジェクトのオーナーに実行要求
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(CountDown));
            }
        }

        public void CountUp()
        {
            // 人数をプラスして、同期要求
            triggerPlayerCount += 1;
            RequestSerialization();
        }

        public void CountDown()
        {
            // 人数をマイナスして、同期要求
            triggerPlayerCount -= 1;
            if (triggerPlayerCount < 0) triggerPlayerCount = 0;
            RequestSerialization();
        }
    }
}
