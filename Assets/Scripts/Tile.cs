using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum State
    {
        Normal,
        Uncovered,
        Flagged
    };

    Board grid;

    public bool isBomb;
    public int neighbourBombCount;
    public State state { get; private set; }

    public int row { get; private set; }
    public int column { get; private set; }

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();

        ChangeState(State.Normal);
    }

    public void Init(Board grid, int row, int column)
    {
        neighbourBombCount = 0;
        this.grid = grid;

        this.row = row;
        this.column = column;

        ChangeState(State.Normal);
    }

    public void Move(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public void OnLeftClick()
    {
        grid.OnUncoverRequested(row, column);
    }

    public void SetFlag()
    {
        // todo

        if (state == State.Normal)
        {
            ChangeState(State.Flagged);
        }
        else if(state == State.Flagged)
        {
            ChangeState(State.Normal);
        }
    }

    public void Uncover()
    {
        ChangeState(State.Uncovered);
    }

    void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Normal:
                image.sprite = GameManager.instance.tileTheme.GetRandomCoveredTileSprite();
                break;

            case State.Flagged:
                image.sprite = GameManager.instance.tileTheme.FlagTileSprite;
                break;

            case State.Uncovered:
                if(isBomb)
                {
                    image.sprite = GameManager.instance.tileTheme.BombTileSprite;
                }
                else
                {
                    image.sprite = GameManager.instance.tileTheme.UncoveredTileSprites[neighbourBombCount];
                }
                break;
        }

        state = newState;
    }

    public bool IsUncovered()
    {
        return state == State.Uncovered;
    }
}
