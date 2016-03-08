using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameDataLevels {

	public static int numHouses (int level) {
		int a = 0;
		int b = 1;
		if (level == 0 || level == 1) {
			return level;
		}
		int result = 0;
		for (int i = 2; i <= level; i++) {
			result = a + b;
			a = b;
			b = result;
		}
		return result;
	}

	public static List<GameObject> initEnemyHouses (int level) {
		List<GameObject> result = new List<GameObject>();
		List<Vector3> positions = new List<Vector3>();
		int numHouse = numHouses(level);
		for (int i = 0; i < numHouse; i++) {
			Vector3 pos;
			float x = Mathf.Floor((Random.Range(30.0f, 70.0f) + 2.5f) / 5.0f) * 5.0f;
			float y = 0.0f;
			float z = Mathf.Floor((Random.Range(0.0f, 50.0f) + 2.5f) / 5.0f) * 5.0f;
			pos = new Vector3(x, y, z);
			positions.Add(pos);
			GameObject house = (GameObject) MonoBehaviour.Instantiate(
				Resources.Load("VillagerHouseEnemy"),
				pos,
				new Quaternion(0.0f, 180.0f, 0.0f, 0.0f)
			);
			result.Add(house);
		}
		return result;
	}
}
