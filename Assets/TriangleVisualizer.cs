using System;
using UnityEngine;

public class TriangleVisualizer : MonoBehaviour
{

    [SerializeField] private Material EmptyMaterial; 
    [SerializeField] private Material ActiveMaterial; 
    [SerializeField] private Material InactiveMaterial;
    [SerializeField] private MeshRenderer _meshRenderer;

    public SubTile SubTile { get; private set; }
    public TileCoordinate Coordinate { get; private set; } //TODO: duplication of state!!

    public void Init(SubTile subTile, TileCoordinate coordinate)
    {
        SubTile = subTile;
        Coordinate = coordinate;
    }

    public void UpdateState()
    {
        switch (SubTile.State)
        {
            case TileState.Active:
                _meshRenderer.material = ActiveMaterial;
                break;
            case TileState.Inactive:
                _meshRenderer.material = InactiveMaterial;
                break;
            case TileState.Empty:
                _meshRenderer.material = EmptyMaterial;
                break;
        }
    }


}
