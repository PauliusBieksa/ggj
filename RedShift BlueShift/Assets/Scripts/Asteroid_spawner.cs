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
    List<Object> blue_asteroids;

    // Start is called before the first frame update
    void Start()
    {
        blue_asteroids = new List<Object>();

        // init placemet matrix
        List<bool> placement_matrix_row = new List<bool>();
        for (int i = 0; i < 25; i++) placement_matrix_row.Add(false);
        List<List<bool>> placement_matrix_grid = new List<List<bool>>();
        for (int i = 0; i < 25; i++) placement_matrix_grid.Add(new List<bool>(placement_matrix_row));
        List<List<List<bool>>> placement_matrix = new List<List<List<bool>>>();
        for (int i = 0; i < 25; i++) placement_matrix.Add(new List<List<bool>>(placement_matrix_grid));

        // spawn asteroids
        for (int i = 0; i < 100; i++)
        {
            float z_offset = Random.Range(spawn_offset, spawn_range);
            float xy_range = Mathf.Tan(0.55f) * z_offset;
            float x_offset = Random.Range(-xy_range * Screen.width / Screen.height, xy_range * Screen.width / Screen.height);
            float y_offset = Random.Range(-xy_range, xy_range);

            // check if anather asteroid inhabits placement matrix cell
            int cell_x = Mathf.Clamp(Mathf.RoundToInt(x_offset / (xy_range * Screen.width / Screen.height) * 25f), 0, 24);
            int cell_y = Mathf.Clamp(Mathf.RoundToInt(y_offset / xy_range * 25f), 0, 24);
            int cell_z = Mathf.Clamp(Mathf.RoundToInt(z_offset / (spawn_range - spawn_offset) * 25f), 0, 24);
            Debug.Log(cell_x + " " + cell_y + " " + cell_z);
            if (placement_matrix[cell_x][cell_y][cell_z])
            {
                i--;
                continue;
            }
            else
            {
                placement_matrix[cell_x][cell_y][cell_z] = true;
            }
            Vector3 placing_pos = new Vector3(player_transform.position.x + x_offset, player_transform.position.y + y_offset, player_transform.position.z + z_offset);
            
            blue_asteroids.Add(Instantiate(blue_asteroid_prefab, placing_pos, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject ba in blue_asteroids)
        {
            if (ba.transform.position.z < player_transform.position.z)
            {
                float z_offset = player_transform.position.z - ba.transform.position.z + spawn_range;
                float xy_range = Mathf.Tan(0.55f) * z_offset;
                float x_offset = Random.Range(-xy_range * Screen.width / Screen.height, xy_range * Screen.width / Screen.height);
                float y_offset = Random.Range(-xy_range, xy_range);
                ba.transform.position = new Vector3(player_transform.position.x + x_offset, player_transform.position.y + y_offset, player_transform.position.z + z_offset);
            }
        }
    }
}
