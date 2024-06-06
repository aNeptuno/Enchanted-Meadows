using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    #region "Singleton"
    public static DayNightSystem2D Instance;
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

    [Header("Controllers")]

    [Tooltip("Global light 2D component, we need to use this object to place light in all map objects")]
    public UnityEngine.Rendering.Universal.Light2D globalLight; // global light

    [Tooltip("This is a current cycle time, you can change for private float but we keep public only for debug")]
    public float cycleCurrentTime; // current cycle time

    [Tooltip("This is a cycle max time in minutes, if current time reach this value we change the state of the day and night cyles")]
    [SerializeField] private float cycleMaxTime;

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

    // Enum
    public DayCycles currentDayCycle;
    public DayCycles CurrentDayCycle
    {
        set
        {
            currentDayCycle = value;

            switch (currentDayCycle)
            {
                case DayCycles.Sunrise:
                    cycleMaxTime = 240f;
                    Debug.Log("<color=#FF66EE>Sunrise</color>");
                    AudioManager.Instance.PlayEnvironment("Day");
                    break;
                case DayCycles.Day:
                    cycleMaxTime = 300f;
                    Debug.Log("<color=#FF66GG>Day</color>");
                    AudioManager.Instance.PlayEnvironment("Day");
                    break;
                case DayCycles.Sunset:
                    cycleMaxTime = 180f;
                    Debug.Log("<color=#FF6699>Sunset</color>");
                    AudioManager.Instance.PlayEnvironment("Day");
                    break;
                case DayCycles.Night:
                    cycleMaxTime = 240f;
                    Debug.Log("<color=#66FF99>Night</color>");
                    AudioManager.Instance.StopEnvironment();
                    break;
                case DayCycles.Midnight:
                    cycleMaxTime = 360f;
                    Debug.Log("<color=#6699FF>Midnight</color>");
                    AudioManager.Instance.PlayEnvironment("Night");
                    break;
            }
        }
        get => currentDayCycle;
    }

    [Header("Animals")]
    public List<GameObject> animalsInScene;

    public float percent;

    void Start()
    {
        cycleColors = new List<Color> { sunrise, day, sunset, night, midnight };
        CurrentDayCycle = ParseFromTimeSystem();
        globalLight.color = cycleColors[(int)CurrentDayCycle]; // start global color at sunrise
    }

    private DayCycles ParseFromTimeSystem()
    {
        int hours = TimeSystem.Instance.hours;

        // Map the hours to the appropriate day cycle
        if (hours >= 6 && hours < 10)
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



    // --- Update cycle time with TimeSystem ---
    public void UpdateCycle(float minutes)
    {
        if (CurrentDayCycle != ParseFromTimeSystem())
        {
            CurrentDayCycle = ParseFromTimeSystem();
            TimeSystem.Instance.TotalMinutesInCicle = 0;
        }

        // --- Update cycle time with TimeSystem ---
        cycleCurrentTime = minutes;

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
        percent = cycleCurrentTime / (cycleMaxTime * 10);

        // Sunrise state (you can do a lot of stuff based on every cycle state, like enable animals only in sunrise )
        if(CurrentDayCycle == DayCycles.Sunrise)
            globalLight.color = Color.Lerp(sunrise, day, percent);

        // Mid Day state
        if(CurrentDayCycle == DayCycles.Day)
            globalLight.color = Color.Lerp(day, sunset, percent);

        // Sunset state
        if(CurrentDayCycle == DayCycles.Sunset)
            globalLight.color = Color.Lerp(sunset, night, percent);

        // Night state
        if(CurrentDayCycle == DayCycles.Night)
            globalLight.color = Color.Lerp(night, midnight, percent);

        // Midnight state
        if(CurrentDayCycle == DayCycles.Midnight)
        {
            if (cycleCurrentTime / cycleMaxTime > 0.90)
                globalLight.color = Color.Lerp(midnight, day, percent);
        }

        ControlEnvironment();
    }

    // Enable map lights (disable only in day states)
    // Disable animals in scene (only show in day states)
    private void ControlEnvironment()
    {
        if (CurrentDayCycle > DayCycles.Sunset)
        {
            ControlLightMaps(true);
            ControlAnimals(false);
        }
        else
        {
            ControlAnimals(true); //enable animals in scene
            ControlLightMaps(false); // disable map light (keep enable only at night)
        }
    }


    void ControlLightMaps(bool status)
    {
        // Loop in light array of objects to enable/disable
        foreach (UnityEngine.Rendering.Universal.Light2D _light in mapLights)
        {
            _light.gameObject.SetActive(status);
        }
    }

    void ControlAnimals(bool status)
    {
        if (animalsInScene != null)
        foreach(GameObject animal in animalsInScene)
        {
            animal.SetActive(status);
        }
    }
}


