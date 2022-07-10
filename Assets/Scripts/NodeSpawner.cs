using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject nodeRect;

    public List<Node> SpawnNodes(Vector2Int blockCount)
    {
        List<Node> nodeList = new List<Node>(blockCount.x * blockCount.y);
        for(int y = 0; y < blockCount.y; ++y)
        {
            for(int x = 0; x<blockCount.x; ++x)
            {
                GameObject clone = Instantiate(nodePrefab, nodeRect.transform);

                Vector2Int point = new Vector2Int(x, y);
                Node node = clone.GetComponent<Node>();
                node.Setup(point);

                clone.name = $"[{node.Point.y}, {node.Point.x}]";

                nodeList.Add(node);
            }
        }

        return nodeList;
    }
}
