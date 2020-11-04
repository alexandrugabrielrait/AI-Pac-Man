using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform[] arrows = new Transform[4];
    [HideInInspector]
    public Trainer trainer;
    [HideInInspector]
    public int row, col;

    public void Start()
    {
        trainer = FindObjectOfType<Trainer>();
    }

    public void ChangeDirections()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (trainer.pq[row, col, i].Count() > 0)
            {
                int direction = trainer.pq[row, col, i].Peek().direction;
                arrows[i].rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90.0f * direction);
            }
            else
                arrows[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
