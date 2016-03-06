using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateBuild : MonoBehaviour {

    // Reference to the singleton GameManager.
    public GameManager gameManager;

    List<GameObject> VHousesPlayer;

    // Build menu variables.
    bool isBuilding;
    GameObject ObjectPrebuild;
    Vector3 PrebuildPos;
    int buildType;
    GameObject GUIBuild;

    // Public functions.
    public void buildSpawn () {
        if (buildType == 0) {
			GameObject villageHouse = cleanSpawnObject ("VillagerHousePlayer", PrebuildPos);
            VHousesPlayer.Add(villageHouse);
            villageHouse.GetComponent<VillageHouse>().gameStateBuild = this;
        }
        buildCancel();
    }

    public void buildCancel () {
        // Exit the build menu.
        isBuilding = false;
        Destroy(ObjectPrebuild);
		setBuildMenuGUI(false);
    }

    public void buildNext () {
        if (buildType == 2)
            buildType = 0;
        else
            buildType = buildType + 1;
        print("Build Type: " + buildType);
    }

    public void buildPrev () {
        if (buildType == 0)
            buildType = 2;
        else
            buildType = buildType - 1;
        print("Build Type: " + buildType);
    }

    public void onPress () {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000f);
        Vector3 position = hit.point;

        // Hacky way of making buildings spawn only on the X-Z plane.
        if (position.y == 0.0f) {
            if (!isBuilding) {
                buildMenu(position);
            }
        }
    }

    // Use this for initialization
    void Awake () {
        // Set references.
        VHousesPlayer = gameManager.VHousesPlayer;
        GUIBuild = gameManager.GUIBuild;
        buildType = 0;

        gameManager.ImageTarget.transform.position = new Vector3(25.0f, 0, 25.0f);
        gameManager.MainCamera.transform.position = new Vector3(25.0f, 20.0f, -30.0f);
    }

    void OnEnable () {
        gameManager.MainCamera.transform.position = new Vector3(25.0f, 20.0f, -30.0f);
        isBuilding = false;
		setBuildMenuGUI(false);
    }

    // Update is called once per frame
    void Update () {

    }

    // Private functions
    void buildMenu (Vector3 position) {
        isBuilding = true;
        PrebuildPos = position;
		ObjectPrebuild = cleanSpawnObject("HousePrebuild", PrebuildPos);
		setBuildMenuGUI (true);
    }

	// Clean the spawn point so that buildings spawn on an even grid.
	GameObject cleanSpawnObject (string resource, Vector3 position) {
		float x = Mathf.Floor((position.x + 2.5f) / 5.0f) * 5.0f;
		float y = position.y;
		float z = Mathf.Floor((position.z + 2.5f) / 5.0f) * 5.0f;
		GameObject result = (GameObject) Instantiate(
			Resources.Load(resource),
			new Vector3(x, y, z),
			new Quaternion(0.0f, 180.0f, 0.0f, 0.0f)
		);
		return result;
	}

	void setBuildMenuGUI (bool mode) {
		GUIBuild.transform.GetChild(0).gameObject.SetActive(mode);
		GUIBuild.transform.GetChild(1).gameObject.SetActive(!mode);
	}
}
