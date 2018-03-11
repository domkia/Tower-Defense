using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public float x { get; set; } //x coordinates on the world
    public float y { get; set; } //y coordinates on the world
    public string type { get; set; } //Type of tile
    public bool canBuild { get; set; } //State if towers can be bult on that tile

    public Tile(float xCord, float yCord, char tileType)
    {
        this.x = xCord;
        this.y = yCord;
        string tilType = null;
        bool canBuild = false;
        choose(tileType, ref canBuild, ref tilType);
        this.type = tilType;
        string text = string.Format("{0};{1} type: {2}", x, y, tilType);
        //Debug.Log(text); //Debug to see coordinates on map and type of that tile

    }
    /// <summary>
    /// Choosing string type and chaning canBuild state
    /// </summary>
    /// <param name="type"> character that determines type</param>
    /// <param name="cBuild"> boolean on state if towers can be build on this tile</param>
    /// <param name="tileType"> tile type string</param>
    void choose(char type, ref bool cBuild, ref string tileType)
    {
        switch (type)
        {
            case 'B': //Tile on which you can build towers
                {
                    tileType = "BuildTile";
                    cBuild = true;
                    break;
                }
            case 'P': //Path tile
                {
                    tileType = "Path";
                    cBuild = false;
                    break;
                }
            case 'S': //Start tile
                {
                    tileType = "Start";
                    cBuild = false;
                    break;
                }
            case 'E': //End tile
                {
                    tileType = "End";
                    cBuild = false;
                    break;
                }
            default:
                {
                    tileType = "NonBuild";
                    cBuild = false;
                    break;
                }
        }
    }
}
