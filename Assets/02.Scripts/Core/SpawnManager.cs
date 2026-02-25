using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Vector3 GetRandomSpawnPosition()
    {
        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index].position;
    }
}