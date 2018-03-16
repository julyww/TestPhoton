using Photon;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonLauncher : PunBehaviour
{
    //UI
    public GameObject connectBtn;
    public GameObject unConnectBtn;
    public Text connectState;

    private const string NetworkVersion = "0.0.1";
    // Use this for initialization
    void Start () {
        EventTriggerListener.Get(connectBtn).onClick = Connect;
        EventTriggerListener.Get(unConnectBtn).onClick = UnConnect;
        Connect(gameObject);
    }
    private void Update()
    {
        connectState.text = GetState();
    }
    public string GetState() {
        string ret = "";
        switch (PhotonNetwork.connectionState)
        {
            case ConnectionState.Disconnected:
                ret = "未连接";
                break;
            case ConnectionState.Connecting:
                ret = "连接中";
                break;
            case ConnectionState.Connected:
                ret = "已连接";
                break;
            case ConnectionState.Disconnecting:
                ret = "断开中";
                break;
            case ConnectionState.InitializingApplication:
                ret = "初始化";
                break;
            default:
                break;
        }
        return ret;
    }
    //连接服务器
    public void Connect(GameObject go)
    {
        PhotonNetwork.ConnectUsingSettings(NetworkVersion);
    }
    //连接服务器
    public void UnConnect(GameObject go)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceneManagerHelper.ActiveSceneName);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom("TestRoom");
    }

    public override void OnJoinedRoom()
    {
        
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
    }

}
