using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStateBuild : MonoBehaviour {

    // Public variables.
    public GameObject GUIBuild;

    // Private variables.
    List<GameObject> VHousesPlayer;
	List<GameObject> BuildPlayer;
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
            SaveManager.GameDataSave.numBrick -= 4;
            SaveManager.GameDataSave.numOre -= 1;
            saveBuildingProperties(villageHouse.transform.position, "VillagerHousePlayer");
        } else if (buildType == 1 && enoughResources(buildType)) {
            GameObject farm = cleanSpawnObject("Armory", PrebuildPos);
			SaveManager.GameDataSave.numWood -= 1;
			SaveManager.GameDataSave.numBrick -= 2;
			SaveManager.GameDataSave.numOre -= 3;
            saveBuildingProperties(farm.transform.position, "Armory");
		} else if (buildType == 2 && enoughResources(buildType)) {
			GameObject farm = cleanSpawnObject("Farm", PrebuildPos);
			SaveManager.GameDataSave.numWood -= 3;
			SaveManager.GameDataSave.numBrick -= 1;
			SaveManager.GameDataSave.numOre -= 1;
			saveBuildingProperties(farm.transform.position, "Farm");
		} else {
            GUIBuild.transform.GetChild(3).gameObject.SetActive(true);
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
		prebuildInstantiate();
		setBuildMenuGUI(true);
    }

    public void buildPrev () {
        if (buildType == 0)
            buildType = 2;
        else
            buildType = buildType - 1;
		prebuildInstantiate();
		setBuildMenuGUI(true);
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
		BuildPlayer = new List<GameObject>();

        // Disable the build GUI, for now.
        GUIBuild.SetActive(false);
    }

    void OnEnable () {
        isBuilding = false;
        for (int i = 0; i < SaveManager.GameDataSave.buildingNum; i++) {
            string resourceName = SaveManager.GameDataSave.buildingName[i];
            GameObject building = (GameObject) Instantiate(
                Resources.Load(resourceName),
                new Vector3(SaveManager.GameDataSave.buildingPos[i][0], SaveManager.GameDataSave.buildingPos[i][1], SaveManager.GameDataSave.buildingPos[i][2]),
                new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)
            );
			BuildPlayer.Add(building);
            if (resourceName == "VillagerHousePlayer") {
				VHousesPlayer.Add(building);
				VillageHouse buildingScript = building.GetComponent<VillageHouse>();
				buildingScript.gameStateBuild = this;
            }
        }
        foreach (GameObject house in VHousesPlayer) {
            VillageHouse houseScript = house.GetComponent<VillageHouse>();
            houseScript.gameStateBuild = this;
        }
		GameManager.instance.MainCamera.transform.position = new Vector3(-25.0f, 20.0f, -30.0f);

        // Enable GUI elements.
        GUIBuild.SetActive(true);
        setBuildMenuGUI(false);
    }

    // Private functions
    void buildMenu (Vector3 position) {
        isBuilding = true;
        PrebuildPos = position;
		prebuildInstantiate();
		setBuildMenuGUI(true);
    }

	void prebuildInstantiate () {
		Destroy(ObjectPrebuild);
		if (buildType == 0) {
			ObjectPrebuild = cleanSpawnObject("HousePrebuild", PrebuildPos);
		} else if (buildType == 1) {
			ObjectPrebuild = cleanSpawnObject("ArmoryPrebuild", PrebuildPos);
		} else {
			ObjectPrebuild = cleanSpawnObject("FarmPrebuild", PrebuildPos);
		}
	}

    bool enoughResources (int buildType) {
        if (buildType == 0 && SaveManager.GameDataSave.numWood - 1 > 0 &&
            SaveManager.GameDataSave.numBrick - 4 > 0 &&
            SaveManager.GameDataSave.numOre - 1 > 0) {
            return true;
        } else if (buildType == 1 && SaveManager.GameDataSave.numWood - 1 > 0 &&
			SaveManager.GameDataSave.numBrick - 2 > 0 &&
			SaveManager.GameDataSave.numOre - 3 > 0) {
            return true;
		} else if (buildType == 2 && SaveManager.GameDataSave.numWood - 3 > 0 &&
			SaveManager.GameDataSave.numBrick - 1 > 0 &&
			SaveManager.GameDataSave.numOre - 1 > 0) {
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
			new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)
		);
		return result;
	}

	void setBuildMenuGUI (bool mode) {
        GUIBuild.transform.GetChild(0).gameObject.SetActive(!mode);
		GUIBuild.transform.GetChild(1).gameObject.SetActive(true);
        GUIBuild.transform.GetChild(2).gameObject.SetActive(mode);
		GUIBuild.transform.GetChild(3).gameObject.SetActive(false);
		GUIBuild.transform.GetChild(4).gameObject.SetActive(!mode);

        // Set the text for the current level.
        GUIBuild.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level " + SaveManager.GameDataSave.GameLevel;

		// Set the text for the current resources.
		GameObject GUIResources = GUIBuild.transform.GetChild(1).GetChild(1).gameObject;
		GUIResources.GetComponent<Text>().text =
			"Wood - " + SaveManager.GameDataSave.numWood + "x\n\n" +
			"Brick - " + SaveManager.GameDataSave.numBrick + "x\n\n" +
			"Ore - " + SaveManager.GameDataSave.numOre + "x";

		// Set the text for the building to build.
		string buildingTitle;
		if (buildType == 0) {
			buildingTitle = "House";
		} else if (buildType == 1) {
			buildingTitle = "Armory";
		} else {
			buildingTitle = "Farm";
		}
		GameObject GUIBuildMenuText = GUIBuild.transform.GetChild(2).GetChild(0).GetChild(1).gameObject;
		GUIBuildMenuText.GetComponent<Text>().text = buildingTitle;
    }

    void saveBuildingProperties (Vector3 pos, string name) {
        SaveManager.GameDataSave.buildingPos.Add(new float[3] { pos.x, pos.y, pos.z });
        SaveManager.GameDataSave.buildingName.Add(name);
        SaveManager.GameDataSave.buildingNum++;
    }

    IEnumerator GUIDisableOverTime (float time) {
        yield return new WaitForSeconds(time);
        GUIBuild.transform.GetChild(3).gameObject.SetActive(false);
    }

    // Do things when this MonoBehavior is disabled.
    void OnDisable () {
        // Remove the village houses.
		foreach (GameObject building in BuildPlayer) {
			Destroy(building);
        }
		BuildPlayer.Clear();
        VHousesPlayer.Clear();
        // Disable GUI elements.
        if (GUIBuild) {
            GUIBuild.SetActive(false);
        }
    }
}
