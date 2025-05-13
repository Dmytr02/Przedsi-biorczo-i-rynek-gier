using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveFile
{
    //public string selfName = string.Empty;
    public SerializableVector3 playerPosition = new Vector3(16,0,5);
    public SerializableQuaternion playerRotation = Quaternion.identity;
}


