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
    // If a space is 1, it's "blue"
    public bool ContainsEx(int x, int y)
    {
        return grid[x, y] == 1;
    }

    // If a space is 2, it's "red"
    public bool ContainsOh(int x, int y)
    {
        return grid[x, y] == 2;
    }

    // If a space is 0, it's empty.
    public bool IsEmpty(int x, int y)
    {
        return grid[x, y] == 0;
    }

    // If the highest space in the column is empty, 
    // you can add another piece.
    public bool ColumnFilled(int column)
    {
        return !IsEmpty(column, height - 1);
    }

    // This function is used to add a piece to a column
    public void AddToColumn(int column)
    {
        // If a column is full, do nothing.
        if (ColumnFilled(column)) return;

        // If either player has already won, do nothing.
        if (ExWin() || OhWin()) return;
        
        // Find the lowest empty space in the column, and add a piece.
        for (var y = 0; y < height; y++)
        {
            if (IsEmpty(column, y))
            {
                if (ohTurn)
                    grid[column, y] = 2;
                else
                    grid[column, y] = 1;

                ohTurn = !ohTurn;

                // once a piece is added, update the display.
                UpdateDisplay();
                return;
            }
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

    // This function checks for four in a row, and returns the number that is four in a row.
    // 1 for blue
    // 2 for red
    public int ThreeInARow()
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (y <= height - 4)
                    if (grid[x,y] != 0 && 
                        grid[x, y] == grid[x, y + 1] && 
                        grid[x, y] == grid[x, y + 2] && 
                        grid[x, y] == grid[x, y + 3])
                        return grid[x, y];
             
                if (x <= width - 4)           
                    //is there a piece there (0 == empty, remember?)?
                    if (grid[x,y] != 0 && 
                        //is the piece that' there equal to the one above it?
                        grid[x, y] == grid[x + 1, y] && 
                        //is this equal to the one above it?
                        grid[x, y] == grid[x + 2, y] && 
                        // and is this equal to the one above it?
                        grid[x, y] == grid[x + 3, y])
                        //if these are all the same, then we have a connect four
                        return grid[x, y];

                if (x <= width - 4 && y <= height - 4)
                    if (grid[x,y] != 0 && 
                        grid[x, y] == grid[x + 1, y + 1] && 
                        grid[x, y] == grid[x + 2, y + 2] && 
                        grid[x, y] == grid[x + 3, y + 3])
                        return grid[x, y];

                if (x >= width - 4 && y <= height - 4)
                    if (grid[x,y] != 0 && 
                        grid[x, y] == grid[x - 1, y + 1] && 
                        grid[x, y] == grid[x - 2, y + 2] && 
                        grid[x, y] == grid[x - 3, y + 3])
                        return grid[x, y];
            }
        }
        return 0;
    }
}
