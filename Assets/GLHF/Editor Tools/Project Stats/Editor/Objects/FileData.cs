using System;

namespace GLHF.ProjectStats
{
    [Serializable]
    public class FileData
    {
        public string LastDataGatherDate;

        public int ScriptFileCount;
        public int ScriptLineCount;
        public int ScriptWordCount;

        public int PrefabCount;
        public int FolderCount;
        public int MaterialCount;
    }
}