using System.Collections.Generic;
using UnityEngine;

public class ShieldCatcher : MonoBehaviour
{
    [SerializeField] float vortexRange = 0.8f;
    [SerializeField] float vortexRotationSpeed = 25f;
    [SerializeField] public int maxBullets = 5;
    [SerializeField] Collider2D vortexCatcher;
    public int polarity = 0;
    public int curBullets = 0;
    public List<bool> bulletSlots = new();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < maxBullets; i++)
        {
            bulletSlots.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
