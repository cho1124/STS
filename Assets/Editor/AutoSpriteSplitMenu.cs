using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

///<summary>
///변환할 스프라이트를 선택하면 항목에 추가됩니다.
///</summary>
public class AutoSpriteSplitMenu : EditorWindow
{
    int width = 32;
    int height = 32;
    int PixelsPerUnit = 16;
    List<Texture2D> spritelist;
    string path;
    void AutoSplitSelectChanged()
    {
        spritelist.Clear();
        foreach (var asset in Selection.objects)
        {
            if (asset.GetType()==typeof(Texture2D))
                spritelist.Add((Texture2D)asset);
        }
    }

    // https://docs.unity3d.com/ScriptReference/MenuItem.html
    [MenuItem("Assets/Auto Sprites Split")]
    static void Init()
    {
        AutoSpriteSplitMenu window = (AutoSpriteSplitMenu)EditorWindow.GetWindow(typeof(AutoSpriteSplitMenu),false,"Auto Sprites Split");
        window.Show();
    }

    void OnEnable() {
        spritelist = new List<Texture2D>();
        if (Selection.count>0)
        {
            foreach (var asset in Selection.objects)
            {
                if (asset.GetType()==typeof(Texture2D))
                    spritelist.Add((Texture2D)asset);
            }
        }
        Selection.selectionChanged += AutoSplitSelectChanged;
    }

    void OnDisable()
    {
        Selection.selectionChanged -= AutoSplitSelectChanged;
    }

    void OnGUI()
    {
        
        width = EditorGUILayout.IntField("Width(px)", width);
        height = EditorGUILayout.IntField("Height(px)", height);
        PixelsPerUnit = EditorGUILayout.IntField("Pixels Per Unit", PixelsPerUnit);

        if (spritelist!=null)
        {
            EditorGUILayout.LabelField($"Texture2D ({spritelist.Count.ToString()})");
            for (int i=0; i<spritelist.Count; i++)
                spritelist[i] = EditorGUILayout.ObjectField(spritelist[i], typeof(Texture2D), allowSceneObjects:false) as Texture2D;
        }
        if( GUILayout.Button("Split") )
        {
            if (spritelist!=null && width!=0 && height!=0)
            {
                foreach (var r in spritelist)
                    spriteSplit(r, width, height);
            }
        }
    }

    public void spriteSplit(Texture2D texture, int width, int height)
    {
        if (texture.width%width!=0 || texture.height%height!=0)
        {
            Debug.Log("The cut size does not fit.");
            return;
        }
        var path = AssetDatabase.GetAssetPath(texture);
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = PixelsPerUnit;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        
        var textureSettings = new TextureImporterSettings();
		importer.ReadTextureSettings(textureSettings);
		textureSettings.spriteMeshType = SpriteMeshType.Tight;
		textureSettings.spriteExtrude = 0;

		importer.SetTextureSettings(textureSettings);

        var rect = InternalSpriteUtility.GenerateGridSpriteRectangles(texture, 
                                            new Vector2(0,0), new Vector2(width,height), new Vector2(0,0));
        var metas = new List<SpriteMetaData>();
        foreach (Rect r in rect)
        {
            var meta = new SpriteMetaData
            {
                // Set sprite naming rule.
                name = $"{texture.name}_{r.x/width}_{r.y/height}",
                rect = r,
                alignment = 0,
                pivot = new Vector2(0.5f,0.5f),
            };
            metas.Add(meta);
        }

        importer.spritesheet = metas.ToArray();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        Debug.Log($"{texture.name} Complete");
    }
}