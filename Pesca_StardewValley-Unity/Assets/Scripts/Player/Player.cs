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

    public Vector2 movement;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    public float gerarPesca;
    public float gerarBoia;
    private Boia boia;

    public Slider forcaVara;
    public float valorForca;
    public float posicaoBoia;

    //Perfect
    public GameObject perfect;
    public bool podeAndar = true;
    private int prizeDraw = 0;

    public GameObject blackout;
    public GameObject bemVindo;
    public GameObject jogaVara;
    

    public string rarity;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayAnimation(idle);
        money = 0f;

        forcaVara.value = 0.1f;
        valorForca = 0.005f;
        forcaVara.gameObject.SetActive(false);

        //Bem vindo - jogaVara
        AtivarSequencialmente(bemVindo, blackout, 3, jogaVara, blackout, 5);
    }

    public void PlayAnimation(string animation)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animation))
            return;

        anim.Play(animation);
        StartCoroutine(WaitForAnimation(animation));
    }

    public void Andar()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            gerarPesca = transform.position.x - 3f;
            posicaoBoia = 5f;
            forcaVara.direction = Slider.Direction.LeftToRight;
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            gerarPesca = transform.position.x + 3f;
            posicaoBoia = -5f;
            forcaVara.direction = Slider.Direction.RightToLeft;
        }
        else{
            gerarPesca = transform.position.x - 3f;
            posicaoBoia = 5f;
        }
    }

    IEnumerator WaitForAnimation(string animation)
    {
        yield return new WaitForEndOfFrame();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        if (animation == trowHook)
        {
            PlayAnimation(fishing);
        }
    }

    public void comecaPescar()
    {
        Debug.Log("✅ comecaPescar() foi chamado! Criando a isca...");

        GameObject hookObj = Instantiate(hookPrefab, new Vector3(gerarPesca, 0, 0), Quaternion.identity);

        if (hookObj == null)
        {
            Debug.LogError("❌ ERRO: HookPrefab não foi instanciado corretamente!");
            isFishing = false; // Permite que o player volte a se mover
            return;
        }

        Debug.Log("🎣 HookPrefab criado com sucesso!");

        // Se a pesca não iniciar corretamente, permitir que o player volte a se mover
        StartCoroutine(VerificarPesca());
    }

    private IEnumerator VerificarPesca()
    {
        yield return new WaitForSeconds(1f); // Dá tempo para a pesca iniciar

        if (!GameObject.Find("Hook(Clone)"))
        {
            isFishing = false; // Reseta para evitar que o player fique travado
        }
    }

    public void SetFishingState(bool state)
    {
        isFishing = state;
        //Debug.Log("🎣 isFishing agora é: " + isFishing);
    }

    void FixedUpdate(){
        if (Input.GetKey(KeyCode.Mouse0) && !isFishing && GameObject.Find("CanvasNota(Clone)") == null && GameObject.Find("Pesca(Clone)") == null)
        {
           forcaVara.gameObject.SetActive(true);
           forcaVara.value = forcaVara.value + valorForca;
           podeAndar = false;
           if(forcaVara.value >= 1)
           {
               valorForca = -0.005f;
           }

           else if(forcaVara.value <= 0)
           {
               valorForca = 0.005f;
           }
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0)){

            isFishing = true; // Agora ele já marca que está pescando, evitando cliques múltiplos
            Debug.Log("Força: " + forcaVara.value);
            if (forcaVara.value > 0.85f)
                perfect.SetActive(true);
            prizeDraw = Random.Range(1, 101);
            Debug.Log("prizeDeaw: " + prizeDraw);
            if (forcaVara.value < 0.25f)
            {
                if(prizeDraw < 70)
                    rarity = "Comum";
                else if(70 <= prizeDraw && prizeDraw < 90)
                    rarity = "Raro";
                else if (90 <= prizeDraw && prizeDraw < 100)
                    rarity = "Épico";
                if (prizeDraw == 100)
                    rarity = "Lendário";
            }
            else if(0.25f <= forcaVara.value && forcaVara.value < 0.5f)
            {
                if (prizeDraw < 60)
                    rarity = "Comum";
                else if (60 <= prizeDraw && prizeDraw < 75)
                    rarity = "Raro";
                else if (80 <= prizeDraw && prizeDraw < 96)
                    rarity = "Épico";
                if (96 <= prizeDraw)
                    rarity = "Lendário";
            }
            else if (0.5f <= forcaVara.value && forcaVara.value < 0.75f)
            {
                if (prizeDraw < 50)
                    rarity = "Comum";
                else if (50 <= prizeDraw && prizeDraw < 70)
                    rarity = "Raro";
                else if (70 <= prizeDraw && prizeDraw < 92)
                    rarity = "Épico";
                if (92 <= prizeDraw)
                    rarity = "Lendário";
            }
            else if (0.75f <= forcaVara.value && forcaVara.value < 1)
            {
                if (prizeDraw < 40)
                    rarity = "Comum";
                else if (40 <= prizeDraw && prizeDraw < 65)
                    rarity = "Raro";
                else if (65 <= prizeDraw && prizeDraw < 88)
                    rarity = "Épico";
                if (88 <= prizeDraw)
                    rarity = "Lendário";
            }

                gerarBoia = transform.position.x + (forcaVara.value * posicaoBoia); //gera a boia
            forcaVara.gameObject.SetActive(false); //some o slider 

            //Debug.Log("🎯 Tentando iniciar a pesca...");

            GameObject boiaObj = Instantiate(boiaPrefab, new Vector3(gerarBoia, transform.position.y, 0), Quaternion.identity);

            if (boiaObj == null)
            {
                Debug.LogError("❌ ERRO: BoiaPrefab não foi instanciado!");
                isFishing = false; // Reseta caso falhe
                return;
            }

            boia = boiaObj.GetComponent<Boia>();
            if (boia == null)
            {
                Debug.LogError("❌ ERRO: O objeto Boia não tem o script Boia.cs anexado!");
                isFishing = false;
                return;
            }
            

            boia.Inicializar(this);
            //Debug.Log("🎣 Boia instanciada com sucesso!");
            forcaVara.value = 0.1f;
        }

        moneyUI.text = "R$" + money.ToString("F2");

        if (!isFishing && podeAndar)
        {
            Andar();
        }
    }
    //tuturial
    public IEnumerator AtivarObjetosPorTempo(GameObject obj1, GameObject obj2, float tempo)
    {
        // Ativa os objetos
        obj1.SetActive(true);
        obj2.SetActive(true);

        // Pausa o tempo
        Time.timeScale = 0;

        // Espera pelo tempo especificado (usando tempo real para ignorar a pausa do tempo)
        yield return new WaitForSecondsRealtime(tempo);

        // Desativa os objetos
        obj1.SetActive(false);
        obj2.SetActive(false);

        // Retorna o tempo ao normal
        Time.timeScale = 1;
    }

    // Função para rodar as ativações uma após a outra
    public void AtivarSequencialmente(GameObject obj1, GameObject obj2, float tempo1, GameObject obj3, GameObject obj4, float tempo2)
    {
        StartCoroutine(SequenciaDeAtivacao(obj1, obj2, tempo1, obj3, obj4, tempo2));
    }

    private IEnumerator SequenciaDeAtivacao(GameObject obj1, GameObject obj2, float tempo1, GameObject obj3, GameObject obj4, float tempo2)
    {
        // Chama a primeira ativação e espera ela terminar
        yield return StartCoroutine(AtivarObjetosPorTempo(obj1, obj2, tempo1));

        // Agora chama a segunda ativação depois que a primeira terminou
        yield return StartCoroutine(AtivarObjetosPorTempo(obj3, obj4, tempo2));
    }
}


