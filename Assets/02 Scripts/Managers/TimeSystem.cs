using UnityEngine;
using System;

public class TimeSystem : MonoBehaviour
{
    public int hours = 8;
    public int minutes;
    public int days = 1;

    public float realTimeToGameMinute = 0.5f; // 1/2 segundo de tiempo real equivale a 1 minuto en el juego
    private float timer;

    public event Action OnMinuteChanged;
    public event Action OnHourChanged;
    public event Action OnDayChanged;

    private int totalMinutesInCicle;

    public int TotalMinutesInCicle {set => totalMinutesInCicle = value;}

    #region "Singleton"
    public static TimeSystem Instance;
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
    #endregion

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameStats loadGame = DataManager.Instance.DeserializeJson();
            if (loadGame != null)
            {
                hours = loadGame.Hours;
                minutes = loadGame.Minutes;
                days = loadGame.Days;
            }
            else
            {
                // First time start
                hours = 8;
                minutes = 0;
            }

            totalMinutesInCicle = GetTotalMinutes();
            if (DayNightSystem2D.Instance != null)
                DayNightSystem2D.Instance.UpdateCycle(totalMinutesInCicle);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= realTimeToGameMinute)
        {
            timer -= realTimeToGameMinute;
            AddMinute();
            AddTotalMinutes();
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

    #region  "Day/Night cycle"
    private void AddTotalMinutes()
    {
        totalMinutesInCicle++;
        if (DayNightSystem2D.Instance != null)
            DayNightSystem2D.Instance.UpdateCycle(totalMinutesInCicle);
    }

    private int GetTotalMinutes()
    {
        if (hours >= 6 && hours < 10)
        {
            if (hours == 6) return minutes;
            else return minutes + (60 * Math.Abs(6-hours));
        }
        else if (hours >= 10 && hours < 17)
        {
            if (hours == 10) return minutes;
            else return minutes + (60 * Math.Abs(10-hours));
        }
        else if (hours >= 17 && hours < 20)
        {
            if (hours == 17) return minutes;
            else return minutes + (60 * Math.Abs(17-hours));
        }
        else if (hours >= 20 && hours < 24)
        {
            if (hours == 20) return minutes;
            else return minutes + (60 * Math.Abs(20-hours));
        }
        else
        {
            if (hours == 24) return minutes;
            else return minutes + (60 * hours);
        }

    }
    #endregion
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

    public void AddDay()
    {
        days++;
        OnDayChanged?.Invoke();
    }

    /// Bed
    public void AddEightHours()
    {
        int i = 0;
        while (i < 8)
        {
            AddHour();
            i++;
        }
        totalMinutesInCicle = GetTotalMinutes();
        if (DayNightSystem2D.Instance != null)
            DayNightSystem2D.Instance.UpdateCycle(totalMinutesInCicle);
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
