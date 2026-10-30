using System;
using UnityEngine;

public class enemy_1 : MonoBehaviour
{
    [SerializeField] Transform pool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator animator;

    [Header("Shooting Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    GameObject GO;
    public GameObject projectile;
    public GameObject FireBox;

    public float fireDelay = 10f;

    public float fireSpeed = 2f;

    public float offset = 0.5f;

    
    void Start()
    {
        GO = this.gameObject;
        //Find hitbox we will use to spawn bullets.
        FireBox = GO.transform.Find("Firebox").gameObject;
        Fire();
        
    }
    void Fire()
    {
        if (projectile == null)
        {
            Debug.LogError("Missing projectile on: " + gameObject.name, gameObject);
            return;
        }

        GameObject bullet = Instantiate(projectile,FireBox.transform, pool);
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        bullet.transform.position = FireBox.transform.position + Vector3.up * offset;
        animator.SetTrigger("Fire");
        //Debug.Log("Spawned: " + bullet.name);
        //Debug.Log("Projectile script: " + bullet.GetComponent<projectile>());


        Invoke("Fire", fireDelay);
    }
}
