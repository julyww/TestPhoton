using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSceneManager : PunBehaviour
{
    private static PhotonSceneManager photonSceneManager;
    public GameObject camera_Vr;
    public GameObject camera_Free;
    public GameObject currentRole;

    public Vector3 cameraRigPosi;
    public static PhotonSceneManager GetInstance() {
        return photonSceneManager;
    }
    private void Awake()
    {
        photonSceneManager = this;
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!PhotonNetwork.connected)
        {
            return;
        }
        //if (photonView.isMine)
        //{
        //    if (PhotonNetwork.isMasterClient)
        //    {
        //        cameraRigPosi = LocalGameObjectManager.GetCameraRig().transform.position;
        //    }
        //}
        //else
        //{
        //    LocalGameObjectManager.GetCameraRig().transform.position = cameraRigPosi;
        //}
    }

    public void CreateRole() {
       // camera_Vr.SetActive(true);
       // camera_Free.SetActive(false);
        currentRole = PhotonNetwork.Instantiate("Role", Vector3.zero, Quaternion.identity, 0);
    }
    public void CreateObserve()
    {
      //  camera_Vr.SetActive(false);
      //  camera_Free.SetActive(true);
    }
    public void SetPlayerMaster()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.player);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(cameraRigPosi);
        }
        else
        {
            this.cameraRigPosi = (Vector3)stream.ReceiveNext();
        }
    }
    public void SetCameraRig( Vector3 setPosi){
        if (photonView.isMine && PhotonNetwork.player.IsMasterClient) {
           // LocalGameObjectManager.GetCameraRig().transform.position = setPosi;
        }        
    }
}
