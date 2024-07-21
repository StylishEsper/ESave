//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: July 2024
// Description: Utility functions for ESave.
//***************************************************************************************

using System;
using System.Globalization;
using System.Threading;
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
            CultureInfo cultureInfo = new CultureInfo(name);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
