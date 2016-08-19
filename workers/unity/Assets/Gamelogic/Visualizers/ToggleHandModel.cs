using UnityEngine;
using System.Collections;
using System;

public class ToggleHandModel : MonoBehaviour
{
    public GameObject RelaxedFingers;
    public GameObject ClenchedFingers;
    public GameObject Palm;

    public Material RelaxedHandMaterial;
    public Material HighlightedHandMaterial;
    public Material ClenchedHandMaterial;

    enum HandState {Relaxed, Highlighted, Clenched}
    private HandState handState;

    void OnEnable()
    {
        handState = HandState.Relaxed;
        SetFingerMaterial(RelaxedFingers, RelaxedHandMaterial);
        SetFingerMaterial(ClenchedFingers, ClenchedHandMaterial);
        UpdateHandAppearance();
    }

    private void SetFingerMaterial(GameObject fingers, Material material)
    {
        foreach (Transform finger in fingers.GetComponentsInChildren<Transform>())
        {
            foreach (Renderer renderer in finger.GetComponentsInChildren<Renderer>())
            {
                renderer.material = material;
            }
        }
    }

    public void RelaxHand()
    {
        handState = HandState.Relaxed;
        UpdateHandAppearance();
    }

    public void HighlightHand()
    {
        if (handState == HandState.Relaxed)
        {
            handState = HandState.Highlighted;
            UpdateHandAppearance();
        }
    }

    public void ClenchHand()
    {
        handState = HandState.Clenched;
        UpdateHandAppearance();
    }

    private void UpdateHandAppearance()
    {
        switch (handState)
        {
            case HandState.Relaxed:
                RelaxedFingers.SetActive(true);
                ClenchedFingers.SetActive(false);
                SetFingerMaterial(RelaxedFingers, RelaxedHandMaterial);
                Palm.GetComponent<Renderer>().material = RelaxedHandMaterial;
                break;
            case HandState.Highlighted:
                SetFingerMaterial(RelaxedFingers, HighlightedHandMaterial);
                Palm.GetComponent<Renderer>().material = HighlightedHandMaterial;
                break;
            case HandState.Clenched:
                RelaxedFingers.SetActive(false);
                ClenchedFingers.SetActive(true);
                Palm.GetComponent<Renderer>().material = ClenchedHandMaterial;
                break;
        }
    }
}
