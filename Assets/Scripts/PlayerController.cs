using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Camera _camera;
    Vector3 touchPosWorld;
    public GameObject foodMenu;
    float touchLastPos = 0f, currentPosition, screenWidth;

    public bool foodActive;

    public Transform lastFoodPlatePos;
    public GameObject lastFoodGlass;

    public GameObject foodSelectCanvas;


    public bool isPlatterActive;

    public GameObject activeGameObject;
    public Vector3 activeObjectStartPos, activeGameObjectLastPos;

    public GameObject[] foodObjects;
    public GameObject winCanvas;
    public GameObject finger;
    public AnimationClip _left_rightAnim, up_downAnim;
    bool activeClip;

    int foodNum = 0;
    private void Start()
    {
        screenWidth = Screen.width;
        finger.gameObject.SetActive(true);
        finger.GetComponent<Animation>().clip = _left_rightAnim;
        finger.GetComponent<Animation>().Play();

    }


    private void FixedUpdate()
    {
        int num = 0;
        foreach (var item in foodObjects)
        {
            
            for (int i = 0; i < item.transform.childCount; i++)
            {
                if (item.transform.GetChild(i).gameObject.tag == "Food")
                {
                    num++;
                    
                }
            }

           
        }

        if (num > 0)
        {
            return;
        }

        winCanvas.SetActive(true);


    }
    // Update is called once per frame
    void Update()
    {







        if (Input.touchCount > 0 /*&& Input.GetTouch(0).phase == TouchPhase.Ended*/)
        {
            finger.gameObject.SetActive(false);
            finger.GetComponent<Animation>().clip = null;
            //touchLastPos = Input.GetTouch(0).position.x;
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                touchLastPos = Input.GetTouch(0).deltaPosition.x;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.name);
                if (hit.collider != null)
                {

                    GameObject touchedObject = hit.transform.gameObject;

                    
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved && !isPlatterActive)
                {
                    if (hit.transform.tag == "FoodPlate")
                    {
                        Debug.Log("moved");

                       

                        float distancePos = Mathf.Abs(Input.GetTouch(0).deltaPosition.x - touchLastPos);
                        //Debug.Log(distancePos);

                        if (foodMenu.transform.localPosition.x >= -0.53f && foodMenu.transform.localPosition.x <= 0.1f)
                        {
                            Debug.Log(distancePos);
                            if (Input.GetTouch(0).deltaPosition.x > touchLastPos)
                            {
                                
                                foodMenu.transform.localPosition = Vector3.Lerp(foodMenu.transform.localPosition, new Vector3(0.1f, foodMenu.transform.localPosition.y, foodMenu.transform.localPosition.z), distancePos / (screenWidth / 3));
                                
                            }
                            else if (Input.GetTouch(0).deltaPosition.x < touchLastPos)
                            {
                               
                                foodMenu.transform.localPosition = Vector3.Lerp(foodMenu.transform.localPosition, new Vector3(-0.53f, foodMenu.transform.localPosition.y, foodMenu.transform.localPosition.z), distancePos / (screenWidth/3));
                               
                            }


                        }
                    }
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (hit.transform.tag == "FoodPlate" /*&& hit.transform.GetComponent<FoodManager>().isNotSelected==false*/)
                    {
                        if (!foodActive)
                        {
                           
                           
                            hit.transform.GetComponent<FoodManager>().SetPosition();
                        }
                        else
                        {
                           

                            foreach (var item in FindObjectsOfType<FoodManager>())
                            {
                                if (item.GetComponent<FoodManager>().isActive)
                                {
                                    item.GetComponent<FoodManager>().SetPosition();
                                }

                            }

                            hit.transform.GetComponent<FoodManager>().SetPosition();
                        }

                        

                    }
                    else if (hit.transform.tag == "Food" /*&& hit.transform.parent.GetComponent<FoodManager>().isNotSelected == false*/)
                    {
                        Debug.Log("Food");

                        if (!isPlatterActive)
                        {
                            hit.transform.parent.GetComponent<FoodManager>().SelectFood();
                        }
                        else
                        {
                            if (!activeClip)
                            {
                                finger.gameObject.SetActive(true);
                                finger.GetComponent<Animation>().clip = up_downAnim;
                                finger.GetComponent<Animation>().Play();
                                activeClip = true;

                            }
                            else
                            {
                                activeClip = true;

                            }

                            activeObjectStartPos = new Vector3(hit.transform.localPosition.x, hit.transform.localPosition.y, hit.transform.localPosition.z);
                            activeGameObject = hit.transform.gameObject;
                        }

                    }
                    else if (hit.transform.tag == "ActiveFoodObject" && isPlatterActive)
                    {
                        if (activeGameObject.transform.parent.tag == "FoodPlate")
                        {

                           
                            activeGameObject.transform.parent = hit.transform.GetComponent<PlateFoodPosManager>().foodPlatePos[foodNum].transform;
                            activeGameObject.transform.DOLocalMove(new Vector3(0, 0, 0), 0.1f);
                            activeGameObject.transform.DOScale(activeGameObject.transform.localScale*=1.5f, 0.5f);
                            
                            foodNum++;
                        }
                       
                    }

                   
                }
                
            }



        }
    }

    public void RestarGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
