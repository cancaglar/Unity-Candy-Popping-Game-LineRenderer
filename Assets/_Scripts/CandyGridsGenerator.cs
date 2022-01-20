using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandyGridsGenerator : MonoBehaviour
{
    private static CandyGridsGenerator _instance;

    public static CandyGridsGenerator Instance
    {
        get { return _instance; }
    }

    private int columnCount = 5;
    [SerializeField] private int rowCount = 9;
    public float gridNewPos = 1f;
    [SerializeField] private GameObject[] candies;

    [SerializeField] private GameObject emptyGameobject;

    // Positions of columns.
    [SerializeField] private List<Transform> colPosList = new List<Transform>();

    // A list will be kept for each column and coins will be kept in them.
    public List<GameObject> gridsCol1 = new List<GameObject>();
    public List<GameObject> gridsCol2 = new List<GameObject>();
    public List<GameObject> gridsCol3 = new List<GameObject>();
    public List<GameObject> gridsCol4 = new List<GameObject>();

    public List<GameObject> gridsCol5 = new List<GameObject>();

    // A List for the columns lists
    List<List<GameObject>> colsList = new List<List<GameObject>>();

    private void Awake()
    {
        // singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        // add lists to list
        colsList.Add(gridsCol1);
        colsList.Add(gridsCol2);
        colsList.Add(gridsCol3);
        colsList.Add(gridsCol4);
        colsList.Add(gridsCol5);
    }

    void Start()
    {
        // generate grids
        GridsGenerator();
    }

    void Update()
    {
        for (int i = 0; i < colsList.Count; i++)
        {
            for (int j = 0; j < colsList[i].Count; j++)
            {
                // if grid[i,j] is empty
                if (colsList[i][j].transform.childCount == 0)
                {
                    // if j is not 0. because if it is 0 we can't get the -1 grid
                    if (j != 0)
                    {
                        // set j-1 grids childs(candy) parent to j(current grid)
                        colsList[i][j - 1].transform.GetChild(0).transform.parent = colsList[i][j].transform;
                    }
                }
                // if the first grid of current column is empty
                if (colsList[i][0].transform.childCount == 0)
                {
                    // choose random candy
                    int randomPiece = Random.Range(0, candies.Length);
                    // Position of first grid of current column
                    Vector2 pos2 = new Vector2(colsList[i][0].transform.position.x,
                        colsList[i][0].transform.position.y);
                    // Instantiate
                    GameObject candy = (GameObject) Instantiate(candies[randomPiece], pos2, Quaternion.identity);
                    // set candy's parent to first grid of current column
                    candy.transform.parent = colsList[i][0].transform;
                }
            }
        }
    }

    private void GridsGenerator()
    {
        // Create Grids
        for (int col = 0; col < columnCount; col++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                // Position of current column - row * gridNewPos
                Vector2 pos = new Vector2(colPosList[col].transform.position.x,
                    colPosList[col].transform.position.y - (row * gridNewPos));
                // Instantiate grid
                GameObject grid = (GameObject) Instantiate(emptyGameobject, pos, Quaternion.identity);
                // set name to know position
                grid.name = "Grid " + col + "," + row;
                // add to current column list
                colsList[col].Add(grid);
            }
        }

        // Create Candies
        for (int col = 0; col < colsList.Count; col++)
        {
            for (int row = 0; row < colsList[col].Count; row++)
            {
                // choose random candy
                int randomPiece = Random.Range(0, candies.Length);
                // Instantiate the candy
                GameObject candy = (GameObject) Instantiate(candies[randomPiece], Vector3.zero, Quaternion.identity);
                // set candy's parent to current grid
                candy.transform.parent = colsList[col][row].gameObject.transform;
                candy.name += col + "" + row; // give name

                candy.transform.localPosition = Vector3.zero; // set localPosition to zero
            }
        }
    }
}