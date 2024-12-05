using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public static CheckPointController instance;

    public CheckPoint[] checkpoints;

    public Vector3 spawnPoint;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Nueva forma recomendada de buscar objetos
        checkpoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);

        spawnPoint = PlayerController.instance.transform.position;
    }
    
    public void DeactivateCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].ResetCheckpoint();
        }
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}