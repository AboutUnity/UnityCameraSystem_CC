using UnityEngine;
using System.Collections.Generic;

public class Obstacle2Transparente : MonoBehaviour
{
    private GameObject player;
    public Material alphaMaterial;//͸������

    private List<RaycastHit> hits;
    private List<HitInfo> changedInfos = new List<HitInfo>();

    private struct HitInfo
    {
        public GameObject obj;
        public Renderer renderer;
        public Material material;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameManager.Instance.playerObj;
            if (player == null) { return; }
        }

        ChangeMaterial();
    }

    private void ChangeMaterial()
    {
        //�����λ�����ɫλ�õĲ�ֵ-1��Ϊ�˱����⵽���棬��߿���ֲ�ԣ��������߼�⵽�Ķ���Ҫ��ȥ����
        hits = new List<RaycastHit>(Physics.RaycastAll(transform.position, transform.forward, Vector3.Distance(this.transform.position, player.transform.position) - 1));

        for (int i = 0; i < hits.Count; i++)
        {
            //���߼�⵽�Ķ����ȥ��ɫ
            if (hits[i].collider.gameObject.name != player.name)
            {
                var hit = hits[i];
                int findIndex = changedInfos.FindIndex(item => item.obj == hit.collider.gameObject);

                //û�ҵ������
                if (findIndex < 0)
                {
                    var changed = new HitInfo();
                    changed.obj = hit.collider.gameObject;
                    changed.renderer = changed.obj.GetComponent<Renderer>();
                    changed.material = changed.renderer.material;
                    changed.renderer.material = alphaMaterial;
                    changedInfos.Add(changed);
                }
            }
        }

        for (int i = 0; i < changedInfos.Count;)
        {
            var changedInfo = changedInfos[i];
            var findIndex = hits.FindIndex(item => item.collider.gameObject == changedInfo.obj);

            //û�ҵ����Ƴ�
            if (findIndex < 0)
            {
                changedInfo.renderer.material = changedInfo.material;//��ԭ����
                changedInfos.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }
}



