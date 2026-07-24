using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board
{
    public int GridSize { get; private set; }
    public Dictionary<TileCoordinate, Tile> _tiles { get; private set; } = new();

    private TileCoordinate _emptyStartTile = new TileCoordinate(1, 1);
    public Board(int gridSize)
    {
        this.GridSize = gridSize;
        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize ; y++)
            {
                var pos = new TileCoordinate(x, y);
                _tiles.Add(pos, new Tile(pos));
            }
        }
    }

    public void GenerateState()
    {
        foreach (var tilesValue in _tiles.Values)
        {
            tilesValue.RandomizeTile();
        }

        var emptyTile = _tiles[_emptyStartTile];
        emptyTile.Empty = true;

        foreach (var subTile in _tiles[_emptyStartTile].SubTiles)
        {
            subTile.State = TileState.Empty;
        }
    }

    public bool InBounds(TileCoordinate coordinate)
    {
        return coordinate.X >= 0 && coordinate.X < GridSize && coordinate.Y >= 0 &&
               coordinate.Y < GridSize;
    }

    public void SwapTiles(TileCoordinate coordinateOne, TileCoordinate coordinateTwo)
    {
        for (int i = 0; i < 8; i++)
        {
            (_tiles[coordinateOne].SubTiles[i].State, _tiles[coordinateTwo].SubTiles[i].State) = 
                (_tiles[coordinateTwo].SubTiles[i].State, _tiles[coordinateOne].SubTiles[i].State);
        }

        (_tiles[coordinateOne].Empty, _tiles[coordinateTwo].Empty) = (_tiles[coordinateTwo].Empty, _tiles[coordinateOne].Empty);
    }

    public void RotateTileLeft(TileCoordinate coordinate)
    {
        var tile = _tiles[coordinate];
        var stateCopy = new TileState[8];

        for (int i = 0; i < 8; i++)
        {
            stateCopy[i] = tile.SubTiles[(i + 2) % 8].State;
        }

        for (int i = 0; i < 8; i++)
        {
            tile.SubTiles[i].State = stateCopy[i];
        }
    }

    public void RotateTileRight(TileCoordinate coordinate)
    {
        var tile = _tiles[coordinate];
        var stateCopy = new TileState[8];

        for (int i = 0; i < 8; i++)
        {
            stateCopy[i] = tile.SubTiles[(i + 6) % 8].State;
        }

        for (int i = 0; i < 8; i++)
        {
            tile.SubTiles[i].State = stateCopy[i];
        }
    }

    public bool InnerEdge(TileCoordinate coordinate, int innerEdgeIndex)
    {
        var a = (_tiles[coordinate].SubTiles[innerEdgeIndex].State == TileState.Active && _tiles[coordinate].SubTiles[(innerEdgeIndex + 8) % 8].State != TileState.Active);
        var b = ((_tiles[coordinate].SubTiles[innerEdgeIndex].State != TileState.Active) && (_tiles[coordinate].SubTiles[(innerEdgeIndex + 8) % 8].State == TileState.Active));

        return a || b;
    }

    public bool OuterEdge(TileCoordinate coordinate, int outEdgeIndex)
    {

        switch (outEdgeIndex)
        {
            case 0 or 1:
                if (!InBounds(coordinate.Down()) && _tiles[coordinate].SubTiles[outEdgeIndex].State == TileState.Active)
                {
                    return true;
                }
                return OuterEdgeFuc(coordinate, coordinate.Down(), outEdgeIndex);
            case 2 or 3:
                if (!InBounds(coordinate.Right()) && _tiles[coordinate].SubTiles[outEdgeIndex].State == TileState.Active)
                {
                    return true;
                }
                return OuterEdgeFuc(coordinate, coordinate.Right(), outEdgeIndex);
            case 4 or 5:
                if (!InBounds(coordinate.Up()) && _tiles[coordinate].SubTiles[outEdgeIndex].State == TileState.Active)
                {
                    return true;
                }
                return OuterEdgeFuc(coordinate, coordinate.Up(), outEdgeIndex);
            case 6 or 7:
                if (!InBounds(coordinate.Left()) && _tiles[coordinate].SubTiles[outEdgeIndex].State == TileState.Active)
                {
                    return true;
                }
                return OuterEdgeFuc(coordinate, coordinate.Left(), outEdgeIndex);
        }
        Debug.LogError("OutOfBounds - what happend?");
        return false;
    }

    private bool OuterEdgeFuc(TileCoordinate innerTile, TileCoordinate outerTile, int outEdgeIndex)
    {
        var a = _tiles[innerTile].SubTiles[outEdgeIndex].State == TileState.Active &&
                _tiles[outerTile].SubTiles[5 ^ outEdgeIndex].State != TileState.Active;
        var b = _tiles[innerTile].SubTiles[outEdgeIndex].State == TileState.Active &&
                _tiles[outerTile].SubTiles[5 ^ outEdgeIndex].State != TileState.Active;

        return a || b;
    }


}

public class Tile
{
    public TileCoordinate Coordinate { get; private set; }
    public SubTile[] SubTiles;
    public bool Empty = false;

    public Tile(TileCoordinate coordinate)
    {
        Coordinate = coordinate;
        SubTiles = new SubTile[8];

        for (int i = 0; i < 8; i++)
        {
            SubTiles[i] = new SubTile(i);
        }
    }
}

public class SubTile
{
    public TileState State { get; set; }
    public int PositionInTile { get; private set; }

    public SubTile(int position, TileState state = TileState.Empty)
    {
        PositionInTile = position;
        State = state;
    }
}

public struct TileCoordinate
{
    public int X;
    public int Y;
    public TileCoordinate(int x, int y)
    {
        X = x;
        Y = y;
    }   
}

public static class TileCoordinateExtensions
{
    public static TileCoordinate Left(this TileCoordinate origin)
    {
        return new TileCoordinate(origin.X - 1, origin.Y);
    }
    public static TileCoordinate Right(this TileCoordinate origin)
    {
        return new TileCoordinate(origin.X + 1, origin.Y);
    }
    public static TileCoordinate Up(this TileCoordinate origin)
    {
        return new TileCoordinate(origin.X, origin.Y + 1);
    }
    public static TileCoordinate Down(this TileCoordinate origin)
    {
        return new TileCoordinate(origin.X, origin.Y - 1);
    }

}

public static class TileExtensions
{
    public static void RandomizeTile(this Tile tile)
    {
        foreach (var tileSubTile in tile.SubTiles)
        {
            if (Random.value > .5f)
            {
                tileSubTile.State = TileState.Active;
            }
            else
            {
                tileSubTile.State = TileState.Inactive;
            }
        }
    }
} 

//public static class SubTileCoordinateExtensions()
//{
    
//}

public enum TileState
{
    Empty, Active, Inactive
}

public class BoardValidator
{

    private Board _board;
    public Dictionary<TileCoordinate, bool> dirtyTiles { get; private set; } = new();


    private static readonly List<int> LargeRocketRight = new() { 7, 0, 1 };
    private static readonly List<int> LargeRocketLeft = new() { 4, 3, 2 };
    private static readonly List<int> LargeRocketBottomLeft = new() { 5, 4 };
    private static readonly List<int> LargeRocketBottomRight = new() { 7, 6 };

    private static readonly List<int> LargeRocketCenterTop = new List<int>() { 0, 1 };
    private static readonly List<int> LargeRocketCenterBottom = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

    private static readonly List<int> LargeRocketRightTopInnerEdges = new List<int>() { 6, 2 };
    private static readonly List<int> LargeRocketRightBottomInnerEdges = new List<int>() { 5, 7 };
    private static readonly List<int> LargeRocketLeftBottomInnerEdges = new List<int>() { 3, 5 };
    private static readonly List<int> LargeRocketLeftTopInnerEdges = new List<int>() { 1, 4 };

    private static readonly List<int> LargeRocketCenterTopInnerEdges = new List<int>() { 0, 2 };
    private static readonly List<int> LargeRocketCenterBottomOuterEdges = new List<int> { 3, 2, 1, 0, 7, 6 };

    private static readonly List<int> SmallRocketRight = new() { 6, 7, 0};
    private static readonly List<int> SmallRocketLeft = new() { 1, 2, 3};

    private static readonly List<int> SmallRocketCenterTop = new List<int>() { 0, 1 };
    private static readonly List<int> SmallRocketCenterBottom = new List<int>() { 3, 4, 5, 6 };

    private static readonly List<int> SmallRocketRightInnerEdges = new List<int>() { 6, 2 };
    private static readonly List<int> SmallRocketLeftInnerEdges = new List<int>() { 1, 4 };

    private static readonly List<int> SmallRocketRightOuterEdges = new List<int>() { 0 };
    private static readonly List<int> SmallRocketLeftOuterEdges = new List<int>() { 1 };

    private static readonly List<int> SmallRocketCenterTopInnerEdges = new List<int>() { 0, 2 };
    private static readonly List<int> SmallRocketCenterBottomInnerEdges = new List<int> { 7, 3 };

    private static readonly List<int> SmallRocketCenterBottomOuterEdges = new List<int> { 3, 6};

    //TODO: ADD SHIELDS AND TIMERS

    public BoardValidator(Board board)
    {
        _board = board;
    }

    public List<Structure> ValidateBoard()
    {
        var list = new List<Structure>();

        var largeRockets = FindLargeRocketsFill();
        for (int i = 0; i < largeRockets; i++ )
        {
            list.Add(Effect.LargeRocket);
        }

        var smalRockets = FindSmallRocketsFill();
        for (int i = 0; i < largeRockets; i++ )
        {
            list.Add(Effect.SmallRocket);
        }

        return list;
    }

    public bool OuterEdges(TileCoordinate coordinate, List<int> outerEdges)
    {
        foreach (var outerEdge in outerEdges)
        {
            if (!_board.OuterEdge(coordinate, outerEdge))
            {
                return false;
            }
        }

        return true;
    }

    public bool InnerEdges(TileCoordinate coordinate, List<int> outerEdges)
    {
        foreach (var outerEdge in outerEdges)
        {
            if (!_board.InnerEdge(coordinate, outerEdge))
            {
                return false;
            }
        }

        return true;
    }
    public bool SubTilesActive(TileCoordinate coordinate, List<int> subTilesIndecies)
    {
        if (!_board.InBounds(coordinate))
        {
            return false;
        }

        bool all = true;

        foreach (var subTileIndex in subTilesIndecies)
        {
            if (_board._tiles[coordinate].SubTiles[subTileIndex].State != TileState.Active)
            {
                all = false;
                break;
            }
        }

        return all;
    }
    public List<SubTile> SetInactive(TileCoordinate coordinate, List<int> subTileIndexes)
    {
        var list = new List<SubTile>();

        foreach (var subTileIndex in subTileIndexes)
        {
            _board._tiles[coordinate].SubTiles[subTileIndex].State = TileState.Inactive;
            list.Add(_board._tiles[coordinate].SubTiles[subTileIndex]);
        }

        return list;
    }

    //######LARGE ROCKETS#############

    public List<Structure> FindLargeRocketsFill()
    {
        var structuresCompleted = new List<Structure>();

        foreach (var coordinate in _board._tiles.Keys)
        {
            if (!SubTilesActive(coordinate, LargeRocketRight))
            {
                continue;
            }

            if (!SubTilesActive(coordinate.Left(), LargeRocketLeft))
            {
                continue;
            }

            if (!SubTilesActive(coordinate.Down(), LargeRocketBottomRight))
            {
                continue;
            }

            if (!SubTilesActive(coordinate.Down().Left(), LargeRocketBottomLeft))
            {
                continue;
            }

            var rocketPieces = new List<SubTile>();

            rocketPieces.AddRange(SetInactive(coordinate, LargeRocketRight));
            rocketPieces.AddRange(SetInactive(coordinate.Left(), LargeRocketLeft));
            rocketPieces.AddRange(SetInactive(coordinate.Down(), LargeRocketBottomRight));
            rocketPieces.AddRange(SetInactive(coordinate.Down().Left(), LargeRocketBottomLeft));

            structuresCompleted.Add(new Structure(rocketPieces, Effect.LargeRocket));
        }

        return structuresCompleted;
    }


    //public int FindLargeRocketEdge()
    //{
    //    int rockets = 0;

    //    foreach (var coordinate in _board._tiles.Keys)
    //    {
    //        if (_board._tiles[coordinate].SubTiles[0].State == TileState.Active)
    //        {
    //            if (!InnerEdges(coordinate, LargeRocketCenterTopInnerEdges))
    //                continue;
    //            if (!OuterEdges(coordinate.Down(), LargeRocketCenterBottomOuterEdges))
    //                continue;

    //            rockets++;

    //            SetInactive(coordinate, LargeRocketCenterTop);
    //            SetInactive(coordinate, LargeRocketCenterBottom);

    //            dirtyTiles.Add(coordinate, true);
    //            dirtyTiles.Add(coordinate.Down(), true);
    //        }
    //    }

    //    foreach (var coordinate in _board._tiles.Keys)
    //    {
    //        if (_board._tiles[coordinate].SubTiles[7].State == TileState.Active)
    //        {
    //            if (!InnerEdges(coordinate, LargeRocketRightTopInnerEdges))
    //                continue;
    //            if (!InnerEdges(coordinate.Down(), LargeRocketRightBottomInnerEdges))
    //                continue;
    //            if (!InnerEdges(coordinate.Down().Left(), LargeRocketLeftBottomInnerEdges))
    //                continue;
    //            if (!InnerEdges(coordinate.Left(), LargeRocketLeftTopInnerEdges))
    //                continue;

    //            rockets++;
    //            SetInactive(coordinate, LargeRocketRight);
    //            SetInactive(coordinate.Left(), LargeRocketLeft);
    //            SetInactive(coordinate.Down(), LargeRocketBottomRight);
    //            SetInactive(coordinate.Down().Left(), LargeRocketBottomLeft);

    //            dirtyTiles.Add(coordinate, true);
    //            dirtyTiles.Add(coordinate.Left(), true);
    //            dirtyTiles.Add(coordinate.Down(), true);
    //            dirtyTiles.Add(coordinate.Down().Left(), true);
    //        }
    //    }

    //    return rockets;
    //}

    //############SMAL ROCKETS################

    public List<Structure> FindSmallRocketsFill()
    {
        var structuresCompleted = new List<Structure>();

        foreach (var coordinate in _board._tiles.Keys)
        {
            if (!SubTilesActive(coordinate, SmallRocketRight))
            {
                continue;
            }

            if (!SubTilesActive(coordinate.Left(), SmallRocketLeft))
            {
                continue;
            }

            var rocketPieces = new List<SubTile>();

            rocketPieces.AddRange( SetInactive(coordinate, SmallRocketRight));
            rocketPieces.AddRange(SetInactive(coordinate.Left(), SmallRocketLeft));
            structuresCompleted.Add(new Structure(rocketPieces, Effect.SmallRocket));
        }
        
        foreach (var coordinate in _board._tiles.Keys)
        {
            if (!SubTilesActive(coordinate, SmallRocketCenterTop))
            {
                continue;
            }

            if (!SubTilesActive(coordinate.Down(), SmallRocketCenterBottom))
            {
                continue;
            }

            var rocketPieces = new List<SubTile>();

            rocketPieces.AddRange( SetInactive(coordinate, SmallRocketCenterTop));
            rocketPieces.AddRange(SetInactive(coordinate.Down(), SmallRocketCenterBottom));

            structuresCompleted.Add(new Structure(rocketPieces, Effect.SmallRocket));
        }

        return structuresCompleted;
    }

    //public int FindSmallRocketEdge()
    //{
    //    int rockets = 0;

    //    foreach (var coordinate in _board._tiles.Keys)
    //    {
    //        if (_board._tiles[coordinate].SubTiles[0].State == TileState.Active)
    //        {
    //            if (!InnerEdges(coordinate, SmallRocketCenterTopInnerEdges))
    //                continue;
    //            if (!InnerEdges(coordinate, SmallRocketCenterBottomInnerEdges))
    //                continue;
    //            if (!OuterEdges(coordinate.Down(), SmallRocketCenterBottomOuterEdges))
    //                continue;

    //            rockets++;

    //            SetInactive(coordinate, SmallRocketCenterTop);
    //            SetInactive(coordinate, SmallRocketCenterBottom);

    //            dirtyTiles.Add(coordinate, true);
    //            dirtyTiles.Add(coordinate.Down(), true);
    //        }
    //    }

    //    foreach (var coordinate in _board._tiles.Keys)
    //    {
    //        if (_board._tiles[coordinate].SubTiles[7].State == TileState.Active)
    //        {
    //            if (!InnerEdges(coordinate, SmallRocketRightInnerEdges))
    //                continue;
    //            if (!InnerEdges(coordinate, SmallRocketRightOuterEdges))
    //                continue;
    //            if (!InnerEdges(coordinate.Left(), SmallRocketLeftInnerEdges))
    //                continue;
    //            if (!InnerEdges(coordinate.Left(), SmallRocketLeftOuterEdges))
    //                continue;

    //            rockets++;
    //            SetInactive(coordinate, SmallRocketRight);
    //            SetInactive(coordinate.Left(), SmallRocketLeft);

    //            dirtyTiles.Add(coordinate, true);
    //            dirtyTiles.Add(coordinate.Left(), true);
    //        }
    //    }

    //    return rockets;
    //}

}

public class Structure
{
    public Structure(List<SubTile> subTiles, Effect effect)
    {
        _effect = effect;
        _subTiles = subTiles;
    }

    private List<SubTile> _subTiles;
    private Effect _effect;
}

public enum Effect
{
    SmallRocket, LargeRocket, Time, Shield
}

public struct SubTilePosition
{
    public TileCoordinate Coordinate;
    public int PositionInTile;
}
