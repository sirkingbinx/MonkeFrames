using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MonkeFrames.Editor.Components
{
    public class ForceSetState : MonoBehaviour
    {
        public static Dictionary<Behaviour, bool> states = new();
        public static Dictionary<GameObject, bool> statesObj = new();

        public static void Set(Behaviour b, bool state)
        {
            if (!states.ContainsKey(b))
                states.Add(b, state);

            states[b] = state;
        }

        public static void Set(GameObject b, bool state)
        {
            if (!statesObj.ContainsKey(b))
                statesObj.Add(b, state);

            statesObj[b] = state;
        }

        void Update()
        {
            foreach (var pair in states)
            {
                if (pair.Key.isActiveAndEnabled != pair.Value)
                    pair.Key.enabled = pair.Value;
            }

            foreach (var pair in statesObj)
            {
                if (pair.Key.activeSelf != pair.Value)
                    pair.Key.SetActive(pair.Value);
            }
        }
    }
}
