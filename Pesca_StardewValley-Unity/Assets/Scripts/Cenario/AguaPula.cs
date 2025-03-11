using UnityEngine;
using System.Collections;

public class AguaPula : MonoBehaviour
{
    int contTime;
    private Animator anim;
    public string idle = "Idle";
    public string pula = "Pula";

    public AudioSource audioSource; // Refer�ncia ao AudioSource
    public AudioClip som;
    int prob = 0;

    void Start()
    {
        // Obt�m automaticamente o Animator do GameObject onde o script est� anexado
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator n�o encontrado! Adicione um Animator ao GameObject.");
        }

        // Obt�m automaticamente o AudioSource do GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource n�o encontrado! Adicione um AudioSource ao GameObject.");
            }
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    void FixedUpdate()
    {
        contTime++;
        if (contTime > 1500)
        {
            prob = Random.Range(0, 100);
            if (prob < 10)
            {
                // Anima��o
                if (anim != null)
                {
                    PlayAnimation(pula);
                }

                // Som
                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
            // Zera o timer
            contTime = 0;
        }
    }

    public void PlayAnimation(string animation)
    {
        if (anim == null)
        {
            Debug.LogError("Animator n�o est� inicializado!");
            return;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animation))
            return;

        anim.Play(animation);
        StartCoroutine(WaitForAnimation(animation));
    }

    IEnumerator WaitForAnimation(string animation)
    {
        yield return new WaitForEndOfFrame();
        if (anim == null)
        {
            Debug.LogError("Animator n�o est� inicializado!");
            yield break;
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        if (animation == pula)
        {
            PlayAnimation(idle);
        }
    }
}
