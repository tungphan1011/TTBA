using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class PlaceableOject
{
    public Item placedItem;
    public Transform targetObject;
    public Vector3Int positionOnGrid;

    public PlaceableOject(Item item, Transform target, Vector3Int pos)
    {
        placedItem = item;
        targetObject = target;
        positionOnGrid = pos;
    }
}

[CreateAssetMenu(menuName ="Data/Placeable Objects Container")]
public class PlaceableObjectsContainer : ScriptableObject
{
    public List<PlaceableOject> placeableOjects;
}
