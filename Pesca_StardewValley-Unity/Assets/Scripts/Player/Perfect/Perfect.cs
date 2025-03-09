using UnityEngine;
using System.Collections;

public class PlayAnimationAndDeactivate : MonoBehaviour
{
    private Animator animator;

    void OnEnable()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("❌ Nenhum Animator encontrado no objeto " + gameObject.name);
            return;
        }

        if (animator.HasState(0, Animator.StringToHash("Perfect")))
        {
            animator.Play("Perfect");
            StartCoroutine(WaitForAnimation());
        }
        else
        {
            Debug.LogError("❌ A animação 'Perfect' não foi encontrada no Animator!");
            gameObject.SetActive(false);
        }
    }

    private IEnumerator WaitForAnimation()
    {
        yield return null; // Aguarda um frame para garantir que a animação começou

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        if (animationLength == 0)
        {
            Debug.LogWarning("⚠️ O tempo da animação 'Perfect' foi retornado como 0. Usando um tempo padrão.");
            animationLength = 0.8f; // Define um tempo padrão se houver erro
        }

        yield return new WaitForSeconds(animationLength);
        gameObject.SetActive(false);
    }
}
