using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;


public class SimpleCloudHandler : MonoBehaviour, ICloudRecoEventHandler {

	public GameObject GUICollect;
	public GameObject GUIAgain;
	public Text GUISuccess;
	public Text GUISearching;
	public RawImage brick;
	public RawImage rock;
	public RawImage log;
	public Text amt_text;

	public ImageTargetBehaviour ImageTargetTemplate;
	private CloudRecoBehaviour mCloudRecoBehaviour;
	private bool mIsScanning = false;
	private string mTargetMetadata = "";
	private Rect buttonRect = new Rect(50,50,120,60);
	private bool mShowGUIButton = false;
	int item = 1;
	int amt = 0;
	bool collected = false;
	// Use this for initialization
	void Start () {
		// register this event handler at the cloud reco behaviour
		mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
		if (mCloudRecoBehaviour)
		{
			mCloudRecoBehaviour.RegisterEventHandler(this);
		}

		GUICollect.SetActive(false);
		GUIAgain.SetActive(false);
		GUISearching.enabled = true;
		GUISuccess.enabled = false;
		amt_text.enabled = false;
		mIsScanning = true;
		brick.enabled = false;
		rock.enabled = false;
		log.enabled = false;
		item = Random.Range(1, 4);
		amt = Random.Range(1, 4);
		amt_text.text = "x " + amt.ToString ();
	}

	public void OnInitialized() {
		Debug.Log ("Cloud Reco initialized");
	}
	public void OnInitError(TargetFinder.InitState initError) {
		Debug.Log ("Cloud Reco init error " + initError.ToString());
	}
	public void OnUpdateError(TargetFinder.UpdateState updateError) {
		Debug.Log ("Cloud Reco update error " + updateError.ToString());
	}

	public void OnStateChanged(bool scanning) {
		mIsScanning = scanning;
		if (scanning)
		{
			// clear all known trackables
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			tracker.TargetFinder.ClearTrackables(false);
		}
	}

	// Here we handle a cloud target recognition event
	public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult) {
		// do something with the target metadata
		//mTargetMetadata = targetSearchResult.MetaData;
		mShowGUIButton = true;

		// stop the target finder (i.e. stop scanning the cloud)


		mCloudRecoBehaviour.CloudRecoEnabled = false;


		// Build augmentation based on target
		if (ImageTargetTemplate) {
			// enable the new result with the same ImageTargetBehaviour:
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			ImageTargetBehaviour imageTargetBehaviour =
				(ImageTargetBehaviour)tracker.TargetFinder.EnableTracking(
					targetSearchResult, ImageTargetTemplate.gameObject);
		}
	}

	public void onPressCollect () {
//		if (item == 1) {
//			SaveManager.GameDataSave.numBrick += amt;
//		} else if (item == 2) {
//			SaveManager.GameDataSave.numOre += amt;
//		} else {
//			SaveManager.GameDataSave.numWood += amt;
//		}
		collected = true;
		GUIAgain.SetActive(true);
		GUICollect.SetActive(false);
		brick.enabled = false;
		rock.enabled = false;
		log.enabled = false;
		amt_text.enabled = false;
		// Do stuff to add resources
		GUISuccess.enabled = true;
	}

	public void onPressAgain () {
		collected = false;
		GUIAgain.SetActive(false);
		mCloudRecoBehaviour.CloudRecoEnabled = true;
		GUISearching.enabled = true;
		GUISuccess.enabled = false;
		item = Random.Range(1, 4);
		amt = Random.Range(1, 4);
		amt_text.text = "x " + amt.ToString ();
	}

	void Update () {
		if (!mIsScanning) {
			if (!collected) {
				amt_text.enabled = true;
				if (item == 1)
					brick.enabled = true;
				else if (item == 2)
					rock.enabled = true;
				else if (item == 3)
					log.enabled = true;
			}
			else {
				brick.enabled = false;
				rock.enabled = false;
				log.enabled = false;
			}
			GUICollect.SetActive(true);
			GUISearching.enabled = false;
		} else {
			GUICollect.SetActive(false);
			brick.enabled = false;
			rock.enabled = false;
			log.enabled = false;
		}
	}

	void OnGUI() {
		// Display current 'scanning' status
//		GUI.Box (new Rect(100,100,200,50), mIsScanning ? "Scanning" : "Not scanning");
//		// Display metadata of latest detected cloud-target
//		GUI.Box (new Rect(100,200,200,50), "Metadata: " + mTargetMetadata);
		// If not scanning, show button
		// so that user can restart cloud scanning
//		if (!mIsScanning) {
//			//draw collect GUI Button
//			if (GUI.Button (buttonRect, "Collect")) {
//				//do something on button click
//
//
//				if (GUI.Button(new Rect(100,300,200,50), "Restart Scanning")) {
//					// Restart TargetFinder
//					mCloudRecoBehaviour.CloudRecoEnabled = true;
//				}
//			}
//
//		}
	}
}

