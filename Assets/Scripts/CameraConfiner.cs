using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfiner : UnityEngine.MonoBehaviour
{
    [SerializeField] CinemachineConfiner confinder;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBounds();
    }

    public void UpdateBounds()
    {
        UnityEngine.GameObject go = UnityEngine.GameObject.Find("CameraConfiner");
        if (go == null )
        {
            confinder.m_BoundingShape2D = null;
            return;
        }

        Collider2D bounds = go.GetComponent<Collider2D>();
        confinder.m_BoundingShape2D = bounds;
    }
}
