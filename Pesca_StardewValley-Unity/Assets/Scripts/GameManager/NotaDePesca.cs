using UnityEngine;
using UnityEngine.UI;

public class NotaDePesca : MonoBehaviour
{
    // O que vai estar fixado na nota
    public Image noteImage;
    public Text noteText;
    public Text noteName;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Destroy(gameObject);
    }
}
