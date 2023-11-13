using HappyHourGames.Object_Pooling;
using UnityEngine;

namespace HappyHourGames.Managers.Board
{
    public interface BoardViewService
    {
        public Tile GetTile(Vector2Int pos);

    }
    public class BoardView : MonoBehaviour,BoardViewService
    {
        private Tile[,] _tileGameObjects;
        private IBoardService _boardService;
        
        public void Initialize(IBoardService boardService)
        {
            
            _boardService = boardService;
         
            CreateBoardView();
        }
        private void CreateBoardView()
        {
            int width = _boardService.Width;
            int height = _boardService.Height;

            _tileGameObjects = new Tile[width, height];

            if (_boardService.hole > 0)
            {
                var houseObject = PoolManager.Instance.GetPoolObject(PoolObjectType.House);
                houseObject.transform.position = new Vector3(_boardService.BoardCenter().x,_boardService.BoardCenter().y);
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Check if there is a tile at this position in the logical board
                    var tileData = _boardService.GetTileAt(new Vector2Int(x, y));

                    if (tileData != null)
                    {
                        Vector3 tilePosition = new Vector3(x, y);

                        var tileObject = PoolManager.Instance.GetPoolObject(PoolObjectType.Tile);
                        tileObject.transform.position = tilePosition;
                        tileObject.transform.rotation = Quaternion.identity;
                        tileObject.TryGetComponent(out Tile tile);
                        if(tile !=null)
                            _tileGameObjects[x, y] = tile;
                        else
                            Debug.LogError("Object has not have Tile Component");
                    }
                }
            }
        }

        public Tile GetTile(Vector2Int pos)
        {
            if (0 > pos.x) return null;
            if (0 > pos.y) return null;
            
            if (_tileGameObjects.GetLength(1) <= pos.x) return null;
            if ( _tileGameObjects.GetLength(0) <= pos.y) return null;
            
            return _tileGameObjects[pos.x, pos.y];

        }
    }
}

