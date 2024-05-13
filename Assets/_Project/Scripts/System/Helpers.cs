using System;
using System.Collections;
using UnityEngine;

public static class Helpers
{
    public static int CompareDistanceBetweenPoints(Vector3 point1, Vector3 point2, float distance)
    {
        float squareDistance = (point1 - point2).sqrMagnitude;

        if (squareDistance > distance * distance)
            return 1;
        if (squareDistance < distance * distance)
            return -1;
        return 0;
    }

    public static int CompareVectorMagnitude(Vector3 vector, float valueToCompare)
    {
        if (vector.sqrMagnitude > valueToCompare * valueToCompare)
            return 1;
        if (vector.sqrMagnitude < valueToCompare * valueToCompare)
            return -1;
        return 0;
    }

    public static Vector3 RotateByAngle(Vector3 current, float angle)
    {
        Vector3 result = Vector3.zero;

        result.x = current.x * Mathf.Cos(angle) - current.z * Mathf.Sin(angle);
        result.z = current.x * Mathf.Sin(angle) + current.z * Mathf.Cos(angle);

        return result;
    }

    public static IEnumerator SetActive(GameObject obj, bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(active);
    }

    public static IEnumerator SetActive(GameObject obj, bool active, float delay, Action doBefore)
    {
        yield return new WaitForSeconds(delay);
        doBefore();
        obj.SetActive(active);
    }

    public static IEnumerator OnNextFrame(Action action)
    {
        yield return null;
        action();
    }

    public static IEnumerator InvokeAfterTime(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}


public class Timer
{
    public float time;
    public Action onTime;
    public bool playing;
    public float currentTime { get; private set; }

    public Timer(bool playing = true)
    {
        this.time = 1f;
        this.onTime = () => { };
        this.playing = playing;
    }

    public Timer(float time, bool playing = true)
    {
        this.time = time;
        this.onTime = () => { };
        this.playing = playing;
    }

    public Timer(float time, Action onTime, bool playing = true)
    {
        this.time = time;
        this.onTime = onTime;
        this.playing = playing;
    }

    public void Update() 
    {
        if (!playing) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0) 
        {
            onTime();
            currentTime = time;
        }
    }

    public void Reset() 
    {
        currentTime = time;
    }
}