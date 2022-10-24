using Unity.VisualScripting;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public Sprite ropeSprite;
    public new Camera camera;
    
    private bool _ropeSpawned;
    private GameObject _rope;
    private GameObject _player;
    private GameObject _swing;

    
    private void Start()
    {
        _player = GameObject.Find("Player");
        _swing = GameObject.Find("Swing");
    }

    private void Update()
    {
        if (_ropeSpawned)
        {
            Vector3 swingPos = _swing.transform.position - new Vector3(0,2,0);
            Vector3 playerPos = _player.transform.position;

            Vector3 diff = swingPos - playerPos;
            

            _rope.transform.position = (playerPos + swingPos) / 2;
            float angle = Mathf.Atan(diff.y / diff.x);
            float degrees = angle * Mathf.Rad2Deg;

            _rope.transform.eulerAngles = new Vector3(0, 0, degrees + 90);
            _rope.transform.localScale = new Vector3(0.05f, Vector3.Distance(swingPos, playerPos), 1);
            
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            SetDistancePhysics();


            if (RopeCut())
            {
                
                Destroy(_rope);
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                Destroy(_player.GetComponent<DistanceJoint2D>());
                _ropeSpawned = false;
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && _ropeSpawned == false)
        {
            SpawnRope();
        }
    }

    void SpawnRope()
    {
        _rope = new GameObject();
        _rope.AddComponent<BoxCollider2D>();
        _rope.AddComponent<SpriteRenderer>().sprite = ropeSprite;
        _rope.transform.localScale = new Vector3(0.05f, 4, 1);
        _ropeSpawned = true;

    }

    void SetDistancePhysics()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        Vector2 beforeVelocity = _player.GetComponent<Rigidbody2D>().GetPointVelocity(_player.transform.position);
        
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (_player.GetComponent<DistanceJoint2D>() == null)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            _player.AddComponent<DistanceJoint2D>();    
        }
        
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        DistanceJoint2D dj = _player.GetComponent<DistanceJoint2D>();
        
        dj.connectedAnchor = _swing.transform.position - new Vector3(0,2,0);
        dj.autoConfigureDistance = true;
        // dj.enableCollision = true;
        
        dj.attachedRigidbody.velocity = beforeVelocity;
        //TODO: Add multiplier
        
    }

    bool RopeCut()
    {
        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (Input.GetMouseButton(0) && hit.collider != null)
        {
            if (hit.collider.gameObject == _rope)
            {
                return true;
            }
        }
        return false;
    }

}
