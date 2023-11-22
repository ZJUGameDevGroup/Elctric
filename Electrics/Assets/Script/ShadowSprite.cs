using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    public Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;

    [Header("ʱ����Ʋ���")]
    public float activeTime;    //��ʾʱ��
    [ReadOnly]
    public float activeStart;   //��ʼ��ʾ��ʱ���

    [Header("��͸���ȿ���")]
    public float alphaOriginal; //͸���ȳ�ʼֵ
    public float alphaMultiplier;   //����˻�
    private float alpha;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaOriginal;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;    //��ǰʱ��
    }

    void FixedUpdate()
    {
        alpha *= alphaMultiplier;

        color = new Color(0.5f, 0.5f, 1, alpha);  //��ɫ

        thisSprite.color = color;

        //��ʱ�䳬����ʾʱ��
        if (Time.time >= activeStart + activeTime)
        {
            //���ض����
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}