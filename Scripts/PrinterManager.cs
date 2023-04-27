using UnityEngine;
using System;
using LCPrinter;
using System.Collections.Generic;

public class PrinterManager : MonoBehaviour
{
    public string printerName = "PrinterHP";
    public int copies = 1;
    public List<string> paths = new List<string>();
    public void PrintByPathButton()
    {
        int index = (int)UnityEngine.Random.Range(0, paths.Count - 1);
        Debug.Log(index + " PRINTER");
        Print.PrintTextureByPath(paths[index].Trim(), copies, printerName);
    }
}
