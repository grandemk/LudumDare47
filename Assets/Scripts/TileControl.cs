using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.UI;

[CreateAssetMenu]
public class TileEnv : ScriptableObject
{
    public bool traversable;
    public bool controlPoint;
};

public class TileFeature
{
    public bool isTriggered;
};

public class MapState
{
    public MapState()
    {
        numControlPoint = 0;
        numTriggeredControlPoint = 0;
    }
    public int numControlPoint;
    public int numTriggeredControlPoint;
};

public class TileControl : MonoBehaviour 
{
    public Tilemap tilemap;
    public TileBase[] tiles;
    public TileEnv[] tileEnvs;
    private Dictionary<TileBase, TileEnv> tilesData = new Dictionary<TileBase, TileEnv>();
    private Dictionary<Vector3Int, TileFeature> tilesFeature = new Dictionary<Vector3Int, TileFeature>();
    private MapState mapState = new MapState();
    public Text winText;

    public Tile triggeredControlPointTile;

    private void SetupTileMap()
    {
        mapState = new MapState();
        for(var i = 0; i < tiles.Length; ++i)
        {
            tilesData[tiles[i]] = tileEnvs[i];
        }

        foreach(var pos in tilemap.cellBounds.allPositionsWithin)
        {
            tilesFeature[pos] = new TileFeature();
            var tile = tilemap.GetTile(pos);
            if (tile == null)
                continue;
            var tileEnv = tilesData[tile];
            if(tileEnv.controlPoint)
            {
                tilesFeature[pos].isTriggered = false;
                ++mapState.numControlPoint;
            }
        }
    }

    private void Start()
    {
        SetupTileMap();
    }

    public TileEnv GetEnv(TileBase tile)
    {
        TileEnv tileEnv;
        if (tilesData.TryGetValue(tile, out tileEnv))
            return tileEnv;
        return null;
    }

    public TileFeature GetFeature(Vector3Int pos)
    {
        TileFeature tileFeature;
        if (tilesFeature.TryGetValue(pos, out tileFeature))
            return tileFeature;
        return null;
    }

    public void Trigger(Vector3Int pos)
    {
        var feature = GetFeature(pos);
        if (feature == null)
            return;
        if (feature.isTriggered == false)
        {
            ++mapState.numTriggeredControlPoint;
            if (mapState.numTriggeredControlPoint == mapState.numControlPoint)
                winText.gameObject.SetActive(true) ;
            tilemap.SetTile(pos, triggeredControlPointTile);
        }
        Debug.Log(String.Format("mapState: {0}, {1}", mapState.numTriggeredControlPoint, mapState.numControlPoint));
        feature.isTriggered = true;
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
