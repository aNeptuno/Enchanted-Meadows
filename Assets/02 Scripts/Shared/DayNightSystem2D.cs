using System.Collections.Generic;
using UnityEngine;

/*
- Creator:    Two TV Games (@gallighanmaker)
- Script:     Day And Night 2D System

    Disclaimer: Thanks TwoTvGames, I modified the script so its works eith my game logic :)
*/

public enum DayCycles // Enum with day and night cycles, you can change or modify with whatever you want
{
    Sunrise = 0,
    Day = 1,
    Sunset = 2,
    Night = 3,
    Midnight = 4
}

public class DayNightSystem2D : MonoBehaviour
{
    [Header("Controllers")]

    [Tooltip("Global light 2D component, we need to use this object to place light in all map objects")]
    public UnityEngine.Rendering.Universal.Light2D globalLight; // global light

    [Tooltip("This is a current cycle time, you can change for private float but we keep public only for debug")]
    public float cycleCurrentTime; // current cycle time

    /* [Tooltip("This is a cycle max time in seconds, if current time reach this value we change the state of the day and night cyles")]
    public float cycleMaxTime = 60; // duration of cycle */

    [Tooltip("This is a cycle max time in minutes, if current time reach this value we change the state of the day and night cyles")]
    [SerializeField] private float cycleMaxTime;

    /* [Tooltip("Enum with multiple day cycles to change over time, you can add more types and modify whatever you want to fits on your project")]
    public DayCycles dayCycle = DayCycles.Sunrise; // default cycle */

    #region  "Cycle colors"
    [Header("Cycle Colors")]

    [Tooltip("Sunrise color, you can adjust based on best color for this cycle")]
    public Color sunrise;

    [Tooltip("(Mid) Day color, you can adjust based on best color for this cycle")]
    public Color day;

    [Tooltip("Sunset color, you can adjust based on best color for this cycle")]
    public Color sunset;

    [Tooltip("Night color, you can adjust based on best color for this cycle")]
    public Color night;

    [Tooltip("Midnight color, you can adjust based on best color for this cycle")]
    public Color midnight;

    private List<Color> cycleColors;
    #endregion

    [Header("Objects")]
    [Tooltip("Objects to turn on and off based on day night cycles, you can use this example for create some custom stuffs")]
    public UnityEngine.Rendering.Universal.Light2D[] mapLights; // enable/disable in day/night states

    // My logic
    public DayCycles currentDayCycle;
    public DayCycles CurrentDayCycle
    {
        set
        {
            currentDayCycle = value;

            switch (currentDayCycle)
            {
                case DayCycles.Sunrise:
                    cycleMaxTime = 300f;
                    cycleCurrentTime = 0;
                    Debug.Log("Sunrise");
                    break;
                case DayCycles.Day:
                    cycleMaxTime = 300f;
                    cycleCurrentTime = 0;
                    Debug.Log("day");
                    break;
                case DayCycles.Sunset:
                    cycleMaxTime = 180f;
                    cycleCurrentTime = 0;
                    Debug.Log("Sunset");
                    break;
                case DayCycles.Night:
                    cycleMaxTime = 240f;
                    cycleCurrentTime = 0;
                    Debug.Log("night");
                    break;
                case DayCycles.Midnight:
                    cycleMaxTime = 300f;
                    cycleCurrentTime = 0;
                    Debug.Log("midnight");
                    break;
            }
        }
        get => currentDayCycle;
    }

    void Awake()
    {
        cycleColors = new List<Color> { sunrise, day, sunset, night, midnight };
    }

    void Start()
    {
        CurrentDayCycle = ParseFromTimeSystem();
        //dayCycle = DayCycles.Sunrise; // start with sunrise state
        globalLight.color = cycleColors[(int)CurrentDayCycle]; // start global color at sunrise
    }

    private DayCycles ParseFromTimeSystem()
    {
        int hours = TimeSystem.Instance.hours;

        // Map the hours to the appropriate day cycle
        if (hours >= 5 && hours < 10)
        {
             return DayCycles.Sunrise;
        }
        else if (hours >= 10 && hours < 17)
        {
            return DayCycles.Day;
        }
        else if (hours >= 17 && hours < 20)
        {
            return DayCycles.Sunset;
        }
        else if (hours >= 20 && hours < 24)
        {
            return DayCycles.Night;
        }
        else return DayCycles.Midnight;
    }

    void Update()
    {
        CurrentDayCycle = ParseFromTimeSystem();
        // --- Update cycle time with TimeSystem ---
        cycleCurrentTime += TimeSystem.Instance.minutes;//Time.deltaTime;

        // Check if cycle time reach cycle duration time
        if (cycleCurrentTime >= cycleMaxTime)
        {
            cycleCurrentTime = 0; // back to 0 (restarting cycle time)
            CurrentDayCycle++; // change cycle state
        }

        // If reach final state we back to sunrise (Enum id 0)
        if(CurrentDayCycle > DayCycles.Midnight)
            CurrentDayCycle = 0;

        // percent it's an value between current and max time to make a color lerp smooth
        float percent = cycleCurrentTime / cycleMaxTime;

        // Sunrise state (you can do a lot of stuff based on every cycle state, like enable animals only in sunrise )
        if(CurrentDayCycle == DayCycles.Sunrise)
        {
            ControlLightMaps(false); // disable map light (keep enable only at night)
            globalLight.color = Color.Lerp(sunrise, day, percent);
        }

        // Mid Day state
        if(CurrentDayCycle == DayCycles.Day)
            globalLight.color = Color.Lerp(day, sunset, percent);

        // Sunset state
        if(CurrentDayCycle == DayCycles.Sunset)
            globalLight.color = Color.Lerp(sunset, night, percent);

        // Night state
        if(CurrentDayCycle == DayCycles.Night)
        {
            ControlLightMaps(true); // enable map lights (disable only in day states)
            globalLight.color = Color.Lerp(night, midnight, percent);
        }

        // Midnight state
        if(CurrentDayCycle == DayCycles.Midnight)
            globalLight.color = Color.Lerp(midnight, day, percent);
    }



    /* private void OnEnable()
    {
        TimeSystem.Instance.OnMinuteChanged += UpdateDayNightCycle;
    }

    private void OnDisable()
    {
        TimeSystem.Instance.OnMinuteChanged -= UpdateDayNightCycle;
    }

    void UpdateDayNightCycle()
    {
        // Determine current time and map to a day cycle
        int hours = TimeSystem.Instance.hours;

        // Map the hours to the appropriate day cycle
        if (hours >= 6 && hours < 10)
        {
            ChangeCycle(DayCycles.Sunrise, sunrise, day);
        }
        else if (hours >= 10 && hours < 16)
        {
            ChangeCycle(DayCycles.Day, day, sunset);
        }
        else if (hours >= 16 && hours < 20)
        {
            ChangeCycle(DayCycles.Sunset, sunset, night);
        }
        else if (hours >= 20 && hours < 24)
        {
            ChangeCycle(DayCycles.Night, night, midnight);
        }
        else
        {
            ChangeCycle(DayCycles.Midnight, midnight, sunrise);
        }
    }

    void ChangeCycle(DayCycles newCycle, Color fromColor, Color toColor)
    {
        int minutes = TimeSystem.Instance.minutes;
        dayCycle = newCycle;
        float percent = (float)minutes / 60f;
        globalLight.color = Color.Lerp(fromColor, toColor, percent);

        if (newCycle == DayCycles.Night)
        {
            ControlLightMaps(true); // enable map lights at night
        }
        else
        {
            ControlLightMaps(false); // disable map lights during the day
        }
    } */

    void ControlLightMaps(bool status)
    {
        // Loop in light array of objects to enable/disable
        foreach (UnityEngine.Rendering.Universal.Light2D _light in mapLights)
        {
            _light.gameObject.SetActive(status);
        }
    }
}


