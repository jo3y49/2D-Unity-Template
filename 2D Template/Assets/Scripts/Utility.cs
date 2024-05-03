using System.Collections;
using UnityEngine;

public static class Utility
{
    public static string FormatTimeToString(float time)
    {
        int hours = (int)time / 3600;
        int minutes = (int)time % 3600 / 60;
        int seconds = (int)time % 3600 % 60;

        return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public static void SwitchActiveObjects(GameObject previous, GameObject next)
    {
        previous.SetActive(false);
        next.SetActive(true);
    }
}