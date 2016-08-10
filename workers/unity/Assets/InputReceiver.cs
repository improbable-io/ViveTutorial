using Improbable.Player;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;


public class InputReceiver : MonoBehaviour
{
    [Require]
    protected PlayerControlsReader PlayerControls;


    void OnEnable()
    {
        PlayerControls.MoveEvent += UpdatePlayerPosition;
    }


    void OnDisable()
    {
        PlayerControls.MoveEvent -= UpdatePlayerPosition;
    }


    void UpdatePlayerPosition(MovementDirectionData direction)
    {
        this.transform.position += direction.MovementDirection.ToUnityVector();
    }
}