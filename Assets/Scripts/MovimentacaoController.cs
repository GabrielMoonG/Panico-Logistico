using UnityEngine;
using UnityEngine.InputSystem; // Adicione isso no topo


public class MovimentacaoController : MonoBehaviour
{
    // Arraste o arquivo "InputSystem_Actions" para este campo no Inspector
    public PlayerInput playerInput;
    private InputAction moveAction;

    public GameObject ObjSelect;

    public GridConfig gridConfig; // Referência ao script de configuração do grid

    void Awake()
    {
        // "Player" é o nome do Action Map que aparece na sua imagem
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        // Detecta quando o jogador aperta a tecla (triggered)
        if (ObjSelect != null && moveAction.triggered)
        {
            Vector2 input = moveAction.ReadValue<Vector2>();

            if (input.x > 0) Mover(ObjSelect, Vector3.right);  // Seta Direita ou D
            if (input.x < 0) Mover(ObjSelect, Vector3.left);   // Seta Esquerda ou A
            if (input.y < 0) Mover(ObjSelect, Vector3.back);   // Seta Baixo ou S
            if (input.y > 0) Mover(ObjSelect, Vector3.forward);// Seta Cima ou W
        }
    }

    bool PodeSeMover(GameObject caixa, Vector3 direcao)
    {
        switch (true)
        {
            case true when Vector3.right == direcao && caixa.transform.position.x < gridConfig.gridXSize - 1:
            case true when Vector3.left == direcao && caixa.transform.position.x > -gridConfig.gridXSize:
            case true when Vector3.forward == direcao && caixa.transform.position.z < gridConfig.gridZSize - 1:
            case true when Vector3.back == direcao && caixa.transform.position.z > -gridConfig.gridZSize:
                return true;
        }

        return false;
    }

    bool Mover(GameObject caixa,  Vector3 direcao)
    {
        //verifica se tem caixa vizinha
        // 1. Checa se tem alguém no caminho usando um raio curto (1 unidade)
        RaycastHit hit;
        // Origem: centro do cubo, Direção: para onde você quer mover, Distância: 1f
        if (Physics.Raycast(caixa.transform.position, direcao, out hit, 1f))
        {
            var objeto = hit.transform.gameObject;

            // Exemplo: Mudar a cor para confirmar a seleção
            objeto.GetComponent<Renderer>().material.color = Color.green;

            // 2. Se bateu tenta movimentar o vinhoho primeiro
            var moveu = Mover(objeto, direcao); // Use o nome do SEU script

            //se o vizinho se moveu, eu me movo
            if (moveu)
            {
                caixa.transform.position += direcao;
                return true;
            }
            else
            {
                return false; // Se o vizinho não se moveu, eu também não me movo
            }
        }

        if (!PodeSeMover(caixa, direcao)) return false;

        caixa.transform.position += direcao;
        // Aqui você checaria se bateu em algo ou saiu do limite

        return true;
    }
}
