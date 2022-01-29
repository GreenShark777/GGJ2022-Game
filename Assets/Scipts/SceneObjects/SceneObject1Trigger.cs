using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject1Trigger : MonoBehaviour
{
    SceneObject1 sceneObject;

    private void Start()
    {
        sceneObject = GetComponentInParent<SceneObject1>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        CollisionsManager cm = collision.GetComponent<CollisionsManager>();

        if(/*collision.gameObject.CompareTag("Player")*/cm && cm.IsPlayer())
        {
            sceneObject.PlayerTrigger();
        }
    }
}
