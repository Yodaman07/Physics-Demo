using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    
    public Directions enterDirection;
    public Directions exitDirection;

    private Transform _portalOut;
    private GameObject _player;

    void Awake()
    {
        _portalOut = transform.parent.GetChild(0).transform;
        _player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        bool correctDirection = GetDirection.CheckHitDirection(col.transform.position - transform.position, enterDirection);
        if (col.CompareTag("Player") && correctDirection)
        {
            //20 pixels is one unit
            //pixel count/20
            _player.transform.position = _portalOut.position + GetDirection.GetOutDirection(exitDirection);
            // Debug.Break();
            
            Rigidbody2D rb = _player.GetComponent<Rigidbody2D>();
            rb.velocity = GetDirection.GetCorrectVelocity(exitDirection, enterDirection, rb.velocity);
        }
    }
    
}