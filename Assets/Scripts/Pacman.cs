using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public MapManager manager;
    public int direction;
    public int row, col;

    private void Start()
    {
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90.0f * direction);
        transform.position = manager.pivot.transform.position + Vector3.down / 1.3f * row + Vector3.right / 1.3f * col;
    }

    public void Move(int direction)
    {
        if (this.direction != direction)
        {
            this.direction = direction;
        }
        else
        {
            if (!manager.IsValidPosition(row, col, direction))
                return;
            int i = row, j = col;
            switch (direction)
            {
                case 0: ++j; break;
                case 1: --i; break;
                case 2: --j; break;
                default: ++i; break;
            }
            if (manager.map[i, j] == 1)
                return;
            row = i;
            col = j;
            if (manager.map[i, j] == 2)
            {
                ++manager.points;
            }
            else if (manager.map[i, j] == 3)
            {
                --manager.points;
            }
        }
        ++manager.turn;
    }
}
