/*
Copyright (C) 1996-1997 Id Software, Inc.
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

// This script is based on the following piece of code
// https://github.com/id-Software/Quake/blob/bf4ac424ce754894ac8f1dae6a3981954bc9852d/QW/client/r_light.c#L33-L53

using UnityEngine;

namespace GLHF.LightStyles
{
    public class LightStyle : MonoBehaviour
    {
        [SerializeField] private string value;
        [Min(0)] [SerializeField] private int _speed = 10;
        [Min(0)] [SerializeField] private float _intensity = 1;

        private int[] _styleValueArray;

        private Light _lightSource => GetComponent<Light>();

        private void Awake()
        {
            _intensity = _lightSource.intensity;
        }

        private void Update()
        {
            SetStyleValueArray();
            AnimateLight();
        }

        private void SetStyleValueArray()
        {
            int k;
            var lightStyle = value;

            if (lightStyle == "")
            {
                lightStyle = "m";
            }

            _styleValueArray = new int[lightStyle.Length];

            for (int index = 0; index < lightStyle.Length; index++)
            {
                k = lightStyle[index] - 'a';
                k *= 22;
                _styleValueArray[index] = k;
            }
        }

        private void AnimateLight()
        {
            var k = (int)(Time.time * _speed);
            var value = k % _styleValueArray.Length;
            var newIntensity = _intensity * ((float)_styleValueArray[value] / 256);

            _lightSource.intensity = newIntensity;
        }
    }
}