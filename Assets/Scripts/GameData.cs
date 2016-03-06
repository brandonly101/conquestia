using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData {

    // Game data variables.
    public int numWood;
    public int numBrick;
    public int numOre;

    // Variables that define player building properties.
    public List<float[]> buildingPos;
    public List<string> buildingName;
    public int buildingNum;

    public GameData () {
        numWood = 50;
        numBrick = 50;
        numOre = 50;
        buildingPos = new List<float[]>();
        buildingName = new List<string>();
        buildingNum = 0;
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
