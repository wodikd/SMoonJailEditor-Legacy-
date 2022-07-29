using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TestUniRx : MonoBehaviour
{
    private BehaviorSubject<string> behaviorSubject = new(string.Empty);

    private void Start()
    {

    }
}
