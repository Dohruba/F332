using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentManager : MonoBehaviour
{
    [SerializeField]
    private ArduinoManager arduinoManager;
    [SerializeField]
    private GameObject contentBlocker;
    public float triggerDistance = 8;
    public int waitingTime = 60;
    [SerializeField]
    private PrinterManager printerManager;

    private void Start()
    {
        StartCoroutine(DistanceControl());
    }

    private IEnumerator DistanceControl()
    {
        float distance = float.MaxValue;
        while (true)
        {
            yield return new WaitForEndOfFrame();
            distance = arduinoManager.Distance;
            if(distance < triggerDistance)
            {
                contentBlocker.SetActive(true);
                printerManager.PrintByPathButton();
                yield return new WaitForSecondsRealtime(waitingTime);
                contentBlocker.SetActive(false);
            }
        }
    }
}
