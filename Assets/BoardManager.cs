using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour  // Define a new component named CreateBoard
{
    public GameObject[] tilesPrefabs;      // Array of tile prefabs (e.g. Dirt, Desert)

    /*
     * VALUES GUIDE
    public GameObject cactusPrefab; desert
    public GameObject treePrefab; dirt
    public GameObject wheatPrefab; grain
    public GameObject cowPrefab; pasture
    public GameObject stonesPrefab; rocks
    public GameObject fishPrefab; water
    public GameObject housePrefab; woods   // Prefab for houses the player places*/

    public GameObject[] objectPrefabs;
    

    public Text score;                    // Reference to UI Text for displaying score
    GameObject[] tiles;                   // Internal array to hold the instantiated tiles

    long[] tilesBB =
    {
        0,
        0,
        0,
        0, 
        0, 
        0, 
        0 
    };

    long[] objectsBB =
    {
        0,
        0,
        0,
        0,
        0,
        0,
        0
    };

    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[64];       // Prepare storage for an 8×8 board
        for (int r = 0; r < 8; ++r)        // Loop over each row (0–7)
        {
            for (int c = 0; c < 8; ++c)    // Loop over each column (0–7)
            {
                int randomTile = UnityEngine.Random.Range(0, tilesPrefabs.Length);
                // Pick a random index into our tile prefabs
                Vector3 pos = new Vector3(c, 0, r);
                // Compute world position (x=c, y=0, z=r)
                GameObject tile = Instantiate(
                    tilesPrefabs[randomTile],
                    pos,
                    Quaternion.identity
                );                     // Spawn that tile at the position with no rotation
                tile.name = tile.tag + "_" + r + "_" + c;
                // Rename for easy debugging, e.g. "Dirt_2_5"
                tiles[r * 8 + c] = tile;
                // Store reference in the 1D array
                if (tile.tag == "Dirt") // if dirt is tag
                {
                    tilesBB[1] = SetCellState(tilesBB[1], r, c);
                }
                if (tile.tag == "Desert") // if dirt is tag
                {
                    tilesBB[0] = SetCellState(tilesBB[0], r, c);
                }
                if (tile.tag == "Grain") // if dirt is tag
                {
                    tilesBB[2] = SetCellState(tilesBB[2], r, c);
                }
                if (tile.tag == "Pasture") // if dirt is tag
                {
                    tilesBB[3] = SetCellState(tilesBB[3], r, c);
                }
                if (tile.tag == "Rock") // if dirt is tag
                {
                    tilesBB[4] = SetCellState(tilesBB[4], r, c);
                }
                if (tile.tag == "Water") // if dirt is tag
                {
                    tilesBB[5] = SetCellState(tilesBB[5], r, c);
                }
                if (tile.tag == "Woods") // if dirt is tag
                {
                    tilesBB[6] = SetCellState(tilesBB[6], r, c);
                }
            } // end for c
        } // end for r
        InvokeRepeating("PlantObjects", 0.25f, UnityEngine.Random.Range(0.25f, 3f));
    } // end Start

    void PrintBB(string name, long BB)
    {
        Debug.Log(name + ": " + Convert.ToString(BB, 2).PadLeft(64, '0'));
    }

    long SetCellState(long Bitboard, int row, int col)
    {
        long newBit = 1L << (row * 8 + col);
        return (Bitboard |= newBit);
    }
    bool GetCellState(long Bitboard, int row, int col)
    {
        long mask = 1L << (row * 8 + col);
        return ((Bitboard & mask) != 0);
    }

    int CellCount(long bitboard) // Count how many Cells is created base on tags
    {
        int count = 1;
        long bb = bitboard;
        while (bb != 0)
        {
            bb &= bb - 1;
            count++;
        }
        return count;
    }

    void PlantObjects()
    {
        int i = UnityEngine.Random.Range(0, 7);
        int rr = UnityEngine.Random.Range(0, 8);//random row
        int rc = UnityEngine.Random.Range(0, 8);//random column
        if (GetCellState(tilesBB[i], rr, rc))
        {
            GameObject tree = Instantiate(objectPrefabs[i]);
            tree.transform.parent = tiles[rr * 8 + rc].transform;//parent it
            tree.transform.localPosition = Vector3.zero;//aligns it
            objectsBB[i] = SetCellState(objectsBB[i], rr, rc);
        }
    }
}
