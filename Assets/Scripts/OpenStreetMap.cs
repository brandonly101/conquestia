using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenStreetMap : MonoBehaviour {

	public float latitude = 34.070068f;
	public float longitude = -118.453298f;
	public int zoom = 17;

	int row = 5;
	int col = 7;

	public GameObject[][] test;
	public GameObject[] GUIImageRowLower;
	public GameObject[] GUIImageRowLow;
	public GameObject[] GUIImageRowMid;
	public GameObject[] GUIImageRowHigh;
	public GameObject[] GUIImageRowHigher;

	// Use this for initialization
	void Start () {
		StartCoroutine(GetTiles(latitude, longitude, zoom));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator GetTiles(float lat, float lon, int z) {
		Vector2 pos = WorldToTilePos(lat, lon, z);
		WWW www;
		int x = (int)pos.x;
		int y = (int)pos.y;

		int ro = y - 2;
		int co = x - 3;

		for (int j = 0; j < 7; j++) {
			string url = "http://a.tile.openstreetmap.org/" +
				z.ToString() + "/" +
				(co + j).ToString () + "/" +
				(ro + 1).ToString () + ".png";
			www = new WWW(url);
			yield return www;
			GUIImageRowLow[j].GetComponent<RawImage>().texture = www.texture;
			GUIImageRowLow[j].GetComponent<RawImage>().SetNativeSize();
		}

		for (int j = 0; j < 7; j++) {
			string url = "http://a.tile.openstreetmap.org/" +
				z.ToString() + "/" +
				(co + j).ToString () + "/" +
				(ro + 2).ToString () + ".png";
			www = new WWW(url);
			yield return www;
			GUIImageRowMid[j].GetComponent<RawImage>().texture = www.texture;
			GUIImageRowMid[j].GetComponent<RawImage>().SetNativeSize();
		}

		for (int j = 0; j < 7; j++) {
			string url = "http://a.tile.openstreetmap.org/" +
				z.ToString() + "/" +
				(co + j).ToString () + "/" +
				(ro + 3).ToString () + ".png";
			www = new WWW(url);
			yield return www;
			GUIImageRowHigh[j].GetComponent<RawImage>().texture = www.texture;
			GUIImageRowHigh[j].GetComponent<RawImage>().SetNativeSize();
		}
	}

	Vector2 WorldToTilePos(float lat, float lon, int zoom) {
		float x = ((lon + 180.0f) / 360.0f * (1 << zoom));
		float y = ((1.0f - Mathf.Log(Mathf.Tan(lat * Mathf.PI / 180.0f) + 
			1.0f / Mathf.Cos(lat * Mathf.PI / 180.0f)) / Mathf.PI) / 2.0f * (1 << zoom));

		return new Vector2(x, y);
	}

//	Vector2 TileToWorldPos(double tile_x, double tile_y, int zoom) {
//		double n = Mathf.PI - ((2.0 * Mathf.PI * tile_y) / Mathf.Pow(2.0, zoom));
//
//		float x = (float) ((tile_x / Mathf.Pow(2.0, zoom) * 360.0) - 180.0);
//		float y = (float) (180.0 / Mathf.PI * Mathf.Atan(Mathf.Sin(n)));
//
//		return new Vector2(x, y);
//	}
}
