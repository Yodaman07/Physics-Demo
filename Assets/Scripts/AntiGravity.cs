using UnityEngine;

public class AntiGravity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (name == "Switch")
            {
                FlipGravity();
            }
        }
    }

    void FlipGravity()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.velocity /= 2;
        rb.gravityScale *= -1;
    }
}