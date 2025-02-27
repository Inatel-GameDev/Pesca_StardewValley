using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject hookPrefab;
    private Animator anim;

    public string idle = "IDLE";
    public string trowHook = "TROWHOOK";
    public string fishing = "FISHING";
    public bool isFishing = false;
    public float money;
    public Text moneyUI;

    void Start()
    {
        anim = GetComponent<Animator>();
        PlayAnimation(idle);
        money = 0f;
    }

    public void PlayAnimation(string animation)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animation))
            return; // Evita que a mesma animação seja reproduzida repetidamente

        anim.Play(animation);
        StartCoroutine(WaitForAnimation(animation));
    }

    IEnumerator WaitForAnimation(string animation)
    {
        yield return new WaitForEndOfFrame(); // Espera um frame para garantir que a animação foi ativada
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        // Aqui você pode fazer algo após a animação acabar
        if (animation == trowHook)
        {
            PlayAnimation(fishing); // Exemplo: Mudar para estado de "pescando" após lançar o anzol
        }
    }

    void Update()
    {
        GameObject canvasNota = GameObject.Find("CanvasNota(Clone)");
        if (Input.GetMouseButtonDown(0) && !isFishing && canvasNota == null)
        {
            isFishing = true;
            PlayAnimation(trowHook);
            Instantiate(hookPrefab, new Vector3(2.5f,0,0), Quaternion.identity);
        }
        moneyUI.text = "R$"+ money.ToString("F2");
    }
}
