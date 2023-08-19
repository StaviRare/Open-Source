using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GLHF.MaterialType
{
    public class Example : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _colorVariant1 = Color.red;
        [SerializeField] private Color _colorVariant2 = Color.blue;
        [SerializeField] private Text _materialTypeText;

        private float _timer = 0f;
        private bool _toggleValue = false;

        private const string _shaderParameterName = "_MaterialType";

        private void Awake()
        {
            ToggleColorsAndShaderParameter();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= 5f)
            {
                _timer = 0f;
                ToggleColorsAndShaderParameter();
            }

            var currentMaterialType = (MaterialType)_renderer.material.GetFloat(_shaderParameterName);
            var materialTypeString = "Material Type is: " + currentMaterialType.ToString();
            _materialTypeText.text = materialTypeString;
        }

        private void ToggleColorsAndShaderParameter()
        {
            var material = _renderer.material;

            _toggleValue = !_toggleValue;

            if (_toggleValue)
            {
                material.color = _colorVariant1;
            }
            else
            {
                material.color = _colorVariant2;
            }

            var shaderValue = _toggleValue ? 1f : 0f;
            material.SetFloat(_shaderParameterName, shaderValue);
        }
    }
}