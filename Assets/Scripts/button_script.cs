using UnityEngine;
using System.Collections;

public class button_script : MonoBehaviour {
	public Texture icon;
	void OnGUI() {
		GUI.Button(new Rect(0, 0, 100, 20), new GUIContent("Click me", icon));
	}
}