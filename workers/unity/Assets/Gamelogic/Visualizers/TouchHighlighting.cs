using UnityEngine;
using System.Collections;

public class TouchHighlighting : MonoBehaviour {

    public Material HighlightedMaterial;

    private Material DefaultMaterial;

    void OnEnable () { 
        DefaultMaterial = GetComponent<Renderer>().material;
	}
	
	void Highlight()
    {
        GetComponent<Renderer>().material = HighlightedMaterial;
    }

    void Dehighlight()
    {
        GetComponent<Renderer>().material = DefaultMaterial;
    }
}
