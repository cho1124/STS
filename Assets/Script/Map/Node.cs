using System.Collections.Generic;
using UnityEngine;
using System;


public enum NodeType
{
    Normal,
    Elite,
    Rest,
    Shop,
    Treasure,
    Event,
    Boss
}
[CreateAssetMenu(fileName = "New Node", menuName = "ScriptableObjects/Node")]
public class Node : ScriptableObject
{
    public NodeType nodeType;
    public string nodeName;
    public GameObject nodeprefab;


    

}
