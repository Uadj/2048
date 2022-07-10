using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { None = -1, Right = 0, Down, Left, Up }
public class Node : MonoBehaviour
{
    public Block placedBlock;
    public Vector2 localPosition;
    public bool combined = false;
    public Vector2Int?[] NeighborNodes { private set; get; }
    public Vector2Int Point { private set; get; }
    private Board board;
    public void Setup(Board board, Vector2Int?[] neighborNodes, Vector2Int point)
    {
        this.board = board;
        NeighborNodes = neighborNodes;
        Point = point;
    }
    public Node FindTarget(Node originalNode, Direction direction, Node farNode = null)
    {
        if(NeighborNodes[(int)direction].HasValue == true)
        {
            Vector2Int point = NeighborNodes[(int)direction].Value;
            Node neighborNode = board.NodeList[point.y * board.BlockCount.x + point.x];
            if(neighborNode != null && neighborNode.combined)
            {
                return this;
            }
            if(neighborNode.placedBlock != null && originalNode.placedBlock != null)
            {
                if(neighborNode.placedBlock.Numeric == originalNode.placedBlock.Numeric)
                {
                    return neighborNode;
                }
                else
                {
                    return farNode;
                }
            }
            return neighborNode.FindTarget(originalNode, direction, neighborNode);
        }
        return farNode;
    }
}
