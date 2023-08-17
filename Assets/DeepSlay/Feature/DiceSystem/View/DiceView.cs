using System;
using System.Linq;
using TMPro;
using UniRx;
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
            var dieNames = Enum.GetNames(typeof(Elements)).ToList();

            dieNames = dieNames.Randomize();

            var time = 0f;

            foreach (var dieName in dieNames)
            {
                Observable.Timer(TimeSpan.FromSeconds(time)).Subscribe(_ =>
                {
                    _diceName.SetText($"{dieName}");
                });

                time += 0.1f;
            }

            Observable.Timer(TimeSpan.FromSeconds(time + 0.1f)).Subscribe(_ =>
            {
                _diceName.SetText($"{element}");
            });
        }
    }
}