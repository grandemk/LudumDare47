using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileEnv : ScriptableObject
{
    public bool traversable;
    public bool controlPoint;
};

public class TileControl : MonoBehaviour 
{
    public TileBase[] tiles;
    public TileEnv[] tileEnvs;
    private Dictionary<TileBase, TileEnv> tilesData = new Dictionary<TileBase, TileEnv>();

    private void Start()
    {
        for(var i = 0; i < tiles.Length; ++i)
        {
            tilesData[tiles[i]] = tileEnvs[i];
        }

    }

    public bool IsControlPoint(TileBase tile)
    {
        if (tile == null)
            return false;

        TileEnv tileEnv;
        if(tilesData.TryGetValue(tile, out tileEnv))
            return tileEnv.controlPoint;
        return false;

    }

    public bool IsTraversable(TileBase tile)
    {
        if (tile == null)
            return true;

        TileEnv tileEnv;
        if(tilesData.TryGetValue(tile, out tileEnv))
            return tileEnv.traversable;
        return false;
    }
}
