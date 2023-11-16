using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public float aimingSpeed = 5f; // Adjust the speed of aiming as needed.

    // Update is called once per frame
    void Update()
    {
        if (GetMouseAiming())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
        else if (Input.GetButton("AimSw"))
        {
            //Fixing auto input
            //https://www.youtube.com/watch?v=tRsJchGPrW8&ab_channel=CursedCode
            float rightStickX = Input.GetAxisRaw("RightStickHorizontal");
            float rightStickY = Input.GetAxisRaw("RightStickVertical");

            if (rightStickY != 0)
            {
                Debug.Log(rightStickY);
            }

            Vector2 aimDirection = new Vector2(rightStickX, rightStickY).normalized;
            Vector3 newPosition = transform.position; // Convert to Vector3
            newPosition += (Vector3)aimDirection * aimingSpeed * Time.deltaTime; // Convert back to Vector2
            transform.position = newPosition;
        }
    }

    private bool GetMouseAiming()
    {
        return Input.GetKey(KeyCode.E) || Input.GetMouseButton(2);
    }

    public bool IsAiming() => GetMouseAiming() || Input.GetButton("AimSw");
}