using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Battle.UI
{
    public class CanvasDistanceController : MonoBehaviour
    {
        [SerializeField]
        private float _distanceOffset = 0.0F;
        [SerializeField]
        private Transform _target;
        
        private Canvas _playerCanvas;
        private Transform _cameraTransform;

        private Canvas PlayerCanvas
        {
            get
            {
                if (!_playerCanvas)
                {
                    _playerCanvas = GetComponent<Canvas>();
                }

                return _playerCanvas;
            }
        }

        private Transform CameraTransform
        {
            get
            {
                if (!_cameraTransform)
                {
                    _cameraTransform = Camera.main.transform;
                }

                return _cameraTransform;
            }
        }

        private void Awake()
        {
            _playerCanvas = GetComponent<Canvas>();
            _cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            CalcDistance();
        }

        private void OnValidate()
        {
            CalcDistance();
        }

        private void CalcDistance()
        {
            if (PlayerCanvas.renderMode != RenderMode.ScreenSpaceCamera) return;

            var positionCanvasLocal = CameraTransform.InverseTransformPoint(_target.position).z;
            positionCanvasLocal += _distanceOffset;

            PlayerCanvas.planeDistance = positionCanvasLocal;
        }
    }   
}