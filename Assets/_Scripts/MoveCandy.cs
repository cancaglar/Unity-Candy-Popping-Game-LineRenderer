using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCandy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    void Update()
    {
        if (transform.parent != null) // candy's has a parent
        {
            if (transform.localPosition == Vector3.zero) // if localPosition is zero just return 
            {
                return;
            }
            else 
            {
                // move to parent object(grid)
                float x = transform.position.x - transform.parent.transform.position.x;
                float y = transform.position.y - transform.parent.transform.position.y;

                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x - x, transform.position.y - y, 0), speed * Time.deltaTime);
            }
        }
    }
}