using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateGather : MonoBehaviour {
	Text GPS_dest;
	Text GPS_curr;
	RawImage brick;
	RawImage rock;
	RawImage log;
	GameObject collect;
	int item = Random.Range(1, 4);
	float lat = Random.Range(8F, 10F);
	float longitude = Random.Range(8F, 10F);
	float curr_lat;
	float curr_long;
	float range = 0.0001f;
	bool collected = false;
	public Text ore_count;
	public Text log_count;
	public Text brick_count;
	public Text rand_amt;
	int amt = 0;


	IEnumerator Start () {
		// Hacky workaround fix

		SaveManager.GameNew();
		SaveManager.GameDataSave.printData();

		GPS_dest = GameObject.Find("GPS_dest").GetComponent<Text>();
		GPS_curr = GameObject.Find("GPS_curr").GetComponent<Text>();

		brick = GameObject.Find ("brick").GetComponent<RawImage> ();
		rock = GameObject.Find ("rock").GetComponent<RawImage> ();
		log = GameObject.Find ("log").GetComponent<RawImage> ();
		collect = GameObject.Find ("collect");

		ore_count.text = "Ore: " + SaveManager.GameDataSave.numOre.ToString() + "x";
		brick_count.text = "Brick: " + SaveManager.GameDataSave.numBrick.ToString() + "x";
		log_count.text = "Wood: " + SaveManager.GameDataSave.numWood.ToString() + "x";
		brick.enabled = false;
		rock.enabled = false;
		log.enabled = false;
		collect.SetActive(false);
		rand_amt.enabled = false;

		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser) {
			GPS_dest.text = "Not Enabled";
			GPS_curr.text = "Not Enabled";
			//print("Not Enabled");
			yield break;
		}

		// Start service before querying location
		Input.location.Start(1f, 0.1f);

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			GPS_dest.text = "Timed out";
			GPS_curr.text = "Timed out";
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			GPS_dest.text = "Unable to determine device location";
			GPS_curr.text = "Unable to determine device location";
			yield break;
		} else {
			lat /= 100000;
			longitude /= 10000000;
			lat = lat + Input.location.lastData.latitude;
			longitude = longitude + Input.location.lastData.longitude;
			// Access granted and location value could be retrieved
			curr_lat = Input.location.lastData.latitude;
			curr_long = Input.location.lastData.longitude;

			GPS_curr.text = "Current Location: " + curr_lat + " " + curr_long;
			GPS_dest.text = "Target Location: " + lat + " " + longitude;
		}
			
		// Stop service if there is no need to query location updates continuously
		//Input.location.Stop();
	}


	IEnumerator Location () {
		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			GPS_curr.text = "Unable to determine device location";
			yield break;
		} else {
			curr_lat = Input.location.lastData.latitude;
			curr_long = Input.location.lastData.longitude;
			if (!collected) {
				GPS_curr.text = "Current Location: " + curr_lat + " " + curr_long;
				if ((curr_lat < lat + range && curr_lat > lat - range) && (curr_long < longitude + range && curr_long > longitude - range)) {
					GPS_curr.text = "YOU MADE IT!!!!!!";
					collected = true;
					if (item == 1)
						brick.enabled = true;
					else if (item == 2)
						rock.enabled = true;
					else
						log.enabled = true;

					amt = Random.Range(1, 4);
					rand_amt.text = "x" + amt.ToString ();
					rand_amt.enabled = true;
					collect.SetActive (true);
				} else {
					brick.enabled = false;
					rock.enabled = false;
					log.enabled = false;
					rand_amt.enabled = false;
					collect.SetActive (false);
				}
			}
		}
	}

	public void collectItem () {
		if (item == 1) {
			SaveManager.GameDataSave.numBrick += amt;
			GPS_curr.text = SaveManager.GameDataSave.numBrick.ToString();
		} else if (item == 2) {
			SaveManager.GameDataSave.numOre += amt;
			GPS_curr.text = SaveManager.GameDataSave.numOre.ToString();
		} else {
			SaveManager.GameDataSave.numWood += amt;
			GPS_curr.text = SaveManager.GameDataSave.numWood.ToString();
		}
		ore_count.text = "Ore: " + SaveManager.GameDataSave.numOre.ToString() + "x";
		brick_count.text = "Brick: " + SaveManager.GameDataSave.numBrick.ToString() + "x";
		log_count.text = "Wood: " + SaveManager.GameDataSave.numWood.ToString() + "x";
		collected = false;
		lat = Random.Range(12F, 15F)/100000;
		longitude = Random.Range(12F, 15F)/10000000;
		lat = lat + Input.location.lastData.latitude;
		longitude = longitude + Input.location.lastData.longitude;
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine(Location());
	}
}

