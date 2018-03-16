namespace NetworkTest
{
    using Photon;
    using UnityEngine;

    public sealed class NetworkManager : PunBehaviour
    {
        private const string NetworkVersion = "0.0.1";
        public GameObject camera_Vr;
        public GameObject camera_Free;
        private void OnEnable()
        {
            PhotonNetwork.ConnectUsingSettings(NetworkVersion);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            PhotonNetwork.CreateRoom("Test Room");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("加入");
            //if (PhotonNetwork.room.PlayerCount == 1)
            //{
            //    camera_Vr.SetActive(false);
            //    camera_Free.SetActive(true);
            //    PhotonNetwork.Instantiate("Sword", new Vector3(-0.105999f, 1.4f, 0.46f), Quaternion.identity, 0, null);
            //}
            //else
            //{
            //    camera_Vr.SetActive(true);
            //    camera_Free.SetActive(false);

            //    PhotonNetwork.Instantiate("VRPlayerNetworkRepresentation", Vector3.zero, Quaternion.identity, 0);
            //}

            camera_Vr.SetActive(true);
            camera_Free.SetActive(false);
            PhotonNetwork.Instantiate("Sword", new Vector3(-0.105999f, 1.4f, 0.46f), Quaternion.identity, 0, null);
            PhotonNetwork.Instantiate("VRPlayerNetworkRepresentation", Vector3.zero, Quaternion.identity, 0);
        }
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            base.OnPhotonPlayerConnected(newPlayer);
           
        }
    }
}
