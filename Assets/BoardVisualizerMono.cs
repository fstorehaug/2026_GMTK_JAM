using System.Collections.Generic;
using UnityEngine;

public class BoardVisualizerMono : MonoBehaviour
{
    [SerializeField] private TriangleVisualizer trianglePrefab;
    [SerializeField] private int BoardSize = 3;
    [SerializeField] private float tilePositionOffset;

    private Board _board;
    private List<TriangleVisualizer> _tiangles = new();

    void Start()
    {
        _board = new Board(BoardSize);
        foreach (var Coordinate in _board._tiles.Keys)
        {
           var tileGo = GenerateTile(_board._tiles[Coordinate]);
           tileGo.transform.position = new Vector3(Coordinate.X, Coordinate.Y, 0) * tilePositionOffset;
        }
        _board.GernrateState();
        UpdateVisualState();
    }

    public void UpdateVisualState()
    {
        foreach (var vis in _tiangles)
        {
            vis.UpdateState();
        }
    }

    public GameObject GenerateTile(Tile tile)
    {
        var tileGameObject = new GameObject();
        tileGameObject.transform.SetParent(this.transform);
        for (int i = 0; i < 8; i++)
        {
            var subTileVisual = Instantiate(trianglePrefab, tileGameObject.transform);
            subTileVisual.Init(tile.SubTiles[i]);

            if (i % 2 == 0)
            {
                subTileVisual.transform.Rotate(Vector3.forward, 45*i);
            }

            if (i % 2 == 1)
            {
                subTileVisual.transform.Rotate(Vector3.up, 180);
                subTileVisual.transform.Rotate(Vector3.forward, 45* (i-1));
            }
            

            _tiangles.Add(subTileVisual);
        }

        return tileGameObject;
    }

}
