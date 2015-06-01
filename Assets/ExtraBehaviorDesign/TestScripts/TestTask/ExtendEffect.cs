using ExtraBehaviorDesign.Runtime.Tasks;
using UnityEngine;
namespace ExtraBehaviorDesign.TestScripts
{
    // Play a fx
    class ExtendEffect : Action
    {
        private Transform trans;
        //private bool isNotFirst = false;
        public override void OnStart()
        {
           trans = transform.FindChild("Enemy/FX");
           removeType = RemoveType.ONLYWHENSUCCESS;
        }

        public override TaskStatus OnUpdate()
        {
            //if (isNotFirst)
            //{
            //    return TaskStatus.Success;
            //}
            GameObject go;
            if (null == trans)
            {
                go = (GameObject)Instantiate(
                    Resources.Load("Prefabs/FX", typeof(GameObject)), transform.position, Quaternion.identity);
                trans = go.transform;
                trans.parent = gameObject.transform;
            }
            else
            {
                go = trans.gameObject;
            }
            go.SetActive(true);
            //isNotFirst = true;
            return TaskStatus.Success;
        }
    }
}
