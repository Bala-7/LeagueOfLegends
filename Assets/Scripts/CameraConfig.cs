using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraConfig : MonoBehaviour
{
    bool editMode = true;
    // Start is called before the first frame update
    void Start()
    {
        editMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!editMode)
            GetComponent<CameraMovement>().CenterAtPlayer();
    }
}
