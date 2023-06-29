using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed = 10f;
    public Vector2 xRange = new Vector2(-0.7f, 8.4f);
    public Vector2 zRange = new Vector2(-0.7f, 8.4f);
    public Vector3 newPos;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (horizontalInput != 0 || verticalInput != 0)
        {
            float hDelta = horizontalInput * speed * Time.deltaTime;
            float vDelta = verticalInput * speed * Time.deltaTime;
            newPos.x = Mathf.Clamp(transform.position.x + hDelta, xRange.x, xRange.y);
            newPos.z = Mathf.Clamp(transform.position.z + vDelta, zRange.x, zRange.y);
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }
}
