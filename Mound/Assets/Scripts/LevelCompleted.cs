using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleted : MonoBehaviour
{
    public GameObject LevelCompleteUI;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Level Complete");
        LevelCompleteUI.SetActive(true);
    }
}
