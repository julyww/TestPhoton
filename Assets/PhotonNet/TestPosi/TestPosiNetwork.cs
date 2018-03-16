namespace NetworkTest
{
    using Photon;
    using UnityEngine;

    public sealed class TestPosiNetwork : PunBehaviour
    {
        private const string NetworkVersion = "0.0.1";
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
            GameObject go =  PhotonNetwork.Instantiate("TestPlayer", new Vector3 (Random.Range(-2,2), Random.Range(-2, 2),0), Quaternion.identity, 0);
            go.GetComponent<PhotonView>().owner.NickName = "aaaaa";
        }
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            base.OnPhotonPlayerConnected(newPlayer);
           
        }
    }
}
