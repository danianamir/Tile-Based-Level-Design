
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
using Random = UnityEngine.Random;
using Unity.Mathematics;







public class LearnerInstantiator : Agent


{

    public int grid_size;


    [HideInInspector]
    public bool huristic_initate_player = true;


    //  huristic action setter
    public int discrete_actions0_setter;
    public int discrete_actions1_setter;
    public int index_position = 0;
    // general
    public GameObject[] items;

    public int max_tile_changed = 0;
    //step distance
    [HideInInspector]
    public float goal_distance;
    [HideInInspector]
    public float initial_distance;



    private Canvas canvas;
    private TMP_Text level_step;
    private TMP_Text level_distance;
    public TMP_Text up;
    public TMP_Text act;



    [HideInInspector]
    public int timeee = 0;
    [HideInInspector]
    public int time = 0;
    [HideInInspector]
    public int step = 0;
    public List<bool> buildable;
    [HideInInspector]
    public PathFinding pathFinding;



    //constraint , goals, w
    [System.Serializable]
    public struct Constraint
    {

        public int enemy_distances;
        public int existence_of_all;
        public int existence_between_amount;
        public int existence_of_path_to_all_treasure;
        public int treasure_amount;
    }
    public float[] wieght;

    public Constraint constraint;
    public Constraint goal;




    [System.Serializable]
    public struct Observations
    {
        public int pahse;
        public List<int> grid_values;
        public Constraint constraint;
        public Vector2 agent_position;


    }


    public Observations obs;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        Transform step = canvas.transform.GetChild(1);
        level_step = step.gameObject.GetComponent<TMP_Text>();

        Transform distance = canvas.transform.GetChild(2);
        level_distance = distance.gameObject.GetComponent<TMP_Text>();
    }

    //#################################################################################################################
    public override void OnEpisodeBegin()
    {
        // Debug.Log("begin");
        // initialize parametere
        Restart_episode();


        // update obsservation and constraint
        obs = observation_update();
        constraint = constraint_calculator();

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Debug.Log("ob");

        // update obsservation and constraint
        obs = observation_update();
        constraint = constraint_calculator();



        // variabels ...................................................................................................







        //grid + bulidable
        for (int i = 0; i < grid_size * grid_size; i++)
        {
            sensor.AddObservation(obs.grid_values[i]);
            sensor.AddObservation(buildable[i]);

        }


        //  agnet_position
        sensor.AddObservation(obs.agent_position);


        //constraint
        sensor.AddObservation(obs.constraint.enemy_distances);
        sensor.AddObservation(obs.constraint.existence_of_all);
        sensor.AddObservation(obs.constraint.existence_between_amount);
        sensor.AddObservation(obs.constraint.existence_of_path_to_all_treasure);
        sensor.AddObservation(obs.constraint.treasure_amount);




    }
    public override void OnActionReceived(ActionBuffers actions)
    {



        // level desing time step
        step++;





        // take step
        if (index_position >= grid_size * grid_size)
        {
            index_position = 0;
        }
        ActionSegment<int> diescrite_actions = actions.DiscreteActions;
        take_step(diescrite_actions, index_position);



        index_position++;


        time = time + 1;
        act.text = time.ToString();

    }



    private void FixedUpdate()
    {
        // Debug.Log("learn");
        timeee = timeee + 1;
        up.text = timeee.ToString();


        // update observations and constraint
        obs = observation_update();
        constraint = constraint_calculator();
        level_distance.text = "distance to goal:" + distance_to_goal_calculator().ToString();
        level_step.text = "level design step:" + step.ToString();




        RequestDecision();

        if (check_end_of_level_design(step))
        {
            float r1;
            r1 = reward_level();

            // Debug.Log(level_reward);
            AddReward(r1);
        }


    }


    public Observations observation_update()
    {

        Observations obs = new Observations();

        obs.grid_values = assign_grid_value();
        obs.agent_position = gameObject.transform.position;
        obs.constraint = constraint_calculator();
        return obs;
    }

    public void take_step(ActionSegment<int> discrite_actions, int index)
    {
        // spontanious movies#####################################################################################################




        int x = index % grid_size;
        int y = index / grid_size;

        gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);





        // ############################################################################################


        if (discrite_actions[0] == 0)
        {
            // do nothing
        }




        if (discrite_actions[0] == 1)
        {


            LayerMask layerMask = ~(1 << gameObject.layer);

            Collider2D[] detected_colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.3f, 0.3f), 0f, layerMask);


            foreach (var item in detected_colliders)
            {
                Destroy(item.gameObject);
            }

            int block_index = (int)(gameObject.transform.position.x - 0.5f) + (int)grid_size * (int)(gameObject.transform.position.y - 0.5f);
            if (buildable[block_index])
            {

                GameObject ob = Instantiate(items[0], gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            }










        }



        if (discrite_actions[0] == 2)
        {


            LayerMask layerMask = ~(1 << gameObject.layer);

            Collider2D[] detected_colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.3f, 0.3f), 0f, layerMask);


            foreach (var item in detected_colliders)
            {
                Destroy(item.gameObject);
            }

            int block_index = (int)(gameObject.transform.position.x - 0.5f) + (int)grid_size * (int)(gameObject.transform.position.y - 0.5f);
            if (buildable[block_index])
            {

                GameObject ob = Instantiate(items[1], gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            }









        }

        if (discrite_actions[0] == 3)
        {


            LayerMask layerMask = ~(1 << gameObject.layer);

            Collider2D[] detected_colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.3f, 0.3f), 0f, layerMask);


            foreach (var item in detected_colliders)
            {
                Destroy(item.gameObject);
            }

            int block_index = (int)(gameObject.transform.position.x - 0.5f) + (int)grid_size * (int)(gameObject.transform.position.y - 0.5f);
            if (buildable[block_index])
            {

                GameObject ob = Instantiate(items[2], gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            }











        }
        if (discrite_actions[0] == 4)
        {


            LayerMask layerMask = ~(1 << gameObject.layer);

            Collider2D[] detected_colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.3f, 0.3f), 0f, layerMask);

            foreach (var item in detected_colliders)
            {
                Destroy(item.gameObject);
            }

            int block_index = (int)(gameObject.transform.position.x - 0.5f) + (int)grid_size * (int)(gameObject.transform.position.y - 0.5f);
            if (buildable[block_index])
            {

                GameObject ob = Instantiate(items[3], gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            }











        }


        if (discrite_actions[0] == 5)
        {


            LayerMask layerMask = ~(1 << gameObject.layer);

            Collider2D[] detected_colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.3f, 0.3f), 0f, layerMask);


            foreach (var item in detected_colliders)
            {
                Destroy(item.gameObject);
            }

            int block_index = (int)(gameObject.transform.position.x - 0.5f) + (int)grid_size * (int)(gameObject.transform.position.y - 0.5f);
            if (buildable[block_index])
            {

                GameObject ob = Instantiate(items[4], gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            }










        }





        if (discrite_actions[0] == 6)
        {


            LayerMask layerMask = ~(1 << gameObject.layer);

            Collider2D[] detected_colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.3f, 0.3f), 0f, layerMask);


            foreach (var item in detected_colliders)
            {
                Destroy(item.gameObject);
            }

            int block_index = (int)(gameObject.transform.position.x - 0.5f) + (int)grid_size * (int)(gameObject.transform.position.y - 0.5f);
            if (buildable[block_index])
            {

                GameObject ob = Instantiate(items[5], gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            }









        }




        //###########################################################################################





    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {

        // set the action 
        ActionSegment<int> discrite_actions = actionsOut.DiscreteActions;
        var continuousActions = actionsOut.ContinuousActions;


        if (huristic_initate_player)
        {
            discrete_actions0_setter = 1;
            huristic_initate_player = false;
        }
        else
        {
            int index = Random.Range(0, 7);
            if (index != 1)
            {
                discrete_actions0_setter = index;
            }



        }




        //set discrete
        discrite_actions[0] = discrete_actions0_setter;



    }

    public List<int> assign_grid_value()
    {
        List<int> grid_values = new List<int>();


        GameObject ob = new GameObject();


        for (int i = 0; i < grid_size; i++)
        {

            for (int j = 0; j < grid_size; j++)
            {

                ob.transform.position = new Vector2(j + 0.5f, i + 0.5f);
                LayerMask layerMask = ~(1 << gameObject.layer);
                Collider2D colliders = Physics2D.OverlapBox(new Vector2(ob.transform.position.x, ob.transform.position.y), new Vector2(0.1f, 0.1f), 0f, layerMask);
                if (colliders != null)
                {

                    Item item = colliders.GetComponent<Item>();
                    if (item != null)
                    {



                        if (item.type == 0)
                        {
                            grid_values.Add(1);

                        }
                        if (item.type == 1)
                        {
                            grid_values.Add(2);

                        }
                        if (item.type == 2)
                        {
                            grid_values.Add(3);

                        }
                        if (item.type == 3)
                        {
                            grid_values.Add(4);

                        }
                        if (item.type == 4)
                        {
                            grid_values.Add(5);

                        }
                        if (item.type == 5)
                        {
                            grid_values.Add(6);

                        }


                    }
                    else
                    {
                        grid_values.Add(0);
                    }



                }
                else
                {
                    grid_values.Add(0);

                }
            }

        }

        Destroy(ob);


        return grid_values;

    }

    public Constraint constraint_calculator()
    {

        Constraint constraint = new Constraint();

        int amount_player = 0;
        int amount_enemy = 0;
        int amount_wall = 0;
        int amount_treasure = 0;
        int amount_health = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(0, 0), float.PositiveInfinity);
        List<GameObject> player = new List<GameObject>();
        List<GameObject> enemy = new List<GameObject>();
        List<GameObject> wall = new List<GameObject>();
        List<GameObject> treasure = new List<GameObject>();
        List<GameObject> healths = new List<GameObject>();





        for (int i = 0; i < colliders.Length; i++)
        {
            Item item = colliders[i].GetComponent<Item>();

            if (item != null)
            {

                if (item.type == 0)
                {

                    amount_player++;
                    player.Add(colliders[i].gameObject);

                }
                if (item.type == 1 || item.type == 2)
                {
                    amount_enemy++;
                    enemy.Add(colliders[i].gameObject);

                }

                if (item.type == 4)
                {
                    amount_treasure++;
                    treasure.Add(colliders[i].gameObject);

                }
                if (item.type == 5)
                {
                    amount_wall++;
                    wall.Add(colliders[i].gameObject);

                }
                if (item.type == 3)
                {
                    amount_health++;
                    healths.Add(colliders[i].gameObject);
                }

            }

        }



        // existence between amount###################################################################################################################################################################################


        int amount_all = enemy.Count + healths.Count + player.Count + treasure.Count + wall.Count;
        constraint.existence_between_amount = amount_all;





        // treasure amount
        constraint.treasure_amount = amount_treasure;






        // path existence to all treasures#########################################################

        int path_number = 0;


        if (player.Count == 1 && treasure.Count > 0)
        {

            pathFinding.target = player[0].transform.position;
            foreach (var item in treasure)
            {

                PathFinding.AdjacentIndexes aj = pathFinding.indexer_grid(item.gameObject.transform.position.x, item.gameObject.transform.position.y, grid_size);
                if (pathFinding.grids.Count > 0)
                {
                    if (pathFinding.grids[aj.adjecent_indexes[0]].directions != PathFinding.Directions.None)
                    {
                        path_number++;
                    }
                }

            }
        }
        else
        {
            pathFinding.target = new Vector2(0.5f, 0.5f);
            path_number = 0;
        }


        if (treasure.Count > 0)
        {
            if (path_number == treasure.Count)
            {
                constraint.existence_of_path_to_all_treasure = 1;
            }
            else
            {
                constraint.existence_of_path_to_all_treasure = 0;
            }
        }
        else
        {
            constraint.existence_of_path_to_all_treasure = 0;
        }













        // all enemy distances >1#########################################################################

        int enemy_count_distance = 0;

        if (enemy.Count > 0 && player.Count > 0)
        {
            foreach (var item in enemy)
            {
                if (Vector3.Distance(player[0].transform.position, item.transform.position) > 1)
                {
                    enemy_count_distance++;
                }
            }



            if (enemy_count_distance == enemy.Count)
            {
                constraint.enemy_distances = 1;
            }
            else
            {
                constraint.enemy_distances = 0;
            }
        }
        else
        {
            constraint.enemy_distances = 0;
        }










        // existence all##################################################################################################################

        if (enemy.Count > 0 && healths.Count > 0 && player.Count > 0 && treasure.Count > 0 && wall.Count > 0)
        {
            constraint.existence_of_all = 1;
        }
        else
        {

            constraint.existence_of_all = 0;
        }





        return constraint;
    }

    public float distance_to_goal_calculator()
    {
        float[] cn = ConvertStructToArray(constraint);
        float[] g = ConvertStructToArray(goal);







        goal_distance = 0f;
        for (int i = 0; i < g.Length; i++)
        {
            goal_distance += Mathf.Abs(g[i] - cn[i]) * wieght[i];
        }




        return goal_distance;



    }

    public float[] ConvertStructToArray(Constraint constraint)
    {
        float[] floatArray = new float[]
        {

            (float)constraint.enemy_distances,
             (float)constraint.existence_of_all,
            (float)constraint.existence_between_amount,
            (float)constraint.existence_of_path_to_all_treasure,
            (float)constraint.treasure_amount,


        };

        return floatArray;
    }



    public int satisied_check()
    {
        int amount = 0;
        if (constraint.existence_between_amount <= 40 && constraint.existence_between_amount >= 30)
        {
            amount++;
        }
        if (constraint.treasure_amount <= 15 && constraint.treasure_amount >= 5)
        {
            amount++;
        }

        if (constraint.existence_of_all == 1)
        {
            amount++;
        }

        if (constraint.enemy_distances == 1)
        {
            amount++;
        }

        if (constraint.existence_of_path_to_all_treasure == 1)
        {
            amount++;
        }


        return amount;
    }













    public bool check_end_of_level_design(int step)
    {



        if (


                   step >= grid_size * grid_size
        // || ln.changed_tiles_percentages >= max_tile_percrntage


        )
        {
            return true;
        }

        else
        {
            return false;
        }
    }


    public void Restart_episode()
    {





        buildable = new List<bool>();
        for (int i = 0; i < (grid_size * grid_size); i++)
        {
            buildable.Add(true);
        }


        int s = UnityEngine.Random.Range(0, 5);


        if (s == 0)
        {
            max_tile_changed = 0;
        }
        if (s == 1)
        {
            max_tile_changed = 2;
        }
        if (s == 2)
        {
            max_tile_changed = 4;
        }
        if (s == 3)
        {
            max_tile_changed = 6;
        }
        if (s == 4)
        {
            max_tile_changed = 8;
        }





        int j = 0;
        while (j < max_tile_changed)
        {

            int ind = UnityEngine.Random.Range(0, grid_size * grid_size);

            if (buildable[ind] == true)
            {
                buildable[ind] = false;
                j++;
            }



        }









        //delete all gameobjects
        CustomTags[] tags = FindObjectsOfType<CustomTags>();

        foreach (CustomTags tag in tags)
        {
            if (tag.destroyable)
            {
                Destroy(tag.gameObject);
            }

        }











        index_position = UnityEngine.Random.Range(0, grid_size * grid_size);
        huristic_initate_player = true;
        step = 0;
        time = 0;
        timeee = 0;
        initial_distance = distance_to_goal_calculator();
        constraint = constraint_calculator();
        obs = observation_update();





    }

    public float reward_level()
    {
        float reward = 0;

        reward = 1 / (1 + math.abs(distance_to_goal_calculator()));

        //  Debug.Log("distance " + ln.distance_to_goal_calculator() + "reward " + reward);

        return reward;
    }
}




























