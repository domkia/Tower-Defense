using UnityEngine;

// Right now, the only thing the game manager does is to push the resources into the PlayerStats instance.
public class GameManager : MonoBehaviour
{
    public LayerMask layer;
    public HexGrid grid;
    //private PlayerStats stats;

    // If anybody can figure out a better way to put resources and their amounts into the player stats,
    // feel free to improve on it.
    public Resource[] resources;

    //void Awake()
    //{
    //    stats = new PlayerStats();
    //}


    private void Awake()
    {
        PlayerStats.Instance.Resources = resources;
    }
    public TileVisual GetTileAtClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            TileVisual tile = hit.collider.GetComponent<TileVisual>();
            return tile;
        }
        return null;
    }
}
