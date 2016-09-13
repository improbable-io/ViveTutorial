using Improbable.Player;
using Improbable.Unity.Visualizer;
using UnityEngine;

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