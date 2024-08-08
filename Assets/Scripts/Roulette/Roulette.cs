using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Roulette : MonoBehaviour
{
    [SerializeField] private Transform              piecePrefab;            // �귿�� ǥ�õǴ� ���� ������
    [SerializeField] private Transform              linePrefab;             // �������� �����ϴ� �� ������
    [SerializeField] private Transform              pieceParent;            // �������� ��ġ�Ǵ� �θ� Transform
    [SerializeField] private Transform              lineParent;             // ������ ��ġ�Ǵ� �θ� Transform
    [SerializeField] private RoulettePieceData[]    roulettePieceData;      // �귿�� ǥ�õǴ� ���� �迭

    [SerializeField] private int                    spinDuration;           // ȸ�� �ð�
    [SerializeField] private RectTransform          spinningRoulette;       // ���� ȸ���ϴ� ȸ���� Transform
    [SerializeField] private AnimationCurve         spinningCurve;          // ȸ�� �ӵ� ��� ���� �׷���

    private float   pieceAngle;                     // ���� �ϳ��� ��ġ�Ǵ� ����
    private float   halfPieceAngle;                 // ���� �ϳ��� ��ġ�Ǵ� ������ ���� ũ��
    private float   halfPieceAngleWithPaddings;     // ���� ���⸦ ����� Padding�� ���Ե� ���� ũ��

    private int     accumulatedWeight;              // ����ġ ����� ���� ����
    private bool    isSpinning = false;             // ���� ȸ�������� �˻��ϴ� ����
    private int     selectedIndex = 0;              // �귿���� ���õ� Piece

    /// <summary>
    /// Result�гο��� �� ��� �����͵�
    /// </summary>
    public RoulettePieceData resultData { get; private set; }
    private void Awake()
    {
        //Setup();
        //SpawnPiecesAndLines();
        //CalculateWeightAndIndices();

        // Debug.Log($"Index : {GetRandomIndex()}");
    }

    private void OnEnable()
    {
        Setup();
        SpawnPiecesAndLines();
    }
    private void OnDisable()
    {
        RemovePieces();
    }
    private void Setup()
    {
        // ��� ������ �ʱ�ȭ
        if (resultData != null)
        {
            resultData = null;
        }

        // �귿 �Ŵ����κ��� ������ ���� ����
        roulettePieceData = RouletteManager.Instance.GetInfoDatas();

        // �귿 ���� �ϳ��� ����
        pieceAngle                  = 360 / roulettePieceData.Length;

        // �귿 ������ ���� ����
        halfPieceAngle              = pieceAngle * .5f;

        // Padding�� ���Ե� ������ ���� ũ��
        halfPieceAngleWithPaddings  = halfPieceAngle - (halfPieceAngle * .25f);
    }
    /// <summary>
    /// ������ �ǽ��� ����
    /// </summary>
    private void RemovePieces()
    {
        var pieces = pieceParent.gameObject.GetComponentsInChildren<Transform>();
        var lines = lineParent.gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < pieces.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            Destroy(pieces[i].gameObject);
        }
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            Destroy(lines[i].gameObject);

        }
    }
    private void SpawnPiecesAndLines()
    {
        Transform piece;
        Transform line;

        // �귿 ��ġ �ʱ�ȭ
        spinningRoulette.rotation = Quaternion.Euler(0, 0, 0);

        for (int i = 0; i < roulettePieceData.Length; i++)
        {
            piece   = null;
            line    = null;

            /// !!! Tip !!!
            /// Instantiate()�� ������Ʈ ���� �� Transform Ÿ���� �������� ���� �� ��ȯ ���� Transform Ÿ������ ��ȯ��

            // �귿 ���� �������� �ǽ� �θ� ��ġ�� ����
            piece = Instantiate(piecePrefab, pieceParent.position, Quaternion.identity, pieceParent);

            // ������ �귿 ������ ���� ����(������, ����)
            piece.GetComponent<RoulettePiece>().Setup(roulettePieceData[i]);

            // ������ �귿 ���� ȸ��
            piece.RotateAround(pieceParent.position, Vector3.back, (pieceAngle * i));

            // �귿 ������ �����ϴ� ���� �θ� �� ��ġ�� ����
            line = Instantiate(linePrefab, lineParent.position, Quaternion.identity, lineParent);

            // ������ �� ȸ��
            line.RotateAround(lineParent.position, Vector3.back, (pieceAngle * i) + halfPieceAngle);
        }
    }
    /// <summary>
    /// ����ġ �ο� �Լ� �� �Լ��� �ʱ�ȭ�� ȣ���ϸ� ����ġ �ο��� ����
    /// </summary>
    private void CalculateWeightAndIndices()
    {
        for (int i = 0; i < roulettePieceData.Length; i++)
        {
            roulettePieceData[i].index = i;

            // ���� ó�� : Ȥ�ö� chance���� 0 ���ϸ� 1�� ���� �ǰ�
            if (roulettePieceData[i].chance <= 0)
            {
                roulettePieceData[i].chance = 1;
            }
            accumulatedWeight += roulettePieceData[i].chance;
            roulettePieceData[i].weight = accumulatedWeight;

            Debug.Log($"({roulettePieceData[i].index}){roulettePieceData[i].description_name} : {roulettePieceData[i].weight}");
        }
    }
    /// <summary>
    /// ����ġ �ο����� �� ȣ���� ���� ���� �Լ�
    /// </summary>
    private int GetRandomIndex() 
    {
        int weight = UnityEngine.Random.Range(0, accumulatedWeight);
        for (int i = 0; i < roulettePieceData.Length; i++)
        {
            if (roulettePieceData[i].weight > weight)
            {
                return i;
            }
        }
        return 0;
    }
    /// <summary>
    /// ����ġ ���� �������� ������ �� �Ѹ� �����ϴ� �Լ�
    /// </summary>
    private int GetRandomIndex(int userCount)
    {
        int weight = UnityEngine.Random.Range(0, userCount);
        return weight;
    }
    private IEnumerator OnSpin(float end, UnityAction<RoulettePieceData> action)
    {
        float current = 0;
        float percent = 0;

        float rotationZ = 0;
        spinningRoulette.rotation = Quaternion.Euler(0, 0, 0);

        while (percent <= 1) // 
        {
            current += Time.deltaTime;
            percent = current / spinDuration;

            // ���� ���� 0���� end(targetAngle)���� Z���� ȸ��
            rotationZ = Mathf.Lerp(0, end, spinningCurve.Evaluate(percent));
            spinningRoulette.rotation = Quaternion.Euler(0, 0, rotationZ);

            yield return null;
        }
        isSpinning = false;

        if (action != null)
        {
            action.Invoke(roulettePieceData[selectedIndex]);
        }
    }
    private void SpinningSFX_Play()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play_SFX(SoundManager.E_SFX_Name.ROULETTE_SPINNING);
        }
    }
    public void Spin(UnityAction<RoulettePieceData> action = null)
    {
        if( isSpinning == true )
        {
            return;
        }

        // �귿 ȸ�� ȿ���� ���
        SpinningSFX_Play();

        // �귿�� ��� �� ����
        selectedIndex = GetRandomIndex(roulettePieceData.Length);
        // ����� ���õ� ������ ����
        resultData = roulettePieceData[selectedIndex];

        // ���õ� ����� �߽� ����
        float   angle           = pieceAngle * selectedIndex;

        // ��Ȯ�� �߽��� �ƴ� ��� �� ���� ���� ������ ���� ����
        float leftOffset = (angle - halfPieceAngleWithPaddings) % 360;
        float rightOffset = (angle + halfPieceAngleWithPaddings) % 360;

        // �귿 ������ ���� ���� ���̿��� ���� �� �ְ� ���� ����
        float   randomAngle     = UnityEngine.Random.Range(leftOffset, rightOffset);

        // ��ǥ ����(targetAngle) = ��� ���� + 360 * ȸ�� �ð� * ȸ�� �ӵ�
        int     rotateSpeed     = 2;
        float   targetAngle     = randomAngle + (360 * spinDuration * rotateSpeed);
        
        isSpinning = true;
        StartCoroutine(OnSpin(targetAngle, action));
    }
}
