using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private Controller controller;
    private Renderer renderer;
    private static bool isActive = false;

    void Start() {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other) {
        if(renderer.enabled) {
            controller.SetRandomItem();
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(isActive) {
            isActive = false;
            controller.IncrementSnake(transform);
        }
    }
}

