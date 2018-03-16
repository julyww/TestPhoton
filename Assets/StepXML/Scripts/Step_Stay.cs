using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step_Stay : Step_Base
{

    protected float stayTime;
    private void OnTriggerStay(Collider other)
    {     
        if (isRun==true &&other.gameObject.name == triggerName && other.GetComponentInParent<PhotonView>().owner.NickName == roleName && !isFinish) {
            stayTime += Time.deltaTime;           
            if (stayTime > cue_Value1) {

                Finish();
            }
        }
    }
    public override void InitStep(string roleName, string cue_Trigger, string isShow, string cue_Value1)
    {
        base.InitStep(roleName, cue_Trigger, isShow, cue_Value1);
        stayTime = 0;
    }

}
