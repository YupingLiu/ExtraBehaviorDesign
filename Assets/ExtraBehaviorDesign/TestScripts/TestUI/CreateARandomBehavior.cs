using ExtraBehaviorDesign.Runtime;
using ExtraBehaviorDesign.TestScripts;
using UnityEngine;

public class CreateARandomBehavior : MonoBehaviour
{
    public GameObject Client;
    private BehaviorManager.BehaviorTree m_BTServerCreated;
    private DeserializeBT m_deserializeBT;

    public void Start()
    {
        m_deserializeBT = Client.GetComponent<DeserializeBT>();
    }

    public void OnCreateAMiddleBTRandom()
    {
        MiddleBTRandom mbt = new MiddleBTRandom();
        m_BTServerCreated = mbt;
    }

    public void OnSerializeButtonClick()
    {
        // 服务器调用序列化
        string serializationFilePath = BehaviorTreeSerialize.Save(m_BTServerCreated);
        // 客户端调用反序列化
        if ("" == serializationFilePath)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Serialize ERRROR... CAN'T Deserialize");
#endif
        }
        else
        {
            m_deserializeBT.OnDeserializeAssetToBehavior(serializationFilePath);
        }
    }
}
