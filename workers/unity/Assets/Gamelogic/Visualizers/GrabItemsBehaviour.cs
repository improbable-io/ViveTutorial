using UnityEngine;
using System.Collections;
using Improbable;
using System.Collections.Generic;

public class GrabItemsBehaviour : MonoBehaviour
{

    public float HandMass = 0.01f;
    public float HandDrag = 0;
    public float HandAngularDrag = 0.05f;
    public ToggleHandModel toggleHand;

    private GameObject touchedObject = null;

    private HashSet<GameObject> reachableObjects;
    private HashSet<GameObject> previousReachableObjects;
    private GameObject closestObject;

    void OnEnable()
    {
        Rigidbody r = gameObject.AddComponent<Rigidbody>();
        r.mass = HandMass;
        r.drag = HandDrag;
        r.angularDrag = HandAngularDrag;
        r.useGravity = false;
        r.isKinematic = true;
        gameObject.AddComponent<FixedJoint>();
        reachableObjects = new HashSet<GameObject>();
        previousReachableObjects = reachableObjects;
        closestObject = null;
    }

    void OnTriggerEnter(Collider other)
    {
        reachableObjects.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        reachableObjects.Remove(other.gameObject);
    }

    public GameObject GetClosestReachableObject()
    {
        return closestObject;
    }

    void Update()
    {
        GameObject previousClosestObject = closestObject;
        closestObject = GetClosestObject(reachableObjects);
        DecideHighlighting(previousClosestObject, closestObject);
        previousReachableObjects = reachableObjects;
    }

    private GameObject GetClosestObject(HashSet<GameObject> currentReachableObjects)
    {
        GameObject closestGameObject = null;
        if (currentReachableObjects.Count > 0)
        {
            float closestDistance = 1000;
            foreach (GameObject reachableObject in currentReachableObjects)
            {
                float reachableObjectDistance = Vector3.Distance(transform.position, reachableObject.transform.position);
                if (reachableObjectDistance < closestDistance)
                {
                    reachableObjectDistance = closestDistance;
                    closestGameObject = reachableObject;
                }
            }
        }
        return closestGameObject;
    }  

    private void DecideHighlighting(GameObject previousClosestObject, GameObject closestObject)
    {
        if (previousClosestObject == null && closestObject != null)
        {
            toggleHand.HighlightHand();
            ToggleHighlight(closestObject, true);
        }
        else if (previousClosestObject != null && closestObject == null)
        {
            toggleHand.RelaxHand();
            ToggleHighlight(previousClosestObject, false);
        }
        else if (previousClosestObject != null && closestObject != null && previousClosestObject != closestObject)
        {
            ToggleHighlight(previousClosestObject, false);
            ToggleHighlight(closestObject, true);
        }
    }

    private void ToggleHighlight(GameObject toggledObject, bool shouldHighlight)
    {
        if (toggledObject.GetComponent<TouchHighlighting>() != null)
        {
            if (shouldHighlight)
            {
                toggledObject.SendMessage("Highlight");
            }
            else
            {
                toggledObject.SendMessage("Dehighlight");
            }
        }
    }
}
