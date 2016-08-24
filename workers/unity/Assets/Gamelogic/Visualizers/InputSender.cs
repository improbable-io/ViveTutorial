using Improbable;
using Improbable.Player;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class InputSender : MonoBehaviour
{
    [Require]
    protected PlayerControlsWriter PlayerControls;

    public bool UseVR = true;
    public Camera NonVRCamera;

    public float MovementSpeed = 4f;
    public float MinMovementMagnitude = 0.1f;
    public Transform HeadsetTransform;
    public SteamVR_TrackedObject LeftController;
    public SteamVR_TrackedObject RightController;

    public GameObject LeftHand;
    public GameObject RightHand;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private GameObject leftHandHeldObject;
    private GameObject rightHandHeldObject;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;

    [SerializeField]
    private UnityStandardAssets.Characters.FirstPerson.MouseLook m_MouseLook;
    [SerializeField]
    private float m_StickToGroundForce;

    void Start() {
        //Maintaining authority over objects
        InvokeRepeating("sendHeartbeats", 1.0f, 1.0f);
        m_MouseLook.Init(transform, NonVRCamera.transform);
    }

    void Update()
    {
        if (UseVR)
        {
            SteamVR_Controller.Device leftDevice = (LeftController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)LeftController.index) : null;
            SteamVR_Controller.Device rightDevice = (RightController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)RightController.index) : null;
            Vector3 headDirection = (HeadsetTransform == null) ? Vector3.zero : new Vector3(HeadsetTransform.forward.x, 0f, HeadsetTransform.forward.z).normalized;


            //Movement
            Vector3 leftInputDirection = (leftDevice == null) ? Vector3.zero : new Vector3(leftDevice.GetAxis().x, 0f, leftDevice.GetAxis().y);
            Vector3 rightInputDirection = (rightDevice == null) ? Vector3.zero : new Vector3(rightDevice.GetAxis().x, 0f, rightDevice.GetAxis().y);
            Vector3 inputDirection = (leftInputDirection + rightInputDirection).normalized;
            if (inputDirection.magnitude >= MinMovementMagnitude)
            {
                Vector3 movementDirection = Quaternion.LookRotation(headDirection) * inputDirection * MovementSpeed * Time.deltaTime;
                PlayerControls.Update.TriggerMoveEvent(movementDirection.ToNativeVector()).FinishAndSend();
            }

            //Picking up objects
            if (leftDevice != null && leftDevice.GetPressDown(triggerButton))
            {
                GameObject reachableObject = LeftHand.GetComponent<GrabItemsBehaviour>().GetClosestReachableObject();
                if (reachableObject != null)
                {
                    leftHandHeldObject = reachableObject;
                    PlayerControls.Update.TriggerPickUpEvent(leftHandHeldObject.EntityId(), "left").FinishAndSend();
                }
            }

            if (rightDevice != null && rightDevice.GetPressDown(triggerButton))
            {
                GameObject reachableObject = RightHand.GetComponent<GrabItemsBehaviour>().GetClosestReachableObject();
                if (reachableObject != null)
                {
                    rightHandHeldObject = reachableObject;
                    PlayerControls.Update.TriggerPickUpEvent(rightHandHeldObject.EntityId(), "right").FinishAndSend();
                }
            }

            //Dropping objects
            if (leftDevice != null && leftDevice.GetPressUp(triggerButton))
            {
                GameObject heldObject = LeftHand.GetComponent<GrabItemsBehaviour>().GetClosestReachableObject();
                if (LeftHand.GetComponent<FixedJoint>().connectedBody != null)
                {
                    PlayerControls.Update.TriggerDropEvent(leftHandHeldObject.EntityId(), "left").FinishAndSend();
                    leftHandHeldObject = null;
                }
            }
            if (rightDevice != null && rightDevice.GetPressUp(triggerButton))
            {
                if (RightHand.GetComponent<FixedJoint>().connectedBody != null)
                {
                    PlayerControls.Update.TriggerDropEvent(rightHandHeldObject.EntityId(), "right").FinishAndSend();
                    rightHandHeldObject = null;
                }
            }
        } else
        {
            NonVRUpdate();
        }
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, NonVRCamera.transform);
    }

    private void NonVRUpdate()
    {
        if (NonVRCamera.transform.gameObject.activeSelf)
        {
            PlayerControls.Update
                .HeadPosition(NonVRCamera.transform.position.ToNativeVector())
                .HeadRotation(NonVRCamera.transform.rotation.eulerAngles.ToNativeVector())
                .FinishAndSend();
        }

        RotateView();
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hitInfo,
                           1.0f / 2f, ~0, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * MovementSpeed;
        m_MoveDir.z = desiredMove.z * MovementSpeed;

        m_MoveDir.y = -m_StickToGroundForce;

        var moveDirection = m_MoveDir;
        if (moveDirection.magnitude >= 0.1)
        {
            //m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);
            Debug.Log(MovementSpeed + " Sent Moving: " + moveDirection);
            if (PlayerControls != null)
            {
                // Trigger spatial movement.
                PlayerControls.Update.TriggerMoveEvent(moveDirection.ToNativeVector() * Time.deltaTime).FinishAndSend();
            } else {
                this.transform.position += moveDirection * Time.deltaTime;
            }
        }

        m_MouseLook.UpdateCursorLock();
    }

    void sendHeartbeats()
    {
        if (UseVR)
        {
            if (LeftHand.GetComponent<FixedJoint>().connectedBody != null)
            {
                PlayerControls.Update.TriggerGrabbingHeartbeatEvent(LeftHand.GetComponent<FixedJoint>().connectedBody.gameObject.EntityId()).FinishAndSend();
            }
            if (RightHand.GetComponent<FixedJoint>().connectedBody != null)
            {
                PlayerControls.Update.TriggerGrabbingHeartbeatEvent(RightHand.GetComponent<FixedJoint>().connectedBody.gameObject.EntityId()).FinishAndSend();
            }
        } else
        {

        }
      
    }

}