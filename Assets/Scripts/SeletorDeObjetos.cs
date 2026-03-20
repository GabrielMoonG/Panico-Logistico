using UnityEngine;
using UnityEngine.InputSystem;

public class SeletorDeObjetos : MonoBehaviour
{
    public Camera cam;
    public MovimentacaoController movController;

    void Update()
    {
        // Verifica se o botão esquerdo do mouse foi clicado
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Cria um raio que vai da posição do mouse na tela para o mundo 3D
            Ray raio = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // Dispara o raio e verifica se atingiu algo
            if (Physics.Raycast(raio, out hit))
            {
                // 'hit.transform' é o objeto que você clicou!
                GameObject clicado = hit.transform.gameObject;

                Debug.Log("Cliquei em: " + clicado.name);

                movController.ObjSelect = clicado; // Define o objeto selecionado no controlador de movimentação

                // Aqui você pode pegar a altura (Y) para mover a "carreira"
                float alturaY = Mathf.Round(clicado.transform.position.y);
                Debug.Log("Vou mover a carreira na altura: " + alturaY);
            }
        }
    }
}
