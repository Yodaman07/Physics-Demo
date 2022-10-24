using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenRopeAngle : MonoBehaviour
{
    private GameObject _rope;
    public Sprite ropeSprite;
    public Sprite circle;
    void Start()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        Vector3 swingPos = GameObject.Find("Swing").transform.position - new Vector3(0,2,0);
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        Vector3 playerPos = transform.position;
        
        Vector3 diff = swingPos - playerPos;
        

        GameObject a = new GameObject();
        a.AddComponent<SpriteRenderer>().sprite = circle;
        a.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        a.transform.position = swingPos;

        GameObject b = new GameObject();
        b.AddComponent<SpriteRenderer>().sprite = circle;
        b.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        b.transform.position = playerPos;
        
        
        
        
        _rope = new GameObject();
        _rope.AddComponent<SpriteRenderer>().sprite = ropeSprite;
        _rope.transform.localScale = new Vector3(0.05f, 4, 1);
        
        _rope.transform.position = (playerPos + swingPos)/2;
        float angle = Mathf.Atan(diff.y / diff.x);
        float degrees = angle * Mathf.Rad2Deg;
        
        _rope.transform.eulerAngles = new Vector3(0, 0, degrees+90);
        
        _rope.transform.localScale = new Vector3(0.05f, Vector3.Distance(swingPos,playerPos), 1);
    }
    
}
