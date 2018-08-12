using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public AudioClip[] infectSounds;
    public float tileSize;
    public Vector2 gridSize;
    public Sprite[] tileSprites;
    public GameObject destroyParticles;
    public int infectCount;
    public Player player;

    Node[,] grid;

    public static Grid instance;
    
	void Start () {
        instance = this;
        infectCount = 0;
        CreateGrid();
	}

    private void Update()
    {
        if (infectCount >= (gridSize.x * gridSize.y) * 0.95f)
        {
            player.GameOver();
        }
    }

    private void CreateGrid()
    {
        grid = new Node[(int)gridSize.x, (int)gridSize.y];
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                grid[x, y] = new GameObject().AddComponent<Node>();
                grid[x, y].transform.parent = transform;
                grid[x, y].transform.localPosition = new Vector2(x + 0.5f, y + 0.5f) * tileSize;
                grid[x, y].gameObject.AddComponent<SpriteRenderer>();
                grid[x, y].gameObject.AddComponent<BoxCollider2D>().size = Vector2.one * tileSize;
                grid[x, y].gameObject.GetComponent<BoxCollider2D>().enabled = false;
                grid[x, y].gameObject.AddComponent<AudioSource>();
                grid[x, y].gameObject.GetComponent<Node>().sounds = infectSounds;
                grid[x, y].gameObject.GetComponent<Node>().gridPosition = new Vector2(x, y);

                if (y == 0)
                {
                    grid[x, y].Infect();
                    grid[x, y].Invinsible();
                }
            }
        }
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (x > 0)
                    grid[x, y].neighbors[0] = grid[x - 1, y];
                if (x < gridSize.x - 1)
                    grid[x, y].neighbors[1] = grid[x + 1, y];
                if (y > 0)
                    grid[x, y].neighbors[2] = grid[x, y - 1];
                if (y < gridSize.y - 1)
                    grid[x, y].neighbors[3] = grid[x, y + 1];
            }
        }
    }

    public Node GetNodeFromWorldPos(Vector3 pos)
    {
        Vector3 nodePos = (pos - transform.position) / tileSize;
        return grid[Mathf.RoundToInt(Mathf.Clamp(nodePos.x-0.5f,0,gridSize.x-1)), Mathf.RoundToInt( Mathf.Clamp(nodePos.y-1,0, gridSize.y-1))];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(gridSize.x / 2 * tileSize, gridSize.y / 2 * tileSize,0), gridSize * tileSize);
        if(grid != null)
        {
            Gizmos.DrawCube(GetNodeFromWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)).transform.position,Vector3.one * tileSize);
        }
    }
}
