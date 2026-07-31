using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileBag
{
    private Queue<TileTemplate> _tileBag = new Queue<TileTemplate>(); 

    public TileTemplate Draw()
    {
        if (_tileBag.Count == 0)
        {
            _tileBag = FillBag();
        }

        return _tileBag.Dequeue();
    }

    private static Queue<TileTemplate> FillBag()
    {
        var tileTemplates = new TileTemplate[8];

        tileTemplates[0] = TileTemplate.peakR();
        tileTemplates[1] = TileTemplate.Hat();
        tileTemplates[2] = TileTemplate.Hat();
        tileTemplates[3] = TileTemplate.Block();
        tileTemplates[4] = TileTemplate.Block();
        tileTemplates[5] = TileTemplate.peakL();
        tileTemplates[6] = TileTemplate.peakL();
        tileTemplates[7] = TileTemplate.peakR();

        tileTemplates = tileTemplates.OrderBy(x => Random.value).ToArray();
        return new Queue<TileTemplate>(tileTemplates);
    }


}

public class TileTemplate
{
    public SubTile[] SubTiles { get; private set; } = new SubTile[8];

    public TileTemplate()
    {
        for (int i = 0; i < 8; i++)
        {
            SubTiles[i] = new SubTile(i);
        }
    }


    public static TileTemplate Hat()
    {
        var template = new TileTemplate();
        
        template.SubTileInactive();
    
        template.SubTiles[0].State = TileState.Active;
        template.SubTiles[1].State = TileState.Active;

        template.RandomRotation();

        return template;
    }

    public static TileTemplate Block()
    {
        var template = new TileTemplate();
        template.SubTileInactive();


        template.SubTiles[0].State = TileState.Active;
        template.SubTiles[1].State = TileState.Active;
        template.SubTiles[7].State = TileState.Active;
        template.SubTiles[2].State = TileState.Active;

        template.RandomRotation();

        return template;
    }

    public static TileTemplate peakL()
    {
        var template = new TileTemplate();
        template.SubTileInactive();


        template.SubTiles[0].State = TileState.Active;
        template.SubTiles[7].State = TileState.Active;
        template.SubTiles[6].State = TileState.Active;

        template.RandomRotation();

        return template;
    }
    public static TileTemplate peakR()
    {
        var template = new TileTemplate();
        template.SubTileInactive();

        template.SubTiles[3].State = TileState.Active;
        template.SubTiles[2].State = TileState.Active;
        template.SubTiles[1].State = TileState.Active;

        template.RandomRotation();

        return template;
    }

    private void RandomRotation()
    {
        var rot = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        var rotated = new SubTile[8];
        for (int i = 0; i < 8; i++)
        {
            rotated[i] = SubTiles[(i + 2) % 8];
        }
        
        SubTiles = rotated;
    }
}

public static class TileTemplateExtensions
{

    public static void SubTileInactive(this TileTemplate template)
    {
        foreach (var templateSubTile in template.SubTiles)
        {
            templateSubTile.State = TileState.Inactive;
        }
    }

}
