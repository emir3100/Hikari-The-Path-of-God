using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private CharacterController2D characterController;
    private Animator animator;

    private float horizontalMove = 0f;

    public float RunSpeed = 40f;

    [Header("Sounds")]
    private AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip[] hurtSounds;
    public AudioClip hurtSound;
    public AudioClip stepSound;
    public AudioClip grassSound;
    [System.NonSerialized] public int whichHurtSound;
    [System.NonSerialized] public string groundType = "grass";

    private static PlayerMovement instance;

    public static PlayerMovement Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<PlayerMovement>();
            return instance;
        }
    }

    private void Awake()
    {
        characterController = CharacterController2D.Instance;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SetGroundType();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * RunSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    private void FixedUpdate()
    {
        characterController.Move(horizontalMove * Time.fixedDeltaTime, false, false);
    }

    public void SetGroundType()
    {
        //If we want to add variable ground types with different sounds, it can be done here
        switch (groundType)
        {
            case "Grass":
                stepSound = grassSound;
                break;
        }
    }

    public void PlayStepSound()
    {
        //Play a step sound at a random pitch between two floats, while also increasing the volume based on the Horizontal axis
        audioSource.pitch = (Random.Range(0.9f, 1.1f));
        audioSource.PlayOneShot(stepSound, Mathf.Abs(Input.GetAxis("Horizontal") / 10));
    }

}
