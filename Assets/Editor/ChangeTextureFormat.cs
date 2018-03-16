using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ChangeTextureFormat : EditorWindow
{
    // Use this for initialization
    void Start () {
        

    }
    [MenuItem("MyMenu/ChangeTextureFormat")]
    static void SavePrefabs_()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 340, 220);
        ChangeTextureFormat window = (ChangeTextureFormat)EditorWindow.GetWindowWithRect(typeof(ChangeTextureFormat), wr, true, "ChangeTextureFormat");
        window.Show();

    }
   
    static string selectPath;
    Rect rect;
    void OnGUI()
    {
        EditorGUILayout.LabelField("第一步拖拽Project下的目录到下面填写框中，用于获取路径");
        //获得一个长300的框
        rect = EditorGUILayout.GetControlRect(GUILayout.Width(300));
        //将上面的框作为文本输入框
        selectPath = EditorGUI.TextField(rect, selectPath);
        //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内
        if ((Event.current.type == EventType.DragUpdated
        || Event.current.type == EventType.DragExited)
        && rect.Contains(Event.current.mousePosition))
        {
            //改变鼠标的外表
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            {
                selectPath = DragAndDrop.paths[0];
            }
        }
        EditorGUILayout.LabelField("第二步，点击按钮转换路径中所有的jpg、png、tga文件的格式");
        if (GUILayout.Button("修改贴图格式", GUILayout.Width(200)))
        {
            LoopSetTexture();
        }
    }
    private static void LoopSetTexture()
    {
        string[] fileInfo = GetTexturePath();
        for (int i = 0; i < fileInfo.Length; i++)
        {
            //获取资源路径
            string path = fileInfo[i];
            SetTextureSettings(path);
            AssetDatabase.ImportAsset(path);
        }
    }
    /// <summary>
    /// 获取贴图设置
    /// </summary>
    public static TextureImporter SetTextureSettings(string path)
    {
       
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        Debug.Log(textureImporter);
        //Texture Type
        textureImporter.textureType = TextureImporterType.Default;
     
        TextureImporterPlatformSettings TempTexture = new TextureImporterPlatformSettings();
        TempTexture.overridden = true;
        TempTexture.name = "Standalone";
        // TempTexture.maxTextureSize = 2048;
        TempTexture.format = TextureImporterFormat.DXT5Crunched;
        //TempTexture.crunchedCompression = true;
        TempTexture.compressionQuality = 50;
        //PlatformTextureSettings
        textureImporter.SetPlatformTextureSettings(TempTexture);
        return textureImporter;
    }
 
    /// <summary>
    /// 获取图片的路径
    /// </summary>
    /// <returns></returns>
    private static string[] GetTexturePath()
    {
        //jpg
        ArrayList jpgList = GetFilePath("*.jpg");
        int jpgLength = jpgList.Count;
        //png
        ArrayList pngList = GetFilePath("*.png");
        int pngLength = pngList.Count;
        //tga
        ArrayList tgaList = GetFilePath("*.tga");
        int tgaLength = tgaList.Count;
        string[] filePath = new string[jpgLength + pngLength + tgaLength];
        for (int i = 0; i < jpgLength; i++)
        {
            filePath[i] = jpgList[i].ToString();
        }
        for (int i = 0; i < pngLength; i++)
        {
            filePath[i + jpgLength] = pngList[i].ToString();
        }
        for (int i = 0; i < tgaLength; i++)
        {
            filePath[i + jpgLength + pngLength] = tgaList[i].ToString();
        }
        return filePath;
    }
    /// <summary>
    /// 获取指定后掇后的文件路径
    /// </summary>
    /// <param name="fileType"></param>
    /// <returns></returns>
    private static ArrayList GetFilePath(string fileType)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(selectPath);
        ArrayList filePath = new ArrayList();
        foreach (FileInfo fi in directoryInfo.GetFiles(fileType, SearchOption.AllDirectories))
        {
            string fileFullName = fi.FullName;
            string path = fileFullName.Substring(fileFullName.LastIndexOf("Assets\\"));
           // Debug.Log(path2);
           // string path = selectPath + "\\" + fi.Name;
            filePath.Add(path);
        }
        return filePath;
    }


}
