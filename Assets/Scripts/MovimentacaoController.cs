using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem; // Adicione isso no topo


public class MovimentacaoController : MonoBehaviour
{
    // Arraste o arquivo "InputSystem_Actions" para este campo no Inspector
    public PlayerInput playerInput;
    private InputAction moveAction;

    public Guid? IDSelecionado = null;
    public GameObject ObjSelect;

    public GridConfig gridConfig; // Referência ao script de configuração do grid

    void Awake()
    {
        // "Player" é o nome do Action Map que aparece na sua imagem
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        //Atualiza o ID selecionado com base no objeto selecionado
        if (ObjSelect != null)
        {
            IDSelecionado = ObjSelect.GetComponent<CaixaConfig>()?.IDCaixa; // Atualiza o ID selecionado
        }

        // Detecta quando o jogador aperta a tecla (triggered)
        if (ObjSelect != null && moveAction.triggered)
        {
            Vector2 input = moveAction.ReadValue<Vector2>();

            //if (input.x > 0) Mover(ObjSelect, Vector3.right);  // Seta Direita ou D
            //if (input.x < 0) Mover(ObjSelect, Vector3.left);   // Seta Esquerda ou A
            //if (input.y < 0) Mover(ObjSelect, Vector3.back);   // Seta Baixo ou S
            //if (input.y > 0) Mover(ObjSelect, Vector3.forward);// Seta Cima ou W
            if (input.x > 0) MoverCaixas(Vector3.right);  // Seta Direita ou D
            if (input.x < 0) MoverCaixas(Vector3.left);   // Seta Esquerda ou A
            if (input.y < 0) MoverCaixas(Vector3.back);   // Seta Baixo ou S
            if (input.y > 0) MoverCaixas(Vector3.forward);// Seta Cima ou W
        }
    }

    void MoverCaixas(Vector3 direcao)
    {
        ObjSelect.GetComponent<CaixaConfig>().Mover(direcao);
    }
}
