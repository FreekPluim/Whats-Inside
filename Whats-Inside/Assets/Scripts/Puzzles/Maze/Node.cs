using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class Node : MonoBehaviour
{
    public int x;
    public int y;

    public Nodes nodeType;
    [SerializeField] GameObject visualsWall;
    [SerializeField] GameObject player;
    [SerializeField] GameObject target;

    private void Update()
    {
        switch (nodeType)
        {
            case Nodes.walkable:
                visualsWall.SetActive(false);
                player.SetActive(false);
                target.SetActive(false);
                break;
            case Nodes.wall:
                player.SetActive(false);
                target.SetActive(false);
                visualsWall.SetActive(true);
                break;
            case Nodes.player:
                target.SetActive(false);
                player.SetActive(true);
                break;
            case Nodes.target:
                target.SetActive(true);
                break;
            default:
                break;
        }
    }
}
