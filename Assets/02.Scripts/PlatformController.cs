using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace controller
{
    public class PlatformController : MonoBehaviour
    {
        public bool isPlatform;
        GameObject enterObj;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.S) && isPlatform)
            {
                enterObj.GetComponent<Collider2D>().isTrigger = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                enterObj = collision.gameObject;
                isPlatform = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerController.Instance.GetComponent<Collider2D>().isTrigger = false;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                enterObj = null;
                isPlatform = false;
            }
        }
    }
}
