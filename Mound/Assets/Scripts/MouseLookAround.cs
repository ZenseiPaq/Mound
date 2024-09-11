using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAround : MonoBehaviour
{
    float mouseX = 0f;
    float mousenY = 0f;
    public float mousesensitive;

   [SerializeField] Transform player;
   // [SerializeField] Transform tool;

    private void Update()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mousesensitive;
        float rotAmountY = mouseY * mousesensitive;

        //Vector3 rotationPlayerTool = tool.transform.rotation.eulerAngles;
        Vector3 rotationPlayer = player.transform.rotation.eulerAngles;

       // rotationPlayerTool.x -= rotAmountY;
        //rotationPlayerTool.z = 0;
        //rotationPlayerTool.y += rotAmountX;
        

        rotationPlayer.y += rotAmountX;
        rotationPlayer.x -= rotAmountY;

       // tool.rotation = Quaternion.Euler(rotationPlayerTool);
        player.rotation = Quaternion.Euler(rotationPlayer);
    }
}
