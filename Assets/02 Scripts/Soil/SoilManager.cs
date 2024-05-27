using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilManager : MonoBehaviour
{
    public GameObject prefab;

    public Transform playerTransform;

    public List<SoilController> soilControllers;

    #region "Singleton"
    public static SoilManager Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // If not, set it to this instance
            DontDestroyOnLoad(gameObject); // Make this instance persist across scenes
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

    public void GenerateSoil()
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
                //Vector3 position = new Vector3(-0.5f + spacingX, 1 - spacingY, 0);
                GameObject instance = Instantiate(prefab, position, Quaternion.identity);
                spacingX += 0.5f;
            }
            spacingY += 0.5f;
        }

        // Obtener todos los SoilControllers en la escena
        SoilController[] allSoilControllers = FindObjectsOfType<SoilController>();
        soilControllers = new List<SoilController>(allSoilControllers);
    }

    void Update()
    {
        if (playerTransform == null)
            playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();

        if (soilControllers != null)
        {
            SoilController facingSoil = GetFacingSoil();
            if (facingSoil != null)
                facingSoil.IsFacing = true;
        }
    }


    SoilController GetFacingSoil()
    {
        // Obtener la dirección hacia la que está mirando el jugador
        Vector3 playerDirection;
        if (playerTransform != null)
        {
            playerDirection = playerTransform.right;

            foreach (SoilController soilController in soilControllers)
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

        return null;
    }
}
