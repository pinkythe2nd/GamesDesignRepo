using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    float speed = 0.06f;
    float zoomSpeed = 10.0f;
    float rotateSpeed = 0.25f;

    float maxHeight = 40f;
    float minHeight = 4f;

    Vector2 p1;
    Vector2 p2;

    private Rigidbody rb;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 0.06f;
            zoomSpeed = 20.0f;
        }
        else
        {
            speed = 0.035f;
            zoomSpeed = 10.0f;
        }

        float hsp = transform.position.y * speed * Input.GetAxis("Horizontal");
        float vsp = transform.position.y * speed * Input.GetAxis("Vertical");
        float scrollSp = Mathf.Log(transform.position.y) * -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

        /*        if ((transform.position.y >= maxHeight) && scrollSp > 0)
                {
                    scrollSp = 0;
                }
                else if ((transform.position.y <= minHeight) && scrollSp < 0)
                {
                    scrollSp = 0;
                }
        */
        if ((transform.position.y + scrollSp) > maxHeight)
        {
            scrollSp = maxHeight - transform.position.y;
        }
        else if ((transform.position.y + scrollSp) < minHeight)
        {
            scrollSp = minHeight - transform.position.y;
        }

        Vector3 verticalMove = new Vector3(0, scrollSp, 0);
        Vector3 lateralMove = hsp * transform.right;
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= vsp;

        Vector3 move = verticalMove + lateralMove + forwardMove;
        transform.position += move;

        getCameraRotation();
    }
    void getCameraRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            p1 = Input.mousePosition;

        }

        if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl))
        {
            p2 = Input.mousePosition;

            float dx = (p2 - p1).x * rotateSpeed;
            float dy = (p2 - p1).y * rotateSpeed;

            transform.rotation *= Quaternion.Euler(0, dx, 0);
            transform.GetChild(0).transform.rotation *= Quaternion.Euler(-dy, 0, 0);

            p1 = p2;


        }

    }
}
