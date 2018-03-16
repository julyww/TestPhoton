using Photon;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
using System;

public enum StepXmlState {
    Other,       //未知状态
    None,       //未连接网络之前的的状态
    Wait,       //步骤管理的等待状态
    CheckOther,   //步骤其他用户状态
    Run,        //步骤运行的状态
    Finish,     //步骤结束的状态
    FinishOther,//所有在线用户步骤结束的状态
    End         //所有结束
}
public class StepXmlManager : PunBehaviour
{
    public  int stepIndex = -1;
    private int stepLength;
    private List<XElement> stepList;
    public StepXmlState currentState;
    public Dictionary<string, bool> playerIsFinishDic;
    //所有的步骤管理集合
    private List<Step_Base> step_BaseList;
    //保存当前步骤涉及到的角色名称
    private List<string> step_CurrPlayerList;
    //动画控制器
    private StepXmlAnimationManager step_Animation;
    /// <summary>
    /// 初始化数据及集合对象
    /// </summary>
    /// <param name="stepList">xml配置文件的数据对象</param>
    public void SetStepList(List<XElement> stepList) {
        this.stepList = stepList;
        stepLength = this.stepList.Count;
        step_BaseList = new List<Step_Base>();
        playerIsFinishDic = new Dictionary<string, bool>();
        step_CurrPlayerList = new List<string>();
    }
	// Use this for initialization
	void Start () {     
        currentState = StepXmlState.None;
        step_Animation = GetComponent<StepXmlAnimationManager>();
    }	
	// Update is called once per frame
	void Update ()
    {
        if (stepList == null) {
            return;
        }
        switch (currentState)
        {
            case StepXmlState.None:
                CheckPlayer();
                break;
            case StepXmlState.Run:
                CheckStep();
                break;
            case StepXmlState.Finish:
                FinishStep();                
                break;
            case StepXmlState.Wait:
                NextStep();
                break;
            case StepXmlState.CheckOther:
                CheckOtherPlayer();
                break;
            case StepXmlState.FinishOther:               
                FinishOtherStep();
                break;
            case StepXmlState.End:
                //
                break;
            default:
                break;
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            LogAllFinish();
        }
    }
    public void CheckPlayer() {       
        if (PhotonSceneManager.GetInstance().currentRole != null) {
            currentState = StepXmlState.Wait;
        }
    }
    public void StartStep(XElement step) {
        step_BaseList.Clear();
       // Debug.Log("当前步骤的配置文件"+step);
        //获取所有角色的配置文件
        IEnumerable<XElement> stepRole = step.Elements("Role");
        string remark = "观察看看情况";
        //每个角色数据
        foreach (var roleItem in stepRole)
        {
            //角色名称
            string roleName = roleItem.Attribute("Name").Value;
            //只获取选中的角色的功能步骤
            if (roleName == PhotonNetwork.player.NickName) {
                //处理步骤下提示信息
                IEnumerable<XElement> stepCue = roleItem.Elements("Cue");
                foreach (var cueItem in stepCue)
                {
                    string cue_Path = cueItem.Attribute("Path").Value;
                    string cue_Trigger = cueItem.Attribute("Trigger").Value;
                    string cue_IsShow = cueItem.Attribute("IsShow").Value;
                    string cueValue1 = cueItem.Attribute("CueValue1").Value;
                    //根据配置文件的路径去获取提示对象
                    GameObject cue_GameObject = GameObject.Find(cue_Path);
                    if (cue_GameObject == null) {
                        Debug.LogError("没有找到指定的提示对象:" + cue_Path);
                    }
                    //获取Step管理脚本
                    Step_Base step_Base = cue_GameObject.GetComponent<Step_Base>();
                    if (step_Base == null)
                    {
                        Debug.LogError("对象物体上没有对应的StepBase脚本");
                    }
                    step_Base.InitStep(roleName, cue_Trigger, cue_IsShow, cueValue1);
                    step_BaseList.Add(step_Base);
                }
                remark = roleItem.Element("Remark").Value;
                step_Animation.SetStepAnimation(roleItem.Elements("AnimationObject"));
            }
            
            step_CurrPlayerList.Add(roleName);
        }
        Debug.Log(remark);
        
    }

  

    /// <summary>
    /// 下一个步骤
    /// </summary>
    public void NextStep()
    {
       
        stepIndex++;
        if (stepIndex < stepLength)
        {
            PhotonNetwork.player.SetStepState(false);
            step_CurrPlayerList.Clear();
            StartStep(stepList[stepIndex]);
            currentState = StepXmlState.Run;           
            step_Animation.PlayAnimation("Begin");
        }
        else {
            currentState = StepXmlState.End;
            Debug.Log("所有的步骤结束了");
        }
        Debug.Log("NextStep");
    }
    /// <summary>
    /// 检测步骤
    /// </summary>
    public void CheckStep() {
        bool stepFinish = true;
        for (int i = 0; i < step_BaseList.Count; i++)
        {
            if (!step_BaseList[i].IsFinish()) {
                stepFinish = false;
                return;
            }
        }
        if (stepFinish) {
            currentState = StepXmlState.Finish;  
        }
    }   
    /// <summary>
    /// 检测其他玩家的步骤
    /// </summary>
    public void CheckOtherPlayer()
    {
        bool stepFinish = true;
        if (!CheckAllIsFinish())
        {
            stepFinish = false;
            return;
        }
        if (stepFinish)
        {
            currentState = StepXmlState.FinishOther;
        }
    }
    public void FinishStep() {
        for (int i = 0; i < step_BaseList.Count; i++)
        {
            step_BaseList[i].AllFinish();
        }
     
        step_Animation.PlayAnimation("End");
        PhotonNetwork.player.SetStepState(true);
        Debug.Log("本地完成一个步骤");
        currentState = StepXmlState.Other; ;
        Invoke("DeleySetCheckOther", 2);
    }
    /// <summary>
    /// 增加延迟修改状态，避免多客户端数据同步延迟的问题
    /// </summary>
    void DeleySetCheckOther() {
        Debug.Log("等待2秒后");
        currentState = StepXmlState.CheckOther;
    }
    public void FinishOtherStep()
    {
        currentState = StepXmlState.Wait;
        Debug.Log("网络同步完成一个步骤");
    }
    [PunRPC] //废弃不使用
    void SetAllPlayerIsFinish(string role,bool isFinish) {
        if (playerIsFinishDic.ContainsKey(role))
        {
            playerIsFinishDic[role] = isFinish;
        }
        else {
            playerIsFinishDic.Add(role, isFinish);
        }
       // Debug.Log(role + " set  " + isFinish);
    }
    bool CheckAllIsFinish() {
        bool ret = true;
        //记录判断用户的次数
        int checkPlayerCount = 0;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            for (int j = 0; j < step_CurrPlayerList.Count; j++)
            {
                if (step_CurrPlayerList[j] == (PhotonNetwork.playerList[i].NickName))
                {
                    checkPlayerCount++;
                    if (PhotonNetwork.playerList[i].GetStepState() == false)
                    {
                        ret = false;
                        break;
                    }
                }
            }
            
        }
        //当真正判断的用户少于数据存储中的用户表示未完成
        if (checkPlayerCount < step_CurrPlayerList.Count) {
            ret = false;
        }
        return ret;
    }
    void LogAllFinish()
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            for (int j = 0; j < step_CurrPlayerList.Count; j++)
            {
                if (step_CurrPlayerList[j] == (PhotonNetwork.playerList[i].NickName))
                {
                    Debug.Log(PhotonNetwork.playerList[i].GetStepState() + " check  " + PhotonNetwork.playerList[i].NickName);
                }
            }

        }
    }
}
