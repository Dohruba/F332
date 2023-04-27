using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoManager : MonoBehaviour
{

    private SerialPort stream;
    public float Distance = 0;
    public void Awake()
    {
        stream = new SerialPort("COM3", 9600);
        stream.ReadTimeout = 1000;
        //stream.Open();
        //stream.Close();
        StartCoroutine
         (
             AsynchronousReadFromArduino
             ((string s) => Debug.Log(s + " From Arduino"),     // Callback //Debug.Log(s) weg und distance float Zahl
                 () => Debug.LogError("Error!"), // Error callback
                 10000f                          // Timeout (milliseconds)
             )
         );
    }
    public void WriteToArduino(string message)
    {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) stream.Close();
        if (Input.GetKeyDown(KeyCode.O)) stream.Open();
    }
    public string ReadFromArduino(int timeout = 0)
    {
        stream.ReadTimeout = timeout;
        try
        {
            return stream.ReadLine();
        }
        catch (TimeoutException e)
        {
            return null;
        }

    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        while (true)
        {
            //Debug.Log("Distance: " + Distance);
            DateTime initialTime = DateTime.Now;
            DateTime nowTime;
            TimeSpan diff = default(TimeSpan);

            string dataString = null;

            do
            {
                stream.Open();
                yield return new WaitForSeconds(.1f);
                WriteToArduino("DISTANCE");
                yield return new WaitForSeconds(.1f);
                try
                {
                    dataString = stream.ReadLine();
                }
                catch (TimeoutException)
                {
                    dataString = null;
                }
                yield return new WaitForSeconds(.1f);
                stream.Close();
                if (dataString != null)
                {
                    callback(dataString);
                    //Debug.Log(dataString);
                    try
                    {
                        float number = float.Parse(dataString);

                        if (number < 300)
                        {
                            Distance = number;
                        }

                    }catch(FormatException e)
                    {
                        Debug.LogError(e);
                    }
                    yield return null; // Terminates the Coroutine 5mal pro sek
                }
                else
                    yield return null; // Wait for next frame

                nowTime = DateTime.Now;
                diff = nowTime - initialTime;

            } while (diff.Milliseconds < timeout);

            if (fail != null)
                fail();
            yield return null;
        }
    }
}
