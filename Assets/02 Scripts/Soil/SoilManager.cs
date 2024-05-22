using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilManager : MonoBehaviour
{
    public GameObject prefab;

    public Transform playerTransform;

    public List<SoilController> soilControllers;

    void Start()
    {
        int rows = 6;
        int columns = 6;
        float spacingX = 0f;
        float spacingY = 0f;

        for (int i = 0; i < rows; i++)
        {
            spacingX = 0f;
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = transform.position + new Vector3(-0.5f + spacingX, 1 - spacingY, 0);
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
        Vector3 playerDirection = playerTransform.right;

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

        return null;
    }
}
