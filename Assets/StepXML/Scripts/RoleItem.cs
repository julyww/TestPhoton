using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleItem : MonoBehaviour {
    public Text roleNameText;
    public GameObject chooseBtn;
    private string roleName;
    private PhotonView rolePhotonView;
	// Use this for initialization
	void Start () {
        EventTriggerListener.Get(chooseBtn).onClick += chooseBtnOnClick;
        //chooseBtn.GetComponent<Button>().interactable = false;
    }
    public void SetRole(string roleName,string roleRemark, PhotonView photonView) {
        this.roleName = roleName;
        this.roleNameText.text = roleRemark;
        this.rolePhotonView = photonView;
    }
    private void chooseBtnOnClick(GameObject go)
    {
        rolePhotonView.RPC("ChooseRole", PhotonTargets.AllBuffered, roleName, PhotonNetwork.player);
    }
    public void DisableChooseBtn() {
        chooseBtn.SetActive(false);
    }
    public string GetRoleName() {
        return roleName;
    }
}
