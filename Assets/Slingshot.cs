using System;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public new Camera camera;
    public Vector3 ballPos;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            ballPos = camera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 1));
        }

        if (Input.GetMouseButtonUp(0))
        {
            transform.Translate(0,ballPos.y-3,1);
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = new Vector2(0, -10);
            
        }
    }
}
