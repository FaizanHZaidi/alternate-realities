﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour {

	public float offDurationMin;
	public float offDurationMax;

	public float onDurationMin;
	public float onDurationMax;

	void Start() {
		StartCoroutine(DoOnOff());
	}

	IEnumerator DoOnOff() {
		while(true) {
			enabled = false;
			yield return new WaitForSeconds(Random.Range(offDurationMin, offDurationMax));
			enabled = true;
			yield return new WaitForSeconds(Random.Range(onDurationMin, onDurationMax));
		}
	}




//		lightningPlacement ();
//	}
//
//	void lightningPlacement()
//	{
//		for (int i = 0; i < numberofLightning; i++) {
//			Instantiate (Lightning, GeneratedPosition (), Quaternion.identity);
//		}
//
//	}
//	Vector3 GeneratedPosition() 
//	{
//		int x, y, z;
//		x = UnityEngine.Random.Range (min, max);
//		y = UnityEngine.Random.Range (min, max);
//		z = UnityEngine.Random.Range (min, max);
//		return new Vector3 (x,y,z);
//	}
	// Update is called once per frame
	void Update () {
		//transform.rotation = Quaternion.Euler(90, 0, 0);
	}
}
