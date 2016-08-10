using UnityEngine;
using Improbable.Unity.Visualizer;
using Improbable.Player;

namespace Assets.Gamelogic.Visualizers
{
    public class LogNameBehaviour : MonoBehaviour
    {
        [Require]
        protected NameReader Name;

        protected void OnEnable()
        {
            Debug.Log("Player entity spawned! Name is: " + Name.Value);
        }
    }
}