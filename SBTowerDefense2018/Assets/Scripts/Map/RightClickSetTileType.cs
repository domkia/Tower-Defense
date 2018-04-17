using System.Collections.Generic;
using UnityEngine;

public class RightClickSetTileType : MonoBehaviour
{
    public buildAtPos build;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TileVisual tv = build.GetTileAtClick();
            if (tv != null)
                if (tv.tile.enemies.Count == 0)
                    tv.ChangeTileType(TileType.Wood);
        }

    }
}
