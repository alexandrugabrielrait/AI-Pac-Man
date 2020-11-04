using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    public int[,] map;
    public int n, m;
    public GameObject[] prefabs;
    public float[,] values;
    public GameObject[,] tiles;
    public GameObject pivot;
    public Pacman pm;
    public TMP_Text pointsText;
    [HideInInspector]
    public int points, pointsMax;
    public int rowStart, colStart, dirStart;
    [HideInInspector]
    public int seed;
    public int turn = 1, generation = 1;
    public const int TURNS_MAX = 100;

    private void Start()
    {
        pointsMax = int.MinValue;
        seed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(seed);
        map = new int[n, m];
        tiles = new GameObject[n, m];
        values = new float[n, m];
        rowStart = Random.Range(1, n - 1);
        colStart = Random.Range(1, m - 1);
        dirStart = Random.Range(0, 4);
        for (int i = 0; i < n; ++i)
            for (int j = 0; j < m; ++j)
                if (IsBorder(i, j))
                    map[i, j] = 1;
                else if (Random.Range(0, 5) == 0)
                    map[i, j] = 1;
        MovePacman(rowStart, colStart, dirStart);
        points = 0;
        BFS();
        BuildObjects();
        GetComponent<Trainer>().Init();
    }

    public void Reset()
    {
        ++generation;
        pointsMax = Mathf.Max(points, pointsMax);
        MovePacman(rowStart, colStart, dirStart);
        turn = 0;
        points = 0;
    }

    private void BuildObjects()
    {
        for (int i = 0; i < n; ++i)
            for (int j = 0; j < m; ++j)
                if (prefabs[map[i, j]])
                {
                    if (map[i, j] > 1)
                    {
                        tiles[i, j] = GameObject.Instantiate(prefabs[0]);
                        tiles[i, j].transform.position = pivot.transform.position + Vector3.down / 1.3f * i + Vector3.right / 1.3f * j;
                        GameObject pickup = GameObject.Instantiate(prefabs[map[i, j]]);
                        pickup.transform.position = pivot.transform.position + Vector3.down / 1.3f * i + Vector3.right / 1.3f * j;
                        if (map[i, j] == 2)
                            values[i, j] = 10;
                        else
                            values[i, j] = -10;
                    }
                    else
                    {
                        tiles[i, j] = GameObject.Instantiate(prefabs[map[i, j]]);
                        tiles[i, j].transform.position = pivot.transform.position + Vector3.down / 1.3f * i + Vector3.right / 1.3f * j;
                        values[i, j] = -1;
                    }
                    ArrowPointer ap = tiles[i, j].GetComponent<ArrowPointer>();
                    if (ap)
                    {
                        ap.row = i;
                        ap.col = j;
                    }
                }
    }

    public bool IsBorder(int i, int j)
    {
        return i == 0 || j == 0 || i == n - 1 || j == m - 1;
    }

    public bool IsOnMap(int i, int j)
    {
        return i >= 0 && i < n & j >= 0 && j < m;
    }

    public void BFS()
    {
        Queue<Pair> q = new Queue<Pair>();
        bool[,] visited = new bool[n, m];
        q.Enqueue(new Pair(pm.row, pm.col));
        visited[pm.row, pm.col] = true;
        while (q.Count > 0)
        {
            Pair p = q.Dequeue();
            int i = p.first;
            int j = p.second;
            if ((i != pm.row && j != pm.col) && map[i, j] == 0 && Random.Range(0, 18) == 0)
            {
                map[i, j] = Random.Range(2, 4);
            }
            AddIfValid(q, visited, i + 1, j);
            AddIfValid(q, visited, i, j + 1);
            AddIfValid(q, visited, i - 1, j);
            AddIfValid(q, visited, i, j - 1);
        }
    }

    public void AddIfValid(Queue<Pair> q, bool[,] visited, int i, int j)
    {
        if (IsOnMap(i, j) && map[i, j] != 1 && !visited[i, j])
        {
            q.Enqueue(new Pair(i, j));
            visited[i, j] = true;
        }
    }

    public bool IsValidPosition(int row, int col, int direction)
    {
        switch (direction)
        {
            case 0: ++col; break;
            case 1: --row; break;
            case 2: --col; break;
            default: ++row; break;
        }

        return IsOnMap(row, col) && map[row, col] != 1;
    }

    public void Update()
    {
        SetArrows();
        if (points < -TURNS_MAX)
            pointsText.text = "Points: " + points + "\nHigh Score: -\nTurn: " + turn + "\nGeneration: " + generation + "\nSeed: " + seed;
        else
            pointsText.text = "Points: " + points + "\nHigh Score: " + pointsMax + "\nTurn: " + turn + "\nGeneration: " + generation + "\nSeed: " + seed;
    }

    public void MovePacman(int i, int j, int d)
    {
        pm.row = i;
        pm.col = j;
        pm.direction = d;
        map[i, j] = 0;
    }

    public void SetArrows()
    {
        for (int i = 0; i < n; ++i)
            for (int j = 0; j < m; ++j)
                if (map[i, j] != 1)
                    tiles[i, j].GetComponent<ArrowPointer>().ChangeDirections();
    }
}
