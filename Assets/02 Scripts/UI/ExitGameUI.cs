using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class ExitGameUI : MonoBehaviour
{
    public GameObject ExitGameUICoins;
    public List<GameObject> ExitGameUIEnergy;
    public GameObject ExitGameUIGameTime;
    public GameObject ExitGameUIGameTimeDay;

    public GameObject playerName;
    void OnEnable()
    {
        if (DataManager.Instance != null)
        {
            GameStats stats = DataManager.Instance.DeserializeJson();
            if (stats != null)
            {
                // Coins
                ExitGameUICoins.GetComponent<TextMeshProUGUI>().text = GetUICoins();

                // Energy
                foreach(GameObject go in ExitGameUIEnergy)
                    go.SetActive(false);
                ExitGameUIEnergy[GetUIEnergy()].SetActive(true);

                // Time
                ExitGameUIGameTime.GetComponent<TextMeshProUGUI>().text = GetUIGameTime();
                ExitGameUIGameTimeDay.GetComponent<TextMeshProUGUI>().text = GetUIGameTimeDay();

                // Player name
                playerName.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.playerName;
            }
        }
    }

	string GetUICoins() => GameManager.Instance.playerCoins.ToString();
    int GetUIEnergy() => GameManager.Instance.playerEnergy;
    string GetUIGameTime() => TimeSystem.Instance.FormatTime();

    string GetUIGameTimeDay() => "Day " + TimeSystem.Instance.days.ToString();

}
