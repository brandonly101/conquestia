using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameDataLevels {

	public static int costHouseWood = 2;
	public static int costHouseBrick = 2;
	public static int costHouseOre = 1;

	public static int costArmoryWood = 3;
	public static int costArmoryBrick = 4;
	public static int costArmoryOre = 4;

	public static int costFarmWood = 4;
	public static int costFarmBrick = 2;
	public static int costFarmOre = 1;

	public static int healthEnemyVillage = 5;
	public static int healthArmory = 1;
	public static int healthFarm = 5;

	public static int numBuild (int level) {
		int a = 0;
		int b = 1;
		if (level == 0 || level == 1) {
			return level + 9;
		}
		int result = 0;
		for (int i = 2; i <= level; i++) {
			result = a + b;
			a = b;
			b = result;
		}
		return result + 9;
	}

	public static List<GameObject> initEnemyHouses (int level, ref List<GameObject> VHouseE, ref int numArmory, ref int numFarm) {
		List<GameObject> result = new List<GameObject>();
		List<Vector3> positions = new List<Vector3>();
		int numBuildings = numBuild(level);

		// Load half of the buildings as houses.
		for (int i = 0; i < ((numBuildings * 2 / 3) + 1); i++) {
			// Add name and position.
			Vector3 pos;
			do {
				float x = Mathf.Floor((Random.Range (10.0f, 40.0f) + 2.5f) / 5.0f) * 5.0f;
				float y = 0.0f;
				float z = Mathf.Floor((Random.Range (-25.0f, 25.0f) + 2.5f) / 5.0f) * 5.0f;
				pos = new Vector3(x, y, z);
			} while (positions.Contains(pos));
			positions.Add(pos);

			// Add building.
			GameObject building = (GameObject) MonoBehaviour.Instantiate(
				Resources.Load("VillagerHouseEnemy"),
				pos,
				new Quaternion(0.0f, 180.0f, 0.0f, 0.0f)
			);
			VHouseE.Add(building);
			result.Add(building);
		}

		// Add all the other buildings.
		for (int i = ((numBuildings * 2 / 3) + 1); i < numBuildings; i++) {
			// Add position.
			Vector3 pos;
			do {
				float x = Mathf.Floor((Random.Range (10.0f, 40.0f) + 2.5f) / 5.0f) * 5.0f;
				float y = 0.0f;
				float z = Mathf.Floor((Random.Range (-25.0f, 25.0f) + 2.5f) / 5.0f) * 5.0f;
				pos = new Vector3(x, y, z);
			} while (positions.Contains(pos));
			positions.Add(pos);

			// Add name.
			string name;
			switch (Random.Range(0, 3)) {
			case 0:
			case 1:
				name = "Farm";
				numFarm++;
				break;
			default:
				name = "Armory";
				numArmory++;
				break;
			}

			// Add building.
			GameObject building = (GameObject) MonoBehaviour.Instantiate(
				Resources.Load(name),
				pos,
				new Quaternion(0.0f, 180.0f, 0.0f, 0.0f)
			);
			result.Add(building);
		}
		return result;
	}
}
