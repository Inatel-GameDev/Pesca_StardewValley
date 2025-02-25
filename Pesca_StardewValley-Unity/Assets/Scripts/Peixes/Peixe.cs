using System.Collections;
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
    int randomFish;

    // Os fishs
    [SerializeField] public GenericalFish[] fishes = new GenericalFish[]
        {
            new GenericalFish("Vegetty Cabeçudo",5,"Heavy",30,30,0.2f,"Crazy"),
            new GenericalFish("Good",5,"Heavy",30,30,0.2f,"Weight"),
            new GenericalFish("Lambari", 1,"Float",5,20,0.1f,"Weight")
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
        randomFish = Random.Range(0, fishes.Length);

        // Agora podemos inicializar os valores
        crazyIwasCrazyOnce.Setup(fishes[randomFish], rb);
        weightFish.Setup(fishes[randomFish], rb); // Vai ser o good peixe tubarãO
    }

    private void Update()
    {
        if (progressBar.value == 1 && !notaOn)
        {
            Instantiate(notaPrefab, new Vector3(0, 3f, 0), Quaternion.identity);
        }
        if(progressBar.value == 0)
        {
            player.PlayAnimation(player.idle);
            Destroy(hookPrefab);
            player.isFishing = false;
        }
        GameObject canvasNota = GameObject.Find("CanvasNota(Clone)");
        if (canvasNota != null)
        {
            Image[] noteImage = canvasNota.GetComponentsInChildren<Image>();
            Text[] texts = canvasNota.GetComponentsInChildren<Text>();

            if (randomFish < fishName.Length)
            {
                Debug.Log(fishName[randomFish]);
                Debug.Log(fishesText[randomFish]);
                noteImage[1].sprite = fishesImages[randomFish];
                texts[0].text = fishesText[randomFish];
                texts[1].text = fishName[randomFish];
                notaOn = true;
            }
        }
        if (progressBar.value == 1)
        {
            player.PlayAnimation(player.idle);
            inventory.AddFish();
            Destroy(hookPrefab);
            player.isFishing = false;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Debug.Log(fishes[randomFish].name);
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
        if (collision.gameObject.tag == "Down")
        {
            Vector2 direction = collision.transform.position - transform.position;
            rb.linearVelocityY = upDownSecurity;
        }
        else if (collision.gameObject.tag == "Up")
        {
            Vector2 direction = collision.transform.position - transform.position;
            rb.linearVelocityY = -upDownSecurity;
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
    /*
    IEnumerator moreCrazyness()
    {
        Debug.Log("Crazyness: " + crazyness);
        StopCoroutine("lessCrazyness");
        if (crazyness < 30f)
        {
            Debug.Log("Crazyness: " + crazyness);
            crazyness += 2f;
            yield return new WaitForSeconds(1f);
            StartCoroutine("moreCrazyness");
        }
        yield return new WaitForSeconds(0);
    }
    IEnumerator lessCrazyness()
    {
        StopCoroutine("moreCrazyness");
        if (crazyness > DefaultCrazyness)
        {
            crazyness -= 2f;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("lessCrazyness");
        }
        yield return new WaitForSeconds(0);
    }
    */
    IEnumerator progressUp()
    {
        StopCoroutine("progressDown");
        if (progressBar.value < 1)
        {
            progressBar.value += 0.03f;
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