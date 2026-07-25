using System.Collections.Generic;
using TMPro;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardVisualizerMono : MonoBehaviour
{
    [SerializeField] private TriangleVisualizer trianglePrefab;
    [SerializeField] private int BoardSize = 3;
    [SerializeField] private float tilePositionOffset;
    [SerializeField] private PopulationCounter populationCounter;


    private Camera _camera;

    private Board _board;
    private BoardValidator _boardValidator;
    private List<TriangleVisualizer> _tiangles = new();

    void Start()
    {
        _camera = Camera.main;
        _board = new Board(BoardSize);
        _boardValidator = new BoardValidator(_board);
        foreach (var coordinate in _board._tiles.Keys)
        {
           var tileGo = GenerateTile(_board._tiles[coordinate]);
           tileGo.transform.position = new Vector3(coordinate.X, coordinate.Y, 0) * tilePositionOffset;
        }
        _board.GenerateState();
        UpdateVisualState();
        CenterBoard();
    }

    private void CenterBoard()
    {
        this.gameObject.transform.localScale = Vector3.one * .5f;
        this.gameObject.transform.position = new Vector3(-1, -4, -90);
    }

    public void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<TriangleVisualizer>(out var tileComp))
                {
                    if (TileCanMove(tileComp.Coordinate, out var swapCoordinate))
                    {
                        if (populationCounter.PayToMove())
                        {
                            _board.SwapTiles(tileComp.Coordinate, swapCoordinate);
                            UpdateVisualState();
                            DoScoring();
                        }
                    }
                }
            }
        }

        if (Mouse.current != null && Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<TriangleVisualizer>(out var tileComp))
                {
                    if (populationCounter.CanPayToRotate())
                    {
                        RotateTileRightClockWise(tileComp.Coordinate);
                        UpdateVisualState();
                        populationCounter.PayToRotate();
                        //if (RotateSubTileRightClockWise(tileComp.Coordinate, tileComp.SubTile.PositionInTile))
                        //{
                        //    populationCounter.PayToRotate();
                        //    UpdateVisualState();

                        //}
                        DoScoring();
                    }
                }
            }
        }

        if (Mouse.current != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<TriangleVisualizer>(out var tileComp))
                {
                    if (populationCounter.CanPayToRotate())
                    {
                        RotateTileLeftCounterClockWise(tileComp.Coordinate);
                        populationCounter.PayToRotate();
                        UpdateVisualState();
                        //if (RotateSubTileLeftCounterClockWise(tileComp.Coordinate, tileComp.SubTile.PositionInTile))
                        //{
                        //    populationCounter.PayToRotate();
                        //    UpdateVisualState();

                        //}
                        
                        DoScoring();
                    }
                }
            }
        }

        if (Mouse.current != null && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (populationCounter.PayToRandomizeBoard())
            {
                _board.GenerateState();
                UpdateVisualState();
                DoScoring();
            }
        }
    }

    public void DoScoring()
    {
        var list = _boardValidator.ValidateBoard();
        if (list.Count == 0)
            return;

        UpdateVisualState();

        foreach (var structure in list)
        {
            populationCounter.SavePeople(structure._effect);
        }
    }

    public void RotateTileRightClockWise(TileCoordinate coordinate)
    {
        _board.RotateTileRight(coordinate);
        UpdateVisualState();
    }
    public void RotateTileLeftCounterClockWise(TileCoordinate coordinate)
    {
        _board.RotateTileLeft(coordinate);
        UpdateVisualState();
    }

    public bool RotateSubTileLeftCounterClockWise(TileCoordinate coordinate, int index)
    {
        if (_board.RotateSubTileLeftCounterClockwise(coordinate, index))
        {
            UpdateVisualState();
            return true;
        }

        return false;
    }
    public bool RotateSubTileRightClockWise(TileCoordinate coordinate, int index)
    {
        if (_board.RotateSubTileRightClockwise(coordinate, index))
        {
            UpdateVisualState();
            return true;
        }
        return false;
    }

public bool TileCanMove(TileCoordinate coordinate, out TileCoordinate swapCoordinate)
{
    if (_board._tiles[coordinate].Empty)
    {
        swapCoordinate = new TileCoordinate(-10, -10);
        return false;
    }

    if (_board.InBounds(coordinate.Up()) && _board._tiles[coordinate.Up()].Empty)
    {
        swapCoordinate = coordinate.Up();
        return true;
    }

    if (_board.InBounds(coordinate.Down()) && _board._tiles[coordinate.Down()].Empty)
    {
        swapCoordinate = coordinate.Down();
        return true;
    }

    if (_board.InBounds(coordinate.Left()) && _board._tiles[coordinate.Left()].Empty)
    {
        swapCoordinate = coordinate.Left();
        return true;
    }

    if (_board.InBounds(coordinate.Right()) && _board._tiles[coordinate.Right()].Empty)
    {
        swapCoordinate = coordinate.Right();
        return true;
    }

    swapCoordinate = new TileCoordinate(-10, -10);
    return false;
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
            int rotIndex = i;
            if (i == 3) rotIndex = 7;
            else if (i == 7) rotIndex = 3;

            var subTileVisual = Instantiate(trianglePrefab, tileGameObject.transform);
            subTileVisual.Init(tile.SubTiles[i], tile.Coordinate); //WIWOO TODO: Duplication of state

            if (rotIndex % 2 == 0)
            {
                subTileVisual.transform.Rotate(Vector3.forward, 45 * rotIndex);
            }
            else
            {
                subTileVisual.transform.Rotate(Vector3.up, 180);
                subTileVisual.transform.Rotate(Vector3.forward, 45 * (rotIndex - 1));
            }


            _tiangles.Add(subTileVisual);
        }

        return tileGameObject;
    }

}
