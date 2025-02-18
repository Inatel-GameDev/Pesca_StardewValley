using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Peixe : MonoBehaviour
{

    [SerializeField] float crazyness;
    [SerializeField] float DefaultCrazyness;
    [SerializeField] float maxVelocity;
    [SerializeField] float upDownSecurity;
    [SerializeField] public GameObject hookPrefab;
    [SerializeField] public Inventory inventory;
    [SerializeField] public Player player;
    VegettiCabeçudo vegetti;

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
        GameObject vegettiObj = new GameObject("VegettiCabeçudo");
        vegetti = vegettiObj.AddComponent<VegettiCabeçudo>();

        // Agora podemos inicializar os valores
        vegetti.Setup(DefaultCrazyness, crazyness, rb);
    }

    private void Update()
    {
        if (progressBar.value == 1)
        {
            player.PlayAnimation(player.idle);
            inventory.AddFish();
            Destroy(hookPrefab);
        }
        if(progressBar.value == 0)
        {
            player.PlayAnimation(player.idle);
            Destroy(hookPrefab);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(vegetti != null)
            vegetti.move();
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
            vegetti.startTriggerFish();
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
            vegetti.stopTriggerFish();
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