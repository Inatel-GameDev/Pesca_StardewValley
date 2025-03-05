using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject hookPrefab;
    [SerializeField] public GameObject boiaPrefab;
    private Animator anim;

    public string idle = "IDLE";
    public string trowHook = "TROWHOOK";
    public string fishing = "FISHING";
    public bool isFishing = false;
    public float money;
    public Text moneyUI;

    //Variaveis novas
    public Vector2 movement;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    public float gerarPesca; //determina a posição do minigame
    public float gerarBoia; //determina a posição da boia
    public Boia boia;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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

    public void Andar()
    {
        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if(movement.x >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            gerarPesca = -2f; //gera na esquerda
            gerarBoia = 3f; 
        }
        else if(movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            gerarPesca = 2f; //gera na direita
            gerarBoia = -3f;
        }

        //Animação
        if (movement.x != 0 || movement.y != 0)
        {
            //INSERIR AQUI AS ANIMAÇÕES DE ANDAR E IDLE
            //INSERIR AQUI AS ANIMAÇÕES DE ANDAR E IDLE
        }
        else
        {
        
        }

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
    
    public void comecaPescar()
    {
        PlayAnimation(trowHook);
        Instantiate(hookPrefab, new Vector3(gerarPesca, 1, 0), Quaternion.identity);
    }

    void FixedUpdate()
    {
        GameObject canvasNota = GameObject.Find("CanvasNota(Clone)");
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && !isFishing && canvasNota == null)
        {
            isFishing = true;
            Instantiate(boiaPrefab, new Vector3(gerarBoia*10, (rb.position.y - 0.5f), 0), Quaternion.identity);
            boia = GameObject.Find("Boia(Clone)").GetComponent<Boia>();
            boia.move(gerarBoia, (rb.position.y));
            //Debug.Log(boia.landWater());
            //if (boia.landWater() == true)
            //{
            //    cPlayAnimation(trowHook);
            //    Instantiate(hookPrefab, new Vector3(gerarPesca, 1, 0), Quaternion.identity);
            //}
            //else
            //    isFishing = false;
        }
        moneyUI.text = "R$"+ money.ToString("F2");

        if(!isFishing){
            Andar();
        }
    }
}
