using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pea : MonoBehaviour
{
    public void SetPostion(float x, float z)
    {
        transform.position = new Vector3(x, 0, z);
    }
}
