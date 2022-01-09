using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
* Project: GPR5100 Networkgame
* File   : Controls.cs
* Date   : 30.12.2021
* Author : René Kraus (RK)
* Version: 1.0
*
* These coded instructions, statements, and computer programs contain
* proprietary information of the author and are protected by Federal
* copyright law. They may not be disclosed to third parties or copied
* or duplicated in any form, in whole or in part, without the prior
* written consent of the author.
*
* History:
*	30.12.21	RK	Created
******************************************************************************/
public static class Controls 
{
    public static float Horizontal(bool _useAxisRaw = false)
    {
        return _useAxisRaw ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
    }

    public static float Vertical(bool _useAxisRaw = false)
    {
        return _useAxisRaw ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");
    }

    public static float MouseX(bool _useAxisRaw = false)
    {
        return _useAxisRaw ? Input.GetAxisRaw("Mouse X") : Input.GetAxis("Mouse X");
    }

    public static float MouseY(bool _useAxisRaw = false)
    {
        return _useAxisRaw ? Input.GetAxisRaw("Mouse Y") : Input.GetAxis("Mouse Y");
    }

    public static bool MouseButtonLeft()
    {
        return Input.GetMouseButton(0);
    }

    public static bool MouseButtonRight()
    {
        return Input.GetMouseButton(1);
    }

    public static bool Jump()
    {
        return Input.GetButtonDown("Jump");
    }

    public static bool Sprint()
    {
        return Input.GetButton("Sprint");
    }

    public static bool Escape()
    {
        return Input.GetButtonDown("Cancel");
    }
}
