using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {  Wait = 0, Processing, End }
public class Board : MonoBehaviour
{
    [SerializeField]
    private NodeSpawner nodeSpawner;
    [SerializeField]
    private TouchController touchController;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private Transform blockRect;
   
    public List<Node> NodeList { private set; get; }
    public Vector2Int BlockCount { private set; get; }
    public List<Block> blockList;

    public State state = State.Wait;
    private void Awake()
    {
        BlockCount = new Vector2Int(4, 4);
        NodeList = nodeSpawner.SpawnNodes(this, BlockCount);
        blockList = new List<Block>();
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
       // if (Input.GetKeyDown("1")) SpwanBlockToRandomNode();
       if(state == State.Wait)
        {
            Direction direction = touchController.UpdateTouch();

            if(direction != Direction.None)
            {
                AllBlocksProcess(direction);
            }
        }
        else
        {
            UpdateState();
        }
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
            if (IsGameOver())
            {
                OnGameOver();
            }
        }
    }

    private void SpawnBlock(int x, int y)
    {
        if (NodeList[y*BlockCount.x + x].placedBlock != null) return;
        GameObject clone = Instantiate(blockPrefab, blockRect);
        Block block = clone.GetComponent<Block>();
        Node node = NodeList[y * BlockCount.x + x];
        clone.GetComponent<RectTransform>().localPosition = node.localPosition;
        block.Setup();
        node.placedBlock = block;
        blockList.Add(block);
    }
    private void AllBlocksProcess(Direction direction)
    {
        if(direction == Direction.Right)
        {
            for(int y = 0; y< BlockCount.y; ++y)
            {
                for(int x = (BlockCount.x-2); x>=0; --x)
                {
                    BlockProcess(NodeList[y * BlockCount.x + x], direction);
                }
            }
        }
        else if(direction == Direction.Down)
        {
            for (int y = (BlockCount.y-2); y >=0; --y)
            {
                for (int x = 0; x < BlockCount.x; ++x)
                {
                    BlockProcess(NodeList[y * BlockCount.x + x], direction);
                }
            }
        }
        else if(direction == Direction.Left)
        {
            for (int y = 0; y < BlockCount.y; ++y)
            {
                for (int x = 1; x < BlockCount.x; ++x)
                {
                    BlockProcess(NodeList[y * BlockCount.x + x], direction);
                }
            }
        }
        else if(direction == Direction.Up)
        {
            for (int y = 1; y < BlockCount.y; ++y)
            {
                for (int x = 0; x < BlockCount.x; ++x)
                {
                    BlockProcess(NodeList[y * BlockCount.x + x], direction);
                }
            }
        }
        foreach(Block block in blockList)
        {
            if(block.Target != null)
            {
                state = State.Processing;
                block.StartMove();
            }
        }
        if (IsGameOver())
        {
            OnGameOver();
        }
    }
    private void BlockProcess(Node node, Direction direction)
    {
        if (node.placedBlock == null) return;
        Node neighborNode = node.FindTarget(node, direction);
        if(neighborNode != null)
        {
            if (node.placedBlock != null && neighborNode.placedBlock != null)
            {
                if(node.placedBlock.Numeric == neighborNode.placedBlock.Numeric)
                {
                    Combine(node, neighborNode);
                }
            }
            else if(neighborNode != null && neighborNode.placedBlock == null)
            {
                Move(node, neighborNode);
            }
        }
    }
    public void Combine(Node from, Node to)
    {
        from.placedBlock.CombineToNode(to);
        from.placedBlock = null;
        to.combined = true;
    }
    private void Move(Node from, Node to)
    {
        from.placedBlock.MoveToNode(to);
        if(from.placedBlock != null)
        {
            to.placedBlock = from.placedBlock;
            from.placedBlock = null;
        }
    }
    private void UpdateState()
    {
        bool targetAllNull = true;

        foreach(Block block in blockList)
        {
            if(block.Target != null)
            {
                targetAllNull = false;
                break;
            }
        }
        if(targetAllNull && state == State.Processing)
        {
            List<Block> removeBlocks = new List<Block>();
            foreach(Block block in blockList)
            {
                if (block.NeedDestroy)
                {
                    removeBlocks.Add(block);
                }
            }
            removeBlocks.ForEach(x =>
            {
                blockList.Remove(x);
                Destroy(x.gameObject);
            });
            state = State.End;
        }
        if(state == State.End)
        {
            state = State.Wait;

            SpwanBlockToRandomNode();

            NodeList.ForEach(x => x.combined = false);
        }
    }
    private bool IsGameOver()
    {
        foreach(Node node in NodeList)
        {
            if (node.placedBlock == null) return false;
            for(int i=0; i<node.NeighborNodes.Length; ++i)
            {
                if (node.NeighborNodes[i] == null) continue;

                Vector2Int point = node.NeighborNodes[i].Value;
                Node neighborNode = NodeList[point.y * BlockCount.x + point.y];

                if(node.placedBlock != null && neighborNode.placedBlock != null)
                {
                    if (node.placedBlock.Numeric == neighborNode.placedBlock.Numeric)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    private void OnGameOver()
    {
        Debug.Log("GameOver");
    }
}
