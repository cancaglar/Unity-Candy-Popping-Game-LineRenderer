using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] private GameObject line;
    private GameObject lineClone = null;
    private GameObject go2;

    RaycastHit2D hit;

    private string selectedObjectTag = null;

    private List<GameObject> objects = new List<GameObject>();
    private List<GameObject> closestobjs = new List<GameObject>();
    
    public AudioManager audioManager;

    void Update()
    {
        if (objects.Count > 0) // if objects(current selected objects) count bigger than 0 
        {
            selectedObjectTag = objects[0].tag; // get first selected objects tag (because we just want to select same type candies)
        }


        if (Input.GetMouseButton(0))
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject go = hit.transform.gameObject;

                if (!objects.Contains(go)) // if selected objects list not contain this object - dont select the object that already selected
                {
                    if (objects.Count == 0) //  if selected there is no have any selected objects
                    {
                        objects.Add(go); // add selected object to the selected objects list


                        lineClone = Instantiate(line); // Instantiate the line
                        // set objects count to the lines positionCount for line vertices
                        lineClone.GetComponent<LineRenderer>().positionCount = objects.Count;
                        // set lines 0 index position to selected object position
                        lineClone.gameObject.GetComponent<LineRenderer>().SetPosition(0, go.transform.position);
                        // play line sound
                        audioManager.PlaySound("LineSound");
                        // set selected object to the go2 object for checking the next selected object
                        go2 = go;
                    }// if there is already have selected objects and the first selected objects tag is equal to the current selected objects tag
                     // and line is not null and current selected object(go) not equals to the previous selected object
                    else if (go.tag == selectedObjectTag && lineClone != null && go != go2)  
                    {
                        // get closest objects
                        closestobjs = GetColliders(objects[objects.Count - 1].transform.position);
                        // if the selected objects tag is equal to the first selected objects tag and if closestobjs list contains selected object 
                        if (go.tag == selectedObjectTag && closestobjs.Contains(go))
                        {
                            // add selected object to the selected objects list 
                            objects.Add(go);
                            // set the objects count to the lines positionCount for line vertices AGAIN
                            lineClone.GetComponent<LineRenderer>().positionCount = objects.Count;
                            // Set the position of the line to the position of the current selected object according to the index of the previous selected object
                            lineClone.gameObject.GetComponent<LineRenderer>()
                                .SetPosition(objects.Count - 1, go.transform.position);
                            // play line sound
                            audioManager.PlaySound("LineSound");
                        }
                    }
                }
                // if selected objects list contains the previous object and selected object count is bigger than 2 because we can only move line to the back
                // when we have more than 2 selected ojects
                else if (objects.Contains(go2) && objects.Count >= 2)
                {
                    // if rays hit object is equal to the previous SELECTED object
                    if (hit.transform.gameObject == objects[objects.Count - 2])
                    {
                        // set the objects count to the lines positionCount for line vertices AGAIN
                        lineClone.GetComponent<LineRenderer>().positionCount = objects.Count;
                        // Set the position of the line to the position of the current rays hit object according to the index of the previous selected object
                        lineClone.gameObject.GetComponent<LineRenderer>().SetPosition(objects.Count - 1,
                            hit.transform.gameObject.transform.position);
                        //  remove last selected object from list because line move to back and object is not selected anymore
                        objects.RemoveAt(objects.Count - 1);
                    }
                }
            }
        }
        // destroy selected candies
        else if (Input.GetMouseButtonUp(0) || hit.collider == null)
        {
            if (objects.Count > 0) // if there is have some selected objects
            {
                if (objects.Count > 2) // and if they are more than 2 because we can destroy at least 3 objects
                {
                    // loop through selected objects
                    foreach (GameObject go in objects)
                    {
                        GameObject.Destroy(go); // destroy the object
                        audioManager.PlaySound("CandySound"); // play candy popping sound
                    }

                    GameObject.Destroy(lineClone); // destroy the line
                }
                else
                {
                    GameObject.Destroy(lineClone);// destroy the line
                }

                // clear some stuff
                objects.Clear();
                closestobjs.Clear();
                selectedObjectTag = "";
                lineClone = null;
            }
        }
        // return the colliders that are in 1.2f radius and colliding in given position
        List<GameObject> GetColliders(Vector3 center)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, 1.2f);
            List<GameObject> goList = new List<GameObject>();
            foreach (var item in hitColliders)
            {
                goList.Add(item.gameObject);
            }

            return goList;
        }
    }
}