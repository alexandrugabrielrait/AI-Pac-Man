using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    [HideInInspector]
    public MapManager manager;
    [HideInInspector]
    public Pacman pm;
    public PriorityQueue<Move>[,,] pq;
    const float stepTime = 0.005f;
    float t = stepTime;
    bool on = false;
    const float learningRate = 0.8f;
    const float discount = 0.99f;

    public void Init()
    {
        manager = GetComponent<MapManager>();
        pm = manager.pm;
        pq = new PriorityQueue<Move>[manager.n, manager.m, 4];
        for (int i = 0; i < manager.n; ++i)
            for (int j = 0; j < manager.m; ++j)
                if (manager.map[i, j] != 1)
                    for (int d = 0; d < 4; ++d)
                    {
                        pq[i, j, d] = new PriorityQueue<Move>();
                        for (int k = 0; k < 4; ++k)
                            if (manager.IsValidPosition(i, j, k))
                                pq[i, j, d].Enqueue(new Move(k, 0));
                    }
        on = true;
    }

    private void Update()
    {
        if (!on)
            return;
        if (manager.turn < MapManager.TURNS_MAX)
        {
            t -= Time.deltaTime;
            if (t <= 0)
            {
                t = stepTime;
                Move move = pq[pm.row, pm.col, pm.direction].Dequeue();
                int row = pm.row;
                int col = pm.col;
                int direction = pm.direction;
                pm.Move(move.direction);
                move.value = (1 - learningRate) * move.value + learningRate * (R(row, col) + discount * GetMaxValue());
                pq[row, col, direction].Enqueue(move);
            }
        }
        else
        {
            manager.Reset();
        }
    }

    public float R(int row, int col)
    {
        if (row != pm.row && col != pm.col)
            return manager.values[pm.row, pm.col];
        return -5;
    }

    private float GetMaxValue()
    {
        return pq[pm.row, pm.col, pm.direction].Peek().value;
    }
}
