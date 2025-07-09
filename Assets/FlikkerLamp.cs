using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlikkerLamp : MonoBehaviour
{
    public float duration = 5f;
    public float flashInterval = 0.2f;

    private Light pointLight;
    private float elapsedTime = 0f;
    private float nextFlashTime = 0f;
    private bool isRed = true;
    private bool isFlashing = false;

    public void StartFlashing()
    {
        if (pointLight == null)
        {
            pointLight = gameObject.AddComponent<Light>();
            pointLight.type = LightType.Point;
            pointLight.range = 10f;
            pointLight.intensity = 2f;
            pointLight.color = Color.red;
        }

        elapsedTime = 0f;
        nextFlashTime = Time.time;
        isFlashing = true;
    }

    void Update()
    {
        if (!isFlashing) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            Destroy(pointLight);
            Destroy(this); // optional
            return;
        }

        if (Time.time >= nextFlashTime)
        {
            pointLight.color = isRed ? Color.blue : Color.red;
            isRed = !isRed;
            nextFlashTime = Time.time + flashInterval;
        }
    }
}