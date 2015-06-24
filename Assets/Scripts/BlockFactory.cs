using UnityEngine;
using System.Collections;

public class BlockFactory : MonoBehaviour {

	// Use this for initialization
    public GameObject[] blockList;
    public GameObject[] bananaList;
    public float minYOffset = 0;
    public float maxYOffset = 1;
    public float minXOffset = -0.5f;
    public float maxXOffset = 0.5f;
    public float minGap = 11;
    public float maxGap = 14;
    public Transform lastBlock;
    private static BlockFactory _instant;
	void Start () {
        _instant = this;
	}

    public static BlockFactory getInstance()
    {
        return _instant;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
    }


    public GameObject CreatBlock()
    {
        Object prefab = blockList[Random.Range(0, blockList.Length)];

        GameObject obj = Instantiate(prefab) as GameObject;
        GameObject obj2 = Instantiate(prefab) as GameObject;

        float offsetX = Random.Range(minXOffset,maxXOffset);
        float offsetY = Random.Range(minYOffset,maxYOffset);
        float gap = Random.Range(minGap, maxGap);
        float oldWidth = GetBlockWidth(lastBlock.gameObject);
        float newWidth = GetBlockWidth(obj);
        obj.transform.position = new Vector3(lastBlock.transform.position.x+oldWidth + newWidth + offsetX, offsetY);
        obj2.transform.position = obj.transform.position + new Vector3(0, gap);
        lastBlock = obj.transform;

        Object bananaPrefab = bananaList[Random.Range(0, bananaList.Length)];
        GameObject banana = Instantiate(bananaPrefab) as GameObject;
        float bananaOffsetX = 0;
        float bananaOffsetY = 6;
        banana.transform.position = new Vector3(obj.transform.position.x + bananaOffsetX, obj.transform.position.y + bananaOffsetY, 0);

        return obj;
    }

    private float GetBlockWidth(GameObject block)
    {
        return block.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

}
