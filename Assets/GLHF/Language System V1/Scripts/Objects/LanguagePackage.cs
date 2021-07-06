using System;
using System.Collections.Generic;

namespace GLHF.LanguageSystemV1
{
    [Serializable]
    public class LanguagePackage
    {
        public bool IsRTL;
        public List<LanguageToken> TokenList;
    }
}