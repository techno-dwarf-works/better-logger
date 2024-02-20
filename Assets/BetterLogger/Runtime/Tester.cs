using System;
using UnityEngine;

namespace Better.Logger.Runtime
{
    public class Tester : MonoBehaviour
    {
        [SerializeField] private bool _test;

        private void OnValidate()
        {
            Debug.Log(DateTime.Now, this);
        }
    }
}