using System.Collections;
using UI;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Slingshot : MonoBehaviour
{   //TODO: Set velocity max
    //TODO: Only trigger when on player & a set num of times
    
    public new Camera camera;
    // [FormerlySerializedAs("ballPos")] 
    public Vector3 mouseBallPos;
    public Sprite pathSprite;
    public float velocityFactor = 10.0f;
    public DisplayType displayType = DisplayType.Levels;

    private Rigidbody2D _rb;
    private GameObject _path;
    private IEnumerator _calcPath;
    private float _distanceFromBall;
    private bool _pathReady;
    private GameObject pause;
    private bool menuDisplayed;


    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _calcPath = CalculateBallPath();
        pause = GameObject.FindGameObjectWithTag("Pause_Screen");
        if (displayType == DisplayType.Levels)
        {
            pause.SetActive(false);    
        }
        
    }

    void Update()
    {
        //Prevents ball interaction during menu screen
        if (Input.GetMouseButtonDown(0) && menuDisplayed) return; 
        
        
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        if (Input.GetMouseButtonDown(0) && CheckTouch(displayType))
        {
            StartCoroutine(_calcPath);
        }

        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        if (Input.GetMouseButtonUp(0) && _pathReady)
        {
            _pathReady = false;
            StopCoroutine(_calcPath);
            Vector3 diff = transform.position - mouseBallPos; 
            //Find the change in x and y with the mouse movements
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = diff * velocityFactor;
            Destroy(_path);
        }
    }

    void RenderPath(float pathLength, DisplayType displayType)
    {
        if (_path != null)
        {
            Destroy(_path);
        }

        Vector3 ballPos = transform.position;
        Vector2 diff = ballPos-mouseBallPos;
        _path = new GameObject();
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        if (displayType == DisplayType.Levels)
        {
            _path.AddComponent<SpriteRenderer>().sprite = pathSprite;
            _path.transform.localScale = new Vector3(2f, pathLength, 1);
        }
        
        else
        {
            RectTransform rectTransform = _path.AddComponent<RectTransform>();
            _path.AddComponent<CanvasRenderer>();
            _path.AddComponent<Image>().sprite = pathSprite;
            
            rectTransform.localScale = new Vector3(2f,pathLength/140,1);
            rectTransform.pivot = new Vector2(0.5f, 0.75f);
        }
        
        _path.transform.SetParent(transform);
        _path.transform.localPosition = new Vector3(0, 0, 0); 
        
        //TODO: Add state anims
        //-(pathLength/2) sets the height to be half of the the height below (sets it directly under the ball) (used before custom sprites w pivot points)
        float angle = Mathf.Atan2(diff.y,diff.x); //Change in y/x with inverse tangent
        float degrees = angle * Mathf.Rad2Deg+90;
        //https://forum.unity.com/threads/why-am-i-getting-the-wrong-angles-with-vector2-angle.209262/ Code from there. A big help for the angles
        _path.transform.RotateAround(ballPos, new Vector3(0,0,1), degrees);
       
    }

    IEnumerator CalculateBallPath()
    {
        while (true)
        {
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            if (displayType == DisplayType.Levels)
            {
                mouseBallPos = camera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 1));
                //The x and y pos of the mouse when pulling back on the ball    
            }
            else
            {
                mouseBallPos = new Vector3(mouseX, mouseY, 1);
            }
            
            _distanceFromBall = Vector2.Distance(mouseBallPos, gameObject.transform.position);
            //Length of the hyp when measuring the distance from the ball and mouse
            
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            RenderPath(_distanceFromBall, displayType);
            _pathReady = true;
            yield return new WaitForSeconds(0);
        } 
    }

    bool CheckTouch(DisplayType displayType)
    {
        RaycastHit2D hit;
        if (displayType == DisplayType.Levels)
        {
            hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);    
        }
        else
        {
            hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
        }


        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }    
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("FinishLevel") && !menuDisplayed)
        {
            menuDisplayed = true;
            MenuHandler.LevelOver(pause, "Win");
        }else if (col.CompareTag("Border") && !menuDisplayed)
        {
            menuDisplayed = true;
            MenuHandler.LevelOver(pause, "Loose");
        }
    }

    // bool CheckInAir()
    // {
    //     ContactFilter2D contactFilter2D = new ContactFilter2D();
    //     // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
    //     int collisionCount = GetComponent<CircleCollider2D>().OverlapCollider(contactFilter2D, new Collider2D[1]);
    //     if (collisionCount >= 1)
    //     {
    //         return true;
    //     }
    //     return false;
    // }
}

public enum DisplayType
{
    Levels,
    Menu
}
