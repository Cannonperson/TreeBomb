using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject explosion;
    public float bombRadius;
    public float bombTimer;
    public int dmg;

	void Start () {
        Invoke("Explode", bombTimer);
	}
	
	void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<Explosion>().StartExplode(bombRadius, dmg);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, bombRadius);
    }
}
