using UnityEngine;
using System;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem Instance;

    public int hours;
    public int minutes;
    public int days = 1;

    public float realTimeToGameMinute = 0.1f; // 1/2 segundo de tiempo real equivale a 1 minuto en el juego
    private float timer;

    public event Action OnMinuteChanged;
    public event Action OnHourChanged;
    public event Action OnDayChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= realTimeToGameMinute)
        {
            timer -= realTimeToGameMinute;
            AddMinute();
        }
    }

    private void AddMinute()
    {
        minutes++;
        OnMinuteChanged?.Invoke();

        if (minutes >= 60)
        {
            minutes = 0;
            AddHour();
        }
    }

    private void AddHour()
    {
        hours++;
        OnHourChanged?.Invoke();

        if (hours >= 24)
        {
            hours = 0;
            AddDay();
        }
    }

    private void AddDay()
    {
        days++;
        OnDayChanged?.Invoke();
    }

    public string FormatTime()
    {
        string period = "AM";
        int displayHours = hours;

        if (hours >= 12)
        {
            period = "PM";
            if (hours > 12)
            {
                displayHours -= 12;
            }
        }
        else if (hours == 0)
        {
            displayHours = 12;
        }

        return string.Format("{0:D2}:{1:D2} {2}", displayHours, minutes, period);
    }
}
