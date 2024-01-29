using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInstantiator : MonoBehaviour
{

    private int x;
    private int y;

    public GameObject restarter;
    public int[,] grid_values = new int[5, 5];


    void Start()
    {
        Restarter r = restarter.GetComponent<Restarter>();
        x = ((int)r.xSpawnMax);
        y = ((int)r.ySpawnMax);
        grid_values = new int[x, y];


    }

    // Update is called once per frame
    void Update()
    {
        grid_values = assign_grid_value(x, y);
    }



    public int[,] assign_grid_value(int x, int y)
    {
        int num_caret;
        int num_target;
        grid_values = new int[x, y];
        for (int i = 0; i < x; i++)
        {
            string log = "";
            for (int j = 0; j < y; j++)
            {

                gameObject.transform.position = new Vector2(j + 0.5f, i + 0.5f);
                num_caret = 0;
                num_target = 0;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 0.499f);

                foreach (var item in colliders)
                {
                    if (item.tag == "caret")
                    {
                        num_caret = num_caret + 1;
                    }
                    if (item.tag == "target")
                    {
                        num_target = num_target + 1;
                    }



                }

                grid_values[i, j] = num_caret * 10000 + num_target;
               


                log += grid_values[i, j] + " ";



            }
            // Debug.Log(log + " ");
            // Debug.Log("\n");
        }


        return grid_values;
    }
}
