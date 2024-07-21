//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: July 2024
// Description: Utility functions for ESave.
//***************************************************************************************

using System;
using UnityEngine;

public static class ESaveUtility
{
    /// <summary>
    /// Sets the current threading culture info.
    /// </summary>
    /// <param name="name">The name (code) of the locale.</param>
    public static void SetCulture(string name)
    {
        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(name);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
