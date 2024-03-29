using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModel : MonoBehaviour
{
 
    float yHight = 0;
    
    public void CreateModel (GameObject model)
    {
        var structure = Instantiate(model,transform);
        yHight = structure.transform.position.y;
    }

    public void SwapModel (GameObject model,Quaternion rotation)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var structure = Instantiate(model,transform);
        structure.transform.localPosition = new Vector3(0,yHight,0);
        structure.transform.localRotation = rotation;
    }


}
