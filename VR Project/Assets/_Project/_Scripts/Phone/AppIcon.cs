using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppIcon : Selectable
{
    [Space]
    public GameObject appToCall;
    
    public override void Select()
    {
        targetGraphic.color = colors.highlightedColor;
    }

    public void Deselect()
    {
        targetGraphic.color = colors.normalColor;
    }

    public virtual void Confirm(Phone phone)
    {
        if (!appToCall)
        {
            Debug.Log("No App Found");
            return;
        }
        
        appToCall.gameObject.SetActive(true);
        IPhoneApp newNav = appToCall.GetComponent<IPhoneApp>();

        if (newNav == null)
            return;
        
        phone.SetNav(newNav);
        newNav.StartApp();
    }
}
