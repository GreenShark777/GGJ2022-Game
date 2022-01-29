using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    [SerializeField] private Transform pupilla;

    [SerializeField] private float Radius = 0.2f;

    private Transform playerTransform;

    private Vector2 _centre;
    private float _angle;

    private void Start()
    {
        _centre = pupilla.localPosition;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (!playerTransform)
            Debug.LogError("Eye can't find player transform");
    }

    private void LateUpdate()
    {
        // Un pò di matematica per trovare la posizione della pupilla tale che guardi al player
        Vector2 targetVector = (playerTransform.position - transform.position);
        _angle = Vector2.SignedAngle(Vector2.right, targetVector);

        var offset = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad)) * Radius;
        pupilla.localPosition = _centre + offset;
    }
}
