using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bozo.ModularCharacters
{
    public class DemoCharacterCreator : MonoBehaviour
    {
        [SerializeField] OutfitSystem character;
        [SerializeField] List<GameObject> _outfits = new List<GameObject>();
        [SerializeField] ColorPickerControl colorPickerControl;

        [SerializeField] Dictionary<string, int> indexes = new Dictionary<string, int>();
        [SerializeField] Dictionary<string, List<GameObject>> outfits = new Dictionary<string, List<GameObject>>();


        [SerializeField] Material[] skinPresets;
        [SerializeField] int skinIndex;
        [SerializeField] Material[] eyesPresets;
        [SerializeField] int eyesIndex;

        private void Awake()
        {
            outfits.Clear();
            foreach (var item in _outfits)
            {
                var sortingList = item.name.Split("_");
                var sorting = sortingList[1];

                if (outfits.TryGetValue(sorting, out List<GameObject> value))
                {
                    value.Add(item);
                }
                else
                {
                    indexes.Add(sorting, 0);
                    outfits.Add(sorting, new List<GameObject>());
                    outfits[sorting].Add(item);
                }
            }
        }

        public void IndexUp(string catagory)
        {
            indexes[catagory] += 1;
            if (indexes[catagory] >= outfits[catagory].Count) indexes[catagory] = 0;
            var index = indexes[catagory];

            var outfit = outfits[catagory][index];

            var inst = Instantiate(outfit, character.transform);
            SetColorPickerObject(inst.transform);


        }

        public void IndexDown(string catagory)
        {
            indexes[catagory] -= 1;
            if (indexes[catagory] < 0) indexes[catagory] = outfits[catagory].Count - 1;
            var index = indexes[catagory];

            var outfit = outfits[catagory][index];

            var inst = Instantiate(outfit, character.transform);
            SetColorPickerObject(inst.transform);
        }

        public void IndexUpSkin()
        {
            skinIndex += 1;
            if (skinIndex >= skinPresets.Length) skinIndex = 0;

            character.SetSkin(skinPresets[skinIndex]);
        }

        public void IndexDownSkin()
        {
            skinIndex -= 1;
            if (skinIndex < 0) skinIndex = skinPresets.Length - 1;

            character.SetSkin(skinPresets[skinIndex]);
        }

        public void IndexUpEyes()
        {
            eyesIndex += 1;
            if (eyesIndex >= eyesPresets.Length) eyesIndex = 0;

            character.SetEyes(eyesPresets[eyesIndex]);
        }

        public void IndexDownEyes()
        {
            eyesIndex -= 1;
            if (eyesIndex <= 0) eyesIndex = eyesPresets.Length - 1;

            character.SetEyes(eyesPresets[eyesIndex]);
        }

        public void SetColorPickerObject(string type)
        {

            var outfit = character.GetOutfit((OutfitType)Enum.Parse(typeof(OutfitType), type));
            colorPickerControl.ChangeObject(outfit);
        }

        public void SetColorPickerObject(Transform outfit)
        {
            colorPickerControl.ChangeObject(outfit);
        }
    }
}
