using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //private Vector3 offset = new Vector3(0, 0, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    public GameObject Player;

    [SerializeField] private Transform target;
    [SerializeField] private float camup, camdown;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        


        if (Player.gameObject.GetComponent<Move>().hit.collider != null)
        {
            if (Player.GetComponent<Move>().right)
            {
                Vector3 offset = new Vector3(4, camup, -10);
                Vector3 targetPos = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            }
            if (Player.GetComponent<Move>().left)
            {
                Vector3 offset = new Vector3(-4, camup, -10);
                Vector3 targetPos = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            }
            if (Player.GetComponent<Move>().right && Player.GetComponent<Move>().left)
            {
                Vector3 offset = new Vector3(0, camup, -10);
                Vector3 targetPos = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            }
            if (!Player.GetComponent<Move>().right && !Player.GetComponent<Move>().left)
            {
                Vector3 offset = new Vector3(0, camup, -10);
                Vector3 targetPos = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
            }
            
        }
        else
        {
            Vector3 offset = new Vector3(0, camdown, -10);
            Vector3 targetPos = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}
