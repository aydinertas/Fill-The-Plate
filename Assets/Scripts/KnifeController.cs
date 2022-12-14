using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnifeController : MonoBehaviour
{
    public Transform startPos, lastPos;

    float touchLastPos, screenWidth;

    public bool knifeObject=false;
    void Start()
    {
        screenWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount>0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                touchLastPos = Input.GetTouch(0).deltaPosition.x;
            }

            if (Input.GetTouch(0).phase==TouchPhase.Moved && !knifeObject)
            {


                float distancePos = Mathf.Abs(Input.GetTouch(0).deltaPosition.x - touchLastPos);
                //Debug.Log(distancePos);

                if (transform.localPosition.x >= -1.1f && transform.localPosition.x <= -0.06f)
                {
                    Debug.Log(distancePos);
                    if (Input.GetTouch(0).deltaPosition.x > touchLastPos)
                    {
                        //foodMenu.transform.position += new Vector3(distancePos / 100, 0, 0);
                       transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.1f, transform.localPosition.y, transform.localPosition.z), distancePos / (screenWidth / 2));
                        //Vector3.MoveTowards(foodMenu.transform.localPosition, new Vector3(0.1f, 0, 0), distancePos/100);
                    }
                    else if (Input.GetTouch(0).deltaPosition.x < touchLastPos)
                    {
                        //foodMenu.transform.position -= new Vector3(distancePos / 100, 0, 0);
                        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.1f, transform.localPosition.y, transform.localPosition.z), distancePos / (screenWidth/2));
                        //Vector3.MoveTowards(foodMenu.transform.localPosition, new Vector3(-0.53f, 0, 0), distancePos/100);
                    }


                }

            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && !knifeObject)
            {
                StartCoroutine(StartSlice());
            }
        }
    }



    IEnumerator StartSlice()
    {
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<SliceObject>().isSliced = true;
        knifeObject = true;
        //GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(0.1f);

        transform.DOLocalMoveY(lastPos.localPosition.y, 0.5f);
        //yield return new WaitForSeconds(1f);
        //GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(0.60f);
        //GetComponent<BoxCollider>().enabled = false;
        //GetComponent<MeshCollider>().enabled = false;
        yield return new WaitForSeconds(0.05f);
        transform.DOLocalMoveY(startPos.localPosition.y, 0.5f);
        yield return new WaitForSeconds(0.5f);
       


        knifeObject = false;
    }

    IEnumerator SetActiveScript(int num)
    {
        yield return new WaitForSeconds(1f);
        if (num==0)
        {
            GetComponent<MeshCollider>().enabled = false;
            //GetComponent<BoxCollider>().enabled = false;
            GetComponent<KnifeController>().enabled = false;
            GetComponent<SliceObject>().enabled = false;
        }
        else
        {
            GetComponent<MeshCollider>().enabled = true;
            GetComponent<KnifeController>().enabled = true;
            GetComponent<SliceObject>().enabled = true;
        }
        
        
    }

    public void SetActive(int num)
    {
        StartCoroutine(SetActiveScript(num));
    }


}
