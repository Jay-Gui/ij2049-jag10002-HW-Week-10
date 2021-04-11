using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TicTacToeGrid : MonoBehaviour
{
    //size of x,y grid
    public int width = 3;
    public int height = 3;
    
    //make the 2D array
    private int[,] grid; // 0 be an empty space, 1 be ex, and 2 oh

    //gets text
    public Text display;
    
    //boolean for ex and oh player
    private bool ohTurn = false;

    //list for spawned pieces
    private List<GameObject> spawnedPieces = new List<GameObject>();

    //prefabs for red and blue pieces
    public GameObject exPrefab, ohPrefab;

    private void Start()
    {
        // Instantiate and initialize the grid.
        
        //create the grid using width and height numbers
        grid = new int[width, height];

        //building the grid using x and y
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = 0;
            }
        }
    }

    private void Update()
    {
        // If you press space, it reloads the scene.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //arbitrary numbers where each number is a state
    // If a space is 1, it's "ex"
    public bool ContainsEx(int x, int y)
    {
        return grid[x, y] == 1;
    }

    // If a space is 2, it's "oh"
    public bool ContainsOh(int x, int y)
    {
        return grid[x, y] == 2;
    }

    // If a space is 0, it's empty.
    public bool IsEmpty(int x, int y)
    {
        return grid[x, y] == 0;
    }

    // This function is used to add a piece to a column
    public void AddToColumn(string column)
    {
        // If either player has already won, do nothing.
        if (ExWin() || OhWin()) return;
        
        //take a string and turn it into two different integers
        if (IsEmpty(int.Parse(column.Substring(0,1)), 
                int.Parse(column.Substring(1, 1))))
            {
                if (ohTurn)
                    grid[int.Parse(column.Substring(0,1)), 
                        int.Parse(column.Substring(1,1))] = 2;
                else
                    grid[int.Parse(column.Substring(0,1)), 
                        int.Parse(column.Substring(1,1))] = 1;

                ohTurn = !ohTurn;

                // once a piece is added, update the display.
                UpdateDisplay();
                return;
            }
    }
    private void UpdateDisplay()
    {
        // To update the display, first destroy all the pieces that were spawned.
        foreach (var piece in spawnedPieces)
        {
            Destroy(piece);
        }
        
        spawnedPieces.Clear();

        // Then, for everything in the grid, if it's not 0, spawn the correct piece.
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (ContainsOh(x, y))
                {
                    var ohPiece = Instantiate(ohPrefab);
                    ohPiece.transform.position = new Vector3(x, y);
                    spawnedPieces.Add(ohPiece);
                }
                if (ContainsEx(x, y))
                {
                    var exPiece = Instantiate(exPrefab);
                    exPiece.transform.position = new Vector3(x, y);
                    spawnedPieces.Add(exPiece);
                }
            }
        }
        
        // check to see if red or blue won.
        if (OhWin())
        {
            display.text = "OH WINS!";
            display.color = Color.black;
        }
        else if (ExWin())
        {
            display.text = "EX WINS!";
            display.color = Color.black;
        }
        else
        {
            display.text = "";
        }
    }
    
    public bool OhWin()
    {
        return ThreeInARow() == 2;
    }

    public bool ExWin()
    {
        return ThreeInARow() == 1;
    }

    // This function checks for three in a row, and returns the number that is three in a row.
    // 1 for ex
    // 2 for oh
    public int ThreeInARow()
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (y <= height - 3)
                    if (grid[x,y] != 0 && 
                        grid[x, y] == grid[x, y + 1] && 
                        grid[x, y] == grid[x, y + 2])
                        return grid[x, y];
             
                if (x <= width - 3)           
                    //is there a piece there (0 == empty, remember?)?
                    if (grid[x,y] != 0 && 
                        //is the piece that's there equal to the one next to it?
                        grid[x, y] == grid[x + 1, y] && 
                        //is this equal to the one next to it?
                        grid[x, y] == grid[x + 2, y])
                        //if these are all the same, then we have three in a row
                        return grid[x, y];

                if (x <= width - 3 && y <= height - 3)
                    if (grid[x,y] != 0 && 
                        grid[x, y] == grid[x + 1, y + 1] && 
                        grid[x, y] == grid[x + 2, y + 2])
                        return grid[x, y];

                if (x == width - 1 && y <= height - 3)
                    if (grid[x,y] != 0 && 
                        grid[x, y] == grid[x - 1, y + 1] && 
                        grid[x, y] == grid[x - 2, y + 2])
                        return grid[x, y];
            }
        }
        return 0;
    }
}
