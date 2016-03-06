using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class InputEvent : MonoBehaviour, IPointerDownHandler {
    
    // Reference to the singleton GameManager.
    public GameManager gameManager;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        if (gameManager.build.enabled) {
            gameManager.build.onPress();
        }
    }
}
