using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;
using UnityEngine.EventSystems;

public class touch : MonoBehaviour
{
    [SerializeField]
    private GameObject[] listobject;
    public bool touched;
    public GameObject pathfinder;
    public static GameObject obj;
    LayerMask uimask;
    // Start is called before the first frame update
    void Start()
    {
        touched=false;
    }

    // Update is called once per frame
    void Update()
    {
        //find selectable objects
        listobject = GameObject.FindGameObjectsWithTag("obj");

        //touch screen
        if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {


                if (Physics.Raycast(ray, out hit))
                {
                    // touch if its not UI
                    if (hit.collider.tag == "obj"&& !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        Debug.Log("obj");
                        touched = true;
                        //ground detection checking
                        if (pathfinder != null)
                        {
                            pathfinder.SetActive(false);
                        }
                        obj = hit.transform.gameObject;
                        
                        //check selected last object enable  rotation
                        //if you have leantouch you can use this code for rotate object
                        foreach(var allobjs in listobject )
                        {
                            allobjs.GetComponent<Lean.Touch.LeanTwistRotateAxis>().enabled = false;
                        }
                        obj.GetComponent<Lean.Touch.LeanTwistRotateAxis>().enabled = true;


                    }
                    else
                    {
                        Debug.Log("its ui");
                    }
                }

            }
            if (obj!=null&&Input.touchCount==1 &&touched && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Ray mray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                Plane plane = new Plane(Vector3.up, obj.transform.position);
                float distance = 0; // this will return the distance from the camera
                if (plane.Raycast(ray, out distance))
                { // if plane hit...
                    Vector3 pos = ray.GetPoint(distance); // get the point
                    //transform with vector lerp 
                    obj.transform.position = Vector3.Lerp(obj.transform.position,pos,Time.deltaTime*2f);                                      // pos has the position in the plane you've touched
                }
            }
        }
    }
 
}
