using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anima : MonoBehaviour
{
 // TOOL ANIMATION TEST SCRIPT ---
    public Animation swing;
 

    void Update()
    {
       if( Input.GetKey(KeyCode.Mouse0))
        {
            swing.Play();
        }
    }
}
