using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySystem : MonoBehaviour
{
    public Camera cam;
    public float scrollSpeed = 10;
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private CsvParser paser;
    const float baseSize = 1f;

    // Start is called before the first frame update
    void Start()
    {
        paser = this.gameObject.GetComponent<CsvParser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0 && cam.orthographicSize < 1100)
        {
            //cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + (Input.mouseScrollDelta.y * scrollSpeed), cam.transform.position.z);
            cam.orthographicSize += Input.mouseScrollDelta.y * scrollSpeed;
            float s = (float)(System.Math.Pow(cam.orthographicSize, 0.5f) * baseSize);
            foreach (Star st in paser.starData.Values)
            {
                st.starObject.transform.localScale = new Vector3(s, s, s);
            }
        }
        else if (Input.mouseScrollDelta.y < 0 && cam.orthographicSize > 100)
        {
            //cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + (Input.mouseScrollDelta.y * scrollSpeed), cam.transform.position.z);
            cam.orthographicSize += Input.mouseScrollDelta.y * scrollSpeed;
            float s = (float)(System.Math.Pow(cam.orthographicSize, 0.5f) * baseSize);
            foreach (Star st in paser.starData.Values)
            {
                st.starObject.transform.localScale = new Vector3(s, s, s);
            }
        }
        else if (Input.mouseScrollDelta.y < 0 && cam.orthographicSize > Input.mouseScrollDelta.y * scrollSpeed*0.1f)
        {
            //cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + (Input.mouseScrollDelta.y * scrollSpeed), cam.transform.position.z);
            cam.orthographicSize += Input.mouseScrollDelta.y * scrollSpeed*0.1f;
            float s = (float)(System.Math.Pow(cam.orthographicSize, 0.5f) * baseSize);
            foreach (Star st in paser.starData.Values)
            {
                st.starObject.transform.localScale = new Vector3(s, s, s);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed * cam.orthographicSize, 0, pos.y * dragSpeed * cam.orthographicSize);

        cam.transform.Translate(move, Space.World);
    }
}
//float s = (float)(System.Math.Pow(cam.orthographicSize, 0.5f) * baseSize);
//gameObject.transform.localScale = new Vector3(s, s, s);