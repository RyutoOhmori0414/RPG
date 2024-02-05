using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Battle.UI
{
    [RequireComponent(typeof(Graphics))]
    public class FollowUIController : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetTransform;

        private RectTransform _rectTransform;
        private Camera _mainCam;
        private RectTransform _canvasRect;

        private RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        private Camera MainCam
        {
            get
            {
                if (!_mainCam)
                {
                    _mainCam = Camera.main;
                }

                return _mainCam;
            }
        }

        private RectTransform CanvasRect
        {
            get
            {
                if (!_canvasRect)
                {
                    _canvasRect = transform.root.GetComponent<RectTransform>();
                }

                return _canvasRect;
            }
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _mainCam = Camera.main;
            _canvasRect = transform.root.GetComponent<RectTransform>();
            Debug.Log(_canvasRect.gameObject.name);
        }

        private void LateUpdate()
        {
            CalcRectPos();
        }

        private void CalcRectPos()
        {
            var sizeDelta = CanvasRect.sizeDelta;
            var positionView = MainCam.WorldToViewportPoint(_targetTransform.position);

            var canvasPos = new Vector2(
                (positionView.x * sizeDelta.x) - (sizeDelta.x * 0.5F),
                (positionView.y * sizeDelta.y) - (sizeDelta.y * 0.5F)
            );

            _rectTransform.anchoredPosition = canvasPos;
        }
    }   
}