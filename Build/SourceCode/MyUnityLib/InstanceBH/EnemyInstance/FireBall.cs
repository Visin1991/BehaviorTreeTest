using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {

    public float speed;
    public float liftime;

    public Vector3 direction;
    // Use this for initialization

    public void SetDirection(Vector3 _direction)
    {
        direction = _direction;
    }

    // Update is called once per frame
    void Update () {
        liftime -= Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;
        if (liftime <= 0) {
            Destroy(gameObject);
        }
	}
}
