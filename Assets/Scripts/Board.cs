using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public Tile tilePrefab;

    public int numberOfBombs;
    public int numberOfRows;
    public int numberOfColumns;

    Tile[,] tiles;

    void Start()
    {
        if(!AreSettingsValid())
        {
            throw new System.Exception("Board settings are not valid");
        }

        InitBoard();
    }

    bool AreSettingsValid()
    {
        int numberOfTiles = numberOfRows * numberOfColumns;

        return numberOfTiles > 0 && 
            numberOfBombs > 0 && 
            numberOfTiles > numberOfBombs;
    }

    void InitBoard()
    {
        CreateBoard();
        PlaceBombs();
        NotifyNeighboursAboutBombs();
    }

    void CreateBoard()
    {
        
        // todo, destroy any existing tiles (feom prev game)
        tiles = new Tile[numberOfRows, numberOfColumns];

        RectTransform tilePrefabRect = tilePrefab.GetComponent<RectTransform>();

        for (int row = 0; row < numberOfRows; ++row)
        {
            for(int column = 0; column < numberOfColumns; ++column)
            {
                Tile tile = Instantiate(tilePrefab);
                tile.Init(this, row, column);

                tile.transform.SetParent(transform, false);
                tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(column * tilePrefabRect.rect.height, row * tilePrefabRect.rect.width);

                tile.name = "Row: " + row + " - Column: " + column;
                tiles[row, column] = tile;
            }
        }

        // Set the size of this container to desired size of all its childern so it will be centered
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, numberOfColumns * tilePrefabRect.rect.width);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, numberOfRows * tilePrefabRect.rect.height);
    }

    void PlaceBombs()
    {
        int bombsLeftToPlace = numberOfBombs;

        while (bombsLeftToPlace > 0)
        {
            int row = Random.Range(0, numberOfRows);
            int column = Random.Range(0, numberOfColumns);

            if (tiles[row, column].isBomb)
            {
                continue;
            }

            tiles[row, column].isBomb = true;
            --bombsLeftToPlace;
        }
    }

    void NotifyNeighboursAboutBombs()
    {
        // cache bombs later todo
        for (int row = 0; row < numberOfRows; ++row)
        {
            for (int column = 0; column < numberOfColumns; ++column)
            {
                if (tiles[row, column].isBomb)
                {
                    NotifyNeighboursAboutABomb(row, column);
                }
            }
        }
    }

    void NotifyNeighboursAboutABomb(int bombRow, int bombColumn)
    {
        for (int row = bombRow - 1; row <= bombRow + 1; ++row)
        {
            for (int column = bombColumn - 1; column <= bombColumn + 1; ++column)
            {
                // We can increase the value without worring if this tile is the bomb,
                // because if it is, we wont use it anyway

                if(row >= 0 && column >= 0 && 
                    row < numberOfRows && column < numberOfColumns)
                {
                    ++tiles[row, column].neighbourBombCount;
                }
            }
        }
    }

    public void OnUncoverRequested(int row, int column)
    {
        Tile tile = tiles[row, column];
        if(tile.isBomb)
        {
            // agme end
            tile.Uncover();

        }
        else
        {
            UncoverFloodFill(row, column);
        }
    }

    void UncoverFloodFill(int row, int column)
    {
        if (row < 0 || column < 0 ||
            row >= numberOfRows || column >= numberOfColumns)
        {
            return;
        }

        Tile tile = tiles[row, column];
        if(tile.state == Tile.State.Uncovered)
        {
            return;
        }

        tile.Uncover();
        if (tile.neighbourBombCount > 0)
        {
            return;
        }

        UncoverFloodFill(row - 1, column);
        UncoverFloodFill(row + 1, column);
        UncoverFloodFill(row, column - 1);
        UncoverFloodFill(row, column + 1);
    }

    void UncoverAll()
    {
        for (int row = 0; row < numberOfRows; ++row)
        {
            for (int column = 0; column < numberOfColumns; ++column)
            {
                tiles[row, column].Uncover();
            }
        }
    }
}
