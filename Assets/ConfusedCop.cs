using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusedCop : MonoBehaviour
{
    public Transform[] pathToScreenPoints;      // 3+ points to reach the screen
    public Transform[] pathToFinalPoints;       // 2+ points to final walk path
    public GameObject questionMarkPrefab;
    public Transform questionMarkSpawnPoint;

    public float driveToScreenDuration = 3f;
    public float driveToFinalDuration = 3f;
    public float rotationDegrees = 90f;
    public float rotationDuration = 1f;
    public float pauseDuration = 2f;


    public void StartRoutine()
    {
        StartCoroutine(DoSequence());
    }

    private IEnumerator DoSequence()
    {
        // Step 1: Move along path to screen
        Vector3[] pathToScreen = new Vector3[pathToScreenPoints.Length];
        for (int i = 0; i < pathToScreenPoints.Length; i++)
            pathToScreen[i] = pathToScreenPoints[i].position;

        yield return transform.DOPath(pathToScreen, driveToScreenDuration, PathType.CatmullRom)
                              .SetEase(Ease.Linear)
                              .SetLookAt(0.01f)
                              .WaitForCompletion();

        // Step 2: Rotate 90 degrees
        Vector3 targetRot = transform.eulerAngles + new Vector3(0, rotationDegrees, 0);
        yield return transform.DORotate(targetRot, rotationDuration)
                              .SetEase(Ease.OutCubic)
                              .WaitForCompletion();

        // Step 3: Show question mark
        GameObject qm = null;
        if (questionMarkPrefab && questionMarkSpawnPoint)
        {
            qm = Instantiate(questionMarkPrefab, questionMarkSpawnPoint.position, Quaternion.Euler(0, 270, 0));
            qm.transform.localScale = Vector3.one * 0.05f;
        }

        // Step 4: Pause
        yield return new WaitForSeconds(pauseDuration);

        // Step 5: Continue moving and destroy question mark
        if (qm != null)
        {
            Destroy(qm);
        }

        Vector3[] pathToFinal = new Vector3[pathToFinalPoints.Length];
        for (int i = 0; i < pathToFinalPoints.Length; i++)
            pathToFinal[i] = pathToFinalPoints[i].position;

        yield return transform.DOPath(pathToFinal, driveToFinalDuration, PathType.CatmullRom)
                              .SetEase(Ease.Linear)
                              .SetLookAt(0.01f)
                              .WaitForCompletion();

    }
}
