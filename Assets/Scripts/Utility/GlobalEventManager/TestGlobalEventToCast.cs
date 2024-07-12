using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGlobalEventToCast : MonoBehaviour
{

    [SerializeField]
    private GlobalEventToCast eventToCast;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GlobalEventManager.CastEvent(eventToCast.EventToCast, eventToCast.Message);
        }
    }

}
