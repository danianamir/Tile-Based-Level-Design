using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Restarter : MonoBehaviour
{
    public GameObject[] itemsToSpawn;


    public int xSpawnMax = 3;

    public int ySpawnMax = 3;



    public void RestartState()
    {
        Items[] allitems = FindObjectsOfType<Items>();
        foreach (Items item in allitems)
        {
            Destroy(item.gameObject);
        }



        for (int i = 0; i < xSpawnMax; i++)
        {
            for (int j = 0; j < ySpawnMax; j++)
            {
                // Pick random item prefab
                int x = Random.Range(0, itemsToSpawn.Length + 1);

                if (x == itemsToSpawn.Length )
                {
                    // do nothing
                }
                else
                {
                    GameObject prefab = itemsToSpawn[x];
                    Vector3 pos = new Vector3(i + 0.5f, j + 0.5f, 0);
                    GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
                }


            }


        }
    }

}

