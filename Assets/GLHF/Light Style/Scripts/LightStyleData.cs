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
// https://github.com/id-Software/Quake/blob/bf4ac424ce754894ac8f1dae6a3981954bc9852d/QW/progs/world.qc#L302-L345

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GLHF.LightStyles
{
    [Serializable]
    public class LightStyleData : ScriptableObject
    {
        public List<LightStyleObject> Items;

        private void Reset()
        {
            Items = new List<LightStyleObject>();

            AddLightStyle("Normal", "m");
            AddLightStyle("Flicker 1", "mmnmmommommnonmmonqnmmo");
            AddLightStyle("Slow Strong Pulse", "abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba");
            AddLightStyle("Candle 1", "mmmmmaaaaammmmmaaaaaabcdefgabcdefg");
            AddLightStyle("Fast Strobe", "mamamamamama");
            AddLightStyle("Gentle Pulse", "jklmnopqrstuvwxyzyxwvutsrqponmlkj");
            AddLightStyle("Flicker 2", "nmonqnmomnmomomno");
            AddLightStyle("Candle 2", "mmmaaaabcdefgmmmmaaaammmaamm");
            AddLightStyle("Candle 3", "mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa");
            AddLightStyle("Slow Strobe", "aaaaaaaazzzzzzzz");
            AddLightStyle("Fluorescent Flicker", "mmamammmmammamamaaamammma");
            AddLightStyle("Slow Pulse Not Fade To Black", "abcdefghijklmnopqrrqponmlkjihgfedcba");
        }

        private void AddLightStyle(string name, string value)
        {
            var newLightStyle = new LightStyleObject()
            {
                Name = name,
                Value = value
            };

            Items.Add(newLightStyle);
        }
    }
}