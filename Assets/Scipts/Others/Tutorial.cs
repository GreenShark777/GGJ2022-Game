using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] float animTime = 2f;

    [SerializeField] private Animator revealAnimator;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Transform doorTransform;

    private Camera mainCam;
    private float prevSize;

    private int revealTriggerHash = Animator.StringToHash("reveal");

    private Transform playerTransform;

    public void StartAnim()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (!playerTransform)
            Debug.LogError("Tutorial script can't find player reference");

        cameraFollow.SetTarget(doorTransform);
        revealAnimator.SetTrigger(revealTriggerHash);

        mainCam = cameraFollow.GetComponent<Camera>();

        if(mainCam)
        {
            prevSize = mainCam.orthographicSize;
            mainCam.orthographicSize = mainCam.orthographicSize * .7f;
        }

        StartCoroutine(AnimTimer());
    }

    IEnumerator AnimTimer()
    {
        yield return new WaitForSeconds(animTime);

        if (mainCam)
        {
            mainCam.orthographicSize = prevSize;
        }

        cameraFollow.SetTarget(playerTransform);
    }
}