using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLifetime = 5f;

    private void Start()
    {
        // set bullet lifetime
        Object.Destroy(gameObject, bulletLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // disable both bullet and enemy
            SoundManager.Instance.PlaySfx("EnemyDeath");
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
    }
}