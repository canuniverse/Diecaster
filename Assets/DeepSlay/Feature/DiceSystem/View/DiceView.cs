using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DeepSlay
{
    public class DiceView : PoolView<DiceView>, IPointerEnterHandler,
        IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private List<Image> _spellIcons;

        public int DiceIndex { get; set; }
        public Elements Element { get; set; }
        public string Spell { get; set; }

        private SignalBus _signalBus;
        private AtlasConfig _atlasConfig;
        private BattlePhaseRepository _phaseRepository;

        [Inject]
        private void Construct(
            SignalBus signalBus,
            AtlasConfig atlasConfig,
            BattlePhaseRepository phaseRepository)
        {
            _signalBus = signalBus;
            _atlasConfig = atlasConfig;
            _phaseRepository = phaseRepository;
        }

        protected override void SetActive()
        {
            transform.DOScale(1, 0);
            
            Spell = string.Empty;
            _iconImage.gameObject.SetActive(false);
            _spellIcons.ForEach(view => view.gameObject.SetActive(false));

            base.SetActive();
        }

        public void SetSpell(SpellModel spellModel)
        {
            Spell = spellModel.Name;
            for (var i = 0; i < _spellIcons.Count; i++)
            {
                var icon = _spellIcons[i];
                var element = spellModel.Combinations[0].Combination[i];
                icon.sprite = _atlasConfig.IconAtlas.GetSprite($"{element}-icon");
                icon.gameObject.SetActive(true);
            }
        }

        private void ShowIcon(string iconName)
        {
            var sprite = _atlasConfig.IconAtlas.GetSprite(iconName + "-icon");
            _iconImage.sprite = sprite;
            _iconImage.gameObject.SetActive(sprite != null);
        }

        public void SetDieFace(Elements element)
        {
            var dieNames = Enum.GetNames(typeof(Elements)).ToList();


            dieNames = dieNames.Randomize();

            var time = 0f;

            foreach (var dieName in dieNames)
            {
                Observable.Timer(TimeSpan.FromSeconds(time)).Subscribe(_ => { ShowIcon($"{dieName}"); });

                time += 0.1f;
            }

            Observable.Timer(TimeSpan.FromSeconds(time + 0.1f)).Subscribe(_ => { ShowIcon($"{element}"); });

            Element = element;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_phaseRepository.BattlePhase == BattlePhase.Discard)
            {
                CursorService.SetCursor(CursorType.CursorDiscard);
            }
            else if (_phaseRepository.BattlePhase == BattlePhase.SelectSpell
                     && !string.IsNullOrEmpty(Spell))
            {
                CursorService.SetCursor(CursorType.CursorSelect);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorService.SetCursor(CursorType.CursorDefault);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_phaseRepository.BattlePhase == BattlePhase.Discard)
            {
                CursorService.SetCursor(CursorType.CursorDefault);
                _signalBus.Fire(new DiscardDiceSignal { DieIndex = DiceIndex });
            }
            else if (_phaseRepository.BattlePhase == BattlePhase.SelectSpell
                     && !string.IsNullOrEmpty(Spell))
            {
                _signalBus.Fire(new SpellSelectedSignal { SpellName = Spell });
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_phaseRepository.BattlePhase == BattlePhase.SelectSpell
                && !string.IsNullOrEmpty(Spell))
            {
                CursorService.SetCursor(CursorType.CursorSelect);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_phaseRepository.BattlePhase == BattlePhase.SelectSpell
                && !string.IsNullOrEmpty(Spell))
            {
                CursorService.SetCursor(CursorType.CursorSelectTap);
            }
        }

        public void Disappear(Action onComplete = null)
        {
            transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}