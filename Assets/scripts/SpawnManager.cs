using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager I { get; private set; }

    [Header("Prefabs")]
    public GameObject[] fishPrefabs;
    public GameObject[] trashPrefabs;

    [Header("Spawn Points")]
    public SpawnPoint[] spawnPoints;

    [Header("Respawn")]
    public float respawnDelay = 1.5f;

    [Header("Random Weights")]
    [Range(0f, 1f)] public float fishChance = 0.75f; // шанс что появится рыба, иначе мусор

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    void Start()
    {
        // первичный спавн на всех точках (можно только на части — если хочешь)
        foreach (var p in spawnPoints)
        {
            if (p == null) continue;
            SpawnRandomAt(p);
        }
    }

    public void OnCaughtAtPoint(SpawnPoint point)
    {
        if (point == null) return;
        point.ClearCurrent();

        // через время спавним новый объект там же
        Invoke(nameof(RespawnAllPending), 0f); // чтобы не плодить корутины — проще: отметим и проверим
        point.Schedule(respawnDelay);
    }

    void RespawnAllPending()
    {
        // ничего
    }

    void Update()
    {
        // простая система таймеров на точках
        foreach (var p in spawnPoints)
        {
            if (p == null) continue;
            if (p.IsReadyToRespawn())
                SpawnRandomAt(p);
        }
    }

    void SpawnRandomAt(SpawnPoint point)
    {
        if (point.HasObject) return;

        bool spawnFish = Random.value <= fishChance;

        GameObject prefab = null;
        if (spawnFish)
        {
            if (fishPrefabs == null || fishPrefabs.Length == 0) return;
            prefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        }
        else
        {
            if (trashPrefabs == null || trashPrefabs.Length == 0) return;
            prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
        }

        var go = Instantiate(prefab, point.transform.position, Quaternion.identity);
        point.SetCurrent(go);
    }
}
