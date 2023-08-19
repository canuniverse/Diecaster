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
    public class DiceView : PoolView<DiceView>, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private List<Image> _spellIcons;
        [SerializeField] private DraggableView _draggableView;
        [SerializeField] private Canvas _canvas;

        public int DiceIndex { get; set; }
        public Elements Element { get; private set; }
        public string Spell { get; private set; }

        private SignalBus _signalBus;
        private AtlasConfig _atlasConfig;
        private EnemyAreaView _enemyAreaView;
        private BattlePhaseRepository _phaseRepository;

        public Transform Parent { get; set; }

        [Inject]
        private void Construct(
            SignalBus signalBus,
            AtlasConfig atlasConfig,
            EnemyAreaView enemyAreaView,
            BattlePhaseRepository phaseRepository)
        {
            _signalBus = signalBus;
            _atlasConfig = atlasConfig;
            _enemyAreaView = enemyAreaView;
            _phaseRepository = phaseRepository;
        }

        private void OnEnable()
        {
            _draggableView.OnDragBegan += OnDragBegan;
            _draggableView.OnDragEnded += OnDragEnded;
        }

        private void OnDisable()
        {
            _draggableView.OnDragBegan -= OnDragBegan;
            _draggableView.OnDragEnded -= OnDragEnded;
        }

        private void OnDragEnded(PointerEventData eventData)
        {
            if (!string.IsNullOrEmpty(Spell))
            {
                _canvas.overrideSorting = false;
                CursorService.SetCursor(CursorType.CursorGrab);

                CheckTargets();
            }
        }

        private void CheckTargets()
        {
            foreach (var enemyView in _enemyAreaView.EnemyViews)
            {
                var isIntersected = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform,
                    ((RectTransform)enemyView.transform).position);

                if (!isIntersected)
                {
                    continue;
                }
                
                _signalBus.Fire(new SpellSelectedSignal { DiceView = this });
                _signalBus.Fire(new EnemySelectedSignal { EnemyView = enemyView });
            }
            
            transform.position = Parent.position;
        }

        private void OnDragBegan(PointerEventData eventData)
        {
            if (!string.IsNullOrEmpty(Spell))
            {
                _canvas.overrideSorting = true;
                CursorService.SetCursor(CursorType.CursorGrabbing);
            }
        }

        protected override void SetActive()
        {
            transform.DOScale(1, 0);
            _draggableView.enabled = false;

            Spell = string.Empty;
            _iconImage.gameObject.SetActive(false);
            _spellIcons.ForEach(view => view.gameObject.SetActive(false));

            base.SetActive();
        }

        public void SetSpell(SpellModel spellModel)
        {
            Spell = spellModel.Name;
            _draggableView.enabled = true;

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
            else if (!string.IsNullOrEmpty(Spell))
            {
                CursorService.SetCursor(CursorType.CursorGrab);
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
        }

        public void Disappear(Action onComplete = null)
        {
            transform.DOScale(0, 0.5f).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}