using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateBuild : MonoBehaviour {

    // Public variables.
    public GameObject GUIBuild;

    // Private variables.
    List<GameObject> VHousesPlayer;
    bool isBuilding;
    GameObject ObjectPrebuild;
    Vector3 PrebuildPos;
    int buildType;

    // Public functions.
    public void buildSpawn () {
        if (buildType == 0 && enoughResources(buildType)) {
            GameObject villageHouse = cleanSpawnObject("VillagerHousePlayer", PrebuildPos);
            VHousesPlayer.Add(villageHouse);
            villageHouse.GetComponent<VillageHouse>().gameStateBuild = this;
            SaveManager.GameDataSave.numWood -= 1;
            SaveManager.GameDataSave.numBrick -= 3;
            SaveManager.GameDataSave.numOre -= 2;
            saveBuildingProperties(villageHouse.transform.position, "VillagerHousePlayer");
        } else {
            GUIBuild.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(GUIDisableOverTime(3.0f));
            return;
        }
        buildCancel();
        SaveManager.GameSave();
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
    }

    public void buildPrev () {
        if (buildType == 0)
            buildType = 2;
        else
            buildType = buildType - 1;
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
        buildType = 0;
        VHousesPlayer = new List<GameObject>();

        // Disable the build GUI, for now.
        GUIBuild.SetActive(false);

        GameManager.GameManagerInstance.ImageTarget.transform.position = new Vector3(25.0f, 0, 25.0f);
        GameManager.GameManagerInstance.MainCamera.transform.position = new Vector3(25.0f, 20.0f, -30.0f);
    }

    void OnEnable () {
        isBuilding = false;
        for (int i = 0; i < SaveManager.GameDataSave.buildingNum; i++) {
            GameObject house = (GameObject) Instantiate(
                    Resources.Load(SaveManager.GameDataSave.buildingName[i]),
                    new Vector3(SaveManager.GameDataSave.buildingPos[i][0], SaveManager.GameDataSave.buildingPos[i][1], SaveManager.GameDataSave.buildingPos[i][2]),
                    new Quaternion(0.0f, 180.0f, 0.0f, 0.0f)
            );
            VHousesPlayer.Add(house);
        }
        foreach (GameObject house in VHousesPlayer) {
            VillageHouse houseScript = house.GetComponent<VillageHouse>();
            houseScript.gameStateBuild = this;
        }
        GameManager.GameManagerInstance.MainCamera.transform.position = new Vector3(25.0f, 20.0f, -30.0f);

        // Enable GUI elements.
        GUIBuild.SetActive(true);
        setBuildMenuGUI(false);
    }

    // Private functions
    void buildMenu (Vector3 position) {
        isBuilding = true;
        PrebuildPos = position;
		ObjectPrebuild = cleanSpawnObject("HousePrebuild", PrebuildPos);
		setBuildMenuGUI(true);
        GUIBuild.transform.GetChild(1).gameObject.SetActive(false);
    }

    bool enoughResources (int buildType) {
        if (buildType == 0 && SaveManager.GameDataSave.numWood - 1 > 0 &&
                SaveManager.GameDataSave.numBrick - 3 > 0 &&
                SaveManager.GameDataSave.numOre - 2 > 0) {
            return true;
        } else {
            return false;
        }
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
        GUIBuild.transform.GetChild(1).gameObject.SetActive(mode);
        GUIBuild.transform.GetChild(2).gameObject.SetActive(!mode);
    }

    void saveBuildingProperties (Vector3 pos, string name) {
        SaveManager.GameDataSave.buildingNum++;
        SaveManager.GameDataSave.buildingPos.Add(new float[3] { pos.x, pos.y, pos.z });
        SaveManager.GameDataSave.buildingName.Add(name);
    }

    IEnumerator GUIDisableOverTime (float time) {
        yield return new WaitForSeconds(time);
        GUIBuild.transform.GetChild(1).gameObject.SetActive(false);
    }

    // Do things when this MonoBehavior is disabled.
    void OnDisable() {
        // Remove the village houses.
        foreach (GameObject house in VHousesPlayer) {
            Destroy(house);
        }
        VHousesPlayer.Clear();
        // Disable GUI elements.
        if (GUIBuild) {
            GUIBuild.SetActive(false);
        }
    }
}
