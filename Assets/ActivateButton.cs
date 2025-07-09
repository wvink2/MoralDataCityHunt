using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActivateButton : MonoBehaviour
{
    public Image fillCircle;
    public Button transitionButton;
    public float fillDuration = 10f;

    void Start()
    {
        transitionButton.gameObject.SetActive(false); // Hide button at start
    }
    public void StartTimer()
    {
        StartCoroutine(FillCircleAndShowButton());
    }

    IEnumerator FillCircleAndShowButton()
    {
        float time = 0f;
        fillCircle.fillAmount = 0f;

        while (time < fillDuration)
        {
            time += Time.deltaTime;
            fillCircle.fillAmount = Mathf.Clamp01(time / fillDuration);
            yield return null;
        }

        // Show the button
        fillCircle.gameObject.SetActive(false); // Optional: hide the circle
        transitionButton.gameObject.SetActive(true);
        transitionButton.interactable = true;
    }
}
