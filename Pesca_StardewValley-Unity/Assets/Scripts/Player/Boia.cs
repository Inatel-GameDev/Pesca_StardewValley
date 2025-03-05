using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boia : MonoBehaviour
{
    Rigidbody2D rb;
    Player player;
    public bool isWater = false;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(player == null)
            player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colidiu com: "+collision.gameObject.tag);
        if(collision.gameObject.tag != "Palanque")
        {
            Debug.Log("pescando");
            isWater = true;
            player.comecaPescar();
        }
        else
        {
            Debug.Log("Epaaa");
            isWater = false;
            player.isFishing = false;
            Destroy(gameObject);
        }
    }

    public bool landWater()
    {
        return isWater;
    }

    public void move(float location, float playerPositionY)
    {
        rb.MovePosition(new Vector3(location, playerPositionY, 0));
    }

}
