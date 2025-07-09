using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CAR_ARREST : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] pathPoints;
    public Transform carReturnPoint;
    public Transform[] suspectPathPoints;
    public Transform[] civilianPathPoints;

    [Header("Spawn Settings")]
    public GameObject copPrefab;
    public GameObject suspectPrefab;
    public GameObject civilianPrefab;
    public Transform copSpawnPoint;
    public Transform suspectSpawnPoint;
    public Transform civilianSpawnPoint;

    [Header("Timing Settings")]
    public float driveToStopDuration = 3f;
    public float waitBeforeArrest = 1f;
    public float arrestDuration = 2f;
    public float waitBeforeDrivingOff = 1f;
    public float driveToExitDuration = 3f;
    public float walkSpeed = 1.5f;
    public float SuspectStoptime = 1.3f;

    [Header("Wheel References")]
    public Transform[] wheels;
    public float wheelSpinSpeed = 360f;

    public void TriggerCarSequence()
    {
        StartCoroutine(StartCarSequence());
    }

    private bool isSpinning = false;

    private void Update()
    {
        if (isSpinning)
        {
            foreach (Transform wheel in wheels)
            {
                wheel.Rotate(Vector3.right, wheelSpinSpeed * Time.deltaTime, Space.Self);
            }
        }
    }

    private IEnumerator StartCarSequence()
    {
        // Spawn suspect and civilian
        GameObject suspect = Instantiate(suspectPrefab, suspectSpawnPoint.position, suspectSpawnPoint.rotation);
        GameObject civilian = Instantiate(civilianPrefab, civilianSpawnPoint.position, civilianSpawnPoint.rotation);

        suspect.transform.localScale = Vector3.one * 0.1f;
        civilian.transform.localScale = Vector3.one * 0.1f;

        // Create paths
        Vector3[] suspectPath = new Vector3[suspectPathPoints.Length];
        for (int i = 0; i < suspectPathPoints.Length; i++)
            suspectPath[i] = suspectPathPoints[i].position;

        Vector3[] civilianPath = new Vector3[civilianPathPoints.Length];
        for (int i = 0; i < civilianPathPoints.Length; i++)
            civilianPath[i] = civilianPathPoints[i].position;

        // Move suspect and civilian
        Tween suspectWalk = suspect.transform.DOPath(suspectPath, suspectPath.Length / 0.1f, PathType.CatmullRom)
                                             .SetEase(Ease.Linear)
                                             .SetLookAt(0.01f);

        Tween civilianWalk = civilian.transform.DOPath(civilianPath, civilianPath.Length / 0.1f, PathType.CatmullRom)
                                               .SetEase(Ease.Linear)
                                               .SetLookAt(0.01f);



        // Pause before cop arrives
        yield return new WaitForSeconds(waitBeforeArrest);

        // Drive to the stop point
        Vector3[] pathToStop = new Vector3[]
        {
            pathPoints[0].position,
            pathPoints[1].position,
            pathPoints[2].position
        };
        isSpinning = true;

        yield return transform.DOPath(pathToStop, driveToStopDuration, PathType.CatmullRom)
                              .SetEase(Ease.Linear)
                              .SetLookAt(0.01f)
                              .WaitForCompletion();
        isSpinning = false;

        // Spawn cop
        GameObject cop = Instantiate(copPrefab, copSpawnPoint.position, copSpawnPoint.rotation);
        cop.transform.localScale = Vector3.one * 0.1f;

        // Walk cop behind suspect
        Vector3 behindSuspect = suspect.transform.position - suspect.transform.forward * 0.3f;

        yield return cop.transform.DOMove(behindSuspect, Vector3.Distance(cop.transform.position, behindSuspect) / walkSpeed)
                                  .SetEase(Ease.Linear);

        cop.transform.DOLookAt(suspect.transform.position, 0.25f);
        yield return new WaitForSeconds(SuspectStoptime);

        suspectWalk.Kill();

        yield return new WaitForSeconds(0.5f);

        // Cop walks to suspect's front-right
        Vector3 approachDir = suspect.transform.forward;
        Vector3 sideOffset = Vector3.Cross(approachDir, Vector3.up).normalized * 0.3f;
        Vector3 inFrontOfSuspect = suspect.transform.position + approachDir * 0.2f + sideOffset;

        yield return cop.transform.DOMove(inFrontOfSuspect, Vector3.Distance(cop.transform.position, inFrontOfSuspect) / 1.3f)
                                  .SetEase(Ease.Linear)
                                  .WaitForCompletion();

        cop.transform.DOLookAt(suspect.transform.position, 0.25f);
        suspect.transform.DOLookAt(cop.transform.position, 0.25f);

        yield return new WaitForSeconds(0.5f);

        // ESCORT: suspect walks to car, cop follows slightly behind
        Vector3 moveDir = (carReturnPoint.position - suspect.transform.position).normalized;
        Vector3 suspectEscortTarget = carReturnPoint.position;
        Vector3 copEscortTarget = carReturnPoint.position - moveDir * 0.3f;

        cop.transform.DOLookAt(copEscortTarget, 0.25f);
        suspect.transform.DOLookAt(suspectEscortTarget, 0.25f);

        Tween escortCopMove = cop.transform.DOMove(copEscortTarget, Vector3.Distance(cop.transform.position, copEscortTarget) / 0.6f)
                                           .SetEase(Ease.Linear);

        Tween escortSuspectMove = suspect.transform.DOMove(suspectEscortTarget, Vector3.Distance(suspect.transform.position, suspectEscortTarget) / 0.6f)
                                                   .SetEase(Ease.Linear);

        yield return DOTween.Sequence()
            .Join(escortCopMove)
            .Join(escortSuspectMove)
            .WaitForCompletion();

        // Clean up
        Destroy(suspect);
        Destroy(cop);

        // Drive away to exit
        Vector3[] pathToExit = new Vector3[]
        {
            pathPoints[2].position,
            pathPoints[3].position,
            pathPoints[4].position
        };
        isSpinning = true;

        yield return transform.DOPath(pathToExit, driveToExitDuration, PathType.CatmullRom)
                              .SetEase(Ease.Linear)
                              .SetLookAt(0.01f)
                              .WaitForCompletion();
        isSpinning = false;
    }
}