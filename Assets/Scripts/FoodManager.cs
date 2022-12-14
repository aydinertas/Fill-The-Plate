using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodManager : MonoBehaviour
{
    public Vector3 normalPos, activePos, platterObjectStartPos;
    //public Transform activePos;
    public Transform foodPos, foodNormalPos;
    public GameObject foodGlass, foodObject, foodMenu, platterObject, knifeObject;
    public bool isActive, isNotSelected;

    public GameObject instantiateObject;

    public Material sliceMat;

    public Transform lastFoodPlatePos, platterObjectLastPos;

    PlayerController playerController;
    KnifeController knifeController;

    public GameObject foodPlatterCanvas;

    void Awake()
    {
        platterObjectStartPos = new Vector3(platterObject.transform.localPosition.x, platterObject.transform.localPosition.y, platterObject.transform.localPosition.z);

        normalPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        activePos = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.11f, transform.localPosition.z + 0.07f);
        foodNormalPos = foodObject.transform;
        playerController = FindObjectOfType<PlayerController>();
        knifeController = FindObjectOfType<KnifeController>();


    }

    public void SetPosition()
    {
        if (!playerController.isPlatterActive)
        {
            if (!isActive)
            {                
                foodGlass.SetActive(false);
                playerController.lastFoodGlass = foodGlass;                
                transform.DOLocalMove(activePos, 0.5f);
               
                isActive = true;
                playerController.foodActive = true;
                
            }
            else
            {                
                foodGlass.SetActive(true);                
                transform.DOLocalMove(normalPos, 0.5f);
                isActive = false;
                playerController.foodActive = false;
               
            }
        }


    }

    public void SelectFood()
    {
        if (!isNotSelected && !playerController.isPlatterActive)
        {
            knifeObject.SetActive(true);
            isActive = false;
            playerController.lastFoodPlatePos = lastFoodPlatePos;
            playerController.lastFoodGlass = foodGlass;
            playerController.enabled = false;
            
            transform.DOLocalMove(normalPos, 0.5f);
            foodMenu.SetActive(false);
            foodObject.transform.DOMove(foodPos.position, 1f);
            foodObject.transform.tag = "Slice";
            foodObject.transform.parent = null;
            foodObject.transform.DOScale(foodObject.transform.localScale *= 3, 0.5f);
            isNotSelected = true;
            FindObjectOfType<SliceObject>().finishedSlice = false;
            knifeController.SetActive(1);

            playerController.foodSelectCanvas.SetActive(true);
        }
        else if (isNotSelected && !playerController.isPlatterActive)
        {
            foodPlatterCanvas.SetActive(true);
            playerController.isPlatterActive = true;
            knifeObject.SetActive(false);
            platterObject.transform.DOLocalMove(platterObjectLastPos.localPosition, 1f);
        }


    }

    public void RestartFood()
    {

        GameObject[] destroyedObject = GameObject.FindGameObjectsWithTag("Slice");
        foreach (var item in destroyedObject)
        {
            Destroy(item);
        }

        Destroy(this.foodObject);

        GameObject newFoodObject = Instantiate(instantiateObject, normalPos, Quaternion.identity);
       
        foodObject = newFoodObject;
        foodObject.GetComponent<MeshCollider>().enabled = false;
        isNotSelected = false;
        SelectFood();
        foodObject.GetComponent<MeshCollider>().enabled = true;
        
    }


    public void TakeInBackFood()
    {
        SetPosition();
        playerController.enabled = true;

        foodMenu.SetActive(true);
       
        foodObject.transform.DOMove(foodNormalPos.position, 1f);
        foodObject.transform.tag = "Food";
        foodObject.transform.parent = this.gameObject.transform;
        foodObject.transform.DOScale(foodObject.transform.localScale /= 3, 0.5f);
        isNotSelected = false;
        knifeController.SetActive(0);
        FindObjectOfType<SliceObject>().finishedSlice = true;
        playerController.foodSelectCanvas.SetActive(false);
       
    }


    public void SetFoodPlatter()
    {
        platterObject.transform.DOLocalMove(platterObjectStartPos, 1f);
        playerController.isPlatterActive = false;
        foodPlatterCanvas.SetActive(false);
    }


}
