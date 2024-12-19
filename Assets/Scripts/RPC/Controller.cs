using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RPC
{
    public class Controller : MonoBehaviour
    {
        private Content _turkChoice;

        private void Awake()
        {
            _turkChoice = Enum.GetValues(typeof(Content)).OfType<Enum>().OrderBy(e => Guid.NewGuid()).Cast<Content>().FirstOrDefault();
            Debug.Log(_turkChoice);
        }

        private void Start()
        {
            
        }

        private IEnumerator BoardLoadedDelay()
        {
            yield return new WaitForSeconds(2f);
        }
    }
}