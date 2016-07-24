using UnityEngine;
using System.Collections;

public class TubeSpawner : MonoBehaviour {



    public Transform SpawnPrefab;

    public int Size;
    public int CountX;
    public int CountY;
    public float Koef;

    public int Added_X = 0;
    public int Added_Y = 0;

    void Start()
    {
        for (int i = 0; i < CountX; i++)
        {
            for (int j = 0; j < CountY; j++)
            {
                Added_X = i;
                Added_Y = j;
                Spawn(i, j);

            }
        }
    }

    void Spawn(int i, int j)
    {
        Vector3 spawnPos = transform.position + new Vector3(i * (Size * Koef), j * (Size * Koef), 0);
        SpawnPrefab.localScale = new Vector3(Size, Size, Size);
        Instantiate(SpawnPrefab, spawnPos, Quaternion.AngleAxis(90, Vector3.right));

    }
}
