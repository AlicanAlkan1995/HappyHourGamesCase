using System.Collections.Generic;
using DG.Tweening;
using HappyHourGames.Managers.Board;
using HappyHourGames.Managers.Tile;
using HappyHourGames.Object_Pooling;
using HappyHourGames.Scripts.Services;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : Entity , IMovable, IPunObservable, IUpdatable
{
    [SerializeField] private TextMeshPro ItemName;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _correctPlayerPos;
    private bool _isMoving;
    
    public PoolObjectType ObjectType;
    public bool isNetworkObject;
    public Vector2Int Size { get; set; }
    public Vector2Int GridPlacement { get; set; }
    public Vector2Int _latestGridPos { get; set; }
    public bool IsOwnedByMe { get; set; }
    public Color Color { get; set; }
    public bool IsMoving { get => _isMoving; }
    public bool IsNetworkObject() => isNetworkObject;
    public Transform GetTransform() => transform;
    
    public void UpdateItemName(string name)
    {
        if (ItemName != null)
        {
            ItemName.text = name;
        }
        
    }
  
    public void MovePath(List<Vector2Int> path)
    {
        GetSpriteRenderer().color = Color;
        _isMoving = true;
        var lastPosition = path[^1];
        
        UpdateItemPosition(lastPosition);

        var convertedPath = path.ConvertAll<Vector3>(input => new Vector3(input.x,input.y));
        transform.DOPath(convertedPath.ToArray(), convertedPath.Count / 2,PathType.Linear).SetEase(Ease.Linear).OnComplete(() =>
        {
            _isMoving = false;
        }).OnWaypointChange((wayPoint) =>
        {
            _latestGridPos = path[wayPoint];
        });
        
    }

    public void UpdatePath(List<Vector2Int> path)
    {
        UpdateItemPosition(_latestGridPos);
        MoveTo(_latestGridPos);
        MovePath(path);
    }
    
    public void MoveTo(Vector2Int newPosition)
    {

        transform.DOKill(false);

        GetSpriteRenderer().color = Color;
        transform.position = new Vector3(newPosition.x, newPosition.y,0);
        _latestGridPos = newPosition;
    }
    
    public void SetPosition(Vector2 newPosition)
    {
        transform.position = newPosition ;
        _latestGridPos = new Vector2Int((int)newPosition.x,(int)newPosition.y);
    }
    public SpriteRenderer GetSpriteRenderer()
    {
        if (!_spriteRenderer)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        return _spriteRenderer;
    }
    
    public void OnItemSelected()
    {
        GetSpriteRenderer().DOFade(.5f, 1f);
    }

    public void OnItemDeselected()
    {
        GetSpriteRenderer().DOFade(1f, 1f);
    }

    public void UpdateItemPosition(Vector2Int position)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("UpdateItemPosition", PhotonTargets.All,position.x,position.y);
        
    }

    [PunRPC]
    void UpdateItemPosition(int positionX, int positionY)
    {
        GameActions.OnItemMoved?.Invoke(this,new Vector2Int(positionX,positionY));
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
            stream.SendNext(transform.position);
        else
            _correctPlayerPos = (Vector3)stream.ReceiveNext();
    }
    
    public void Update()
    {
        if (!IsOwnedByMe)
        {
            transform.position = Vector3.Lerp(transform.position, _correctPlayerPos, Time.deltaTime * 5);
        }
    }

  
}