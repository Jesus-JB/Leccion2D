using UnityEngine;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerTransform;
    public Camera mainCamera;

    [Header("Prefabs")]
    public GameObject platformPrefab;
    public GameObject obstaclePrefab;

    [Header("Parámetros de Generación")]
    public float platformWidth = 3f;
    public float platformHeight = 0.5f;
    public float generationDistance = 10f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Opciones de Obstáculos")]
    public float obstacleSpawnChance = 0.5f;

    private List<GameObject> activePlatforms = new List<GameObject>();
    private float lastPlatformX;

    void Start()
    {
        // Generar una plataforma inicial en la posición del jugador
        Vector3 initialPlatformPosition = new Vector3(playerTransform.position.x, playerTransform.position.y - platformHeight / 2, 0);
        GameObject initialPlatform = Instantiate(platformPrefab, initialPlatformPosition, Quaternion.identity);
        activePlatforms.Add(initialPlatform);

        // Actualizar la última posición de plataforma generada
        lastPlatformX = initialPlatformPosition.x;

        // Generar plataformas iniciales en diferentes niveles
        for (int i = 0; i < 10; i++)
        {
            GeneratePlatform();
        }
    }

    void Update()
    {
        // Calcular la posición del borde derecho de la cámara
        float cameraRightEdge = mainCamera.transform.position.x + 
            mainCamera.orthographicSize * mainCamera.aspect;

        // Generar nuevas plataformas si estamos cerca del borde
        if (cameraRightEdge >= lastPlatformX - generationDistance)
        {
            GeneratePlatform();
        }

        // Limpiar plataformas que ya no son visibles
        CleanUpPlatforms();
    }

    void GeneratePlatform()
    {
        // Garantizar una separación mínima y máxima entre plataformas en Y
        float minYOffset = 2.0f; // Separación mínima entre plataformas en Y
        float maxYOffset = 4.0f; // Separación máxima entre plataformas en Y

        // Determinar el nivel de la nueva plataforma: abajo, medio, o arriba
        float[] levels = { minY, (minY + maxY) / 2, maxY }; // Niveles posibles
        float levelY = levels[Random.Range(0, levels.Length)];

        // Calcular la posición de la nueva plataforma
        Vector3 newPlatformPosition = new Vector3(
            lastPlatformX + platformWidth, // Posición X continua
            levelY,                        // Posición Y definida por los niveles
            0
        );

        // Crear la nueva plataforma
        GameObject newPlatform = Instantiate(platformPrefab, newPlatformPosition, Quaternion.identity);
        activePlatforms.Add(newPlatform);

        // Actualizar la posición de la última plataforma generada
        lastPlatformX = newPlatformPosition.x;

        // Generar obstáculo si corresponde
        if (Random.value < obstacleSpawnChance)
        {
            GenerateObstacle(newPlatform);
        }
    }

    void GenerateObstacle(GameObject platform)
    {
        // Posicionar obstáculo sobre la plataforma
        Vector3 obstaclePosition = platform.transform.position + new Vector3(
            Random.Range(-platformWidth/2, platformWidth/2),
            platformHeight/2 + 0.5f, 
            0
        );

        Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity, platform.transform);
    }

    void CleanUpPlatforms()
    {
        // Calcular posición del borde izquierdo de la cámara
        float cameraLeftEdge = mainCamera.transform.position.x - 
            mainCamera.orthographicSize * mainCamera.aspect;

        // Eliminar plataformas que están completamente fuera de la vista
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i].transform.position.x < cameraLeftEdge - platformWidth)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }
}