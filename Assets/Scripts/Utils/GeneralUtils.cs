using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUtils
{
    public static void DestroyAll(List<GameObject> gameObjectList)
    {
        if (gameObjectList != null)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                Object.Destroy(gameObject);
            }
        }
    }

    public static void DestroyAllChildren(Transform transform)
    {
        if (transform != null)
        {
            foreach (Transform child in transform)
            {
                if (child != null)
                    Object.Destroy(child.gameObject);
            }
        }
    }


    public static void SetAllActive(List<GameObject> gameobjectList, bool active)
    {
        foreach (GameObject gameObject in gameobjectList)
        {
            gameObject.SetActive(active);
        }
    }
}