using UnityEngine;
using System.Collections;

public class GrabItemsBehaviour : MonoBehaviour {

    public Material EmptyHandedMaterial;
    public Material GrabbingMaterial;
    public Renderer handRenderer;

    public GameObject touchedObject = null;

    //Note: this an overly simplistic way of handling this that will fail if we collide with multiple things
    void OnTriggerEnter(Collider other)
    {
        handRenderer.material = GrabbingMaterial;
        touchedObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        handRenderer.material = EmptyHandedMaterial;
        touchedObject = null;
    }
}
