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

// This LightStyle system is based on the following source code
// https://github.com/id-Software/Quake/blob/bf4ac424ce754894ac8f1dae6a3981954bc9852d/QW/client/r_light.c#L33-L53


using UnityEngine;
using System;

namespace GLHF.LightStyle
{
    public static class LightStyleUtility
    {
        static LightStyleUtility()
        {
            var styleLenght = Enum.GetValues(typeof(LightStyleType)).Length;

            if (styleLenght != Value.Length)
            {
                Debug.LogError("Fetal error!");
            }
        }

        // 'm' is normal light, 'a' is no light, 'z' is double bright
        public static readonly string[] Value =
        {
		    // 0 Normal
		    "m",

		    // 1 Flicker (first variety)
		    "mmnmmommommnonmmonqnmmo",

		    // 2 Slow Strong Pulse
		    "abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba",

		    // 3 Candle (first variety)
		    "mmmmmaaaaammmmmaaaaaabcdefgabcdefg",

		    // 4 Fast Strobe
		    "mamamamamama",

		    // 5 Gentle Pulse 1
		    "jklmnopqrstuvwxyzyxwvutsrqponmlkj",

		    // 6 Flicker (second variety)
		    "nmonqnmomnmomomno",

		    // 7 Candle (second variety)
		    "mmmaaaabcdefgmmmmaaaammmaamm",

		    // 8 Candle (third variety)
		    "mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa",

		    // 9 Slow Strobe (fourth variety)
		    "aaaaaaaazzzzzzzz",

		    // 10 Fluorescent Flicker
		    "mmamammmmammamamaaamammma",

		    // 11 Slow Pulse Not Fade To Black
		    "abcdefghijklmnopqrrqponmlkjihgfedcba",
        };
    }
}