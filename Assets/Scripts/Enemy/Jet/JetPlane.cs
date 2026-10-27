
using UnityEngine;

public class JetPlane : MonoBehaviour
{
    [Header("Flight")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float loopRadius = 2.5f;
    [SerializeField] private Vector2 loopCenter = new Vector2(0f, 2f);
    [SerializeField] private float leftEdge = -12f;
    [SerializeField] private float rightEdge = 12f;

    [Header("Shooting")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float fireInterval = 1.5f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Health health;

    private enum FlightState
    {
        Entering,
        Looping,
        Exiting
    }

    private FlightState state = FlightState.Entering;

    private float angle;
    private float nextShotTime;
    private bool isDead = false;

    void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (health == null)
            health = GetComponent<Health>();
    }

    void OnEnable()
    {
        if (health == null)
            return;

        health.OnDamaged += PlayHit;
        health.OnDeath += PlayExplosion;
    }

    void OnDisable()
    {
        if (health == null)
            return;

        health.OnDamaged -= PlayHit;
        health.OnDeath -= PlayExplosion;
    }

    void Start()
    {
        if (player == null)
        {
            GameObject target = GameObject.FindWithTag("Player");

            if (target != null)
                player = target.transform;
        }

        ResetFlight();
    }

    void Update()
    {
        if (isDead)
            return;

        MoveJet();

        if (player != null &&
            projectile != null &&
            Time.time >= nextShotTime)
        {
            Fire();
            nextShotTime = Time.time + fireInterval;
        }
    }

    void MoveJet()
    {
        Vector3 position = transform.position;

        switch (state)
        {
            case FlightState.Entering:
                {
                    float targetX = loopCenter.x - loopRadius;

                    position.x = Mathf.MoveTowards(
                        position.x,
                        targetX,
                        speed * Time.deltaTime
                    );

                    if (position.x >= targetX)
                    {
                        state = FlightState.Looping;
                        angle = Mathf.PI;
                    }

                    break;
                }

            case FlightState.Looping:
                {
                    float angularSpeed = speed / loopRadius;

                    angle += angularSpeed * Time.deltaTime;

                    position.x = loopCenter.x +
                                 Mathf.Cos(angle) * loopRadius;

                    position.y = loopCenter.y +
                                 Mathf.Sin(angle) * loopRadius;

                    if (angle >= 3f * Mathf.PI)
                    {
                        state = FlightState.Exiting;

                        position.x = loopCenter.x - loopRadius;
                        position.y = loopCenter.y;
                    }

                    break;
                }

            case FlightState.Exiting:
                {
                    position.x += speed * Time.deltaTime;

                    if (position.x >= rightEdge)
                    {
                        ResetFlight();
                        return;
                    }

                    break;
                }
        }

        transform.position = position;
    }

    void ResetFlight()
    {
        transform.position = new Vector3(
            leftEdge,
            loopCenter.y,
            transform.position.z
        );

        state = FlightState.Entering;
        angle = Mathf.PI;
    }

    void Fire()
    {
        if (firePoint == null ||
            projectile == null ||
            player == null)
            return;

        Vector3 direction =
            (player.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(
            projectile,
            firePoint.position,
            Quaternion.identity
        );

        JetProjectile jetBullet =
            bullet.GetComponent<JetProjectile>();

        if (jetBullet != null)
        {
            jetBullet.SetDirection(direction, bulletSpeed);
        }
    }

    private void PlayHit()
    {
        if (animator == null)
            return;

        animator.SetTrigger("Jet_hit");
    }

    private void PlayExplosion(GameObject jet)
    {
        isDead = true;

        if (animator == null)
            return;

        animator.ResetTrigger("Jet_hit");
        animator.SetTrigger("Jet_explosion");
    }
}
