using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Destructible {

    public GameObject sprout;
    public float growthTime;
    private Color infectedColor = new Color(0.9f, 0.5f, 1);

    private bool infected;


    void Start () {
        hp = 15;
        StartCoroutine("GrowBombs");
	}

    public void Infect()
    {
        if (!infected)
        {
            infected = true;
            StopCoroutine("GrowBombs");
            GetComponent<SpriteRenderer>().color = infectedColor;
        }
    }

    public override void Damage(int dmg)
    {
        base.Damage(dmg);

        if (infected)
            GetComponent<SpriteRenderer>().color = infectedColor + new Color(0, (hp - 15f) * 0.1f, (hp - 15f) * 0.1f);
        else
            GetComponent<SpriteRenderer>().color = Color.white + new Color(0, (hp - 15f) * 0.1f, (hp - 15f) * 0.1f);
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator GrowBombs()
    {
        while (true)
        {
            sprout.transform.localPosition = Vector3.zero;
            sprout.transform.localScale = Vector3.zero;
            sprout.GetComponent<SpriteRenderer>().color = Color.white;

            float timeStamp = Time.time;
            while(Time.time < timeStamp + growthTime)
            {
                yield return null;
                sprout.transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, (Time.time - timeStamp) / growthTime);
            }
            timeStamp = Time.time;
            while(Time.time < timeStamp + 0.2f)
            {
                yield return null;
                sprout.transform.position += Vector3.up * 0.1f;
                sprout.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, (Time.time - timeStamp) / 0.2f);
            }
            Player.ammunition++;
        }
    }
}
