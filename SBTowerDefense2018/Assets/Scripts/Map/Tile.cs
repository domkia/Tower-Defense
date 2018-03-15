using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    //More info about axial coordinates system: https://www.redblobgames.com/grids/hexagons/#coordinates-axial
    public float x { get; set; } //x coordinate on axial grid
    public float y { get; set; } //y coordinate on axial grid
    public bool canBuild { get; set; } //State if towers can be bult on that tile
    public Vector3 worldPos { get; set; } //Position in world

    public Tile(float xCord, float yCord, Vector3 worldPos)
    {
        this.x = xCord;
        this.y = yCord;
        bool canBuild = false;
        this.worldPos = worldPos;
    }
    
}
