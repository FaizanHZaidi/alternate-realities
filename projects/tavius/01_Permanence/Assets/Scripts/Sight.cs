﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public int castDistance = 100;
    public float unlockTimer = 2.0f;
    public float viewTime;
    public GameObject inView;
    public int inViewID;
    public bool unlocked = false;
    public bool canPaintWorld = false;
    public float fog; // RenderSettings.fogDensity // 0.0f to 1.0f;
    private int rockCount = 0;
    private int treeCount = 0;
    private TexturePainting my_texture;

    void Start()
    {
        viewTime = Time.time;
    }

    void Update()
    {
        Vector3 forward = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, castDistance))
        {   
            // Check if object-in-view is the same:
            if (hit.collider.gameObject.GetInstanceID() == inViewID)
            {
                // Assess each object to be contemplated:

                // UnlockRock
                if (inView.CompareTag("Unlock"))
                {
                    if (!unlocked)
                    {
                        if (inView.GetComponent<EnvObjQualities>().seen == false)
                        {
                            // This should be an outside call the alpha editing method:
                            inView.GetComponent<EnvObjQualities>().alpha = 1;
                            inView.tag = "HideFromPaint";
                            inView.GetComponent<EnvObjQualities>().seen = true;
                            GameObject.Find("World").AddComponent<TexturePainting>();
                            my_texture = GameObject.Find("World").gameObject.GetComponent<TexturePainting>();
                            unlocked = true;
                            RenderSettings.fogDensity = 0.05f;
                        }
                    }
                }

                // Rocks and SmolRocks
                if (inView.CompareTag("Rock") || inView.CompareTag("SmolRock"))
                {
                    if (unlocked)
                    {
                        if (inView.GetComponent<EnvObjQualities>().seen == false)
                        {
                            inView.GetComponent<EnvObjQualities>().alpha = 1;
                            inView.tag = "HideFromPaint";
                            inView.GetComponent<EnvObjQualities>().seen = true;
                            rockCount++;
                            RenderSettings.fogDensity -= 0.0015f;
                        }
                    }
                }

                // Trees
                if (inView.CompareTag("Tree"))
                {
                    if (unlocked)
                    {
                        if (inView.GetComponent<EnvObjQualities>().seen == false)
                        {
                            inView.GetComponent<EnvObjQualities>().alpha = 1;
                            inView.tag = "HideFromPaint";
                            inView.GetComponent<EnvObjQualities>().seen = true;
                            treeCount++;
                            RenderSettings.fogDensity -= 0.0015f;
                        }
                    }
                }

                // World
                if (rockCount > 15 && !canPaintWorld)
                {
                    GameObject.Find("World").tag = "World";
                    canPaintWorld = true;
                    my_texture.Restart(32);
                }

                //Debug.Log(rockCount);

            }

            // Otherwise object-in-view is new. Reset the vars:
            else
            {
                viewTime = Time.time;
                inView = hit.collider.gameObject;
                inViewID = hit.collider.gameObject.GetInstanceID();
            }

        }
    }

    void editAlpha()
    {

    }
}


//  this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);

//  if (Time.time - viewTime > unlockTimer)

//  Debug.Log(Time.time - viewTime);

//  rayDistance += 5;
