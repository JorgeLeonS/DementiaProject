using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to visually display Gizmos on where the characters need to move.
/// This was originally planned because of the initial thought of having multiple NPC characters having dialogue and actions.
/// These should be automatically found on the <see cref="CharacterController.CharacterWaypoints"/> variable.
/// </summary>
public class Waypoints : MonoBehaviour
{
    // Gizmos Color variables
    Color JamesSphereColor = Color.green;
    Color JamesLineColor = Color.white;

    Color RachelSphereColor = Color.blue;
    Color RachelLineColor = Color.magenta;
    private void OnDrawGizmos()
    {
        // Characters
        // Change the color for every player
        switch (transform.name.ToLower())
        {
            case "james":
                DrawPlayersWaypoints(JamesSphereColor, JamesLineColor);
                return;
            case "rachel":
                DrawPlayersWaypoints(RachelSphereColor, RachelLineColor);
                return;
            default:
                break;
        }


        void DrawPlayersWaypoints(Color sphereColor, Color lineColor)
        {
            // Each character Waypoints
            foreach (Transform t in transform)
            {
                // Draw spheres for each created child
                Gizmos.color = sphereColor;
                Gizmos.DrawWireSphere(t.position, 0.1f);

                // Draw lines between all waypoints
                Gizmos.color = lineColor;
                for (int i = 0; i < transform.childCount - 1; i++)
                {
                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
                }

                // Draw a line to connect to the first one
                //Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);

            }
        }
    }


}
