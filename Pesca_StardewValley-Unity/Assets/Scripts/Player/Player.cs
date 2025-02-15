using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject hookPrefab;
    private Animator anim;

    public string idle = "IDLE";
    public string trowHook = "TROWHOOK";
    public string fishing = "FISHING";

    void Start()
    {
        anim = GetComponent<Animator>();
        PlayAnimation(idle);
    }

    public void PlayAnimation(string animation)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animation))
            return; // Evita que a mesma anima��o seja reproduzida repetidamente

        anim.Play(animation);
        StartCoroutine(WaitForAnimation(animation));
    }

    IEnumerator WaitForAnimation(string animation)
    {
        yield return new WaitForEndOfFrame(); // Espera um frame para garantir que a anima��o foi ativada
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        // Aqui voc� pode fazer algo ap�s a anima��o acabar
        if (animation == trowHook)
        {
            PlayAnimation(fishing); // Exemplo: Mudar para estado de "pescando" ap�s lan�ar o anzol
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayAnimation(trowHook);
            Instantiate(hookPrefab, new Vector3(2.5f,0,0), Quaternion.identity);
        }
    }
}
