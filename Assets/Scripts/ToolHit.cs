using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHit : UnityEngine.MonoBehaviour
{
    public virtual void Hit()
    {

    }

    public virtual bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return true;
    }
}
