using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] Transform ChaseTrs;

    
    void Update()
    {
        Vector3 fixedPos = ChaseTrs.position;

        fixedPos.z = transform.position.z;

        transform.position = fixedPos;
    }
}
