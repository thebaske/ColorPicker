using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILinkable
{
    void SetMyLink(Transform link);
    Transform GetMyLink();
    Transform GetMyTransform();
}
