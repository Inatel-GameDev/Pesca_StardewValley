using UnityEngine;
using System.Collections;

public class AguaPula : MonoBehaviour
{
    int contTime;
    private Animator anim;
    public string idle = "Idle";
    public string pula = "Pula";

    public AudioSource audioSource; // Referência ao AudioSource
    public AudioClip som;
    int prob = 0;

    void Start()
    {
        // Obtém automaticamente o Animator do GameObject onde o script está anexado
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator não encontrado! Adicione um Animator ao GameObject.");
        }

        // Obtém automaticamente o AudioSource do GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource não encontrado! Adicione um AudioSource ao GameObject.");
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
                // Animação
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
            Debug.LogError("Animator não está inicializado!");
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
            Debug.LogError("Animator não está inicializado!");
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
