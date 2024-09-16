using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotateSpeed = new Vector3(0, 20, 0);

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(rotateSpeed * Time.deltaTime);
    }
}
