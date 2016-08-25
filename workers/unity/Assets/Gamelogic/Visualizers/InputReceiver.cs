using Improbable.Core.Entity;
using Improbable.Player;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using IoC;
using UnityEngine;


public class InputReceiver : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
  

    [Require]
    protected PlayerControlsReader PlayerControls;

    [Inject]
    protected IUniverse IUniverse;

    void OnEnable()
    {
        PlayerControls.MoveEvent += UpdatePlayerPosition;
        PlayerControls.PickUpEvent += MakeJoint;
        PlayerControls.DropEvent += BreakJoint;
    }


    void OnDisable()
    {
        PlayerControls.MoveEvent -= UpdatePlayerPosition;
        PlayerControls.PickUpEvent -= MakeJoint;
        PlayerControls.DropEvent -= BreakJoint;
    }


    void UpdatePlayerPosition(MovementDirectionData direction)
    {
        this.transform.position += direction.MovementDirection.ToUnityVector();
    }

    void MakeJoint(PickUpObjectData target)
    {
        if (target.Hand == "left")
        {
            LeftHand.GetComponentInChildren<ToggleHandModel>().ClenchHand();
            LeftHand.GetComponent<FixedJoint>().connectedBody = IUniverse.Get(target.PickUpTargetId).UnderlyingGameObject.GetComponent<Rigidbody>();
        }
        else if (target.Hand == "right") {
            RightHand.GetComponentInChildren<ToggleHandModel>().ClenchHand();
            RightHand.GetComponent<FixedJoint>().connectedBody = IUniverse.Get(target.PickUpTargetId).UnderlyingGameObject.GetComponent<Rigidbody>();
        }
        else
        {
            //un-HAND-led case.
        }
        
    }

    void BreakJoint(DropObjectData target)
    {
        if (target.Hand == "left")
        {
            LeftHand.GetComponentInChildren<ToggleHandModel>().RelaxHand();
            LeftHand.GetComponent<FixedJoint>().connectedBody.velocity = LeftHand.GetComponent<TrackPositionBehaviour>().SmoothedVelocity;
            LeftHand.GetComponent<FixedJoint>().connectedBody = null;
        }
        else if (target.Hand == "right")
        {
            RightHand.GetComponentInChildren<ToggleHandModel>().RelaxHand();
            RightHand.GetComponent<FixedJoint>().connectedBody.velocity = RightHand.GetComponent<TrackPositionBehaviour>().SmoothedVelocity;
            RightHand.GetComponent<FixedJoint>().connectedBody = null;
        }
        else
        {
            //un-HAND-led case.
        }
    }
}