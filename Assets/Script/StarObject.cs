using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarObject : MonoBehaviour
{
    private Camera cam;
    const float baseSize = 2f;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float s = (float)(System.Math.Pow(cam.orthographicSize, 0.5f) * baseSize);
        gameObject.transform.localScale = new Vector3(s,s,s);
    }
}
