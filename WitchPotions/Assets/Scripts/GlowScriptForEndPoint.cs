using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowScriptForEndPoint : MonoBehaviour
{
    public GameObject glow1;
    public GameObject glow2;

    public void ToggleGlow(bool state)
    { glow1.SetActive(state);
        glow2.SetActive(state);
    }
}
