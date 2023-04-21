using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone_ImageAlbum : Phone_MultiPage
{
    
    
    
    private bool _isConfirmingDelete = false;
    
    public override void OnJoystickMove(float x, float y)
    {
        if (_isConfirmingDelete) return;
        
        base.OnJoystickMove(x, y);
    }
    
    public override void OnThumbStickDown()
    {
        _isConfirmingDelete = true;
    }
}
