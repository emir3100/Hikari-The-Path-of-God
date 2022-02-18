using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    //public DialogueBoxController dialogueBoxController;
    public CameraEffects cameraEffects;
    [SerializeField] public AudioTrigger gameMusic;
    [SerializeField] public AudioTrigger gameAmbience;


    public AudioSource AudioSource;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void HurtEffect()
    {
        CharacterController2D.Instance.GetHit();
        AudioSource.PlayOneShot(PlayerMovement.Instance.hurtSound);
        StartCoroutine(FreezeFrameEffect());
        AudioSource.PlayOneShot(PlayerMovement.Instance.hurtSounds[PlayerMovement.Instance.whichHurtSound]);

        if (PlayerMovement.Instance.whichHurtSound >= PlayerMovement.Instance.hurtSounds.Length - 1)
        {
            PlayerMovement.Instance.whichHurtSound = 0;
        }
        else
        {
            PlayerMovement.Instance.whichHurtSound++;
        }
        cameraEffects.Shake(100, 1f);
    }

    public IEnumerator FreezeFrameEffect(float length = .007f)
    {
        Time.timeScale = .1f;
        yield return new WaitForSeconds(length);
        Time.timeScale = 1f;
    }
}
