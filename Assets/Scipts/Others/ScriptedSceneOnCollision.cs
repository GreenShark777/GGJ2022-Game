using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptedSceneOnCollision : MonoBehaviour, iScriptableScene
{

    [SerializeField] private DoorsBehaviour doorBehaviour;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Animator uiAnimator;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private float cameraFollowSpeed = 3;

    [Space]
    [Header("Timers")]
    [SerializeField] private float waitTimeAfterMoveCamera = 2f; // Tempo di attesa tra il movimento della camera sulla porta e l'apertura della stessa
    [SerializeField] private float waitTimeAfterDoorOpen = 2f; // Tempo di attesa tra l'apertura della porta ed il fade in del bianco
    [SerializeField] private float waitTimeAfterFade = 2f; // Tempo di attesa tra il fade in bianco ed lo switch di scena

    private int fadeBoolHash = Animator.StringToHash("FadeIn");

    // Flag che ci permette di non far avviare più volte l'animazione per errore
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartScene();
        }
    }

    public void StartScene()
    {
        StartCoroutine(ScriptedScene());
    }

    IEnumerator ScriptedScene()
    {

        PauseManager.SetPauseState(true);

        // Muovi la telecamera sulla porta finale
        if (cameraFollow)
        {
            cameraFollow.SetFollowSpeed(cameraFollowSpeed);
            cameraFollow.SetYOffset(0);
            cameraFollow.SetTarget(doorTransform);
        }

        yield return new WaitForSecondsRealtime(waitTimeAfterMoveCamera);

        // Avvia l'apertura della porta
        if (doorBehaviour)
            StartCoroutine(doorBehaviour.OpenDoor());

        yield return new WaitForSecondsRealtime(waitTimeAfterDoorOpen);

        // Avvia il fade dell'immagine bianca
        Image image = uiAnimator.transform.GetComponent<Image>();
        if (image)
            image.color = Color.white;

        uiAnimator.speed = .5f;
        uiAnimator.SetBool(fadeBoolHash, true);

        yield return new WaitForSecondsRealtime(waitTimeAfterFade);

        SceneManager.LoadScene("Endings");
    }
}