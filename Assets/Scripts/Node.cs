using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : Destructible {

    public Vector2 gridPosition;
    public AudioClip[] sounds;
    bool infected;
    bool invinsible;

    public Node[] neighbors = new Node[4];

    private void Start()
    {
        hp = 5;
    }

    public void Infect()
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.4f);
        GetComponent<AudioSource>().clip = sounds[Random.Range(0, sounds.Length)];
        GetComponent<AudioSource>().Play();
        infected = true;
        GetComponent<SpriteRenderer>().sprite = GetComponentInParent<Grid>().tileSprites[Random.Range(0, GetComponentInParent<Grid>().tileSprites.Length)];
        GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine("SpreadInfection");
        foreach(Collider2D target in Physics2D.OverlapPointAll(transform.position))
        {
            if(target.GetComponent<Farm>() != null)
            {
                target.GetComponent<Farm>().Infect();
            }
        }
        hp = 5;
        Grid.instance.infectCount++;
    }

    public void Invinsible()
    {
        invinsible = true;
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    public override void Damage(int dmg)
    {
        if (!invinsible)
        {
            base.Damage(dmg);
            if (hp <= 0)
            {
                infected = false;
                GetComponent<SpriteRenderer>().sprite = null;
                StopCoroutine("SpreadInfection");
                GetComponent<BoxCollider2D>().enabled = false;
                Grid.instance.infectCount--;
                Instantiate(Grid.instance.destroyParticles, transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator SpreadInfection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Mathf.Clamp(Random.Range(2f, 15f) - ((Time.time - Player.gameTimeStamp) / 100f), 0.001f, 20f));

            if (invinsible)
            {
                if (Random.Range(0, 10) == 0)
                {
                    int randomNeighbor = Random.Range(0, neighbors.Length);
                    if (neighbors[randomNeighbor] != null)
                    {
                        if (neighbors[randomNeighbor].infected)
                            neighbors[randomNeighbor].Invinsible();
                    }
                }
            }
            for (int i = 0; i < 1 + (Time.time - Player.gameTimeStamp) / 100; i++)
            {
                if (Random.Range(0, 10 + Grid.instance.infectCount) != 0)
                {
                    int randomNeighbor = Random.Range(0, neighbors.Length);
                    if (neighbors[randomNeighbor] != null)
                    {
                        if (!neighbors[randomNeighbor].infected)
                            neighbors[randomNeighbor].Infect();
                    }
                }
            }
            if(Random.Range(0,3) == 0)
            {
                Player.mun++;
            }
        }
    }
}
