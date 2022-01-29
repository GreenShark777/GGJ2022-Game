using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject1 : MonoBehaviour
{
    [SerializeField] private GameObject eye;

    [SerializeField] private float fallSpeed = 7f;
    [SerializeField] private float ariseSpeed = 4f;
    [SerializeField] private float beforeFallTime = .5f;
    [SerializeField] private float stunTime = 2f;

    private Rigidbody2D rb;

    private Vector3 startPos;
    private bool terrainCollided;

    private bool isExecuting;

    private void Start()
    {
        isExecuting = false;
        startPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
    }

    public void PlayerTrigger()
    {
        if (isExecuting) return;

        terrainCollided = false;
        isExecuting = true;
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(beforeFallTime);

        while(!terrainCollided)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(stunTime);

        bool flag = true;
        float lastDist = 100f;

        while (flag)
        {
            transform.Translate(Vector3.up * ariseSpeed * Time.deltaTime);
            float dist = Vector3.Distance(transform.position, startPos);

            if (dist <= .4f)
            {
                flag = false;
            }

            // Se dist è maggiore allora lo abbiamo superato
            if (dist < lastDist)
                lastDist = dist;
            else
                flag = false;

            yield return null;
        }

        transform.position = startPos;
        isExecuting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            terrainCollided = true;
        }
    }

    private void OnDisable()
    {
        rb.simulated = false;

        eye.SetActive(false);
        if (isExecuting)
            StopAllCoroutines();
    }
}
