using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneAppHelper : MonoBehaviour
{
    [SerializeField] private Phone phone;
    [SerializeField] private App appToGo;

    private void Awake()
    {
        if (!phone)
            phone = GetComponentInParent<Phone>();
    }

    public void StartApp()
    {
        phone.EnterApp(appToGo);
    }
}
