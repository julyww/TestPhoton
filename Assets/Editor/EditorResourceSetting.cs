using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorResourceSetting : AssetPostprocessor
{

    //纹理导入之前调用，针对入到的纹理进行设置  
    public void OnPreprocessTexture0()
    {
        //string dirName = System.IO.Path.GetDirectoryName(assetPath);  
        //string folderStr = System.IO.Path.GetFileName(dirName);  

        Debug.Log("导入Texture图片前处理");
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        Debug.Log(textureImporter.assetPath);
        string FileName = System.IO.Path.GetFileName(assetPath);
        string[] FileNameArray = FileName.Split(new string[] { "_" }, System.StringSplitOptions.RemoveEmptyEntries);

        if (FileNameArray.Length >= 3)
        {
            Debug.Log("符合命名规范图片");
            textureImporter.mipmapEnabled = false;//关闭mipmap  
            //图片类型   
            textureImporter.textureType = TextureImporterType.Default;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            //图片PackingTag  
            textureImporter.spritePackingTag = FileNameArray[0];
            //图片尺寸  
            if (FileNameArray[1] == "bg")
            {
                Debug.Log("名称：" + textureImporter.GetDefaultPlatformTextureSettings().name);
                TextureImporterPlatformSettings TempTexture = new TextureImporterPlatformSettings();
                TempTexture.overridden = true;
                TempTexture.name = "PC";
                TempTexture.maxTextureSize = 512;
                TempTexture.format = TextureImporterFormat.ETC_RGB4;
                textureImporter.SetPlatformTextureSettings(TempTexture);
            }
            else if (FileNameArray[1] == "btn")
            {
                textureImporter.maxTextureSize = 256;
            }
            else if (FileNameArray[1] == "icon")
            {
                textureImporter.maxTextureSize = 64;
            }
            else
            {
                TextureImporterPlatformSettings TempTexture = new TextureImporterPlatformSettings();
                TempTexture.overridden = true;
                TempTexture.name = "Standalone";
                TempTexture.maxTextureSize = 2048;
                TempTexture.format = TextureImporterFormat.DXT5Crunched;
               // TempTexture.crunchedCompression = true;
                TempTexture.compressionQuality = 50;
                textureImporter.SetPlatformTextureSettings(TempTexture);
            }
            Debug.Log("名称：" + textureImporter.GetDefaultPlatformTextureSettings().name);

        }
        else
        {
            Debug.Log("不是指定文件");
        }
    }

    //纹理导入之前调用，针对入到的纹理进行设置  
    public void OnPreprocessTextureBak()
    {
        //string dirName = System.IO.Path.GetDirectoryName(assetPath);  
        //string folderStr = System.IO.Path.GetFileName(dirName);  

        Debug.Log("导入Texture图片前处理");
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        string FileName = System.IO.Path.GetFileName(assetPath);
        Debug.Log(FileName);
        string[] FileNameArray = FileName.Split(new string[] { "_" }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log(FileNameArray.Length);
        if (FileNameArray.Length >= 3)
        {
            Debug.Log("符合命名规范图片");
            textureImporter.mipmapEnabled = false;//关闭mipmap  
            //图片类型   
            textureImporter.textureType = TextureImporterType.Default;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            //图片PackingTag  
            textureImporter.spritePackingTag = FileNameArray[0];
            //图片尺寸  
            if (FileNameArray[1] == "bg")
            {
                Debug.Log("名称：" + textureImporter.GetDefaultPlatformTextureSettings().name);
                TextureImporterPlatformSettings TempTexture = new TextureImporterPlatformSettings();
                TempTexture.overridden = true;
                TempTexture.name = "PC";
                TempTexture.maxTextureSize = 512;
                TempTexture.format = TextureImporterFormat.ETC_RGB4;
                textureImporter.SetPlatformTextureSettings(TempTexture);
            }
            else if (FileNameArray[1] == "btn")
            {
                textureImporter.maxTextureSize = 256;
            }
            else if (FileNameArray[1] == "icon")
            {
                textureImporter.maxTextureSize = 64;
            }
            else
            {
                textureImporter.maxTextureSize = 1024;
                textureImporter.textureCompression = TextureImporterCompression.Compressed;
                textureImporter.crunchedCompression = true;
                textureImporter.compressionQuality = 60;
            }
            Debug.Log("名称：" + textureImporter.GetDefaultPlatformTextureSettings().name);

        }
        else
        {
            Debug.Log("不是指定文件");
        }
    }
}
