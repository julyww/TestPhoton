using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class ChooseRoleManager : PunBehaviour
{
    public Transform chooseRoleUI;

    private GameObject roleItemTemp;
    private List<RoleItem> roleItemList;
    // Use this for initialization
    void Start () {
        roleItemTemp = Resources.Load("RoleItem") as GameObject;
        roleItemList = new List<RoleItem>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //获取配置文件的角色列表
        List<XElement> roleList = XMLLoad.GetInstance().GetRoleList();
        //根据角色列表生成选择UI
        for (int i = 0; i < roleList.Count; i++)
        {
            GameObject roleItemGO = Instantiate(roleItemTemp);
            roleItemGO.transform.SetParent(chooseRoleUI);
            RoleItem roleItem = roleItemGO.GetComponent<RoleItem>();
            string roleName = roleList[i].Attribute("Name").Value;
            string roleRemark = roleList[i].Attribute("Remark").Value;
            //设置选择角色的控件，传递一个photonView，让其可以使用这个photonView的RPC
            roleItem.SetRole(roleName, roleRemark, photonView);
            roleItemList.Add(roleItem);
        }
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //离开房间清空角色选择列表对象
        for (int i = 0; i < roleItemList.Count ; i++)
        {
            Destroy(roleItemList[i].gameObject);
           
        }
        roleItemList.Clear();
        chooseRoleUI.gameObject.SetActive(true);
    }
    [PunRPC]
    public void ChooseRole(string chooseName,PhotonPlayer photonPlayer) {
        //RPC对象为AllBuffered，需要根据是否为本地玩家进行判断处理不同的逻辑
        if (photonPlayer.IsLocal)
        {
            //设置玩家的名称
            PhotonNetwork.player.NickName = chooseName;
            //隐藏UI
            DisableChooseRoleUI();
            //获取角色的类型
            string roleType = GetRoleType(chooseName);

            if (roleType == "Operation")
            {
                PhotonSceneManager.GetInstance().CreateRole();
            }
            else if (roleType == "Observe") {
                PhotonSceneManager.GetInstance().CreateObserve();
            }
            //判断角色是为主机
            if (GetRoleIsMaster(chooseName) == "true") {
                PhotonSceneManager.GetInstance().SetPlayerMaster();
            }
        }
        else {
            RoleItem roleItem = GetRoleItemByName(chooseName);
            roleItem.DisableChooseBtn();
        }    
    }

    string GetRoleType(string roleName) {
        string ret = "";
        XElement retXE = XMLLoad.GetInstance().GetRoleList().Find(delegate(XElement xe) {
            return xe.Attribute("Name").Value == roleName;
        });
        ret = retXE.Attribute("Type").Value;
        return ret;
    }
    string GetRoleIsMaster(string roleName)
    {
        string ret = "";
        XElement retXE = XMLLoad.GetInstance().GetRoleList().Find(delegate (XElement xe) {
            return xe.Attribute("Name").Value == roleName;
        });
        ret = retXE.Attribute("IsMaster").Value;
        return ret;
    }
    /// <summary>
    /// 关闭角色选择界面
    /// </summary>
    /// <param name="photonPlayer"></param>
    void DisableChooseRoleUI() {
        chooseRoleUI.gameObject.SetActive(false);  
    }
    public RoleItem GetRoleItemByName(string chooseName) {
        return roleItemList.Find(delegate (RoleItem r) { return r.GetRoleName() == chooseName; });
    }

}
