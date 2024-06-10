using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    [SerializeField] CinemachineConfiner confiner;

    void Start()
    {
        Updatebounds();
    }

    public void Updatebounds()
    {
        GameObject go = GameObject.Find("CameraConfiner");
        if (go == null) 
        {
            confiner.m_BoundingShape2D = null;
            return;
        }
        Collider2D bounds = go.GetComponent<Collider2D>();
        confiner.m_BoundingShape2D = bounds;
    }
}
