using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bozo.ModularCharacters
{
    public class OutfitHeightChange : MonoBehaviour
    {
        [SerializeField] float HeightOffset;
        private void Start()
        {
            var System = GetComponentInParent<OutfitSystem>();
            if (System == null) return;

            System.transform.localPosition = new Vector3 (System.transform.localPosition.x, HeightOffset, System.transform.localPosition.z);
        }
    }
}
