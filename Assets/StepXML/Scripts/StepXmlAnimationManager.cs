using Photon;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class StepXmlAnimationManager : PunBehaviour
{
    private List<GameObject> stepAnimationGameObjectList;
    private List<StepAnimation> stepAnimationList;
    // Use this for initialization
    void Start () {　
        stepAnimationGameObjectList = new List<GameObject>();
        stepAnimationList = new List<StepAnimation>();
    }
    public void SetStepAnimation(IEnumerable<XElement> animationXmls) {
        stepAnimationList.Clear();
        foreach (var animationXml in animationXmls)
        {
            StepAnimation stepAnimation = new StepAnimation();
            stepAnimation.Path = animationXml.Attribute("Path").Value;
            stepAnimation.PlayAnimation = animationXml.Attribute("PlayAnimation").Value;
            stepAnimation.BeginOrEnd = animationXml.Attribute("BeginOrEnd").Value;
            stepAnimationList.Add(stepAnimation);
        }
    }
    public void PlayAnimation(string BeginOrEnd)
    {
        for (int i = 0; i < stepAnimationList.Count; i++)
        {

            if (stepAnimationList[i].BeginOrEnd == BeginOrEnd)
            {
                Debug.Log("动画名称:" +stepAnimationList[i].PlayAnimation);
                photonView.RPC("Play", PhotonTargets.AllBuffered, stepAnimationList[i].Path, stepAnimationList[i].PlayAnimation);
            }
        }
       
    }
    [PunRPC]
    void Play(string goPath, string animationName) {
        GetGameObjectAnimation(goPath).GetComponent<Animator>().Play(animationName);
    }
    GameObject GetGameObjectAnimation(string goPath) {
        string goName = goPath.Substring(goPath.LastIndexOf('/')+1, goPath.Length- goPath.LastIndexOf('/')-1);
        GameObject ret = stepAnimationGameObjectList.Find(
            delegate (GameObject find) {
                return find.name == goName;
            }
        );
        if (ret == null) {
            ret = GameObject.Find(goPath);
            stepAnimationGameObjectList.Add(ret);
        }
        return ret;
    }
}
public struct StepAnimation {
    public string Path;
    public string PlayAnimation;
    public string BeginOrEnd;
}
