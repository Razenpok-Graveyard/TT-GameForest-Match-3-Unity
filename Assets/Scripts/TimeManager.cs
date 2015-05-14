using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    
    //public delegate void Action();
    private static Text textField;

    private static float startTime;
    private const int TimeLimit = 60;
    public static Action OnTimeUp;

    void Start()
    {
        startTime = Time.time;
        textField = gameObject.GetComponent<Text>();
        if (!textField)
            Destroy(this);
    }

    private void Update()
    {
        var remainingTime = TimeLimit - Time.time + startTime;
        textField.text = "Time: " + (remainingTime % 60).ToString("F0");
        if (remainingTime > 0) return;
        OnTimeUp();
        Destroy(this);
    }
}
