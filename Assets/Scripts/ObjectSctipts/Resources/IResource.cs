using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResource
{
    public enum ResourceType { wood, iron, stone }

    public ResourceType resourceType { get; set; }
    string name { get; }
    int resourceAmount { get; }
}
