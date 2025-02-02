﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Emotion : MonoBehaviour {

	public Emotion emotionPrefab;
	public static bool isEmotion = true;
	private bool isTrigger;
	public string emotionType; // JOY, SAD, FEA, ECS, DES, SUR, TER, ANX, MEL
    Color[] color =        {   joy,   sad,   fea,   ecs,   des,   sur,   ter,   anx,   mel };
    float[] diameter =     {  0.2f, 0.35f, 0.15f,  0.3f, 0.45f,    5f,    6f,    7f,    8f };
    bool[] gravity =       { false,  true,  true, false,  true, false,  true,  true, false };
    float[] drag =         { 0.17f,  50f,    20f,    1f,    4f,    5f,    6f,    7f,    8f };
    float[] angularDrag =  {  0.3f,  2f,    10f,  0.2f,    4f,    5f,    6f,    7f,    8f };

    // Color bank:
    public static Color joy = new Color(1f, 0.933333333f, 0f);
    public static Color sad = new Color(0.11764705882f, 0.17647058823f, 0.627451f);
    public static Color fea = new Color(0.31372549019f, 0f, 0.49803921568f);
    public static Color ecs = new Color(1f, 0f, 0f);
    public static Color des = new Color(0f, 0f, 0f);
    public static Color sur = new Color(0f, 0f, 0f);
    public static Color ter = new Color(0f, 0f, 0f);
    public static Color anx = new Color(0f, 0f, 0f);
    public static Color mel = new Color(0f, 0f, 0f);

    public void Start()
    {
        Make(emotionType);
    }

	public void Update()
	{
		isTrigger = gameObject.GetComponent<SphereCollider>().isTrigger;
	}

    private void Make(string thisEmotion)
    {
        int e = DetermineEmotion(emotionType);

        if (e == -1)
        {
            Debug.LogError("Invalid input. Check EmotionType of Emotion component.");
        }
        else
        {
            if(e == 0)
            {
                gameObject.AddComponent<JoyFollower>();
            }
            gameObject.transform.localScale = new Vector3(diameter[e], diameter[e], diameter[e]);
            gameObject.GetComponent<Rigidbody>().useGravity = gravity[e];
            gameObject.GetComponent<Rigidbody>().drag = drag[e];
            gameObject.GetComponent<Rigidbody>().angularDrag = angularDrag[e];
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", color[e]);
        }
    }

    // Quick function to test given Emotion Type passed to this component:
    private int DetermineEmotion(string thisEmotion)
    {
        //thisEmotion = emotionType;
        switch (thisEmotion)
        {
            case "JOY":
                return 0;
            case "SAD":
                return 1;
            case "FEA":
                return 2;
            case "ECS":
                return 3;
            case "DES":
                return 4;
            case "SUR":
                return 5;
            case "TER":
                return 6;
            case "ANX":
                return 7;
            case "MEL":
                return 8;
            default:
                return -1;
        }
    }

	// This function is called by Unity when THIS emotion is acting as a trigger, and is touching another collider:
	void OnTriggerStay(Collider otherObject)
	{
		int e = DetermineEmotion(emotionType);
		// repelStrength is a variable so that different objects can be repelled with greater or lesser force. (Should this be a property of each emotion?)
		float repelStrength = 2f;

		// Distance between this emotion and the collided object:
		float dist = Vector3.Distance(gameObject.transform.position, otherObject.transform.position);

		// If this emotion is one of the main three:
		if (gameObject.GetComponent<Emotion>().emotionType.Equals("JOY") || gameObject.GetComponent<Emotion>().emotionType.Equals("SAD") || gameObject.GetComponent<Emotion>().emotionType.Equals("FEA")) {

			// If colliding with your face, change ground color:
			if (otherObject.CompareTag("MainCamera")) {
				Debug.Log (dist);
				if (dist < 1) {
					GameObject.Find("Ground").GetComponent<Renderer> ().material.SetColor("_Color", color [e]);
					//GameObject newEmotion = Instantiate(groundPrefab, Ground.transform.position, Ground.transform.rotation);  // Useful for switching the ground prefab
				}
			} 

			// Otherwise check for a colliding emotion:
			if (otherObject.CompareTag("emotion")) {
				// TODO: Check that both objects are emotions before sending them to tryCombo. (Otherwise the rest below here won't run)
				string newType = EmotionCombos.tryCombo(gameObject, otherObject.gameObject);


				// If the emotions are not compatible, repel:
				if (newType.Equals("nope")) {
					if (dist < diameter[e] * 1.1) {
						// Debug.Log(rightEmotion.name + " repelled " + otherEmotion.gameObject.name);
						Vector3 repelVector = otherObject.transform.position - gameObject.transform.position;
						otherObject.attachedRigidbody.AddForce(repelVector * (dist / dist) * repelStrength);
					}
				}

				// If the colliding emotion IS compatible:
				else {
					// Once centers are within 15% of each other:
					if (dist < diameter[e] * 0.15) {
						// Debug.Log(rightEmotion.name + " combined with " + otherEmotion.gameObject.name);
						Destroy(otherObject.gameObject);
						Debug.Log ("Destroyed " + otherObject.gameObject);
						CreateEmotion(newType);
						Destroy(gameObject);
						Debug.Log ("And destroyed " + gameObject);
					}
				}
			}
		}
	}

	void OnTriggerExit(Collider otherEmotion)
	{
		if (otherEmotion.CompareTag("incompatibleEmotion"))
		{
			otherEmotion.attachedRigidbody.velocity = Vector3.zero;
		}
	}

	public void CreateEmotion(string emotionType)
	{
		Emotion newEmotion = Instantiate(emotionPrefab, gameObject.transform.position, gameObject.transform.rotation) as Emotion;
		newEmotion.emotionType = emotionType;
		//Debug.Log (newEmotion.name + " created");
	}

	public void CreateEmotionInHand(string emotionType)
	{
		Emotion newEmotion = Instantiate(emotionPrefab, GameObject.FindWithTag("left").transform.position, GameObject.FindWithTag("left").transform.rotation) as Emotion;
		newEmotion.emotionType = emotionType;
		int i = DetermineEmotion(emotionType);
		newEmotion.transform.Translate(Vector3.up * diameter[i] / 2f, Space.World);
	}

}