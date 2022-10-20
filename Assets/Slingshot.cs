using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Slingshot : MonoBehaviour
{   //TODO: Set velocity max
    public new Camera camera;
    [FormerlySerializedAs("ballPos")] public Vector3 mouseBallPos;
    public Sprite pathSprite;
    public float velocityFactor = 10.0f;


    private Rigidbody2D _rb;
    private GameObject _path;
    private IEnumerator _calcPath;
    private float _distanceFromBall;

    
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _calcPath = CalculateBallPath();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(_calcPath);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine(_calcPath);
            Vector3 diff = transform.position - mouseBallPos; 
            //Find the change in x and y with the mouse movements
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = diff * velocityFactor;
            Destroy(_path);
        }
    }

    void RenderPath(float pathLength)
    {
        if (_path != null)
        {
            Destroy(_path);
        }
        _path = new GameObject();
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        _path.AddComponent<SpriteRenderer>().sprite = pathSprite;
        _path.transform.localScale = new Vector3(0.2f,pathLength,1);
        _path.transform.SetParent(transform);
        
        _path.transform.localPosition = new Vector3(0, -(pathLength/2), 0);
        //-(pathLength/2) sets the height to be half of the the height below (sets it directly under the ball)

        
        
        // Vector2 rtAngle = new Vector2(mouseBallPos.x, ballPos.y);
        // float hyp = Vector2.Distance(ballPos, mouseBallPos);
        // float op = Vector2.Distance(ballPos, rtAngle);
        // float angle = MathF.Acos((op / hyp));


        float angle = Vector2.Angle(mouseBallPos, transform.position);
        
        _path.transform.RotateAround(transform.position, new Vector3(0,0,1), 1);
    }

    IEnumerator CalculateBallPath()
    {
        while (true)
        {
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;
            mouseBallPos = camera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 1));
            //The x and y pos of the mouse when pulling back on the ball
            _distanceFromBall = Vector2.Distance(mouseBallPos, gameObject.transform.position);
            //Length of the hyp when measuring the distance from the ball and mouse
            
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            RenderPath(_distanceFromBall);
            yield return new WaitForSeconds(0);
        } 
    }
}
