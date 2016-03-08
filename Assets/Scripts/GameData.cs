using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData {

    // Game data variables.
	public int GameLevel = 1;
	public int numWood = 50;
	public int numBrick = 50;
	public int numOre = 50;
    public int healthVillage = 20;
	public int healthVillager = 5;
	public int numHouse = 0;
	public int numArmory = 0;
	public int numFarm = 0;

    // Variables that define player building properties.
	public List<float[]> buildingPos = new List<float[]>();
	public List<string> buildingName = new List<string>();
    public int buildingNum = 0;

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
