using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour {
    // Start is called before the first frame update
    public int xdim;
    public int ydim;
    public float filltime;
    public enum pieceType {
        NORMAL,
        EMPTY,
        COUNT, //count我还没明白是怎么用的
    };

    private Dictionary<pieceType, GameObject> piecePrefabDict;
    //字典无法从inspector面板中显示，所以需要一个结构体和字典关联
    //拖拽预制体到结构体里，然后遍历结构体给字典赋值。

    [System.Serializable]
    public struct PiecePrefeb {
        public pieceType type;
        public GameObject prefeb;
    }

    public PiecePrefeb[] piecePrefebs;
    public GameObject backGroundPrefeb; //背景，还没用到

    public GameObject Levelsystem;
    private gamePiece[,] pieces; //声明刷新阵列，其实就是一个二维数组

    private gamePiece pressedPiece;
    private gamePiece enteredPiece;

    void Start() {
        piecePrefabDict = new Dictionary<pieceType, GameObject>();

        //在字典中添加不同类型的魔法球，现在只有Empty和Normal，以后可能会有obstacle
        for (int i = 0; i < piecePrefebs.Length; i++) {
            if (!piecePrefabDict.ContainsKey(piecePrefebs[i].type)) { //字典不能出现两个key，如果没有key那么加入一个新的key
                piecePrefabDict.Add(piecePrefebs[i].type, piecePrefebs[i].prefeb);
            }
        }

        //添加背景
        //TODO:背景暂时还没有
        for (int x = 0; x < xdim; x++) {
            for (int y = 0; y < ydim; y++) {
                ;
            }
        }

        //生成空阵列
        pieces = new gamePiece[xdim, ydim];
        for (int x = 0; x < xdim; x++) {
            for (int y = 0; y < ydim; y++) {
                SpawnNewPiece(x, y, pieceType.EMPTY);
            }
        }

        //填充阵列
        StartCoroutine(Fill());

        Levelsystem = (GameObject)Instantiate(Levelsystem, GetWorldPositon(13, -2), Quaternion.identity);
        Levelsystem.transform.parent = transform;
    }

    // Update is called once per frame
    void Update() {

    }

    //填充函数，需要填充的时候直接调
    public IEnumerator Fill() {
        while (FillStep()) {
            yield return new WaitForSeconds(filltime);
        }

        for (int x = 0; x < xdim; x++) {
            for (int y = 0; y < ydim; y++) {
                pieces[x, y].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
            pieces[6, 0].gameObject.GetComponent<BoxCollider2D>().enabled = false;
            pieces[6, 1].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
            //单步填充计算
    public bool FillStep() {
        bool movedPiece = false;

        for (int x = 1; x < xdim; x++) {
            for (int y = 0; y < ydim; y++) {
                gamePiece piece = pieces[x, y];
                if (piece.isMovable()) {
                    gamePiece pieceBelow = pieces[x - 1, y];
                    if (pieceBelow.getType() == pieceType.EMPTY) {
                        Destroy(pieceBelow.gameObject);
                        piece.MovableComponent.Move(x - 1, y, filltime);
                        pieces[x - 1, y] = piece;
                        SpawnNewPiece(x, y, pieceType.EMPTY);
                        movedPiece = true;
                    };
                }
            }
        }

        for (int y = 0; y < ydim; y++) {
            gamePiece pieceBelow = pieces[xdim - 1, y];

            if (pieceBelow.getType() == pieceType.EMPTY) {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[pieceType.NORMAL], GetWorldPositon(xdim-0.84f, y), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[xdim - 1, y] = newPiece.GetComponent<gamePiece>();
                pieces[xdim - 1, y].Init(xdim - 1, y, this, pieceType.NORMAL);
                pieces[xdim - 1, y].MovableComponent.Move(xdim-0.84f, y, filltime);
                pieces[xdim - 1, y].ColorPieceComponent.SetColor((ColorPiece.ColorType)Random.Range(0, pieces[xdim - 1, y].ColorPieceComponent.NumColors));
                movedPiece = true;

            }
        }
        return movedPiece;
    }


    //从这里得出坐标
    public Vector3 GetWorldPositon(float x, float y) {
        return new Vector3(transform.position.x - xdim / 2.0f + x*1.22f - 0.2f, transform.position.y + ydim / 2.0f - 1.25f*y +0.2f, -0.2f);
    }

    public gamePiece SpawnNewPiece(int x, int y, pieceType type) {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPositon(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;
        pieces[x, y] = newPiece.GetComponent<gamePiece>();
        pieces[x, y].Init(x, y, this, type);
        return pieces[x, y];
    }

    public bool IsAdjacent(gamePiece piece1, gamePiece piece2) {
        return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1 ||
            piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
        //是否相邻
    }

    public bool IsSameColor(gamePiece piece1, gamePiece piece2) {
        return (piece1.ColorPieceComponent.Color == piece2.ColorPieceComponent.Color);
    }

    //TODO:这个地方需要想一想是清除里面填充，还是分开填充
    public bool ClearPiece(int x, int y) {
        if (pieces[x, y].IsClearable()) {
            pieces[x, y].ClearableComponent.clear();
            SpawnNewPiece(x, y, pieceType.EMPTY);
            StartCoroutine(Fill());
            return true;
        }
        return false;
    }

    public void SearchClearablePiece(gamePiece piece) {

        if (piece.ColorPieceComponent.Color == ColorPiece.ColorType.RED) {
            piece.ClearType = (int)gamePiece.ClearTypeEnum.AttackSingleClear;
        } else if (piece.ColorPieceComponent.Color == ColorPiece.ColorType.BLUE) {
            piece.ClearType = (int)gamePiece.ClearTypeEnum.DefenseSingleClear;
        }

        for (int x = 0; x < xdim-1; x++) {
            for (int y = 0; y < ydim; y++) {
                gamePiece piecePotential = pieces[x, y];
                if (IsAdjacent(piece, piecePotential) && IsSameColor(piece, piecePotential)) {
                    ChangeColor(piecePotential, 180);
                    if (piecePotential.ColorPieceComponent.Color == ColorPiece.ColorType.RED) {
                        piecePotential.ClearType = (int)gamePiece.ClearTypeEnum.AttackDoubleClear;
                    } else if (piecePotential.ColorPieceComponent.Color == ColorPiece.ColorType.BLUE) {
                        piecePotential.ClearType = (int)gamePiece.ClearTypeEnum.DefenseDoubleClear;
                    }   
                    //这几个是二消的
                    //Debug.Log(piecePotential.GetComponentInChildren<SpriteRenderer>());
                }
            }
        }
        for (int x = 0; x < xdim-1; x++) {
            for (int y = 0; y < ydim; y++) {
                gamePiece piecePotential = pieces[x, y];
                if ((int)Mathf.Abs(piece.X - piecePotential.X) == 1 && (int)Mathf.Abs(piece.Y - piecePotential.Y) == 1 && IsSameColor(piece, piecePotential)) {
                    //如果点选的x大于进入的x,不用考虑边界
                    if (piece.X < piecePotential.X) {
                        gamePiece piece1 = pieces[piece.X + 1, piece.Y];
                        gamePiece piece2 = pieces[piecePotential.X - 1, piecePotential.Y];
                        if (IsSameColor(piece1, piece) && IsSameColor(piece2, piece)) {
                            if (piecePotential.ColorPieceComponent.Color == ColorPiece.ColorType.RED) {
                                piecePotential.ClearType = (int)gamePiece.ClearTypeEnum.AttackTripleClear;
                            } else if (piecePotential.ColorPieceComponent.Color == ColorPiece.ColorType.BLUE) {
                                piecePotential.ClearType = (int)gamePiece.ClearTypeEnum.DefenseTripleClear;
                            }
                            ChangeColor(piecePotential, 100);
                        }
                    }

                    if(piece.X > piecePotential.X) {
                        gamePiece piece1 = pieces[piece.X - 1, piece.Y];
                        gamePiece piece2 = pieces[piecePotential.X + 1, piecePotential.Y];
                        if (IsSameColor(piece1, piece) && IsSameColor(piece2, piece)) {
                            if (piecePotential.ColorPieceComponent.Color == ColorPiece.ColorType.RED) {
                                piecePotential.ClearType = (int)gamePiece.ClearTypeEnum.AttackTripleClear;
                            } else if (piecePotential.ColorPieceComponent.Color == ColorPiece.ColorType.BLUE) {
                                piecePotential.ClearType = (int)gamePiece.ClearTypeEnum.DefenseTripleClear;
                            }
                            ChangeColor(piecePotential, 100);
                        }
                    }
                }
            }
        }
    }

    public void PressPiece(gamePiece piece) { //这是第一个选择的块
        pressedPiece = piece;
        //判定亮起
        ChangeColor(piece, 180);
        SearchClearablePiece(piece);
        //显示UI
    }

    public void EnterPiece(gamePiece piece) { //这是最后一个选择的块
        enteredPiece = piece;
        //判定是否构成连锁消除
        //TODO:根据种类显示UI
    }

    public void ReleasePiece() {
        // RealeasedPiece = piece;
        //以后这里都要改成按照状态判断
        gamePiece piece1 = enteredPiece;
        gamePiece piece2 = pressedPiece;
        switch (enteredPiece.ClearType) {
            case 1:
                ClearPiece(pressedPiece.X, pressedPiece.Y);
                Levelsystem.GetComponent<LevelSystem>().Hero.GetComponent<CharacterProperties>().StartSkill((int)gamePiece.ClearTypeEnum.AttackSingleClear);
                Debug.Log("1消了进攻球");
                break;
            case 2:
                ClearPiece(pressedPiece.X, pressedPiece.Y);
                ClearPiece(enteredPiece.X, enteredPiece.Y);
                Levelsystem.GetComponent<LevelSystem>().Hero.GetComponent<CharacterProperties>().StartSkill((int)gamePiece.ClearTypeEnum.AttackDoubleClear);
                Debug.Log("2消了进攻球");
                break;
            case 3:
                if (pressedPiece.X < enteredPiece.X) {
                    piece1 = pieces[pressedPiece.X + 1, pressedPiece.Y];
                    piece2 = pieces[enteredPiece.X - 1, enteredPiece.Y];
                }
                if (pressedPiece.X > enteredPiece.X) {
                    piece1 = pieces[pressedPiece.X - 1, pressedPiece.Y];
                    piece2 = pieces[enteredPiece.X + 1, enteredPiece.Y];
                }
                ClearPiece(pressedPiece.X, pressedPiece.Y);
                ClearPiece(enteredPiece.X, enteredPiece.Y);
                ClearPiece(piece1.X, piece1.Y);
                ClearPiece(piece2.X, piece2.Y);
                Levelsystem.GetComponent<LevelSystem>().Hero.GetComponent<CharacterProperties>().StartSkill((int)gamePiece.ClearTypeEnum.AttackTripleClear);
                Debug.Log("4消了进攻球");
                break;
            case 4:
                ClearPiece(pressedPiece.X, pressedPiece.Y);
                Levelsystem.GetComponent<LevelSystem>().Hero.GetComponent<CharacterProperties>().StartSkill((int)gamePiece.ClearTypeEnum.DefenseSingleClear);
                Debug.Log("1消了防御球");
                break;
            case 5:
                ClearPiece(pressedPiece.X, pressedPiece.Y);
                ClearPiece(enteredPiece.X, enteredPiece.Y);
                Levelsystem.GetComponent<LevelSystem>().Hero.GetComponent<CharacterProperties>().StartSkill((int)gamePiece.ClearTypeEnum.DefenseDoubleClear);
                Debug.Log("2消了防御球");
                break;
            case 6:
                Debug.Log("4消了防御球");
                piece1 = enteredPiece;
                piece2 = pressedPiece;

                if (pressedPiece.X < enteredPiece.X) {
                    piece1 = pieces[pressedPiece.X + 1, pressedPiece.Y];
                    piece2 = pieces[enteredPiece.X - 1, enteredPiece.Y];
                }
                if (pressedPiece.X > enteredPiece.X) {
                    piece1 = pieces[pressedPiece.X - 1, pressedPiece.Y];
                    piece2 = pieces[enteredPiece.X + 1, enteredPiece.Y];
                }

                ClearPiece(pressedPiece.X, pressedPiece.Y);
                ClearPiece(enteredPiece.X, enteredPiece.Y);
                ClearPiece(piece1.X, piece1.Y);
                ClearPiece(piece2.X, piece2.Y);
                Levelsystem.GetComponent<LevelSystem>().Hero.GetComponent<CharacterProperties>().StartSkill((int)gamePiece.ClearTypeEnum.DefenseTripleClear);

                break;
            default:
                Debug.Log("没有判断出消球");
                break;
        }
        /*if (enteredPiece.ClearType == (int)gamePiece.ClearTypeEnum.AttackSingleClear) {
            ClearPiece(pressedPiece.X, pressedPiece.Y);
            Levelsystem.GetComponent<LevelSystem>().battleInitial.hero.GetComponent<CharacterProperties>().StartSkill((int)gamePiece.ClearTypeEnum.AttackSingleClear);
            Debug.Log("1消了进攻球");
        }
        else if (enteredPiece.ClearType == (int)gamePiece.ClearTypeEnum.AttackDoubleClear) {
            ClearPiece(pressedPiece.X, pressedPiece.Y);
            ClearPiece(enteredPiece.X, enteredPiece.Y);
            Levelsystem.GetComponent<LevelSystem>().battleInitial.hero.GetComponent<CharacterProperties>().StartSkill((int)gamePiece.ClearTypeEnum.AttackDoubleClear);
        } else if (enteredPiece.ClearType == (int)gamePiece.ClearTypeEnum.AttackTripleClear) {
            gamePiece piece1 = enteredPiece;
            gamePiece piece2 = pressedPiece;

            if (pressedPiece.X < enteredPiece.X) {
                piece1 = pieces[pressedPiece.X + 1, pressedPiece.Y];
                piece2 = pieces[enteredPiece.X - 1, enteredPiece.Y];
            }
            if (pressedPiece.X > enteredPiece.X) {
                piece1 = pieces[pressedPiece.X - 1, pressedPiece.Y];
                piece2 = pieces[enteredPiece.X + 1, enteredPiece.Y];
            }

            ClearPiece(pressedPiece.X, pressedPiece.Y);
            ClearPiece(enteredPiece.X, enteredPiece.Y);
            ClearPiece(piece1.X, piece1.Y);
            ClearPiece(piece2.X, piece2.Y);
        }*/
        RestorePiece();
    } 
    
    //TODO:改变透明度，以后可能改变模型
    public void RestorePiece() {
        for (int x = 0; x < xdim; x++) {
            for (int y = 0; y < ydim; y++) {
                gamePiece piece = pieces[x, y];
                piece.GetComponentInChildren<SpriteRenderer>().material.color = new Color32(255, 255, 255, 255);
                piece.ClearType = (int)gamePiece.ClearTypeEnum.NONE;
                piece.transform.Find("magicRing").gameObject.SetActive(false);
            }
        }
    }

    public void ChangeColor(gamePiece piece, float newAlpha) {
        piece.transform.Find("magicRing").gameObject.SetActive(true);
        //piece.GetComponentInChildren<SpriteRenderer>().material.color = new Color(1, 1, 1, newAlpha/255);
    }
}
