using UnityEngine;
using System.Collections;

public class Boia : MonoBehaviour
{
    private Player player;
    private bool onceFishing = false;
    private bool hitSolidObject = false;

    public void Inicializar(Player player)
    {
        this.player = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("🌊 Colidiu com: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Palanque"))
        {
            Debug.Log("🛑 Boia caiu no palanque! Cancelando pesca.");
            player.SetFishingState(false);
            hitSolidObject = true;
            Destroy(gameObject);
            return;
        }

        if (!hitSolidObject && collision.gameObject.name == "Água")
        {
            //Debug.Log("✅ Boia tocou na água! Tentando iniciar a pesca...");
            StartCoroutine(IniciarPesca());
        }
    }

    private IEnumerator IniciarPesca()
    {
        yield return new WaitForSeconds(0.2f); // Pequeno delay para garantir que a boia processa a colisão corretamente

        if (!onceFishing)
        {
            //Debug.Log("🎣 CHAMANDO comecaPescar()...");
            onceFishing = true;
            player.SetFishingState(true);
            player.comecaPescar();
        }
        else
        {
            Debug.LogWarning("⚠️ ERRO: onceFishing já estava true antes de chamar comecaPescar!");
        }
    }



}
