using UnityEngine;

public class GarbageProjectile : MonoBehaviour
{
    public float speed = 5f;
    public int damageAmount = 1;
    public float lifetime = 5f;

    private bool hasHitShelter = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!hasHitShelter)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShelterChecker shelterCheck = other.GetComponent<PlayerShelterChecker>();

            if (shelterCheck == null || !shelterCheck.isUnderShelter)
            {
                PlayerHealth health = other.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damageAmount);
                }
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Shelter"))
        {
            hasHitShelter = true;
            Destroy(gameObject);
        }
    }
}
