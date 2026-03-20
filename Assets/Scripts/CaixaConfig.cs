using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class CaixaConfig : MonoBehaviour
{
    [Header("Configurações")]
    public float velocidadeQueda = 5.0f;
    public float alturaDoChao = 0.5f; // Ajuste conforme o Pivot do seu cubo

    public List<Color> CoresDisponiveis;
    public Color CorAtual = Color.gray;

    public Guid IDCaixa;
    private Renderer rend;

    public MovimentacaoController movControl;

    public float intensidadeBrilho = 1f; // Intensidade do brilho quando selecionada

    public List<CaixaConfig> GrupoConectado; // Lista de caixas conectadas da mesma cor

    public bool AtivarConectadosRaycast = false; // se verdadeiro exibe os raios para debug


    void Start()
    {
        if (CoresDisponiveis != null && CoresDisponiveis.Count > 0)
            CorAtual = CoresDisponiveis.OrderBy(o => Guid.NewGuid()).First();

        rend = GetComponent<Renderer>();
        rend.material.color = CorAtual; //Muda a cor da caixa para a cor sorteada


        IDCaixa = Guid.NewGuid();

        movControl = FindFirstObjectByType<MovimentacaoController>();
    }

    void Update()
    {
        ObterGrupoConectado();

        if (GrupoConectado.Find(o => o.IDCaixa == movControl.IDSelecionado)) //se o selecionado tiver na lista ativa o brilho
        {
            Brilho(true);
        }
        else
        {
            Brilho(false);
        }

        // 1. Se já está no limite do chão (Y mínimo), não faz nada
        if (transform.position.y <= alturaDoChao + 0.01f)
        {
            SnapNoGrid();
            return;
        }

        // 2. Checa se tem um obstáculo (outra caixa) logo abaixo
        if (TemObstaculoAbaixo())
        {
            SnapNoGrid();
            return;
        }

        // 3. Se não tem nada embaixo e não chegou no chão, cai de forma fluida
        CairFluido();
    }

    bool TemObstaculoAbaixo()
    {
        RaycastHit hit;
        // Raio disparado um pouco abaixo do centro para evitar bater em si mesmo
        Vector3 origem = transform.position + Vector3.down * 0.1f;

        // Distância curta (0.45f) para detectar apenas o que está "encostado"
        if (Physics.Raycast(origem, Vector3.down, out hit, 0.45f))
        {
            // Opcional: verifique se o que atingiu tem a Tag "Cubo"
            return true;
        }
        return false;
    }

    void CairFluido()
    {
        transform.position += Vector3.down * velocidadeQueda * Time.deltaTime;
    }

    void SnapNoGrid()
    {
        // O "Snap" agora acontece constantemente enquanto estiver parado, 
        // garantindo que ele esteja sempre alinhado.
        float x = Mathf.Round(transform.position.x);
        float z = Mathf.Round(transform.position.z);
        float y = Mathf.Round(transform.position.y);

        // Se a diferença for muito pequena, a gente trava na posição inteira
        // Isso evita o "tremido" pois só teleporta se estiver fora do lugar
        Vector3 posicaoAlinhada = new Vector3(x, y, z);

        if (Vector3.Distance(transform.position, posicaoAlinhada) > 0.01f)
        {
            transform.position = posicaoAlinhada;
        }
    }

    public void Brilho(bool ativar)
    {
        if (ativar)
        {
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", CorAtual * intensidadeBrilho);
        }
        else
        {
            // Desativar Brilho
            rend.material.SetColor("_EmissionColor", Color.black);
        }
    }

    public void ObterGrupoConectado(List<CaixaConfig> grupoJaEncontrado = null)
    {
        // 1. Inicialização
        if (grupoJaEncontrado == null)
            grupoJaEncontrado = new List<CaixaConfig>();

        // 2. Trava de segurança: Se eu já estou na lista, não faço nada (evita o erro e o loop infinito)
        if (grupoJaEncontrado.Contains(this)) return;

        // 3. Me adiciono à lista
        grupoJaEncontrado.Add(this);

        // 4. Procuro vizinhos
        Vector3[] direcoes = { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.forward, Vector3.back };

        foreach (Vector3 dir in direcoes)
        {
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 1.1f))
            {
                CaixaConfig vizinho = hit.transform.GetComponent<CaixaConfig>();

                // Se o vizinho é da mesma cor e ainda não foi explorado nesta "onda"
                if (vizinho != null && vizinho.CorAtual == this.CorAtual)
                {
                    if (AtivarConectadosRaycast)
                        Debug.DrawRay(transform.position, dir * 1.1f, Color.green, 0.1f);


                    if (!grupoJaEncontrado.Contains(vizinho))
                    {
                        // RECURSÃO: O vizinho se encarrega de continuar a cascata
                        vizinho.ObterGrupoConectado(grupoJaEncontrado);
                    }
                }
                else
                {
                    if (AtivarConectadosRaycast)
                        Debug.DrawRay(transform.position, dir * 1.1f, Color.red, 0.1f);
                }
            }
        }

        // 5. No final de tudo, todos os objetos do grupo apontam para a mesma lista
        this.GrupoConectado = grupoJaEncontrado;
    }
}
