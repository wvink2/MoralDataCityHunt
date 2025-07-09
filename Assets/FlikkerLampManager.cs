using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlikkerLampManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lightsParents; // The parent GameObject that contains all the flashing lights

    private FlikkerLamp[] flashingLights;

    void Awake()
    {
        foreach (GameObject light in lightsParents)
        {
            if (light != null)
            {
                flashingLights = light.GetComponentsInChildren<FlikkerLamp>();
            }
            else
            {
                Debug.LogWarning("PoliceLightManager: No parent object assigned.");
                flashingLights = new FlikkerLamp[0];
            }
        }
    }

    public void StartAllFlashing()
    {
        foreach (var light in flashingLights)
        {
            if (light != null)
            {
                light.StartFlashing();
            }
        }
    }
}
