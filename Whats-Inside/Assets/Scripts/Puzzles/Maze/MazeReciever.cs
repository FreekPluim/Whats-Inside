using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Nodes { walkable, wall, player, target}

public class MazeReciever : Puzzle
{
    MazeHub stream;

    int reset = 8;
    public int buttonPressed;

    [SerializeField] GameObject nodesParent;
    [SerializeField] GameObject nodePrefab;

    [SerializeField] List<Node> nodes = new List<Node>();
    [SerializeField] Dictionary<Vector2Int, Node> nodeLocations = new Dictionary<Vector2Int, Node>();
    Node playerNode;
    Node targetNode;

    bool won;

    private void Start()
    {
        stream = transform.parent.parent.GetComponent<MazeHub>();
        buttonPressed = reset;
        setupPlayer();
        setupTarget();
        foreach (var node in nodes)
        {
            nodeLocations.Add(new Vector2Int(node.x, node.y), node);
        }
        nodeLocations[new Vector2Int(playerNode.x, playerNode.y)].nodeType = Nodes.player;
    }

    void setupPlayer()
    {
        playerNode = new Node();
        playerNode.x = 1;
        playerNode.y = 0;
    }

    void setupTarget()
    {
        targetNode = new Node();
        targetNode.x = 18;
        targetNode.y = 1;
    }

    private void Update()
    {
        if(buttonPressed != reset)
        {
            switch (buttonPressed)
            {
                case 0:
                    handleUp();
                    break;
                case 1:
                    handleLeft();
                    break;
                case 2:
                    handleDown();
                    break;
                case 3:
                    handleRight();
                    break;
                default:
                    break;
            }
            buttonPressed = reset;
        }

        if (playerNode.x == targetNode.x && playerNode.y == targetNode.y)
        {
            won = true;
            stream.SendIfWon(won);
        }
    }

    void handleUp()
    {
        if (nodeLocations.ContainsKey(new Vector2Int(playerNode.x, playerNode.y + 1)))
        {
            if (nodeLocations[new Vector2Int(playerNode.x, playerNode.y + 1)].nodeType == Nodes.walkable)
            {
                setPlayerNode(nodeLocations[new Vector2Int(playerNode.x, playerNode.y)], nodeLocations[new Vector2Int(playerNode.x, playerNode.y + 1)]);
            }
        }
    }
    void handleLeft()
    {
        if (nodeLocations.ContainsKey(new Vector2Int(playerNode.x - 1, playerNode.y)))
        {
            if (nodeLocations[new Vector2Int(playerNode.x - 1, playerNode.y)].nodeType == Nodes.walkable)
            {
                setPlayerNode(nodeLocations[new Vector2Int(playerNode.x, playerNode.y)], nodeLocations[new Vector2Int(playerNode.x - 1, playerNode.y)]);
            }
        }
    }
    void handleDown()
    {
        if (nodeLocations.ContainsKey(new Vector2Int(playerNode.x, playerNode.y - 1)))
        {
            if (nodeLocations[new Vector2Int(playerNode.x, playerNode.y - 1)].nodeType == Nodes.walkable)
            {
                setPlayerNode(nodeLocations[new Vector2Int(playerNode.x, playerNode.y)], nodeLocations[new Vector2Int(playerNode.x, playerNode.y - 1)]);
            }
        }
    }
    void handleRight()
    {
        if (nodeLocations.ContainsKey(new Vector2Int(playerNode.x + 1, playerNode.y)))
        {
            if (nodeLocations[new Vector2Int(playerNode.x + 1, playerNode.y)].nodeType == Nodes.walkable)
            {
                setPlayerNode(nodeLocations[new Vector2Int(playerNode.x, playerNode.y)], nodeLocations[new Vector2Int(playerNode.x + 1, playerNode.y)]);
            }
        }
    }
    private void setPlayerNode(Node oldNode, Node newNode)
    {
        oldNode.nodeType = Nodes.walkable;
        newNode.nodeType = Nodes.player;
        playerNode = newNode;
    }


    public void CreateMaze()
    {
        for (int y = 0; y < 13; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                GameObject go = Instantiate(nodePrefab, nodesParent.transform);
                go.transform.localPosition = new Vector3(x, y, 0);

                Node node = go.GetComponent<Node>();
                node.x = x;
                node.y = y;
                nodes.Add(node);
            }
        }
    }
    public void ClearMaze()
    {
        for (int i = nodesParent.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(nodesParent.transform.GetChild(i).gameObject);
        }
        nodes.Clear();
    }

}
