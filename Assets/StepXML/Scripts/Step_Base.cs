using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Step_Base : MonoBehaviour {
    public string roleName;
    public string triggerName;
    public bool isShow;
    public bool isRun = false;
    public float cue_Value1;

    public UnityEvent StartEvent;
    public UnityEvent FinishEvent;
    public UnityEvent AllFinishEvent;

    protected bool isFinish = false;
   
	// Use this for initialization
	void Start () {
        SetHide();

    }	
	// Update is called once per frame
	void Update () {
		
	}
    protected virtual void Finish() {
        isFinish = true;
        isRun = false;
        SetHide();
        if (FinishEvent != null)
        {
            FinishEvent.Invoke();
        }
    }
    public  bool  IsFinish()
    {
        return  isFinish;
    }
    /// <summary>
    /// 初始化步骤基类
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="cue_Trigger"></param>
    /// <param name="cue_StayTime"></param>
    public virtual void InitStep(string roleName,string cue_Trigger,string isShow, string cue_Value1) {
        this.roleName = roleName;
        this.triggerName = cue_Trigger;
        this.isShow = isShow == "true" ? true : false;
        this.cue_Value1 = float.Parse(cue_Value1);
        this.isRun = true;

        SetShow(this.isShow);
        if (StartEvent!=null) {
            StartEvent.Invoke();
        }
    }
    public virtual void Reset() {
        this.roleName = "";
        this.triggerName = "";
        this.isShow = false;
        this.cue_Value1 = 0;
        this.isRun = false;
        this.isFinish = false;
    }
    public void AllFinish() {
        Reset();
        if (AllFinishEvent != null)
        {
            AllFinishEvent.Invoke();
        }
    }

    public virtual void SetShow()
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
    public virtual void SetShow(bool isShow)
    {
        if (isShow)
        {
            GetComponentInChildren<MeshRenderer>().enabled = true;
        }
        GetComponent<Collider>().enabled = true;
    }
    public virtual void SetHide()
    {
        //Debug.Log("隐藏");
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
