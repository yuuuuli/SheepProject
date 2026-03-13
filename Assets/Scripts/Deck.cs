using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; set; }
    public int cardWidth = 213;
    public int cardHeight = 213;
    public GameObject cardGo;
    private int row = 7;
    
    private int column = 7;
    private int layer = 8;
    private List<Card> cards = new List<Card>();
    public RectTransform deckTrans;//������Ҫ���ɵ���λ�õ�transform����
    public Transform[] pickDeckPosTrans;//���ƶѵĸ���λ��
    public RectTransform centerDeckTrans;//�м��ƶѵĻ���λ�ã�ԭ�㣩
    public RectTransform leftColumnDeckTrans;
    public RectTransform rightColumnDeckTrans;
    public RectTransform leftDownDeckTrans;
    public RectTransform rightDownDeckTrans;
    public int[] pickDeckCardIDs;//��ŵ�ǰѡ�п��ƶ���Ŀ���ID������ǰλ��һһ��Ӧ��
    private int totalCardNum = 168;
    private int createCardNum = 0;

    public GameObject gameOverPanelGo;
    public AudioSource audioSource;
    public AudioClip clickSound;

    private int[,,] centerDeck = new int[,,]//�� �� ��
    {
        //��5��
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,2,2,2},
            {0,0,0,0}
        },
        //������
        {
            {0,0,0,0},
            {0,1,1,1},
            {0,0,1,1},
            {0,0,1,1},
            {0,1,2,2},
            {0,2,2,1},
            {0,0,0,0}
        },
        {
            {0,0,0,0},
            {0,1,1,3},
            {0,0,2,2},
            {3,3,3,3},
            {0,0,0,3},
            {0,0,3,3},
            {0,0,0,0}
        },
        //���ϲ�
        {
            {0,3,1,2},
            {0,0,0,0},
            {3,2,2,2},
            {3,3,3,3},
            {0,0,2,2},
            {0,0,0,0},
            {0,0,1,3}
        }
    };

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pickDeckCardIDs = new int[7] { -1,-1,-1,-1,-1,-1,-1};

        //�����м��һ��
        //��
        for (int k = 0; k < layer; k++)
        {
            //��
            for (int j = 0; j < row; j++)
            {
                //�����ǰ���Ƿ�ƫ��
                bool ifMoveX = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                int dirX = 0;
                if (ifMoveX)
                {
                    //ƫ�Ʒ�����������
                    dirX = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
                }
                //�����ǰ���Ƿ�ƫ��
                bool ifMoveY = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                int dirY = 0;
                if (ifMoveY)
                {
                    //ƫ�Ʒ������ϻ�����
                    dirY = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
                }
                //׼����ʱ�������ǰ�벿��������Ŀ���״̬���ԶԳ����ɺ�벿��״̬
                CREATESTATE[] halfState = new CREATESTATE[column / 2];
                //��
                for (int i = 0; i < column; i++)
                {                    
                    GameObject go = null;
                    CREATESTATE cs;
                    if (i<=column/2)
                    {
                        //ǰ�벿��ֱ�Ӵ���ά����ȡ1 2 3 4 5 6 7
                        //                      2 1 0   0 1 2
                        cs = (CREATESTATE)centerDeck[k, j, i];
                        if (i!=column/2)
                        {
                            halfState[column / 2 - i - 1] = cs;
                        }
                    }
                    else
                    {
                        cs = halfState[i - column / 2 - 1];
                    }
                    switch (cs)
                    {
                        case CREATESTATE.NONE:
                            break;
                        case CREATESTATE.CREATE:
                            go = CreateCardGo(i, j, dirX, dirY);
                            break;
                        case CREATESTATE.RANDOM:
                            if (UnityEngine.Random.Range(0,2)==0?true:false)
                            {
                                //�������
                                go= CreateCardGo(i,j,dirX,dirY);
                            }
                            break;
                        case CREATESTATE.ONLYCREATE:
                            go = CreateCardGo(i, j, 0, 0);
                            break;
                        default:
                            break;
                    }
                    if (go)
                    {
                        Card card = go.GetComponent<Card>();
                        card.SetCardSprite();
                        SetCoverState(card);
                        cards.Add(card);
                        createCardNum++;
                        go.name = "I:" + i.ToString() + " J:" + j.ToString() + " K:" + k.ToString();
                    }        
                }
            }
        }

        //���������ĵ�
        int createNum = (totalCardNum - createCardNum) / 4;
        int leftNum = totalCardNum - createCardNum - createNum * 4;
        //����
        for (int i = createNum + leftNum; i>0 ; i--)
        {
            CreateCard(leftColumnDeckTrans,0,-i);
        }
        //����
        for (int i = 0; i < createNum; i++)
        {
            CreateCard(rightColumnDeckTrans,0,-i);
        }
        //����
        for (int i = 0; i < createNum; i++)
        {
            CreateCard(leftDownDeckTrans,i,0);
        }
        //����
        for (int i = createNum; i >0 ; i--)
        {
            CreateCard(rightDownDeckTrans, i, 0);
        }
    }
    /// <summary>
    /// �����Ա��ĵ�����
    /// </summary>
    /// <param name="zeroTrans"></param>
    /// <param name="indexX"></param>
    /// <param name="indexY"></param>
    private void CreateCard(RectTransform zeroTrans,int indexX,int indexY)
    {
        GameObject go= Instantiate(cardGo,deckTrans);
        RectTransform rft = go.GetComponent<RectTransform>();
        rft.anchoredPosition = zeroTrans.anchoredPosition +
            new Vector2(cardWidth*indexX*0.15f,cardHeight*indexY*0.15f);
        Card card = go.GetComponent<Card>();
        card.SetCardSprite();
        SetCoverState(card);
        cards.Add(card);
    }


    /// <summary>
    /// ����������Ϸ����
    /// </summary>
    private GameObject CreateCardGo(int column,int row,int dirX,int dirY)
    {
        GameObject go = Instantiate(cardGo, deckTrans);
        go.GetComponent<RectTransform>().anchoredPosition
            = centerDeckTrans.anchoredPosition +
            new Vector2(cardWidth * (column + 0.5f * dirX), -cardHeight * (row + 0.5f * dirY));
        return go;
    }
    /// <summary>
    /// ���õ�ǰ�����ɵĿ������������Ƶĸ��ǹ�ϵ
    /// </summary>
    /// <param name="card"></param>
    private void SetCoverState(Card card)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            card.SetCoverCardState(cards[i]);
        }
    }
    /// <summary>
    /// ��ȡ��ǰ���ƶѵ�Ŀ��λ��
    /// </summary>
    /// <param name="currentID">��ǰ��ѡ�е���</param>
    /// <param name="posID">����λ��ID����</param>
    /// <returns></returns>
    public Transform GetPickDeckTargetTrans(int currentID, out int posID)
    {
        posID = -1;
        for (int i = 0; i < pickDeckCardIDs.Length; i++)
        {
            //��ǰ���ƶ��еĿ�����û������ѡ�еĿ���ID���
            if (pickDeckCardIDs[i]==currentID&&i+1<=pickDeckCardIDs.Length)
            {
                posID = i + 1;
                return pickDeckPosTrans[i + 1];
            }
        }
        //��ǰ���ƶ�û������ѡ������ͬ��ID
        Transform sf= GetEmptyPickDeckTargetTrans();
        if (sf)
        {
            return sf;
        }
        return null;
    }

    /// <summary>
    /// ��ȡѡ�п��ƶѵĿ�λ��
    /// </summary>
    /// <param name="posID">��ǰѡ�п��ƶѵĸ���λ������ID</param>
    /// <returns></returns>
    public Transform GetEmptyPickDeckTargetTrans(int posID=-1)
    {
        for (int i = 0; i < pickDeckCardIDs.Length; i++)
        {
            if (pickDeckCardIDs[i]==-1)
            {
                //pickDeckCardIDs[i] = posID;
                //������������������ͬID�Ŀ��ƣ���Ҫ����λ��
                if (posID!=-1)
                {
                    pickDeckPosTrans[i].SetSiblingIndex(posID);
                }
                return pickDeckPosTrans[i];
            }
        }
        //Debug.Log("��Ϸ����");
        return null;
    }
    /// <summary>
    /// ������λ�ú�ID
    /// </summary>
    public void SortCardAndCardID()
    {
        SortCard();
        SortID();
    }
    /// <summary>
    /// ��������
    /// </summary>
    public void SortCard()
    {
        Transform[] tempTrans = new Transform[pickDeckPosTrans.Length];
        for (int i = 0; i < pickDeckPosTrans.Length; i++)
        {
            int siblingIndex= pickDeckPosTrans[i].GetSiblingIndex();
            tempTrans[siblingIndex] = pickDeckPosTrans[i];
        }
        for (int i = 0; i < pickDeckPosTrans.Length; i++)
        {
            pickDeckPosTrans[i] = tempTrans[i];
        }
    }
    /// <summary>
    /// ID����
    /// </summary>
    public void SortID()
    {
        for (int i = 0; i < pickDeckCardIDs.Length; i++)
        {
            if (pickDeckPosTrans[i].childCount > 0)
            {
                pickDeckCardIDs[i] = pickDeckPosTrans[i].GetChild(0).GetComponent<Card>().id;
            }
            else
            {
                pickDeckCardIDs[i] = -1;
            }
        }
    }
    /// <summary>
    /// ���������ж�����
    /// </summary>
    public void JudgeClearCard()
    {
        SortCardAndCardID();
        //����ж�
        ClearCards();
        Invoke("SortGridPos",0.1f);
    }
    /// <summary>
    /// ����
    /// </summary>
    public void ClearCards()
    {
        int sameCount = 0;
        int judgeID = -1;
        int startIndex = -1;
        for (int i = 0; i <pickDeckCardIDs.Length; i++)
        {
            //�ո���
            if (pickDeckCardIDs[i]==-1)
            {
                break;
            }
            //��ǰ�����ж���ʼ�ĵ�һ��Ԫ��
            if (sameCount==0)
            {
                sameCount++;
                judgeID = pickDeckCardIDs[i];
                startIndex = i;
            }
            else
            {
                if (judgeID == pickDeckCardIDs[i])
                {
                    sameCount++;
                }
                else
                {
                    sameCount = 1;
                    judgeID = pickDeckCardIDs[i];
                    startIndex = i;
                }
            }
            if (sameCount>=3)
            {
                for (int j = startIndex; j < startIndex+3; j++)
                {
                    pickDeckCardIDs[j] = -1;
                    Destroy(pickDeckPosTrans[j].GetChild(0).gameObject);
                }
                PlayClickSound();
                break;
            }
            if (i>= pickDeckCardIDs.Length-1)
            {
                gameOverPanelGo.SetActive(true);
            }
        }
    }
    /// <summary>
    /// ����������ĸ���λ�ã��������룩
    /// </summary>
    private void SortGridPos()
    {
        for (int i = 0; i < pickDeckPosTrans.Length; i++)
        {
            if (pickDeckPosTrans[i].childCount<=0)
            {
                //�ո���
                pickDeckPosTrans[i].SetSiblingIndex(6);
            }
        }
        SortCardAndCardID();
    }

    public void ReturnToMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Replay()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
/// <summary>
/// ���Ƶ�����״̬
/// </summary>
public enum CREATESTATE
{
    NONE,//��λ�ò����ɿ���
    CREATE,//���ɲ���λ�ÿ���ƫ��
    RANDOM,//��������Ҳ����ƫ��
    ONLYCREATE//����һ����ƫ��
}
