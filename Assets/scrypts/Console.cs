using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Console : MonoBehaviour
{
    public Cell[,] cells;
    public int[,] grid;
    Vector2Int PlayerPosition = new Vector2Int(0, 0);
    private int stepCounter = 0;
    [SerializeField] private DialogText dialogText;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text text;
    [SerializeField] private List<Levels> levels;
    private int level = 0;
    [SerializeField] private AudioSource musicController;

    [Header("")]
    [SerializeField] private Cell cell;
    [Header("Sprites")]
    [Header("Sprites")] private new Dictionary<Vector2Int, Sprite> spritePlayer;
    [SerializeField]private new List<Sprite> spritesPlayer;
    [SerializeField] private Sprite spriteBox;
    [SerializeField] private Sprite spriteTarget;
    [SerializeField] private Sprite spriteWall;
    [SerializeField] private Sprite spriteWallCwantum;
    [SerializeField] private Sprite spriteFloor;
    
    
    [Header("Objects")]
    [SerializeField] private Vector3 objectsPosition;
    private Dictionary<Vector2Int, Transform> objects3D = new Dictionary<Vector2Int, Transform>();
    //[SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabBox;
    //[SerializeField] private GameObject prefabTarget;
    [SerializeField] private GameObject prefabWall;
    [SerializeField] private WallCwantum prefabWallCwantum;
    //[SerializeField] private GameObject prefabFloor;

    [SerializeField]private int compliteCount = 0;
    private bool isFirstSeate = false;
    
    void Start()
    {
        spritePlayer = new Dictionary<Vector2Int,Sprite>()
        {
            { new Vector2Int(1, 0), spritesPlayer[0] },
            { new Vector2Int(0, -1), spritesPlayer[1] },
            { new Vector2Int(-1, 0), spritesPlayer[2] },
            { new Vector2Int(0, 1), spritesPlayer[3] },
        };
        
        cells = new Cell[10, 10];
        grid = new int[10, 10];
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells[i, j] = Instantiate(cell, transform.position, Quaternion.identity, transform);
                cells[i, j].image.sprite = spriteFloor;
                (cells[i, j].transform as RectTransform).anchoredPosition = new Vector2((i-cells.GetLength(0)*0.5f+0.5f)*(cell.transform as RectTransform).rect.width, (j-cells.GetLength(1)*0.5f+0.5f)*(cell.transform as RectTransform).rect.height);
                grid[i, j] = 0;
            }
        }

        loadLevel(levels[level]);
    }
    
    void Update()
    {
        if (!PlayerMove.I.isCanMove && !PouseManuManager.isPoused)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                movePlayer(new Vector2Int(0, 1));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                movePlayer(new Vector2Int(0, -1));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                movePlayer(new Vector2Int(-1, 0));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                movePlayer(new Vector2Int(1, 0));
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("up");
                StartCoroutine(PlayerMove.I.IsCanMove(true, 1));
                StartCoroutine(PlayerMove.I.LerpPos(new Vector3(PlayerMove.I.transform.position.x, 0, PlayerMove.I.transform.position.z), 1));
            }
        }
    }

    

    void movePlayer(Vector2Int direction)
    {
        if(dialogPanel.activeSelf)
            return;
        if (PlayerPosition.x+direction.x > grid.GetLength(0) - 1 || PlayerPosition.y+direction.y > grid.GetLength(1) - 1 || PlayerPosition.x + direction.x < 0 || PlayerPosition.y + direction.y < 0)
            return;
        
        if (grid[PlayerPosition.x + direction.x, PlayerPosition.y + direction.y] == 0 || grid[PlayerPosition.x + direction.x, PlayerPosition.y + direction.y] == 3)
        {
            if (levels[level].gridTargets.Contains(PlayerPosition))
            {
                cells[PlayerPosition.x, PlayerPosition.y].image.sprite = spriteTarget;
                grid[PlayerPosition.x, PlayerPosition.y] = 3;
            }else{
                cells[PlayerPosition.x, PlayerPosition.y].image.sprite = spriteFloor;
                grid[PlayerPosition.x, PlayerPosition.y] = 0;
            }
            PlayerPosition += direction;
            stepCounter++;text.text = $"Step {stepCounter}";
            cells[PlayerPosition.x, PlayerPosition.y].image.sprite = spritePlayer[direction];
            grid[PlayerPosition.x, PlayerPosition.y] = 1;
        }else if (grid[PlayerPosition.x + direction.x, PlayerPosition.y + direction.y] == 2)
        {
            if (PlayerPosition.x+direction.x*2 > grid.GetLength(0) - 1 || PlayerPosition.y+direction.y*2 > grid.GetLength(1) - 1 || PlayerPosition.x + direction.x*2 < 0 || PlayerPosition.y + direction.y*2 < 0)
                return;
            if (grid[PlayerPosition.x + direction.x*2, PlayerPosition.y + direction.y*2] == 0 || grid[PlayerPosition.x + direction.x*2, PlayerPosition.y + direction.y*2] == 3)
            {
                if (grid[PlayerPosition.x + direction.x * 2, PlayerPosition.y + direction.y * 2] == 3) compliteCount++;
                if (levels[level].gridTargets.Contains(PlayerPosition + direction)) compliteCount--;
                if (compliteCount == levels[level].gridTargets.Length)
                {
                    foreach (var obj in objects3D)
                    {
                        Destroy(obj.Value.gameObject);
                    }
                    objects3D = new Dictionary<Vector2Int, Transform>();
                    level++;
                    musicController.clip = levels[level].musicSound;
                    musicController.Play();
                    loadLevel(levels[level]);
                    dialogPanel.SetActive(true);
                    dialogText.SetDialog(new List<DialogString>()
                    {
                        new DialogCoise("Zaczynasz rozumieć. Dobrze. Więc jest nadzieja.", dialogText.sprites[0], "Co teraz?", "Coś sobie przypomniałem…", "Po prostu się mną bawisz, prawda?",new UnityEvent(), new UnityEvent(), new UnityEvent()),
                    });
                    dialogText.NextText();
                    return;
                }
                grid[PlayerPosition.x + direction.x * 2, PlayerPosition.y + direction.y * 2] = grid[PlayerPosition.x + direction.x, PlayerPosition.y + direction.y];
                cells[PlayerPosition.x + direction.x*2, PlayerPosition.y + direction.y*2].image.sprite = spriteBox;
                objects3D[new Vector2Int(PlayerPosition.x + direction.x, PlayerPosition.y + direction.y)].position = (new Vector3(transform.position.x + objectsPosition.x + PlayerPosition.x + direction.x*2,0, transform.position.y+objectsPosition.x+PlayerPosition.y+direction.y*2)*objects3D[new Vector2Int(PlayerPosition.x + direction.x, PlayerPosition.y + direction.y)].localScale.x);
                objects3D.Add(new Vector2Int(PlayerPosition.x + direction.x * 2, PlayerPosition.y + direction.y * 2) ,objects3D[new Vector2Int(PlayerPosition.x + direction.x, PlayerPosition.y + direction.y)]);
                objects3D.Remove(new Vector2Int(PlayerPosition.x + direction.x, PlayerPosition.y + direction.y));
                if (levels[level].gridTargets.Contains(PlayerPosition))
                {
                    cells[PlayerPosition.x, PlayerPosition.y].image.sprite = spriteTarget;
                    grid[PlayerPosition.x, PlayerPosition.y] = 3;
                }else{
                    cells[PlayerPosition.x, PlayerPosition.y].image.sprite = spriteFloor;
                    grid[PlayerPosition.x, PlayerPosition.y] = 0;
                }
                PlayerPosition += direction;
                stepCounter++;
                text.text = $"Step {stepCounter}";
                cells[PlayerPosition.x, PlayerPosition.y].image.sprite = spritePlayer[direction];
                grid[PlayerPosition.x, PlayerPosition.y] = 1;
            }
        }
        if (levels[level].stepCount < stepCounter)
        {
            foreach (var obj in objects3D)
            {
                Destroy(obj.Value.gameObject);
            }
            objects3D = new Dictionary<Vector2Int, Transform>();
            loadLevel(levels[level]);
            
            dialogPanel.SetActive(true);
            dialogText.SetDialog(new List<DialogString>()
            {
                new DialogCoise("Nie rozumiesz. Nie można tak po prostu naprawić wszystkiego, co zostało złamane.", dialogText.sprites[0], "Pozwól mi spróbować jeszcze raz.", "Nie rozumiem, czego ode mnie chcesz!", "Podoba ci się to, prawda?",new UnityEvent(), new UnityEvent(), new UnityEvent()),
            });
            dialogText.NextText();
            return;
        }
    }
    
    void loadLevel(Levels lvl)
    {
        compliteCount = 0;
        stepCounter = 0;
        stepCounter++;text.text = $"Step {stepCounter}";
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            { 
                grid[i, j] = 0;
                cells[i, j].image.sprite = spriteFloor;
            }
        }

        PlayerPosition = lvl.player;
        cells[lvl.player.x, lvl.player.y].image.sprite = spritePlayer[new Vector2Int(0, -1)];
        grid[lvl.player.x, lvl.player.y] = 1;
        foreach (Vector2Int pos in lvl.gridBoxs)
        {
            objects3D.Add(pos, Instantiate(prefabBox, transform.position+objectsPosition + new Vector3(pos.x*prefabBox.transform.localScale.x, 0, pos.y*prefabBox.transform.localScale.y), Quaternion.Euler(0, 0, 0)).transform);
            grid[pos.x, pos.y] = 2;
            cells[pos.x, pos.y].image.sprite = spriteBox;
        }
        foreach (Vector2Int pos in lvl.gridTargets)
        {
            grid[pos.x, pos.y] = 3;
            cells[pos.x, pos.y].image.sprite = spriteTarget;
        }foreach (Vector2Int pos in lvl.gridWalls)
        {
            objects3D.Add(pos, Instantiate(prefabWall, transform.position+objectsPosition + new Vector3(pos.x*prefabWall.transform.localScale.x, 0, pos.y*prefabWall.transform.localScale.y), Quaternion.identity).transform);
            grid[pos.x, pos.y] = 4;
            cells[pos.x, pos.y].image.sprite = spriteWall;
        }

        foreach (WallList pos in lvl.CwantumWalls)
        {
            WallCwantum obj = Instantiate(prefabWallCwantum, transform.position + objectsPosition + new Vector3(pos.walls[0].x * prefabWall.transform.localScale.x, 0, pos.walls[0].y * prefabWall.transform.localScale.y), Quaternion.identity);
            obj.positions = pos.walls;
            obj.console = this;
            obj.id = pos.id;
            objects3D.Add(obj.id, obj.transform);
            grid[pos.walls[0].x, pos.walls[0].y] = 4;
            cells[pos.walls[0].x, pos.walls[0].y].image.sprite = spriteWallCwantum;
        }
        
    }

    public void CangeWallPosition(WallCwantum wall)
    {
        if (grid[wall.positions[(wall.positionID + 1) % wall.positions.Count].x,
                wall.positions[(wall.positionID + 1) % wall.positions.Count].y] == 0)
        {
            cells[wall.positions[wall.positionID].x, wall.positions[wall.positionID].y].image.sprite = spriteFloor;
            grid[wall.positions[wall.positionID].x, wall.positions[wall.positionID].y] = 0;
            wall.positionID = (wall.positionID + 1) % wall.positions.Count;
            cells[wall.positions[wall.positionID].x, wall.positions[wall.positionID].y].image.sprite =
                spriteWallCwantum;
            grid[wall.positions[wall.positionID].x, wall.positions[wall.positionID].y] = 4;
            objects3D[wall.id].position = transform.position + objectsPosition +
                                          new Vector3(
                                              wall.positions[wall.positionID].x * prefabWall.transform.localScale.x, 0,
                                              wall.positions[wall.positionID].y * prefabWall.transform.localScale.y);
        }
    }

    public void Sit()
    {
        if (!isFirstSeate)
        {
            isFirstSeate = true;
            dialogPanel.SetActive(true);
        }

        musicController.clip = levels[level].musicSound;
        musicController.Play();
        Debug.Log("Sit");
        StartCoroutine(PlayerMove.I.IsCanMove(false));
        StartCoroutine(PlayerMove.I.LerpPos(transform.position+transform.forward*-0.7f+new Vector3(0, -1.8f, 0), 2));
        StartCoroutine(PlayerMove.I.LerpRot(transform.rotation, 2));
    } 
}
