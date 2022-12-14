using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("rock"))
        {
            // destroy scissors and replace with rock
        }
    }
}
