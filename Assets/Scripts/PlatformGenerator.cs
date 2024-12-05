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
        // Generar plataformas iniciales
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
        // Calcular posición de la nueva plataforma
        Vector3 newPlatformPosition = new Vector3(
            lastPlatformX + platformWidth, 
            Random.Range(minY, maxY), 
            0
        );

        // Crear plataforma
        GameObject newPlatform = Instantiate(platformPrefab, newPlatformPosition, Quaternion.identity);
        activePlatforms.Add(newPlatform);

        // Potencialmente añadir obstáculo
        if (Random.value < obstacleSpawnChance)
        {
            GenerateObstacle(newPlatform);
        }

        // Actualizar última posición de plataforma
        lastPlatformX = newPlatformPosition.x;
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