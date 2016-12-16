using UnityEngine;
using System.Collections;

public class MyMathlibTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector2 v1 = new Vector2(1,-1);
		Vector2 v2 = new Vector2(-1,-1);

		//float angle = WeiVector2.AngleBetween(v1,v2);
		float angle = new WeiVector2Poarl(v1).a;
		Debug.Log(angle);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
