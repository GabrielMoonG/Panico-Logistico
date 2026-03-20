using UnityEngine;

public class SpawnerCaixas : MonoBehaviour
{
    public GridConfig GridConfig; //Para saber o tamanho do grid e onde spawnar as caixas
    public GameObject PrefebCaixa; //Caixa que será instanciada

    public void SpawnarCaixa()
    {
        // Spawnar a caixa no topo do grid, em uma posição aleatória no eixo X
        float xPos = Random.Range(-GridConfig.gridXSize, GridConfig.gridXSize);
        float zPos = Random.Range(-GridConfig.gridZSize, GridConfig.gridZSize);
        Vector3 spawnPosition = new Vector3(xPos, 10f, zPos); // Y alto para a queda
        Instantiate(PrefebCaixa, spawnPosition, Quaternion.identity);
    }
}
