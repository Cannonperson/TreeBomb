using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public AudioClip throwSounds;
    public AudioClip[] placeSounds;
    public AudioClip deniedSound;
    public Text ammoText;
    public Text munText;
    public Text timeText;
    public Text gameOverText;

    public float maxX;
    public float speed;
    public float throwStrength;
    public GameObject bomb;
    public GameObject farm;
    public Sprite farmSprite;
    public static int ammunition = 5;
    public static int mun = 50;
    public static float gameTimeStamp;

    private float horizontal;
    private bool fire;
    private bool placingFarm = false;
    private bool gameOver;

    private void Start()
    {
        ammunition = 5;
        mun = 50;
        gameTimeStamp = Time.time;
    }

    void Update () {
        if (!gameOver)
        {
            Controls();
            ammoText.text = "Ammo\n" + ammunition;
            munText.text = "Mun\n" + mun;
            timeText.text = "Time\n" + (int)(Time.time - gameTimeStamp);
        }
        else if(Input.anyKey)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void Controls()
    {
        horizontal = Input.GetAxis("Horizontal");
        fire = Input.GetButtonDown("Fire1");


        transform.position = new Vector3(Mathf.Clamp(transform.position.x + horizontal * speed * Time.deltaTime, -maxX, maxX), transform.position.y, transform.position.z);
        if (fire && ammunition > 0)
            Fire();
        if (Input.GetButtonDown("Fire2") && placingFarm == false && mun > 100)
        {
            StartCoroutine("PlaceFarm");
        }
    }

    void Fire()
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().clip = throwSounds;
        GetComponent<AudioSource>().Play();

        GameObject thrownBomb = Instantiate(bomb, transform.position, Quaternion.identity);
        thrownBomb.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position, throwStrength);
        ammunition--;
    }

    IEnumerator PlaceFarm()
    {
        placingFarm = true;
        GameObject newFarm = new GameObject();
        newFarm.AddComponent<SpriteRenderer>().sprite = farmSprite;
        newFarm.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
        while (!Input.GetButtonUp("Fire2"))
        {
            newFarm.transform.position = Grid.instance.GetNodeFromWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)).transform.position;
            yield return null;
        }
        if (!Physics2D.OverlapBox(Grid.instance.GetNodeFromWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)).transform.position, Vector2.one * Grid.instance.tileSize * 2.5f, 0))
        {

            GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().clip = placeSounds[Random.Range(0, placeSounds.Length)];

            GameObject createdFarm = Instantiate(farm, Grid.instance.GetNodeFromWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)).transform);
            createdFarm.GetComponent<Farm>().growthTime = 20 * (Grid.instance.GetNodeFromWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gridPosition.y / Grid.instance.gridSize.y);
            mun -= 100;
        }
        else
        {
            GetComponent<AudioSource>().clip = deniedSound;
        }
        GetComponent<AudioSource>().Play();
        Destroy(newFarm);
        placingFarm = false;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.text = "GAME\nOVER";
    }
}
