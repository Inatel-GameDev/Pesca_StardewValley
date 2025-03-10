using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Peixe : MonoBehaviour
{

    [SerializeField] float crazyness;
    [SerializeField] float maxCrazyness;
    [SerializeField] float crazynessIncrease;
    [SerializeField] float DefaultCrazyness;
    [SerializeField] float maxVelocity;
    [SerializeField] float upDownSecurity;
    [SerializeField] public GameObject hookPrefab;
    [SerializeField] public GameObject notaPrefab;
    public Inventory inventory;
    public Player player;
    [SerializeField] public NotaDePesca nota;
    CrazyFish crazyIwasCrazyOnce;
    WeightFish weightFish;
    public int randomFish;
    // Os fishs
    [SerializeField] public GenericalFish[] fishes = new GenericalFish[]
        {
            new GenericalFish("Vegetty Cabeçudo",5,"Heavy",30,30,0.2f,"Crazy",50),
            new GenericalFish("Good",5,"Heavy",30,30,0.2f,"Weight",40),
            new GenericalFish("Lambari", 1,"Float",5,20,0.1f,"Weight",35),
            new GenericalFish("Baiacu",15,"Float",1.5f,30,0.01f,"Weight",50),
            new GenericalFish("Piranha",25, "Float",13.7f,70,0.4f,"Weight",70),
            new GenericalFish("Good", 20, "Heavy", 28.5f,40,0.4f,"Weight", 200)
        };
    [SerializeField] public string[] fishName;
    [SerializeField] public Sprite[] fishesImages;
    [SerializeField] public string[] fishesText;
    bool notaOn = false;
    private Rigidbody2D rb;
    public Slider progressBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        DefaultCrazyness = crazyness;
        rb = GetComponent<Rigidbody2D>();
        progressBar.value = 0.2f;
        // Criando um novo GameObject e adicionando o script VegettiCabeçudo corretamente
        crazyIwasCrazyOnce = new GameObject("CrazyFish").AddComponent<CrazyFish>();
        weightFish = new GameObject("WeightFish").AddComponent<WeightFish>();

        // Sorteio? Que sorteio? de qual peixe vai ser
        // Peixe 0 = Crazy fish - Peixe 1 = Weight fish

        if(player.rarity == "Comum")
        {
            Debug.Log("Comum");
            randomFish = Random.Range(0, 3);
        }
        else if(player.rarity == "Raro")
        {
            Debug.Log("Raro");
            randomFish = Random.Range(3, 3);
        }
        else if (player.rarity == "Épico")
        {
            Debug.Log("Épico");
            randomFish = Random.Range(4, 4);
        }
        else if (player.rarity == "Lendário")
        {
            Debug.Log("Lendário");
            randomFish = Random.Range(5, 5);
        }

            // Agora podemos inicializar os valores
            crazyIwasCrazyOnce.Setup(fishes[randomFish], rb);
        weightFish.Setup(fishes[randomFish], rb); // Vai ser o good peixe tubarãO
    }

    private void Update()
    {
        // Verifica se a boia já foi atribuída
        GameObject boiaInstance = GameObject.Find("Boia(Clone)");

        // Instancia a nota se o progresso for 1 e ainda não tiver sido criada
        if (progressBar.value == 1 && !notaOn)
        {
            Instantiate(notaPrefab, new Vector3(0, 3f, 0), Quaternion.identity);
        }

        // Se o progresso for 0, finaliza a pesca e destrói os objetos
        if (progressBar.value == 0)
        {
            player.PlayAnimation(player.idle);
            player.isFishing = false;

            // Destroi a boia apenas se a referência não for nula
            if (boiaInstance != null)
            {
                Destroy(boiaInstance);
            }

            // Destroi o hook se a referência não for nula
            if (hookPrefab != null)
            {
                Destroy(hookPrefab);
            }
        }

        GameObject canvasNota = GameObject.Find("CanvasNota(Clone)");
        if (canvasNota != null)
        {
            Image[] noteImage = canvasNota.GetComponentsInChildren<Image>();
            Text[] texts = canvasNota.GetComponentsInChildren<Text>();

            if (randomFish < fishName.Length)
            {
                noteImage[1].sprite = fishesImages[randomFish];
                texts[0].text = fishesText[randomFish];
                texts[1].text = fishName[randomFish];
                notaOn = true;
            }
        }
        if (progressBar.value == 1)
        {
            player.money += fishes[randomFish].price;
            player.PlayAnimation(player.idle);
            inventory.AddFish();
            player.isFishing = false;
            player.podeAndar = true;
            if (boiaInstance != null)
            {
                Destroy(boiaInstance);
            }
            Destroy(hookPrefab);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (fishes[randomFish].classFish == "Crazy")
        {
            //Debug.Log("Crazy? I was crazy once. They lockedme in a room");
            if (crazyIwasCrazyOnce != null)
                crazyIwasCrazyOnce.move();
            else
                Debug.Log("Crazy fish is null");
        }
        else
        {
            //Debug.Log("Weight fish");
            if (weightFish != null)
                weightFish.move();
            else
                Debug.Log("Weight fish is null");
        }
        // Segurança de velocidade para não deixar o peixe sair da tela
        if (rb.linearVelocityY > maxVelocity)
        {
            rb.linearVelocityY = maxVelocity;

        }
        if(rb.linearVelocityY < -maxVelocity)
        {
            rb.linearVelocityY = -maxVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hook")
        {
            Debug.Log("🐟 Peixe colidiu com o Hook! Iniciando pesca...");

            if (progressBar.value == 0)
            {
                progressBar.value = 0.2f; // Garante que a pesca começa e não é cancelada imediatamente
            }

            if (fishes[randomFish].classFish == "Crazy")
            {
                if (crazyIwasCrazyOnce != null)
                    crazyIwasCrazyOnce.startTriggerFish();
            }
            else
            {
                if (weightFish != null)
                    weightFish.startTriggerFish();
            }

            StartCoroutine(progressUp());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hook")
        {
            if (fishes[randomFish].classFish == "Crazy")
            {
                if (crazyIwasCrazyOnce != null)
                    crazyIwasCrazyOnce.stopTriggerFish();
            }
            else
            {
                if (weightFish != null)
                    weightFish.stopTriggerFish();
            }
            StartCoroutine(progressDown());
        }
    }
    IEnumerator progressUp()
    {
        StopCoroutine("progressDown");
        if (progressBar.value < 1)
        {
            progressBar.value += 0.018f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine("progressUp");
        }
        yield return new WaitForSeconds(0);
    }

    IEnumerator progressDown()
    {
        StopCoroutine("progressUp");
        if (progressBar.value > 0)
        {
            progressBar.value -= 0.03f;
            yield return new WaitForSeconds(0.15f);
            StartCoroutine("progressDown");
        }
        yield return new WaitForSeconds(0);
    }

}