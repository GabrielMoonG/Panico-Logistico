using UnityEngine;

public class GridConfig : MonoBehaviour
{
    public int gridXSize = 10; // Tamanho do grid (1 unidade)
    public int gridZSize = 2; // Tamanho do grid (1 unidade)

    public Color gridColor = Color.white;

    //deixa o grid visível na cena para facilitar o desenvolvimento
    void OnDrawGizmos()
    {
        Gizmos.color = gridColor;
        for (int x = -gridXSize; x < gridXSize; x++)
        {
            for (int z = -gridZSize; z < gridZSize; z++)
            {
                Gizmos.DrawWireCube(new Vector3(x, 0, z), new Vector3(1, 0, 1));
            }
        }
    }
}
