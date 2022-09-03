using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour, ILinkable
{

    private Vector3 targetchords;

    //   | CAMERA WILL MATCH ROTATION OF ANY OBJECT YOUR STICK IN HERE|
    //   v                                                            v
    public Transform player;
    //   ^                                                            ^
    //   |                                                            |

    public float Turnspeed = 2.0f;
    public Quaternion Turnto;

    [Header(header: "Offset")]
    public Vector3 offset;
    private bool readyTOGO;

    private void Start()
    {
        //targetchords = player.transform.position;
    }

    void FixedUpdate()
    {
        if (!readyTOGO) return;

        if (player != null)
        {
            Turnto = player.transform.rotation;
            transform.position = player.transform.position + offset;
            transform.rotation = Quaternion.Slerp(transform.rotation, Turnto, Time.fixedDeltaTime * Turnspeed); 
        }

    }

    public void SetMyLink(Transform link)
    {
        readyTOGO = true;
        player = link;
    }

    public Transform GetMyTransform()
    {
        return transform;
    }

    public Transform GetMyLink()
    {
        return player;
    }
}
