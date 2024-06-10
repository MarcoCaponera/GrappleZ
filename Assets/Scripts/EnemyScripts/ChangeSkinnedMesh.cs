using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkinnedMesh : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField]
    private Mesh meshToChange;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            ChangeMesh();
        }
    }

    public void ChangeMesh()
    {
        skinnedMeshRenderer.sharedMesh = meshToChange;
    }
}
