using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objects;

    public void SetAllObjectActive(bool isActive)
    {
        foreach (var obj in _objects)
        {
            obj.SetActive(false);
        }
    }
}
