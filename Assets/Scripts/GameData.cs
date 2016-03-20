using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData {

    // Game data variables.
	public int GameLevel = 1;
	public int numWood = 40;
	public int numBrick = 40;
	public int numOre = 40;
    public int healthVillage = 20;
	public int healthVillager = 5;
	public int numHouse = 0;
	public int numArmory = 0;
	public int numFarm = 0;

    // Variables that define player building properties.
	public List<float[]> buildingPos = new List<float[]>();
	public List<string> buildingName = new List<string>();
    public int buildingNum = 0;

	public void addBuilding (Vector3 pos, string name) {
		buildingPos.Add(new float[3] { pos.x, pos.y, pos.z });
		buildingName.Add(name);
		switch (name) {
		case "VillagerHousePlayer":
			numHouse++;
			break;
		case "Armory":
			numArmory++;
			break;
		default:
			numFarm++;
			break;
		}
	}

	public void removeBuilding (int n) {
		switch (buildingName[n]) {
		case "VillagerHousePlayer":
			numHouse--;
			break;
		case "Armory":
			numArmory--;
			break;
		default:
			numFarm--;
			break;
		}
		buildingPos.RemoveAt(n);
		buildingName.RemoveAt(n);
	}

    // Debug functions.
    public void printDataConstruct () {
        MonoBehaviour.print("Wood: " + numWood + " (Construction)");
        MonoBehaviour.print("Brick: " + numBrick + " (Construction)");
        MonoBehaviour.print("Ore: " + numOre + " (Construction)");
    }

    public void printData () {
        MonoBehaviour.print("Wood: " + numWood);
        MonoBehaviour.print("Brick: " + numBrick);
        MonoBehaviour.print("Ore: " + numOre);
    }
}
