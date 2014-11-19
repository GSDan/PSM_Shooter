using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ABundlesExample : MonoBehaviour {

	private List<string> bundles = new List<string>();

	private void Start()
	{
		bundles.Add ("Cube.prefab");
		bundles.Add ("Sphere.prefab");
		bundles.Add ("Cylinder.prefab");
		bundles.Add ("Complex.prefab");
		bundles.Add ("SceneBundle.unity3d");
	}

	private string CreateURL(string name)
	{
		return "file://" + Application.streamingAssetsPath + "/" + name;	
//		return "http://" + host + "/StreamingAssets/" + name;	
	}

	private IEnumerator DownloadAssetBundle(string name, int crc)
	{
		WWW www = WWW.LoadFromCacheOrDownload(CreateURL(name), crc/*Random.Range (1,12234)*/);

		yield return www;

		if (www.error!=null)
		{
			Debug.Log ("ERROR: "+www.error);
			yield return 0;
		}

		AssetBundle ab = www.assetBundle;

		if(ab!=null)
		{
			Object obj = (Object) ab.mainAsset;
			GameObject pGO = (GameObject) obj;
			if (pGO == null)
				Application.LoadLevelAdditive(System.IO.Path.GetFileNameWithoutExtension(name));
			else
				Instantiate(pGO,new Vector3(.0f,.0f,.0f),pGO.transform.rotation);
		}
		else
		{
			Debug.Log ("Bundle is null...");
		}
	}

	private void OnGUI()
	{
		for(int i=0;i<bundles.Count;i++)
		{
			if(GUILayout.Button("LoadFromCacheOrDownload( " + bundles[i] + " )"))
			{
				Destroy(GameObject.Find ("Cube"));
				StartCoroutine(DownloadAssetBundle(bundles[i],i));
			}
			bool cached = Caching.IsVersionCached(CreateURL(bundles[i]), i);
			GUI.enabled = cached;
			if(GUILayout.Button("Caching.MarkAsUsed( " + bundles[i] + " )"))
			{
				GUILayout.Label("Caching.MarkAsUsed(" + bundles[i] + ", " + i + ") = " + Caching.MarkAsUsed(CreateURL(bundles[i]), i));

			}
			GUI.enabled = true;
			GUILayout.Label("Caching.IsVersionCached(" + bundles[i] + ", " + i + ") = " + cached);
		}

		if (GUILayout.Button("Caching.CleanCache"))
			Caching.CleanCache();

		GUILayout.Label("-------------------------");

		GUILayout.Label("Caching.enabled = " +Caching.enabled);
		GUILayout.Label("Caching.expirationDelay = " +Caching.expirationDelay);
		GUILayout.Label("Caching.maximumAvailableDiskSpace = " +Caching.maximumAvailableDiskSpace);
		GUILayout.Label("Caching.ready = " +Caching.ready);
		GUILayout.Label("Caching.spaceFree = " +Caching.spaceFree);
		GUILayout.Label("Caching.spaceOccupied = " +Caching.spaceOccupied);
	}
}
