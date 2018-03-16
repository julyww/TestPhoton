using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public class XMLLoad : MonoBehaviour {

    private static XMLLoad _XMLLoad;
    
    public static XMLLoad GetInstance()
    {
        return _XMLLoad;
    }
    private void Awake()
    {
       
        _XMLLoad = this;
    }
    
    public string stepName;

    public bool _IsLoad = false;

    // Use this for initialization

    private string path;
    //xml配置文件
    private XElement root;
    // Use this for initialization
    void Start()
    {       
        LoadAsset();               
    }

    public virtual void LoadAsset() {
        _IsLoad = true;
        path = Application.dataPath;
        path = path.Substring(0, path.LastIndexOf("/"));
        path = path + "/Config/";
        //////////////////////////////////////////////////
        root = XElement.Load(@path + stepName + ".xml");
        IEnumerable<XElement> elementCollection = root.Elements("TitleName");
        //http://blog.csdn.net/duanzi_peng/article/details/24018431        
        IEnumerable<XElement> elementSteps = root.Elements("Step");

        GetComponent<StepXmlManager>().SetStepList(elementSteps.ToList());

        //foreach (var item in elementSteps)
        //{
        //    Debug.Log(item);
        //}
        //IEnumerable<XElement> roles =  root.Element("Roles").Elements("Role");       
        //foreach (var item in roles)
        //{
        //    //Debug.Log(item);
        //}
    }
    public List<XElement> GetRoleList() {
        return root.Element("Roles").Elements("Role").ToList();
    }


    //加载配置文件
    IEnumerator LoadTextAsset()
    {
        WWW www = new WWW("file://" + path + stepName + ".xml");
        yield return www;
        Debug.Log(www.text);
        _IsLoad = true;
    }
}
