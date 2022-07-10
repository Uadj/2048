using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private NodeSpawner nodeSpawner;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private Transform blockRect;

    public List<Node> NodeList { private set; get; }
    public Vector2Int BlockCount { private set; get; }

    private void Awake()
    {
        BlockCount = new Vector2Int(4, 4);
        NodeList = nodeSpawner.SpawnNodes(BlockCount);
    }
    private void Start()
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(nodeSpawner.GetComponent<RectTransform>());

        foreach(Node node in NodeList)
        {
            node.localPosition = node.GetComponent<RectTransform>().localPosition;
        }
        SpwanBlockToRandomNode();
        SpwanBlockToRandomNode();
        
  
    }
    private void Update()
    {
        if (Input.GetKeyDown("1")) SpwanBlockToRandomNode();
    }
    private void SpwanBlockToRandomNode()
    {
        List<Node> emptyNodes = NodeList.FindAll(x => x.placedBlock == null);

        if (emptyNodes.Count != 0)
        {
            int index = Random.Range(0, emptyNodes.Count);
            Vector2Int point = emptyNodes[index].Point;
            SpawnBlock(point.x, point.y);
        }
        else
        {

        }
    }
    private void SpawnBlock(int x, int y)
    {
        if (NodeList[y*BlockCount.x + x].placedBlock != null) return;
        GameObject clone = Instantiate(blockPrefab, blockRect);
        Block block = clone.GetComponent<Block>();
        Node node = NodeList[y * BlockCount.x + x];
        clone.GetComponent<RectTransform>().localPosition = node.localPosition;

        node.placedBlock = block;
    }
}
