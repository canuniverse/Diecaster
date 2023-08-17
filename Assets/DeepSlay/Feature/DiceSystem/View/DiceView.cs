using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeepSlay
{
    public class DiceView : PoolView<DiceView>
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _diceName;

        public void SetDieFace(Elements element)
        {
            _diceName.SetText($"{element}");
        }
    }
}