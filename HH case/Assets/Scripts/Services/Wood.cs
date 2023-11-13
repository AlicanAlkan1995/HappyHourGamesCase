using DG.Tweening;
using HappyHourGames.Managers.Board;
using HappyHourGames.Managers.ItemMovement;
using UnityEngine;

public class Wood : Entity
{
    private IItemMovementService boardService;
    private void Start()
    {
        boardService = CoreManager.Instance._serviceLocator.GetService<IItemMovementService>();
        transform.DOShakeScale(0.25f, 0.1f, 10).SetLoops(-1);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.TryGetComponent(out Item item);
        
        if (item != null)
        {
            if (item.IsOwnedByMe)
            {
                PhotonNetwork.player.AddScore(1);
                var randomPosition = boardService.GetRandomPosition();
                UpdateScoreBoard(PhotonNetwork.player.NickName, PhotonNetwork.player.GetScore(),randomPosition);
            }
        }
    }
    
    public void UpdateScoreBoard(string player, int score, Vector3 newPosition)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("UpdateScoreAndRelocate", PhotonTargets.All,player,score,newPosition);
        
    }

    [PunRPC]
    void UpdateScoreAndRelocate(string player, int score,Vector3 newPosition)
    {
        GameActions.OnPlayerScoreChanged.Invoke(player,score);
        transform.position = newPosition;
    }

}
