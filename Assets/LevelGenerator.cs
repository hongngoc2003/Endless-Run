using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Transform respawnPosition;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            Transform part = levelPart[Random.Range(0, levelPart.Length)];

            Transform newPart = Instantiate(part, respawnPosition.position,transform.rotation, transform);
        }
    }
}
