using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    public GameObject[] garbagePrefabs; // Assign prefabs (e.g., bottle, can)
    public float spawnInterval = 2f; // Time between spawns
    public bool spawnFromLeft = true; // Direction of movement

    private bool isSpawning = true;

    void Start()
    {
        InvokeRepeating(nameof(SpawnGarbage), 1f, spawnInterval);
    }

    void SpawnGarbage()
    {
        if (!isSpawning) return;

        // Spawn at the exact position of the spawner
        Vector3 spawnPos = transform.position;

        // Pick a random garbage object from the list
        GameObject prefab = garbagePrefabs[Random.Range(0, garbagePrefabs.Length)];
        GameObject garbage = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Get its movement script
        GarbageProjectile projectile = garbage.GetComponent<GarbageProjectile>();

        // Flip speed and sprite direction based on side
        projectile.speed *= spawnFromLeft ? 1 : -1;

        garbage.transform.localScale = new Vector3(
            spawnFromLeft ? 1 : -1, 1, 1
        );
    }

    // Optional: Call this method to stop spawning externally (e.g., when player touches boat)
    public void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke(nameof(SpawnGarbage));
    }
}
