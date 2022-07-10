using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Block placedBlock;
    public Vector2 localPosition;

    public Vector2Int Point { private set; get; }
    public void Setup(Vector2Int point)
    {
        Point = point;
    }
}
