using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotater : MonoBehaviour
{
    [SerializeField] Vector3 _rotateAngle;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotateAngle * Time.deltaTime);
    }
}
