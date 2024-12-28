using System.Collections.Generic;
using UnityEngine;

public class MagnetizedObj : MonoBehaviour
{
    [SerializeField] int polarity = 0;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float strength = 1.0f;
    [SerializeField] float fieldRadius = 1.0f;
    [SerializeField] bool isStatic = false;

    [SerializeField] LayerMask magnetizedLayer;
    [SerializeField] private List<MagnetizedObj> neighborMagnets = new();
    [SerializeField] private Collider2D magnetCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (!isStatic)
        {

            rb = GetComponent<Rigidbody2D>();
        }

        magnetCollider.isTrigger = true;
        magnetCollider.includeLayers = magnetizedLayer;
        magnetCollider.excludeLayers = ~magnetizedLayer;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        MagnetizedObj tempObj = collision.GetComponent<MagnetizedObj>();
        if(collision is not null && tempObj is not null && tempObj != this && !neighborMagnets.Contains(tempObj))
        {
            neighborMagnets.Add(tempObj);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MagnetizedObj tempObj = collision.GetComponent<MagnetizedObj>();
        if (tempObj is not null)
        {
            //If object is not in list, it returns false and doesn't care
            neighborMagnets.Remove(tempObj);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //CheckRadius();
        ApplyForceToNeighbors();
    }

    /*
    public void CheckRadius()
    {
        //Find all the magnetized objects within the radius

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldRadius, magnetizedLayer);

        foreach (Collider2D obj in objects) {
            MagnetizedObj tempObj = obj.GetComponent<MagnetizedObj>();
            if (tempObj != null && tempObj != this)
            {
                ApplyForce(tempObj);
            }
        }
    }
    */

    private void ApplyForceToNeighbors()
    {
        foreach(MagnetizedObj magnet in neighborMagnets)
        {
            ApplyForce(magnet);
        }
    }

    public void ApplyForce(MagnetizedObj otherObj)
    {
        Vector2 DirectionToObj = DetermineDirection(otherObj);

        float dist = DirectionToObj.magnitude;

        if (!otherObj.isStatic)
        {
            //Squared falloff
            //otherObj.rb.AddForce(-DetermineSign(otherObj.GetPolarity()) * strength * (DirectionToObj.normalized / DirectionToObj.sqrMagnitude), ForceMode2D.Force);

            //Linear falloff
            otherObj.rb.AddForce(-DetermineSign(otherObj.GetPolarity()) * strength * (DirectionToObj.normalized / DirectionToObj.magnitude), ForceMode2D.Force);
        }
    }

    public Vector2 DetermineDirection(MagnetizedObj otherObj)
    {
        return transform.position - otherObj.transform.position;
    }

    private int DetermineSign(int otherPolarity)
    {
        int result = polarity * otherPolarity;
        return result;
    }

    public void SetPolarity(int value)
    {
        if (value > 0)
        {
            polarity = 1;
        }
        else if (value < 0) {
            polarity = -1;
        }
        else { polarity = 0; }
    }

    public int GetPolarity()
    {
        return polarity;
    }

    //Visual for determining appropriate magnetism range
    /* We're using real colliders now, so this isn't necessary
    private void OnDrawGizmosSelected()
    {
        if ( polarity > 0)
        {
            Gizmos.color = Color.red;
        }
        else if (polarity < 0) {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.white;
        }

        Gizmos.DrawWireSphere(transform.position, fieldRadius);
    }
    */
}
