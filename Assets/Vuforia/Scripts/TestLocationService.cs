using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestLocationService : MonoBehaviour
{
	Text GPS_text;
	IEnumerator Start()
	{
		GPS_text = GameObject.Find("GPS").GetComponent<Text>();
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser) {
			GPS_text.text = "Not Enabled";
			//print("Not Enabled");
			yield break;
		}

		// Start service before querying location
		Input.location.Start();

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
			//print("Timed out");
			GPS_text.text = "Timed out";
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			GPS_text.text = "Unable to determine device location";
			//print("Unable to determine device location");
			yield break;
		}
		else
		{
			// Access granted and location value could be retrieved
			GPS_text.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
			//print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
		}

		// Stop service if there is no need to query location updates continuously
		//Input.location.Stop();
	}
}

