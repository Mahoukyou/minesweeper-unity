using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField]
    struct SizeSettings
    {
        int amountOfBombs;
        int amountOfRows;
        int amountOfColumns;
    }

    [SerializeField]
    struct GenerationSettings
    {
        bool generateBombsAfterFirstClick;
        bool spaceBetweenBombsAndFirstClick;
    }


    public Tile tilePrefab;

//    SizeSettings sizeSettings;

    int amountOfBombs;
    int amountOfRows;
    int amountOfColumns;

    public bool generateBombsAfterFirstClick = false;
    public int spaceBetweenBombsAndFirstClick = 3;

    public float floodFillUncoverTickTime = 0.1f;

    bool isInitialized = false;
    bool isUncoveringTiles = false;

    Tile[,] tiles = null;

    public void InitializeBoard(int amountOfBombs, int[] boardDimensions)
    {
        this.amountOfBombs = amountOfBombs;
        amountOfRows = boardDimensions[0];
        amountOfColumns = boardDimensions[1];

        InitializeBoard();
    }

    bool AreSettingsValid()
    {
        int totalAmountOfTiles = amountOfRows * amountOfColumns;

        if(totalAmountOfTiles <= 0 || amountOfBombs <= 0)
        {
            return false;
        }

        if(generateBombsAfterFirstClick)
        {
            // space * 2 (for left and right side for the space) + middle
            int diameterOfTheSpace = spaceBetweenBombsAndFirstClick * 2 + 1;
            if (diameterOfTheSpace >= amountOfRows ||
                diameterOfTheSpace >= amountOfColumns)
            {
                return false;
            }

            int amountOfSpaceTiles = spaceBetweenBombsAndFirstClick > 0 ? (int)Mathf.Pow(diameterOfTheSpace, 2.0f) : 0;

            return totalAmountOfTiles - amountOfSpaceTiles > amountOfBombs;
        }

        return totalAmountOfTiles > amountOfBombs;
    }

    void InitializeBoard()
    {
        if (!AreSettingsValid())
        {
            throw new System.Exception("Board settings are not valid");
        }

        isInitialized = false;

        InitializeTiles();
        if (!generateBombsAfterFirstClick)
        {
            PlaceBombs();
            NotifyNeighboursAboutBombs();
            isInitialized = true;
        }

        isUncoveringTiles = false;
    }

    void InitializeTiles()
    {
        if (tiles != null)
        {
            // for now destroy all, later use pooling etc
            for (int row = 0; row < amountOfRows; ++row)
            {
                for (int column = 0; column < amountOfColumns; ++column)
                {
                    Tile tile = GetTile(row, column);
                    if(tile)
                    {
                        Destroy(tile.gameObject);
                    }
                }
            }
        }

        tiles = new Tile[amountOfRows, amountOfColumns];

        RectTransform tilePrefabRect = tilePrefab.GetComponent<RectTransform>();

        for (int row = 0; row < amountOfRows; ++row)
        {
            for(int column = 0; column < amountOfColumns; ++column)
            {
                Tile tile = Instantiate(tilePrefab);
                tile.transform.SetParent(transform, false);

                SetTilePositionInWorld(tile, row, column);

                tile.Init(this, row, column);
                tiles[row, column] = tile;
            }
        }

        // Set the size of this container to desired size of all its childern so it will be centered
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, amountOfColumns * tilePrefabRect.rect.width);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, amountOfRows * tilePrefabRect.rect.height);
    }

    void SetTilePositionInWorld(Tile tile, int row, int column)
    {
        RectTransform tilePrefabRect = tilePrefab.GetComponent<RectTransform>();
        tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(column * tilePrefabRect.rect.height, row * tilePrefabRect.rect.width);

        tile.name = "[Tile] Row: " + row + " - Column: " + column;
    }

    void PlaceBombs(int clickRow = 0, int clickColumn = 0)
    {
        int bombsLeftToPlace = amountOfBombs;

        while (bombsLeftToPlace > 0)
        {
            int row = Random.Range(0, amountOfRows);
            int column = Random.Range(0, amountOfColumns);

            if (generateBombsAfterFirstClick)
            {
                if(spaceBetweenBombsAndFirstClick == 0)
                {
                    if(row == clickRow && column == clickColumn)
                    {
                        continue;
                    }
                }
                else if (Mathf.Abs(row - clickRow) <= spaceBetweenBombsAndFirstClick &&
                    Mathf.Abs(column - clickColumn) <= spaceBetweenBombsAndFirstClick)
                {
                    continue;
                }
            }

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
        for (int row = 0; row < amountOfRows; ++row)
        {
            for (int column = 0; column < amountOfColumns; ++column)
            {
                Tile tile = GetTile(row, column);
                if (tile != null && tile.isBomb)
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

                Tile tile = GetTile(row, column);
                if (tile != null)
                {
                    ++tile.neighbourBombCount;
                }
            }
        }
    }

    public void OnUncoverRequested(int row, int column)
    {
        if(!GameManager.instance.isPlaying || isUncoveringTiles)
        {
            return;
        }

        if(!isInitialized && generateBombsAfterFirstClick)
        {
            PlaceBombs(row, column);
            NotifyNeighboursAboutBombs();
            isInitialized = true;
        }

        Tile tile = tiles[row, column];
        if(tile.isBomb)
        {
            UncoverAllBombs();
            GameManager.instance.GameLost();
        }
        else
        {
            StartCoroutine(BeginUncoverFloodFill(row, column));
        }
    }

    IEnumerator BeginUncoverFloodFill(int row, int column)
    {
        isUncoveringTiles = true;

        yield return StartCoroutine(UncoverFloodFillIterative(row, column));

        isUncoveringTiles = false;
    }

    /* unsued 
    IEnumerator UncoverFloodFill(int row, int column)
    {
        if (row < 0 || column < 0 ||
            row >= numberOfRows || column >= numberOfColumns)
        {
            yield break;
        }

        Tile tile = tiles[row, column];
        if(tile.state == Tile.State.Uncovered)
        {
            yield break;
        }

        tile.Uncover();
        if (tile.neighbourBombCount > 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(floodFillUncoverTickTime);

        yield return StartCoroutine(UncoverFloodFill(row - 1, column));
        yield return StartCoroutine(UncoverFloodFill(row + 1, column));
        yield return StartCoroutine(UncoverFloodFill(row, column - 1));
        yield return StartCoroutine(UncoverFloodFill(row, column + 1));
    }
    */

    IEnumerator UncoverFloodFillIterative(int row, int column)
    {
        if (row < 0 || column < 0 ||
            row >= amountOfRows || column >= amountOfColumns)
        {
            yield break;
        }

        Queue<Queue<Tile>> tilesToUncover = new Queue<Queue<Tile>>();

        Tile firstTile = tiles[row, column];
        if (firstTile.state == Tile.State.Uncovered)
        {
            yield break;
        }

        Queue<Tile> queue = new Queue<Tile>();
        queue.Enqueue(firstTile);
        tilesToUncover.Enqueue(queue);

        while(tilesToUncover.Count > 0)
        {
            Queue<Tile> dequeued = tilesToUncover.Dequeue();

            queue = new Queue<Tile>();

            while (dequeued.Count > 0)
            {
                Tile tile = dequeued.Dequeue();
                if (tile.state == Tile.State.Uncovered)
                {
                    continue;
                }

                tile.Uncover();
                if (tile.neighbourBombCount > 0)
                {
                    continue;
                }

                row = tile.row;
                column = tile.column;


                tile = GetTile(row - 1, column);
                if (tile)
                {
                    queue.Enqueue(tile);
                }

                tile = GetTile(row + 1, column);
                if (tile)
                {
                    queue.Enqueue(tile);
                }

                tile = GetTile(row, column - 1);
                if (tile)
                {
                    queue.Enqueue(tile);
                }

                tile = GetTile(row, column + 1);
                if (tile)
                {
                    queue.Enqueue(tile);
                }

            }

            if (queue.Count > 0)
            {
                tilesToUncover.Enqueue(queue);
            }

            yield return new WaitForSecondsRealtime(floodFillUncoverTickTime);
        }

        yield return null;
    }

    /* Checked tile getter
     * Returns tile at row and column or nullptr if out of bounds */
    Tile GetTile(int row, int column)
    {
        if (row < 0 || column < 0 ||
                  row >= amountOfRows || column >= amountOfColumns)
        {
            return null;
        }

        return tiles[row, column];
    }

    void UncoverAll()
    {
        for (int row = 0; row < amountOfRows; ++row)
        {
            for (int column = 0; column < amountOfColumns; ++column)
            {
                Tile tile = tiles[row, column];
                if(tile != null)
                {
                    tile.Uncover();
                }
            }
        }
    }

    void UncoverAllBombs()
    {
        for (int row = 0; row < amountOfRows; ++row)
        {
            for (int column = 0; column < amountOfColumns; ++column)
            {
                Tile tile = tiles[row, column];
                if (tile != null && tile.isBomb)
                {
                    tile.Uncover();
                }
            }
        }
    }
}
