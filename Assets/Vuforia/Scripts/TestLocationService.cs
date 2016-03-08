using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestLocationService : MonoBehaviour
{
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
	float range = .0001f;

	IEnumerator Start()
	{
		GPS_dest = GameObject.Find("GPS_dest").GetComponent<Text>();
		GPS_curr = GameObject.Find("GPS_curr").GetComponent<Text>();

		brick = GameObject.Find ("brick").GetComponent<RawImage> ();
		rock = GameObject.Find ("rock").GetComponent<RawImage> ();
		log = GameObject.Find ("log").GetComponent<RawImage> ();
		collect = GameObject.Find ("collect");

		brick.enabled = false;
		rock.enabled = false;
		log.enabled = false;
		collect.SetActive(false);


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
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			
			GPS_dest.text = "Timed out";
			GPS_curr.text = "Timed out";
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			GPS_dest.text = "Unable to determine device location";
			GPS_curr.text = "Unable to determine device location";

			yield break;
		}
		else
		{
			
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

			GPS_curr.text = "Current Location: " + curr_lat + " " + curr_long;

			if ((curr_lat < lat + range && curr_lat > lat - range) && (curr_long < longitude + range && curr_long > longitude - range)) {
				GPS_curr.text = "YOU MADE IT!!!!!!";
				if (item == 1)
					brick.enabled = true;
				else if (item == 2)
					rock.enabled = true;
				else
					log.enabled = true;
				collect.SetActive(true);
			} else {
				brick.enabled = false;
				rock.enabled = false;
				log.enabled = false;
				collect.SetActive(false);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		StartCoroutine(Location());
	}
}

