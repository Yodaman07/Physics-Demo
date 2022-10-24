using UnityEngine;

public class SpawnCircle : MonoBehaviour
{
    // Update is called once per frame
    public GameObject ball;
    public new Camera camera;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;
            //Makes the mouse pos relative to the world pos
            Vector3 newObjectPos = camera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 1));
            Instantiate(ball, newObjectPos, Quaternion.identity);
        }
    }
}
