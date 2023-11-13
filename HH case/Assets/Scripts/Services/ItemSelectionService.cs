using DG.Tweening;
using HappyHourGames.Managers.Board;
using HappyHourGames.Managers.ItemMovement;
using HappyHourGames.Scripts.InputSystem;
using HappyHourGames.Scripts.PahtFinding;
using UnityEngine;

namespace HappyHourGames.Scripts.Services
{
    public interface IItemSelectionService
    {
        
    }

    public class ItemSelectionService : GameService, IItemSelectionService, IUpdatable
    {
        private Item _selectedItem;
        private Tile _sellectedTile = null;
        private Vector2 _initialMouseClickPosition;
        private readonly IInputHandler _inputHandler;
        private readonly IItemMovementService _itemMovementService;
        private readonly IPathFinding _pathFinding;
        private readonly BoardViewService _boardView;
        public ItemSelectionService(IServiceLocator serviceLocator, IServiceLocator cameraServiceLocator) : base(
            serviceLocator)
        {
            _inputHandler = serviceLocator.GetService<IInputHandler>();
            _itemMovementService = serviceLocator.GetService<IItemMovementService>();
            _pathFinding = serviceLocator.GetService<IPathFinding>();
            _boardView = serviceLocator.GetService<BoardViewService>();
            _inputHandler.OnClickPerformed += OnClickPerformed;
        }
        
        private void OnClickPerformed(Vector3 mousePosition)
        {
            Vector2 mouseWorldPosition = _inputHandler.GetMouseWorldPosition();
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, 10);
            Vector2Int worldToGridPoint = WorldToGrid(_inputHandler.GetMouseWorldPosition());
            if (_selectedItem == null)
            {
                if (hit.collider == null) return;
                
                hit.transform.gameObject.TryGetComponent(out Item character);
                
                if(character == null) return;
                
                if (character.IsOwnedByMe)
                {
                    _selectedItem = character;
                    _selectedItem.GetSpriteRenderer().color = Color.green;
                    _selectedItem.transform.DOShakeScale(0.25f, 0.1f, 10);
                }
            }
            else
            {
                if (hit.collider == null)
                {
                    if (_itemMovementService.CanMoveItem(_selectedItem, worldToGridPoint))
                    {

                        if (_selectedItem.IsMoving)
                        {
                            var path = _pathFinding.FindPath(_selectedItem._latestGridPos, new Vector2Int((int)worldToGridPoint.x,(int)worldToGridPoint.y));
                            _selectedItem.UpdatePath(path);
                        }
                        else
                        {
                            var path = _pathFinding.FindPath(_selectedItem.GridPlacement, new Vector2Int((int)worldToGridPoint.x,(int)worldToGridPoint.y));
                            _selectedItem.MovePath(path);
                        }
                            
                        
                        _selectedItem = null;
                    }
                    else
                    {
                        Vector2Int foundClosestGridPosition = _itemMovementService.FindClosestEmptyTile(_selectedItem, worldToGridPoint);
                        if (foundClosestGridPosition != worldToGridPoint)
                        {
                          
                            
                         
                            if (_selectedItem.IsMoving)
                            {
                                var path = _pathFinding.FindPath(_selectedItem._latestGridPos, foundClosestGridPosition);
                                _selectedItem.UpdatePath(path);
                            }
                            else
                            {
                                var path = _pathFinding.FindPath(_selectedItem.GridPlacement, foundClosestGridPosition);
                                _selectedItem.MovePath(path);
                            }
                           
                            
                            _selectedItem = null;
                        }
                    }
                   
                }
                else
                {
                    hit.transform.gameObject.TryGetComponent(out Wood collectable);
                    if (collectable != null)
                    {

                        if (_itemMovementService.CanMoveItem(_selectedItem, worldToGridPoint))
                        {
                            var position = WorldToGrid( collectable.transform.position);
                            
                            if (_selectedItem.IsMoving)
                            {
                                var path = _pathFinding.FindPath(_selectedItem._latestGridPos, new Vector2Int(position.x,position.y));
                                _selectedItem.UpdatePath(path);
                            }
                            else
                            {
                                var path = _pathFinding.FindPath(_selectedItem.GridPlacement, new Vector2Int(position.x,position.y));
                                _selectedItem.MovePath(path);
                            }
                            
                            _selectedItem = null;
                        }
                        else
                        {
                            Vector2Int foundClosestGridPosition = _itemMovementService.FindClosestEmptyTile(_selectedItem, worldToGridPoint);
                            if (foundClosestGridPosition != worldToGridPoint)
                            {
                                if (_selectedItem.IsMoving)
                                {
                                    var path = _pathFinding.FindPath(_selectedItem._latestGridPos, foundClosestGridPosition);
                                    _selectedItem.UpdatePath(path);
                                }
                                else
                                {
                                    var path = _pathFinding.FindPath(_selectedItem.GridPlacement, foundClosestGridPosition);
                                    _selectedItem.MovePath(path);
                                }
                                        
                                _selectedItem = null;
                            }
                        }
                        
                    }
                    else
                    {
                        if (_itemMovementService.CanMoveItem(_selectedItem, worldToGridPoint))
                        {
                            if (_selectedItem.IsMoving)
                            {
                                var path = _pathFinding.FindPath(_selectedItem._latestGridPos, worldToGridPoint);
                                _selectedItem.UpdatePath(path);
                            }
                            else
                            {
                                var path = _pathFinding.FindPath(_selectedItem.GridPlacement, worldToGridPoint);
                                _selectedItem.MovePath(path);
                            }
                            
                            _selectedItem = null;
                        }
                        else
                        {
                            Vector2Int foundClosestGridPosition = _itemMovementService.FindClosestEmptyTile(_selectedItem, worldToGridPoint);
                            if (foundClosestGridPosition != worldToGridPoint)
                            {
                                if (_selectedItem.IsMoving)
                                {
                                    var path = _pathFinding.FindPath(_selectedItem._latestGridPos, foundClosestGridPosition);
                                    _selectedItem.UpdatePath(path);
                                }
                                else
                                {
                                    var path = _pathFinding.FindPath(_selectedItem.GridPlacement, foundClosestGridPosition);
                                    _selectedItem.MovePath(path);
                                }
                                
                                _selectedItem = null;
                            }
                        }
                       
                    }
                };
                
            }
        }
        
        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));
        }
        
        public void Destroy()
        {
            _inputHandler.OnClickPerformed -= OnClickPerformed;
        }

        public void Update()
        {
            if (_selectedItem != null)
            {
                Vector2Int worldToGridPoint = WorldToGrid(_inputHandler.GetMouseWorldPosition());

                if (_sellectedTile == null)
                {
                    _sellectedTile = _boardView.GetTile(worldToGridPoint);
                }
                else if(_sellectedTile != _boardView.GetTile(worldToGridPoint))
                {
                    _sellectedTile.DOKill();
                    _sellectedTile = _boardView.GetTile(worldToGridPoint);
                }
                else if(!DOTween.IsTweening(_sellectedTile.transform))
                {
                    _sellectedTile.transform.DOShakeScale(0.25f, 0.25f, 10);
                }
            }
        }
    }
}