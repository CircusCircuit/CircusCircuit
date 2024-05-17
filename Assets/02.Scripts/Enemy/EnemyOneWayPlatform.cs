using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    [SerializeField] private Collider2D enemyCollider;
    // Update is called once per frame
    public void DownJump()
    {
        if(currentOneWayPlatform != null){
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Wall")){
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Wall")){
            currentOneWayPlatform = null;
        }        
    }


    private IEnumerator DisableCollision(){
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.25f);
        // Physics2D.IgnoreCollision(enemyCollider, platformCollider, false);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;


    }
}


