using UnityEngine;

[CreateAssetMenu(fileName = "New Theme", menuName = "Tile Theme ")]
public class TileTheme : ScriptableObject
{
    public Sprite[] CoveredTileSprites;
    public Sprite[] UncoveredTileSprites;
    public Sprite BombTileSprite;
    public Sprite FlagTileSprite;

    public Sprite GetRandomCoveredTileSprite()
    {
        return CoveredTileSprites[Random.Range(0, CoveredTileSprites.Length)];
    }

    public Sprite GetRandomUncoveredTileSprite()
    {
        return UncoveredTileSprites[Random.Range(0, UncoveredTileSprites.Length)];
    }
}

