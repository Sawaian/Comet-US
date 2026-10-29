
using System.Collections;
using UnityEngine;

public class MissileTruck : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float driveSpeed = 4f;

    [Header("Weapons")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireDelay = 2f;
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float spread = 0.3f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Health health;
    private Vector3 destination;

    public void Initialize(Vector3 target)
    {
        destination = target;
    }

    private void Awake()
    {
        health = GetComponent<Health>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (health != null)
            health.OnDamaged += PlayHit;

        animator.SetTrigger("spawn");
        StartCoroutine(DriveIn());
    }

    private IEnumerator DriveIn()
    {
        while (Vector3.Distance(
            transform.position, destination) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                driveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = destination;

        Debug.Log("Missile Truck stopped");

        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(fireDelay);
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        animator.SetTrigger("Fire");

        for (int i = 0; i < bulletCount; i++)
        {
            float offset = i - (bulletCount - 1) / 2f;

            Vector3 direction = new Vector3(
                offset * spread, 1f, 0f
            ).normalized;

            GameObject bullet = Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

            SnakeBullet movement =
                bullet.GetComponent<SnakeBullet>();

            if (movement != null)
                movement.Initialize(direction);
        }
    }

    private void PlayHit()
    {
        animator.SetTrigger("hit");
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnDamaged -= PlayHit;
    }
}
