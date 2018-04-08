using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("Mouse Raycast Settings")]
    public LayerMask mouseHitMask; // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    public float mouseRayLength = 100f; // The length of the ray from the camera into the scene.

    void Update()
    {
        TurnPosition();
    }

    void TurnPosition()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, mouseRayLength, mouseHitMask) && Time.timeScale > 0f)
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            // Ensure the vector is entirely along the floor plane.
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            // Set the player's rotation to this new rotation.
            transform.LookAt(playerToMouse);
        }
    }
}