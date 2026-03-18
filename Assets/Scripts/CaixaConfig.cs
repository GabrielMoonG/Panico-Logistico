using UnityEngine;

public class CaixaConfig : MonoBehaviour
{
    [Header("Configurações")]
    public float velocidadeQueda = 5.0f;
    public float alturaDoChao = 0.5f; // Ajuste conforme o Pivot do seu cubo

    void Update()
    {
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
}
