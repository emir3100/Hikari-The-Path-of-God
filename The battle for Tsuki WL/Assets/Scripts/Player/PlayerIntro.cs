
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.XR;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerIntro : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody;
    private float speed;
    public AudioClip Land;
    private int nextScene;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();    
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void Update()
    {
        speed = rigidbody.velocity.magnitude;   
        animator.SetFloat("Speed", speed);
    }

    public void PlayerLanded()
    {
        GameManager.Instance.AudioSource.PlayOneShot(Land);
        StartCoroutine("ChangeScene");
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(nextScene);
    }
}
    