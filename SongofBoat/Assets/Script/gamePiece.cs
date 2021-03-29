using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamePiece : MonoBehaviour
{
    // Start is called before the first frame update
    //xy是piece的位置
    private int x;
    private int y;

    public int X {
        get { return x; }
        set { if (isMovable()) {
                x = value;
            }
        }
    }

    public int Y {
        get { return y; }
        set {
            if (isMovable()) {
                y = value;
            }
        }
    }

    public enum ClearTypeEnum {
        AttackSingleClear = 1,
        AttackDoubleClear = 2,
        AttackTripleClear = 3,
        DefenseSingleClear = 4,
        DefenseDoubleClear = 5,
        DefenseTripleClear = 6,
        NONE = 7,
        COUNT, //count我还没明白是怎么用的
    };

    private int clearType;
    public int ClearType {
        get { return clearType; }
        set { clearType = value; }
    }

    private BattleGrid.pieceType type;
    public BattleGrid.pieceType getType() {
        return type;
    }

    private BattleGrid grid;
    public BattleGrid GridRef {
        get { return grid; }
    }

    private movablepiece movableComponent;
    public movablepiece MovableComponent {
        get { return movableComponent; }
    }

    private ColorPiece colorPieceComponent;
    public ColorPiece ColorPieceComponent {
        get { return colorPieceComponent; }
    }

    private Clearablepiece clearableComponent;
    public Clearablepiece ClearableComponent {
        get { return clearableComponent; }
    }

    private void Awake() {
        movableComponent = GetComponent<movablepiece>();
        colorPieceComponent = GetComponent<ColorPiece>();
        clearableComponent = GetComponent<Clearablepiece>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int x,int y , BattleGrid grid, BattleGrid.pieceType type) {
        this.x = x;
        this.y = y;
        this.grid = grid;
        this.type = type;
        this.clearType = (int)ClearTypeEnum.NONE;
    }

    private void OnMouseEnter() {
        grid.EnterPiece(this);
    }

    private void OnMouseDown() {
        grid.PressPiece(this);
    }

    private void OnMouseUp() {
        grid.ReleasePiece();
    }

    public bool isMovable() {
        return movableComponent != null;
    }

    public bool IsColored() {
        return colorPieceComponent != null;
    }

    public bool IsClearable() {
        return clearableComponent != null;
    }
}
