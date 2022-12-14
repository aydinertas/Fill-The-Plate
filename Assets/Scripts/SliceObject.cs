using UnityEngine;
using EzySlice;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class SliceObject : MonoBehaviour
{
    public Material materialSliceSide;

    public GameObject[] slicedObjects;
    int num = 0;
    PlayerController _playerController;

    public bool finishedSlice,restartObject;

    public bool isSliced;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        restartObject = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slice") && isSliced)
        {
            isSliced = false;
            materialSliceSide=other.GetComponent<FoodMaterial>()._Foodmaterial;

            SlicedHull sliceObj = Slice(other.gameObject, materialSliceSide);

            if (sliceObj != null)
            {
                GameObject sliceObjTop = sliceObj.CreateUpperHull(other.gameObject, materialSliceSide);
                GameObject sliceObjDown = sliceObj.CreateLowerHull(other.gameObject, materialSliceSide);
                slicedObjects[num] = sliceObjTop;
                slicedObjects[num + 1] = sliceObjDown;

                
               
                AddComponents(sliceObjDown);
                AddComponents(sliceObjTop);

                
                Destroy(other.gameObject);
                num += 2;
            }
            else
            {
                return;
            }



        }
    }



    private SlicedHull Slice(GameObject obj, Material mat)
    {
        return obj.Slice(transform.position, direction: transform.up, mat);
    }


    void AddComponents(GameObject obj)
    {
        obj.tag = "Slice";
        obj.AddComponent<MeshCollider>();
        obj.AddComponent<MeshCollider>().convex = true ;
        obj.AddComponent<FoodMaterial>();
        obj.GetComponent<FoodMaterial>()._Foodmaterial= materialSliceSide;
      

        obj.GetComponent<MeshCollider>().convex = true;
        var rigidbody = obj.AddComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.AddForce(Vector3.right * 25f);
    }



    public void FinishedSlice()
    {

        if (!finishedSlice && !SliceObjectsNull())
        {
            _playerController.foodMenu.SetActive(true);
            Debug.Log("finish");
            foreach (var item in slicedObjects)
            {
                if (item != null && item.tag == "Slice")
                {
                    
                    item.transform.parent = _playerController.lastFoodPlatePos.parent;
                    item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    item.GetComponent<Rigidbody>().useGravity = true;                 


                    item.transform.DOLocalMove(_playerController.lastFoodPlatePos.localPosition, 0.5f);
                    
                    item.transform.localScale /= 3;                   

                    item.tag = "Food";
                   

                }
            }
            
            
            _playerController.enabled = true;

           
            GetComponent<KnifeController>().enabled = false;
            _playerController.foodSelectCanvas.SetActive(false);

            num = 0;
            finishedSlice = true;
            Invoke("ActiveGlass", 1f);
        }


    }

    public bool SliceObjectsNull()
    {
        foreach (var item in slicedObjects)
        {
            if (item != null && item.tag=="Slice")
            {
                return false;
            }
        }



        return true;
    }

    public void RestartSlice()
    {
       
            GetComponent<KnifeController>().enabled = false;

            foreach (var item in slicedObjects)
            {
                Destroy(item);
            }

            _playerController.lastFoodGlass.transform.parent.GetComponent<FoodManager>().RestartFood();
            GetComponent<KnifeController>().enabled = true;
            
        
    }

    void ActiveGlass()
    {
        _playerController.lastFoodGlass.SetActive(true);
        _playerController.lastFoodGlass.GetComponent<MeshCollider>().enabled = true;
        this.GetComponent<SliceObject>().enabled = false;
       
        GetComponent<MeshCollider>().enabled = false;
    }
   

}
