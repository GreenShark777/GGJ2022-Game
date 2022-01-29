using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject2 : MonoBehaviour
{
    [SerializeField] private GameObject eye;

    [SerializeField] private float beforeFallTime = 2f;
    [SerializeField] private float fallSpeed = 7f;

    private bool terrainCollided;
    private bool isPetrified = false;
    private bool isExecuting = false;
    private bool isDestroyed = false;

    private Rigidbody2D rb;
    private CharacterMovement characterMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void StartFall()
    {
        if (isExecuting || isDestroyed) return;

        isExecuting = true;
        StartCoroutine(Fall());
        characterMovement.onLandedCallback -= StartFall;
    }

    IEnumerator Fall()
    {
        Debug.Log("FAAALl");
        yield return new WaitForSeconds(beforeFallTime);

        terrainCollided = false;

        while (!terrainCollided)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return null;
        }

        isExecuting = false;
        isDestroyed = true;
        rb.simulated = false;
    }

    private void OnDisable()
    {
        eye.SetActive(false);
        isPetrified = true;

        this.enabled = true;
    }

    private void OnEnable()
    {
        eye.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isPetrified) return;

        //Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Ground"))
        {
            terrainCollided = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            characterMovement = collision.gameObject.GetComponent<CharacterMovement>();
            characterMovement.onLandedCallback += StartFall;
        }
    }
}
