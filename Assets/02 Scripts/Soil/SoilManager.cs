using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SoilManager : MonoBehaviour
{
    public GameObject prefab;

    public Transform playerTransform;

    public SoilController[,] soilControllers = new SoilController[6, 6];

    #region "Singleton"
    public static SoilManager Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // If not, set it to this instance
            //DontDestroyOnLoad(gameObject); // Make this instance persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if another instance already exists
        }
    }
    #endregion

    void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();
    }

    public void RemoveAllSoilControllers()
    {
        SoilController[] soilControllers = FindObjectsOfType<SoilController>();
        if (soilControllers != null)
        {
            foreach (SoilController soilController in soilControllers)
            {
                Destroy(soilController.gameObject);
            }
        } else Debug.Log("SOIL CONTROLLERS NULL");
    }
    public void GenerateSoil(bool temp)
    {
        playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();

        int rows = 6;
        int columns = 6;
        float spacingX = 0f;
        float spacingY = 0f;

        for (int i = 0; i < rows; i++)
        {
            spacingX = 0f;
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = transform.position + new Vector3(3f + spacingX, 0.7f - spacingY, 0);
                GameObject instance = Instantiate(prefab, position, Quaternion.identity);
                spacingX += 0.5f;

                // Add to matrix
                SoilController soilController = instance.GetComponent<SoilController>();
                soilController.iMatrix = i;
                soilController.jMatrix = j;
                soilControllers[i, j] = soilController;
            }
            spacingY += 0.5f;
        }
        LoadSoilState(temp);
    }

    public void LoadSoilState(bool temp)
    {
        MatrixSoilState LoadedSoil;
        if (!temp)
            LoadedSoil = DataManager.Instance.DeserializeJsonSoil();
        else
            LoadedSoil = DataManager.Instance.DeserializeJsonSoilTemp();

        if (LoadedSoil != null)
            DataManager.Instance.LoadSoilMatrix(LoadedSoil);
        else Debug.Log("matrix is null");

        string text = "Loaded soil: \r\n" + JsonConvert.SerializeObject(LoadedSoil, Formatting.Indented);
        //Debug.Log(text);

        foreach(SoilController soil in soilControllers)
            soil.LoadCropSprite();
    }

    #region "Update / Get Facing Soil"
    void Update()
    {
        if (soilControllers != null && playerTransform != null)
        {
            SoilController facingSoil = GetFacingSoil();
            if (facingSoil != null)
                facingSoil.IsFacing = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
        }
    }

    SoilController GetFacingSoil()
    {
        // Obtener la dirección hacia la que está mirando el jugador
        Vector3 playerDirection;
        if (playerTransform != null)
        {
            playerDirection = playerTransform.right;

            for (int i = 0; i < soilControllers.GetLength(0); i++)
            {
                for (int j = 0; j < soilControllers.GetLength(1); j++)
                {
                    SoilController soilController = soilControllers[i, j];
                    if (soilController != null)
                    {
                        Vector3 directionToSoil = soilController.transform.position - playerTransform.position;
                        float angle = Vector3.Angle(playerDirection, directionToSoil);

                        // Permitir un ángulo de visión de 45 grados (ajusta según sea necesario)
                        if (angle < 45f && soilController.PlayerInTrigger)
                        {
                            return soilController;
                        }
                    }
                }
            }
        }

        return null;
    }
    #endregion


    // Force grow crop
    public void ForceGrowCropOnEachSoil()
    {
        int rows = 6;
        int columns = 6;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (soilControllers[i, j] == null) Debug.Log("<color=red>soil controller is null</color>");
                if (soilControllers[i, j].currentCrop != null)
                {
                    soilControllers[i, j].ForceToGrow();
                    Debug.Log($"<color=red>Force to grow i {i} j: {j}</color>");
                }
            }
        }
    }
}
