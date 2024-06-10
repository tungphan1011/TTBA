using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceableObject
{
    public Item placedItem;
    public Transform targetObject;
    public Vector3Int positionOnGrid;
    //serialized JSON string which contains the state of the object
    public string objectState;

    public PlaceableObject(Item item, Vector3Int pos)
    {
        placedItem = item;
        positionOnGrid = pos;
    }
}

[CreateAssetMenu(menuName ="Data/Placeable Object Container")]
public class PlaceableObjectsContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;

    internal PlaceableObject Get(Vector3Int position)
    {
        return placeableObjects.Find(x => x.positionOnGrid == position);
    }

    internal void Remove(PlaceableObject placedObject)
    {
        placeableObjects.Remove(placedObject);
    }
}
