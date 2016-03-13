using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;

public class GameStateGather : MonoBehaviour, ICloudRecoEventHandler {

	public GameObject GUIGather;
	public GameObject GUICurrRes;
	public GameObject GUICollect;
	public GameObject GUIAgain;
	public Text GUISuccess;
	public Text GUISearching;
	public RawImage GUIWood;
	public RawImage GUIBrick;
	public RawImage GUIOre;
	public Text GUIAmount;
	public ImageTargetBehaviour ImageTargetTemplate;
	public GameObject Terrain;
	public CloudRecoBehaviour mCloudRecoBehaviour;

	bool mIsScanning = false;
	string mTargetMetadata = "";
	bool mShowGUIButton = false;
	int item = 1;
	int amt = 0;
	bool collected = false;

	// Public functions.
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
		if (item == 1) {
			SaveManager.GameDataSave.numBrick += amt;
		} else if (item == 2) {
			SaveManager.GameDataSave.numOre += amt;
		} else {
			SaveManager.GameDataSave.numWood += amt;
		}
		collected = true;
		GUIAgain.SetActive(true);
		GUICollect.SetActive(false);
		GUIBrick.enabled = false;
		GUIOre.enabled = false;
		GUIWood.enabled = false;
		GUIAmount.enabled = false;
		GUISuccess.enabled = true;

		// Update the current resources GUI.
		GUICurrRes.transform.GetChild(0).gameObject.GetComponent<Text>().text =
			"Wood: " + SaveManager.GameDataSave.numWood + "x\n\n" +
			"Brick: " + SaveManager.GameDataSave.numBrick + "x\n\n" +
			"Ore: " + SaveManager.GameDataSave.numOre + "x";
	}

	public void onPressAgain () {
		collected = false;
		GUIAgain.SetActive(false);
		mCloudRecoBehaviour.CloudRecoEnabled = true;
		GUISearching.enabled = true;
		GUISuccess.enabled = false;
		item = Random.Range(1, 4);
		amt = Random.Range(1, 4);
		GUIAmount.text = "x " + amt.ToString ();
		SaveManager.GameSave();
	}

	void Awake () {
		// register this event handler at the cloud reco behaviour
		if (mCloudRecoBehaviour) {
			mCloudRecoBehaviour.RegisterEventHandler(this);
		}

		GUIGather.SetActive(false);
	}

	void OnEnable () {
		// clear all known trackables
		ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
		tracker.TargetFinder.ClearTrackables(false);
		mIsScanning = true;
		mCloudRecoBehaviour.CloudRecoEnabled = true;
		item = Random.Range(1, 4);
		amt = Random.Range(1, 4);
		GUIAmount.text = "x " + amt.ToString ();
		Terrain.SetActive(false);

		GUIGather.SetActive(true);
		GUICurrRes.SetActive (true);
		GUICollect.SetActive(false);
		GUIAgain.SetActive(false);
		GUISearching.enabled = true;
		GUISuccess.enabled = false;
		GUIAmount.enabled = false;
		GUIBrick.enabled = false;
		GUIOre.enabled = false;
		GUIWood.enabled = false;

		// Update the current resources GUI.
		GUICurrRes.transform.GetChild(0).gameObject.GetComponent<Text>().text =
			"Wood: " + SaveManager.GameDataSave.numWood + "x\n\n" +
			"Brick: " + SaveManager.GameDataSave.numBrick + "x\n\n" +
			"Ore: " + SaveManager.GameDataSave.numOre + "x";
	}

	void Update () {
		if (!mIsScanning) {
			if (!collected) {
				GUICollect.SetActive(true);
				GUIAmount.enabled = true;
				if (item == 1)
					GUIBrick.enabled = true;
				else if (item == 2)
					GUIOre.enabled = true;
				else if (item == 3)
					GUIWood.enabled = true;
			} else {
				GUICollect.SetActive(false);
				GUIBrick.enabled = false;
				GUIOre.enabled = false;
				GUIWood.enabled = false;
			}
			GUISearching.enabled = false;
		} else {
			GUICollect.SetActive(false);
			GUIBrick.enabled = false;
			GUIOre.enabled = false;
			GUIWood.enabled = false;
		}
	}

	void OnDisable () {
		GUIGather.SetActive(false);
		if (Terrain) {
			Terrain.SetActive (true);
		}
	}
}

