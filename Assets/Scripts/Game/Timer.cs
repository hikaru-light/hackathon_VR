using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    static float countTime;
    static bool isCounting;

    public static void StartCount()
    {
        countTime = 0f;
        isCounting = true;
    }

    public static void StopCount()
    {
        isCounting = false;
    }

    public static string GetTimeStr()
    {
        return countTime.ToString("0.00");
    }

    void Update()
    {
        if (!isCounting) { return; }
        countTime += Time.deltaTime;
    }
}
