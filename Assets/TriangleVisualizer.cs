using System;
using UnityEngine;

public class TriangleVisualizer : MonoBehaviour
{

    [SerializeField] private Material EmptyMaterial; 
    [SerializeField] private Material ActiveMaterial; 
    [SerializeField] private Material InactiveMaterial;
    [SerializeField] private MeshRenderer _meshRenderer;

    private SubTile _subTile;
    public TileCoordinate Coordinate { get; private set; } //TODO: duplication of state!!

    public void Init(SubTile subTile, TileCoordinate coordinate)
    {
        _subTile = subTile;
        Coordinate = coordinate;
    }

    public void UpdateState()
    {
        switch (_subTile.State)
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
