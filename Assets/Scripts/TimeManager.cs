using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private const int TimeLimit = 60;
    //public delegate void Action();
    private static Text textField;
    private static float startTime;
    public static Action OnTimeUp;

    private void Start()
    {
        startTime = Time.time;
        textField = gameObject.GetComponent<Text>();
        if (!textField)
            Destroy(this);
    }

    private void Update()
    {
        var remainingTime = TimeLimit - Time.time + startTime;
        textField.text = "Time: " + (remainingTime%60).ToString("F0");
        if (remainingTime > 0) return;
        OnTimeUp();
        Destroy(this);
    }
}