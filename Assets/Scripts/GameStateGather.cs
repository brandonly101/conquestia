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
	public GameObject ModelWood;
	public GameObject ModelBrick;
	public GameObject ModelOre;
	public Text GUIAmount;
	public ImageTargetBehaviour ImageTargetTemplate;
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
		if (scanning) {
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
			SaveManager.GameDataSave.numWood += amt;
		} else if (item == 2) {
			SaveManager.GameDataSave.numBrick += amt;
		} else {
			SaveManager.GameDataSave.numOre += amt;
		}
		collected = true;
		GUIAgain.SetActive(true);
		GUICollect.SetActive(false);
		ModelBrick.SetActive(false);
		ModelOre.SetActive(false);
		ModelWood.SetActive(false);
		GUIAmount.enabled = true;
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
		GUIAmount.enabled = false;
		item = Random.Range(1, 4);
		amt = Random.Range(1, 4);
		GUIAmount.text = amt.ToString() + "x";
		SaveManager.GameSave();
	}

	void Awake () {
		// register this event handler at the cloud reco behaviour
		if (mCloudRecoBehaviour) {
			mCloudRecoBehaviour.RegisterEventHandler(this);
		}
		OnStateChanged(false);
		GUIGather.SetActive(false);
	}

	void OnEnable () {
		// clear all known trackables
		OnStateChanged(true);
		mCloudRecoBehaviour.CloudRecoEnabled = true;
		item = Random.Range(1, 4);
		amt = Random.Range(1, 4);
		GUIAmount.text = amt.ToString() + "x";

		GUIGather.SetActive(true);
		GUICurrRes.SetActive (true);
		GUICollect.SetActive(false);
		GUIAgain.SetActive(false);
		GUISearching.enabled = true;
		GUISuccess.enabled = false;
		GUIAmount.enabled = false;
		ModelBrick.SetActive(false);
		ModelOre.SetActive(false);
		ModelWood.SetActive(false);

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
				GameObject resource;
				if (item == 1) {
					resource = ModelWood;
				} else if (item == 2) {
					resource = ModelBrick;
				} else {
					resource = ModelOre;
				}
				resource.SetActive(true);
				resource.transform.Rotate(Vector3.up * Time.deltaTime * 20.0f);
			} else {
				GUICollect.SetActive(false);
				ModelBrick.SetActive(false);
				ModelOre.SetActive(false);
				ModelWood.SetActive(false);
			}
			GUISearching.enabled = false;
		} else {
			GUICollect.SetActive(false);
			ModelBrick.SetActive(false);
			ModelOre.SetActive(false);
			ModelWood.SetActive(false);
		}
	}

	void OnDisable () {
		OnStateChanged(false);

		// Set GUI elements.
		GUIGather.SetActive(false);
	}
}

