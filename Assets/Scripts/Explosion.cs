using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public AudioClip[] sounds;

    public void StartExplode(float size, int dmg)
    {
        StartCoroutine(Explode(size, dmg));
    }

    public IEnumerator Explode(float size, int dmg)
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().clip = sounds[Random.Range(0, sounds.Length)];
        GetComponent<AudioSource>().Play();
        float timeStamp = Time.time;
        transform.localScale = Vector3.zero;
        while(Time.time < timeStamp + 0.1f)
        {
            yield return null;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * size, (Time.time - timeStamp) / 0.1f);
        }

        foreach (Collider2D node in Physics2D.OverlapCircleAll(transform.position, size * 2f))
        {
            if (node.GetComponent<Destructible>() != null)
            {
                if(Vector2.Distance(node.transform.position, transform.position) <= size*1f)
                    node.GetComponent<Destructible>().Damage(  dmg  );
                else
                    node.GetComponent<Destructible>().Damage( dmg / 2 );
            }
        }

        timeStamp = Time.time;
        while (Time.time < timeStamp + 0.6f)
        {
            yield return null;
            GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1,1,1,0.5f), Color.clear, (Time.time - timeStamp) / 0.6f);
        }
        Destroy(gameObject);
    }
}
