using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStateBuild : MonoBehaviour {

    // Public variables.
	public GameObject GUIBuild;
	public GameObject GUIBuildMenu;

    // Private variables.
	List<GameObject> BuildingPlayer;
    bool isBuilding;
    GameObject ObjectPrebuild;
    Vector3 PrebuildPos;
    int buildType;

    // Public functions.
    public void buildSpawn () {
		GameObject building;
        if (buildType == 0 && enoughResources(buildType)) {
            building = cleanSpawnObject("VillagerHousePlayer", PrebuildPos);
            SaveManager.GameDataSave.numWood -= 1;
            SaveManager.GameDataSave.numBrick -= 4;
            SaveManager.GameDataSave.numOre -= 1;
			saveBuildingProperties(building.transform.position, "VillagerHousePlayer");
        } else if (buildType == 1 && enoughResources(buildType)) {
			building = cleanSpawnObject("Armory", PrebuildPos);
			SaveManager.GameDataSave.numWood -= 1;
			SaveManager.GameDataSave.numBrick -= 2;
			SaveManager.GameDataSave.numOre -= 3;
			saveBuildingProperties(building.transform.position, "Armory");
		} else if (buildType == 2 && enoughResources(buildType)) {
			building = cleanSpawnObject("Farm", PrebuildPos);
			SaveManager.GameDataSave.numWood -= 3;
			SaveManager.GameDataSave.numBrick -= 1;
			SaveManager.GameDataSave.numOre -= 1;
			saveBuildingProperties(building.transform.position, "Farm");
		} else {
            GUIBuildMenu.transform.GetChild(7).gameObject.SetActive(true);
            StartCoroutine(GUIDisableOverTime(3.0f));
            return;
        }
		BuildingPlayer.Add(building);
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

        // Hacky way of making buildings spawn only on the X-Z plane and on the player side.
		if (position.y == 0.0f && position.x <= 0) {
            if (!isBuilding) {
                buildMenu(position);
            }
        }
    }

    // Use this for initialization
    void Awake () {
        buildType = 0;
		BuildingPlayer = new List<GameObject>();

        // Disable the build GUI, for now.
		GUIBuild.SetActive(false);
		GUIBuildMenu.SetActive(false);
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
			BuildingPlayer.Add(building);
        }

        // Enable GUI elements.
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
		GUIBuild.SetActive(!mode);
		GUIBuildMenu.SetActive(mode);
		GUIBuildMenu.transform.GetChild(7).gameObject.SetActive(false);

        // Set the text for the current level, village strength, and villager health.
		GUIBuild.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Level " + SaveManager.GameDataSave.GameLevel;
		GUIBuild.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "Village Strenth - " +
			(SaveManager.GameDataSave.healthVillage + SaveManager.GameDataSave.numFarm * GameDataLevels.healthFarm);
		GUIBuild.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text = "Villager Health - " +
			(SaveManager.GameDataSave.healthVillager + SaveManager.GameDataSave.numArmory * GameDataLevels.healthArmory);

		// Set the text for the current resources.
		GameObject GUIResources = GUIBuild.transform.GetChild(1).GetChild(0).gameObject;
		GUIResources.GetComponent<Text>().text =
			"Wood: " + SaveManager.GameDataSave.numWood + "x\n\n" +
			"Brick: " + SaveManager.GameDataSave.numBrick + "x\n\n" +
			"Ore: " + SaveManager.GameDataSave.numOre + "x";
		GUIResources = GUIBuildMenu.transform.GetChild(0).GetChild(0).gameObject;
		GUIResources.GetComponent<Text>().text =
			"Wood: " + SaveManager.GameDataSave.numWood + "x\n\n" +
			"Brick: " + SaveManager.GameDataSave.numBrick + "x\n\n" +
			"Ore: " + SaveManager.GameDataSave.numOre + "x";

		// Set the text for the building to build.
		GameObject GUIResourcesReq = GUIBuildMenu.transform.GetChild(1).GetChild(0).gameObject;
		if (buildType == 0) {
			GUIResourcesReq.GetComponent<Text>().text = "Wood: 1x\n\nBrick: 4x\n\nOre: 1x";
		} else if (buildType == 1) {
			GUIResourcesReq.GetComponent<Text>().text = "Wood: 1x\n\nBrick: 2x\n\nOre: 3x";
		} else {
			GUIResourcesReq.GetComponent<Text>().text = "Wood: 3x\n\nBrick: 1x\n\nOre: 1x";
		}
    }

    void saveBuildingProperties (Vector3 pos, string name) {
        SaveManager.GameDataSave.buildingPos.Add(new float[3] { pos.x, pos.y, pos.z });
        SaveManager.GameDataSave.buildingName.Add(name);
        SaveManager.GameDataSave.buildingNum++;
		if (name == "VillagerHousePlayer") {
			SaveManager.GameDataSave.numHouse++;
		} else if (name == "Armory") {
			SaveManager.GameDataSave.numArmory++;
		} else {
			SaveManager.GameDataSave.numFarm++;
		}
    }

    IEnumerator GUIDisableOverTime (float time) {
        yield return new WaitForSeconds(time);
        GUIBuildMenu.transform.GetChild(7).gameObject.SetActive(false);
    }

    // Do things when this MonoBehavior is disabled.
    void OnDisable () {
        // Remove the village houses.
		foreach (GameObject building in BuildingPlayer) {
			Destroy(building);
        }
		BuildingPlayer.Clear();

        // Disable GUI elements.
        if (GUIBuild) {
            GUIBuild.SetActive(false);
        }
    }
}
