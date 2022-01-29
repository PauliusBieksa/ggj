using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_spawner : MonoBehaviour
{
    [SerializeField]
    GameObject blue_asteroid_prefab;
    [SerializeField]
    GameObject red_asteroid_prefab;
    [SerializeField]
    float spawn_offset;
    [SerializeField]
    float spawn_range;
    [SerializeField]
    Transform player_transform;

    [SerializeField]
    int num_of_dead_zones = 2;
    [SerializeField]
    float min_length_of_dead_zone;
    [SerializeField]
    float max_length_of_dead_zone;
    [SerializeField]
    float radius_of_dead_zone = 4;
    List<GameObject> blue_asteroids;

    [SerializeField]
    private int cell_density = 50;
    [SerializeField]
    private int num_of_asteroids = 1000;

    float screenspace_x, screenspace_y;

    List<List<Vector3>> dead_zones = new List<List<Vector3>>();

    // returns random roughly visible point in player local space. bound by serialized fields
    Vector3 random_point_in_view(float xy_adjust = 1.0f, float z_adjust = 1.0f)
    {
        float z_offset = Random.Range(spawn_offset, spawn_range * z_adjust);
        float xy_range = Mathf.Tan(0.55f) * z_offset * xy_adjust;
        float x_offset = Random.Range(-xy_range * Screen.width / Screen.height, xy_range * Screen.width / Screen.height);
        float y_offset = Random.Range(-xy_range, xy_range);
        Vector3 ret = new Vector3(x_offset, y_offset, z_offset);
        return ret;
    }

    bool in_dead_zone(List<List<Vector3>> dead_zones, Vector3 point)
    {
        for (int i = 0; i < dead_zones.Count; i++)
        {
            if (Vector3.Dot(point - dead_zones[i][0], dead_zones[i][1] - dead_zones[i][0]) < 0 || Vector3.Dot(point - dead_zones[i][1], dead_zones[i][0] - dead_zones[i][1]) < 0)
                continue;
            float angle = Vector3.Dot(point - dead_zones[i][0], dead_zones[i][1] - dead_zones[i][0]) 
                / ((point - dead_zones[i][0]).magnitude * (dead_zones[i][1] - dead_zones[i][0]).magnitude);
            float projection_length = (point - dead_zones[i][0]).magnitude * angle;
            float dist = Mathf.Sqrt((point - dead_zones[i][0]).sqrMagnitude - projection_length * projection_length);
            if (dist < radius_of_dead_zone)
                return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        screenspace_y = Mathf.Tan(0.55f) * spawn_range;
        screenspace_x = screenspace_y * Screen.width / Screen.height;

        blue_asteroids = new List<GameObject>();

        // init placemet matrix
        List<bool> placement_matrix_row = new List<bool>();
        for (int i = 0; i < cell_density; i++) placement_matrix_row.Add(false);
        List<List<bool>> placement_matrix_grid = new List<List<bool>>();
        for (int i = 0; i < cell_density; i++) placement_matrix_grid.Add(new List<bool>(placement_matrix_row));
        List<List<List<bool>>> placement_matrix = new List<List<List<bool>>>();
        for (int i = 0; i < cell_density; i++) placement_matrix.Add(new List<List<bool>>(placement_matrix_grid));

        // dead zones
        for (int i = 0; i < num_of_dead_zones; i++)
        {
            float xy_range = Mathf.Tan(0.55f) * spawn_range;
            dead_zones.Add(new List<Vector3>());
            dead_zones[i] = new List<Vector3>();
            //Vector3 start_point = random_point_in_view(1.0f, 0.2f);
            Vector3 start_point = new Vector3(Random.Range(-xy_range * 0.7f, xy_range * 0.7f), Random.Range(-xy_range * 0.7f, xy_range * 0.7f), Random.Range(15, 25));
            Vector3 direction = new Vector3(Random.Range(0, 0.3f), Random.Range(0, 0.3f), 1);
            Vector3 end_point = new Vector3();
            end_point = start_point + direction * (Random.Range(min_length_of_dead_zone, max_length_of_dead_zone));
            dead_zones[i].Add(start_point);
            dead_zones[i].Add(end_point);
        }

        int abs = 0;
        // spawn asteroids
        for (int i = 0; i < num_of_asteroids; i++)
        {
            float xy_range = Mathf.Tan(0.55f) * spawn_range;
            Vector3 offset_point = random_point_in_view();
            Vector3 biased_point = new Vector3();
            biased_point = new Vector3(offset_point.x + xy_range * (Screen.width / Screen.height), offset_point.y + xy_range, offset_point.z);
            xy_range *= 2;

            // check if anather asteroid inhabits placement matrix cell
            int cell_x = Mathf.Clamp(Mathf.RoundToInt(biased_point.x / (xy_range * Screen.width / Screen.height) * (float)cell_density), 0, cell_density - 1);
            int cell_y = Mathf.Clamp(Mathf.RoundToInt(biased_point.y / xy_range * (float)cell_density), 0, cell_density - 1);
            int cell_z = Mathf.Clamp(Mathf.RoundToInt(biased_point.z / (spawn_range - spawn_offset) * (float)cell_density), 0, cell_density - 1);
            if (placement_matrix[cell_x][cell_y][cell_z] || in_dead_zone(dead_zones, offset_point))
            {
                if (abs >= 1000)
                {
                    Debug.LogError("Failed to spawn all asteroids in " + abs + " attempts. Asteroids spawned: " + i);
                    break;
                }
                i--;
                abs++;
                continue;
            }
            else
            {
                placement_matrix[cell_x][cell_y][cell_z] = true;
            }
            Vector3 placing_pos = new Vector3(player_transform.position.x + offset_point.x, player_transform.position.y + offset_point.y, player_transform.position.z + offset_point.z);
            
            blue_asteroids.Add(Instantiate(blue_asteroid_prefab, placing_pos, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // recreate dead zones when they have been passed by player
        for (int i = 0; i < dead_zones.Count; i++)
        {
            if (dead_zones[i][1].z < player_transform.position.z)
            {
                Vector3 start_point = new Vector3(Random.Range(-screenspace_x * 0.7f, screenspace_x * 0.7f), Random.Range(-screenspace_y * 0.7f, screenspace_y * 0.7f),
                    Random.Range(player_transform.position.z + 15, player_transform.position.z + 25));
                Vector3 direction = new Vector3(Random.Range(0, 0.3f), Random.Range(0, 0.3f), 1);
                Vector3 end_point = start_point + direction * (Random.Range(min_length_of_dead_zone, max_length_of_dead_zone));
                dead_zones[i][0] = start_point;
                dead_zones[i][1] = end_point;
            }
        }

        int retry_count = 0;


        // asteroid respawning
        for (int i = 0; i < blue_asteroids.Count; i++)
        {
            if (blue_asteroids[i].transform.position.z < player_transform.position.z)
            {
                Vector3 offset_point = new Vector3(Random.Range(-screenspace_x, screenspace_x), Random.Range(-screenspace_y, screenspace_y), spawn_range);
                Vector3 new_pos = new Vector3(player_transform.position.x + offset_point.x, player_transform.position.y + offset_point.y, player_transform.position.z + offset_point.z);

                if (retry_count >= 50)
                {
                    Debug.LogError("Failed to respawn asteroid");
                    break;
                }
                bool retry = false;
                for (int j = 0; j < dead_zones.Count; j++)
                {
                    if (in_dead_zone(dead_zones, new_pos))
                    {
                        retry = true;
                        break;
                    }
                }
                if (retry)
                {
                    i--;
                    retry_count++;
                    continue;
                }

                blue_asteroids[i].transform.position = new_pos;
            }
        }

        for (int i = 0; i < dead_zones.Count ; i++)
        {
            Debug.DrawLine(dead_zones[i][0], dead_zones[i][1]);
        }
    }
}
