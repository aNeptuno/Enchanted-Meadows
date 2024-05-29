using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    public GameObject StartGameUICoins;
    public List<GameObject> StartGameUIEnergy;
    public GameObject StartGameUIGameTime;
    public GameObject StartGameUIGameTimeDay;

    public GameObject playerName;
    void OnEnable()
    {
        if (DataManager.Instance != null)
        {
            GameStats stats = DataManager.Instance.DeserializeJson();
            if (stats != null)
            {
                string text = "Loaded data (game stats): \r\n" + JsonConvert.SerializeObject(stats, Formatting.Indented);
                Debug.Log(text);
                // Coins
                StartGameUICoins.GetComponent<TextMeshProUGUI>().text = stats.PlayerCoins.ToString();

                // Energy
                foreach(GameObject go in StartGameUIEnergy)
                    go.SetActive(false);
                StartGameUIEnergy[stats.PlayerEnergy].SetActive(true);

                // Time
                StartGameUIGameTime.GetComponent<TextMeshProUGUI>().text = DataManager.Instance.FormatTime(stats);
                StartGameUIGameTimeDay.GetComponent<TextMeshProUGUI>().text = "Day " + stats.Days.ToString();

                // Player name
                playerName.GetComponent<TextMeshProUGUI>().text = stats.PlayerName;
            }
        }
    }

}
