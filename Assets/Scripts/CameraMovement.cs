using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector2 cameraPosition;
    public GameObject Grid;

    void Start()
    {
        this.GetComponent<Camera>().orthographicSize = 25;
    }

    // Update is called once per frame
    void Update()
    {
        int height = Grid.GetComponent<CaveGenerator>().height;
        int width = Grid.GetComponent<CaveGenerator>().width;
        if (Grid.GetComponent<CaveGenerator>().isGenerating)
        {
            this.transform.position = new Vector3(width / 2.0f, height / 2.0f, - 10);
            this.GetComponent<Camera>().orthographicSize = Mathf.Max(height / 2.0f, width / 2.0f);
            Grid.GetComponent<CaveGenerator>().isGenerating = false;
        }
        if(height > 0 && width > 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                this.GetComponent<Camera>().orthographicSize++;
                this.GetComponent<Camera>().orthographicSize = Mathf.Min(this.GetComponent<Camera>().orthographicSize, Mathf.Max(height / 2.0f, width / 2.0f));
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                this.GetComponent<Camera>().orthographicSize--;
                this.GetComponent<Camera>().orthographicSize = Mathf.Max(this.GetComponent<Camera>().orthographicSize, 1);
            }
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            if(horizontalInput != 0 || verticalInput != 0)
            {
                this.transform.position += (new Vector3(horizontalInput, verticalInput, 0) * 0.25f);
                cameraPosition.x = Mathf.Clamp(this.transform.position.x, 0, width);
                cameraPosition.y = Mathf.Clamp(this.transform.position.y, 0, height);
                this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -10);
            }
        }
    }
}
