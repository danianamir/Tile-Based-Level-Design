using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using System.ComponentModel;
using Unity.Collections;


public class LearnerInstantiator : Agent
{
    public int timestep = 0;
    public int discrete_actions0_setter;
    public int discrete_actions1_setter;
    public GameObject[] items;
    public GameObject grid;
    private GridInstantiator gridInstantiator;
    public GameObject restarter;
    private Restarter r;
    private Vector2 area_size = new Vector2();




    // observations
    private int[] grid_values;


    // goals
    public int amount_caret_object;
    public int amount_target_object;

    public float[] goal = new float[2];
    private float[] s = new float[2];
    private float[] s_prime = new float[2];



    private void Start()
    {

        r = restarter.GetComponent<Restarter>();
        gridInstantiator = grid.GetComponent<GridInstantiator>();
        grid_values = new int[25];
        area_size = new Vector2(r.xSpawnMax, r.ySpawnMax);
    }



    public override void OnEpisodeBegin()
    {

        // initialize parametere
        timestep = 0;
        r.RestartState();
        s[0] = 0;
        s_prime[0] = 0;
        gameObject.transform.position = new Vector3(Random.Range(0, r.xSpawnMax) + 0.5f, Random.Range(0, r.ySpawnMax) + 0.5f, 0);


        // initialize the observation 
        grid_value_assigner();

    }



    public override void OnActionReceived(ActionBuffers actions)
    {



        s[0] = s_prime[0];
        s[1] = s_prime[1];



        if (timestep > 1)
        {

            // set the actions 
            ActionSegment<int> discrite_actions = actions.DiscreteActions;
            var continuousActions = actions.ContinuousActions;




            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 0.0001f);

            if (discrite_actions[0] == 0)
            {
                // do nothing
            }
            if (discrite_actions[0] == 1)
            {
                // delete
                if (colliders.Length != 0)
                {
                    Destroy(colliders[0].gameObject);
                }

            }
            if (discrite_actions[0] == 2)
            {
                // delete and replace with item 0
                if (colliders.Length != 0)
                {
                    Destroy(colliders[0].gameObject);
                }
                Instantiate(items[0], gameObject.transform.position, new Quaternion(0, 0, 0, 0));

            }
            if (discrite_actions[0] == 3)
            {
                // delete and replace with item 0
                if (colliders.Length != 0)
                {
                    Destroy(colliders[0].gameObject);
                }
                Instantiate(items[1], gameObject.transform.position, new Quaternion(0, 0, 0, 0));

            }







            //set discrete [1] for change position 

            // do nothing
            if (discrite_actions[1] == 0)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);

            }
            // go one unit foward in x
            if (discrite_actions[1] == 1)
            {
                if (gameObject.transform.position.x + 1 <= area_size.x)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, 0);

                }
            }

            // go one unit backward in x
            if (discrite_actions[1] == 2)
            {
                if (gameObject.transform.position.x - 1 >= 0)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, 0);

                }
            }

            // go one unit forward in y
            if (discrite_actions[1] == 3)
            {
                if (gameObject.transform.position.y + 1 <= area_size.y)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, 0);

                }
            }

            // go one unit backward in y
            if (discrite_actions[1] == 4)
            {
                if (gameObject.transform.position.y - 1 >= 0)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, 0);

                }
            }




        }



        update_s_s_prime();

        s_prime[0] = amount_caret_object;
        s_prime[1] = amount_target_object;

        if (timestep > 1)
        {
            calculate_reward(s, s_prime, goal);
        }


        end_episode(s_prime, goal);

        timestep = timestep + 1;
    }

    private void Update()
    {
        //discrete_actions0_setter = Random.Range(0, 4);
        // discrete_actions1_setter = Random.Range(0, 6);
        //update timestep


        // update agent observation each frame
        grid_value_assigner();

    }

    public override void CollectObservations(VectorSensor sensor)
    {


        // 16 grid value
        for (int i = 0; i < area_size.x * area_size.y; i++)
        {
            sensor.AddObservation(grid_values[i]);
        }


        //  3 position x, y , z
        sensor.AddObservation(gameObject.transform.position);


        // 2 int
        for (int i = 0; i < goal.Length; i++)
        {
            sensor.AddObservation(s_prime[i]);
        }

        // 2 int
        for (int i = 0; i < goal.Length; i++)
        {
            sensor.AddObservation(s[i]);
        }


        //1 int
        sensor.AddObservation(timestep);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        // set the action 
        ActionSegment<int> discrite_actions = actionsOut.DiscreteActions;
        var continuousActions = actionsOut.ContinuousActions;


        // set discrete
        //  discrite_actions[0] = discrete_actions0_setter;
        //  discrite_actions[1] = discrete_actions1_setter;


    }

    public void calculate_reward(float[] s, float[] s_prime, float[] g)

    {
        float lt_1 = 0f;
        for (int i = 0; i < g.Length; i++)
        {
            lt_1 += Mathf.Abs(g[i] - s[i]);
        }



        float lt = 0f;
        for (int i = 0; i < g.Length; i++)
        {
            lt += Mathf.Abs(g[i] - s_prime[i]);
        }




        SetReward(lt_1 - lt);



    }

    public void end_episode(float[] s_prime, float[] g)
    {

        for (int i = 0; i < g.Length; i++)
        {
            if (s_prime[i] != g[i])
            {
                return;

            }

        }

        EndEpisode();

    }

    public void grid_value_assigner()
    {
        int index = 0;
        for (int i = 0; i < area_size.x; i++)
        {
            for (int j = 0; j < area_size.y; j++)
            {
                grid_values[index] = gridInstantiator.grid_values[i, j];
                index++;
            }
        }
    }

    public void update_s_s_prime()
    {
        amount_caret_object = 0;
        amount_target_object = 0;


        GameObject[] carets = GameObject.FindGameObjectsWithTag("caret");
        GameObject[] targets = GameObject.FindGameObjectsWithTag("target");

        foreach (GameObject caret in carets)
        {
            amount_caret_object = amount_caret_object + 1;
        }
        foreach (GameObject target in targets)
        {
            amount_target_object = amount_target_object + 1;
        }
    }

}
