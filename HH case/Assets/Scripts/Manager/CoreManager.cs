using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using HappyHourGames.Managers.Board;
using HappyHourGames.Managers.ItemMovement;
using HappyHourGames.Managers.Tile;
using HappyHourGames.Object_Pooling;
using HappyHourGames.Scripts.InputSystem;
using HappyHourGames.Scripts.Network;
using HappyHourGames.Scripts.PahtFinding;
using HappyHourGames.Scripts.Services;
using HappyHourGames.Scripts.UI;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class CoreManager : Photon.MonoBehaviour
{
    private readonly ItemDatabase  _itemDatabase = new ItemDatabase();

    private readonly CameraServiceLocator _cameraServiceLocator  = new CameraServiceLocator();
    private readonly IPhotonNetwork _photonNetwork = new CoreNetwork();
    private readonly IUpdateService _updateManager = new UpdateService();

    private const int StoneCount = 50;
    private const int WoodCount = 10;

    [SerializeField] private TextMeshProUGUI scoreText;
    private Dictionary<string, int> _scoreBoard = new Dictionary<string, int>();
    
    public readonly ServiceLocator _serviceLocator  = new ServiceLocator();
    public static CoreManager Instance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
        
    }
    private  void Start()
    {
        _photonNetwork.Init();
        
        InitializeServices();

        InitializeUpdatables();
        
        
        _photonNetwork.OnSystemsInitialized(_serviceLocator);

        foreach (var player in _photonNetwork.GetOtherPlayers())
        {
            _scoreBoard[player.NickName] = 0;
            HandleScoreBoard(player.NickName, 0);
        }
        
        HandleScoreBoard(_photonNetwork.GetPlayer().NickName, 0);
       
        GameActions.OnPlayerScoreChanged += HandleScoreBoard;
        UIManager.Instance.ShowPopup(new StartGameDefinition(OnPlayerReady));
    }

    private void OnPlayerReady()
    {
        _photonNetwork.setPlayerState(true);

        InitializeCharacters();
        
        InitializeStones();

        InitializeWoods();
    }
    
   
    private void InitializeServices()
    {
        InputHandler inputHandler = new InputHandler();
        inputHandler.OnEnable();
        _serviceLocator.RegisterService<IInputHandler>(inputHandler);
        _cameraServiceLocator.RegisterService<IInputHandler>(inputHandler);
        
        var boardService = new BoardService();
        _serviceLocator.RegisterService<IBoardService>(boardService);
        boardService.InitializeBoard();
        
        _serviceLocator.RegisterService(_updateManager);
        
        var boardView = new GameObject().AddComponent<BoardView>();
        boardView.name = "BoardView";
        boardView.Initialize(boardService); 
        
        _serviceLocator.RegisterService<BoardViewService>(boardView);
     
        
        var itemControllerService = new ItemControllerService(boardService.Width, boardService.Height);
        _serviceLocator.RegisterService<ITileController>(itemControllerService);
        
        var itemView = new GameObject().AddComponent<ItemView>();
        itemView.name = "itemView";
        itemView.Initialize(itemControllerService,boardService.Width, boardService.Height);
        
        var itemMovementService = new ItemMovementService(boardService, itemControllerService, _itemDatabase);
        _serviceLocator.RegisterService<IItemMovementService>(itemMovementService);

       

        CameraPanner cameraPanner = new CameraPanner(1.25f);
        CameraZoom cameraZoom = new CameraZoom(1.2f, 10, 100, 300);

        _cameraServiceLocator.RegisterService<ICameraZoom>(cameraZoom);
        _cameraServiceLocator.RegisterService<ICameraPanner>(cameraPanner);

        CameraController cameraController = new CameraController
        (
            _cameraServiceLocator.GetService<IInputHandler>(),
            _cameraServiceLocator.GetService<ICameraPanner>(),
            _cameraServiceLocator.GetService<ICameraZoom>()
        );
        
        _cameraServiceLocator.RegisterService<ICameraController>(cameraController);
        
        cameraPanner.SetCameraPosition(new Vector3(boardService.BoardCenter().x,boardService.BoardCenter().y,-15));
        
        PathFinding pathFinding = new PathFinding(_serviceLocator);
        _serviceLocator.RegisterService<IPathFinding>(pathFinding);
        
        
        ItemSelectionService itemSelectionService = new ItemSelectionService(_serviceLocator, _cameraServiceLocator);
        _serviceLocator.RegisterService<IItemSelectionService>(itemSelectionService);
        
    }

    private void InitializeStones()
    {
        var itemMovementService = _serviceLocator.GetService<IItemMovementService>();
        List<IItemData> _itemdata = new List<IItemData>();
        if (_photonNetwork.IsMasterClient())
        {
            for (int i = 0; i < StoneCount; i++)
            {
                var item = new BasicItem("stone" + i, Vector2Int.one, PoolObjectType.Stone);
                _itemDatabase.AddItem(item);
                _itemdata.Add(item);
            }
            itemMovementService.PlaceRandomly(_itemdata,100);
        }
    }

    private void InitializeWoods()
    {
        var boardService = _serviceLocator.GetService<IItemMovementService>();

        if (PhotonNetwork.isMasterClient)
        {
            foreach (var pos in  boardService.GetRandomPositions(10))
            {
                PhotonNetwork.Instantiate("wood", pos, quaternion.identity, 0,null);
            }
        }
    }
    
    private void InitializeUpdatables()
    {
        var cameraPanner = _cameraServiceLocator.GetService<ICameraPanner>();
        var itemSellection = _serviceLocator.GetService<IItemSelectionService>();
        
        _updateManager.RegisterUpdatable(itemSellection as IUpdatable);
        _updateManager.RegisterUpdatable(cameraPanner as IUpdatable);
    }
    
    private void Update()
    {
        _updateManager.UpdateAll();
    }

    private void FixedUpdate()
    {
        _updateManager.FixedUpdateAll();
    }

    private void LateUpdate()
    {
        _updateManager.LateUpdateAll();
    }

    private void InitializeCharacters()
    {
        var positions = _serviceLocator.GetService<IBoardService>().GetCharacterPosition(_photonNetwork.PlayerNumber());
        
        var movementService = _serviceLocator.GetService<IItemMovementService>();
        
        _itemDatabase.AddItem(new BasicItem(PhotonNetwork.player.NickName + "Character0",Vector2Int.one, PoolObjectType.Character));
        _itemDatabase.AddItem(new BasicItem(PhotonNetwork.player.NickName + "Character1",Vector2Int.one, PoolObjectType.Character));
        _itemDatabase.AddItem(new BasicItem(PhotonNetwork.player.NickName + "Character2",Vector2Int.one, PoolObjectType.Character));

        for (var index = 0; index < positions.Length; index++)
        {
            var position = positions[index];
            movementService.PlaceItem(position,PhotonNetwork.player.NickName + "Character" + index);
        }
    }

    void HandleScoreBoard(string playerName, int score)
    {
        _scoreBoard[playerName] = score;
        string scoreBoardText = null;
        foreach (var playerScore in _scoreBoard)
        {
            scoreBoardText += $"{playerScore.Key} = {playerScore.Value}\n";
        }

        scoreText.text = scoreBoardText;

    }
    
 
    
    
}
