using System.Collections;
using UnityEngine;

public class Peixe_Teste : MonoBehaviour
{

    [SerializeField] float crazyness;
    [SerializeField] float DefaultCrazyness;
    [SerializeField] float maxVelocity;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DefaultCrazyness = crazyness;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(0f, Random.Range(-crazyness, crazyness));
        rb.AddForce(direction);
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
            StartCoroutine(moreCrazyness());
        }
        if (collision.gameObject.tag == "Down")
        {
            Vector2 direction = collision.transform.position - transform.position;
            rb.linearVelocityY = 0.7f;
        }
        else if (collision.gameObject.tag == "Up")
        {
            Vector2 direction = collision.transform.position - transform.position;
            rb.linearVelocityY = -0.7f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hook")
        {
            StartCoroutine(lessCrazyness());
        }
    }

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

}
