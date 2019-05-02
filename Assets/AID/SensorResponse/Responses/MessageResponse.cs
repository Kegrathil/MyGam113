using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AID
{
    public class MessageResponse : ABResponse
    {

        public string msg;
        public List<GameObject> sendMsgTo = new List<GameObject>();

        public override void Fire(SensorResponseRouter r)
        {
            gameObject.SendMessageToAll(sendMsgTo, msg, SendMessageOptions.DontRequireReceiver);
        }
    }
}