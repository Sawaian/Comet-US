
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    [Header("Health")]
    public int health = 3;

    [Header("Debug")]
    [SerializeField] private bool invulnerable = false;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;

    [Header("Hit Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private float hitCooldown = 0.5f;

    [Header("Life Icons")]
    [SerializeField] private Image[] lifeIcons;

    private Color originalColor;
    private Coroutine flashCoroutine;
    private bool isDead = false;
    private bool isRecovering = false;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer =
                GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        UpdateLives();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || invulnerable ||
            isRecovering || damage <= 0)
            return;

        health = Mathf.Max(0, health - damage);

        // Remove a life icon.
        UpdateLives();

        // Flash player red.
        FlashRed();

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageCooldown());
        }
    }

    private void UpdateLives()
    {
        if (lifeIcons == null || lifeIcons.Length == 0)
        {
            Debug.LogError("Life icons are not assigned!");
            return;
        }

        Debug.Log("Player health: " + health);

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (lifeIcons[i] != null)
            {
                bool showLife = i < health;

                lifeIcons[i].gameObject.SetActive(showLife);

                Debug.Log(
                    "Life " + (i + 1) + ": " + showLife
                );
            }
        }
    }

    private void FlashRed()
    {
        if (spriteRenderer == null) return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(flashDuration);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        flashCoroutine = null;
    }

    private IEnumerator DamageCooldown()
    {
        isRecovering = true;

        yield return new WaitForSeconds(hitCooldown);

        isRecovering = false;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(
                deathSound,
                Camera.main != null
                    ? Camera.main.transform.position
                    : transform.position
            );
        }

        Destroy(gameObject);

        GameManager.Instance.LoseGame();
    }
}
