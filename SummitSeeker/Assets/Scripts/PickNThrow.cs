using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickNThrow : MonoBehaviour
{
    public Transform playerCamera; // Kamera des Spielers
    public float pickUpRange = 3f; // Reichweite zum Aufheben
    public float throwForce = 10f; // Kraft beim Werfen
    public Transform holdPoint; // Position, an der das Objekt gehalten wird

    private Rigidbody heldObjectRb; // Rigidbody des Objekts, das gehalten wird
    private GameObject heldObject; // Referenz auf das gehaltene Objekt

    void Update()
    {
        // Überprüfen, ob die Taste zum Aufheben/Werfen gedrückt wurde
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                TryPickUp();
            }
            else
            {
                ThrowObject();
            }
        }

        if (heldObject != null)
        {
            heldObject.transform.rotation = holdPoint.rotation;
        }
    }

    void TryPickUp()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Prüfen, ob ein Objekt in Reichweite ist
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("PickUp")) // Tag für aufhebbare Objekte
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {
                    // collider deaktivieren
                    Collider heldCollider = heldObject.GetComponent<Collider>();
                    if (heldCollider != null)
                        heldCollider.enabled = false;
                    // Objekt vorbereiten
                    heldObjectRb.isKinematic = true; // Physik deaktivieren, solange es gehalten wird
                    heldObject.transform.position = holdPoint.position; // Position setzen
                    heldObject.transform.parent = holdPoint; // Objekt an den Haltepunkt heften
                }
            }
        }
    }

    void ThrowObject()
    {
        if (heldObject != null)
        {
            // collider deaktivieren
            Collider heldCollider = heldObject.GetComponent<Collider>();
            if (heldCollider != null)
                heldCollider.enabled = true;

            // Objekt loslassen und werfen
            heldObjectRb.isKinematic = false; // Physik wieder aktivieren
            heldObject.transform.parent = null; // Vom Haltepunkt lösen
            heldObjectRb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse); // Kraft hinzufügen

            // Variablen zurücksetzen
            heldObject = null;
            heldObjectRb = null;
        }
    }
}
