using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SuperExporter : MonoBehaviour {

	[MenuItem ("Assets/Build all Prefab AssetBundles PSM")]
	public static void Export()
	{
		ArrayList items = new ArrayList();
		string pathTO = Application.streamingAssetsPath+"/";
		string pathFROM = "Assets/Prefabs/";
		
		items.Add("Cube.prefab");
		items.Add("Sphere.prefab");
		items.Add ("Cylinder.prefab");
		items.Add ("Complex.prefab");
		
		for(int i=0;i<items.Count;i++)
		{
			string filepath = pathFROM+items[i];
			Debug.Log(filepath);
			Object obj = UnityEditor.AssetDatabase.LoadMainAssetAtPath(filepath);
			Debug.Log("OBJ: "+obj);
			Object[] objs = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(filepath);
			BuildPipeline.BuildAssetBundle(obj, objs, pathTO+Path.GetFileNameWithoutExtension(items[i]+".unity3d"), BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.PSM);
		}

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	[MenuItem ("Assets/Build export SceneBundle PSM")]
	public static void SceneBundle()
	{
		string pathTO = Application.streamingAssetsPath+"/";
		string[] levels = new string[] { "Assets/ExportScenes/SceneBundle.unity" };
		BuildPipeline.BuildStreamedSceneAssetBundle( levels, pathTO+Path.GetFileNameWithoutExtension(levels[0])+".unity3d", BuildTarget.PSM, BuildOptions.BuildAdditionalStreamedScenes | BuildOptions.UncompressedAssetBundle); 
	}

}
