
using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int health = 3;

    private int currentHealth;
    private bool isDead = false;

    // Events for GameManager and enemy animations.
    public event Action<GameObject> OnDeath;
    public event Action OnDamaged;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Hit Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        currentHealth = health;

        if (spriteRenderer == null)
        {
            spriteRenderer =
                GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Compatibility with existing laser.cs.
    public void hit(int damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0) return;

        currentHealth -= damage;

        // Notify enemy scripts to play hit animations.
        OnDamaged?.Invoke();

        // Flash red.
        FlashRed();

        // Play hit sound, including killing blows.
        PlayHitSound();

        if (currentHealth <= 0)
        {
            Die();
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

    private void PlayHitSound()
    {
        if (hitSound == null) return;

        AudioSource.PlayClipAtPoint(
            hitSound,
            GetSoundPosition(),
            0.7f
        );
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(
                deathSound,
                GetSoundPosition(),
                0.8f
            );
        }

        // Notify GameManager.
        OnDeath?.Invoke(gameObject);

        Destroy(gameObject);
    }

    private Vector3 GetSoundPosition()
    {
        if (Camera.main != null)
        {
            return Camera.main.transform.position;
        }

        return transform.position;
    }
}
