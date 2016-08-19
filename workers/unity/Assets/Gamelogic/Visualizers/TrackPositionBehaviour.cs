using UnityEngine;
using System.Collections;

public class TrackPositionBehaviour : MonoBehaviour {

    public Vector3 SmoothedVelocity;
    private Vector3 prevPosition;
	
    void OnStart()
    {
        prevPosition = this.transform.position;
        SmoothedVelocity = Vector3.zero;
    }

	
	void Update () {
        //Exponential update law
        float delta = Time.deltaTime;
        SmoothedVelocity = SmoothedVelocity * 0.5f + (this.transform.position - prevPosition) * 0.5f / delta;
        prevPosition = this.transform.position;
	}
}
