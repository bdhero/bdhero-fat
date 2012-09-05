using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDInfo.controllers
{
    class ChapterGen : AbstractExternalTool
    {
        private string dll_path;
        private string exe_path;

        protected override string GetToolName() { return "ChapterGen"; }

        protected override void ExtractResources()
        {
            dll_path = this.ExtractResource("MediaInfo.dll");
            exe_path = this.ExtractResource("ChapterGen.exe");
        }

        public override void Test()
        {
            ExtractResources();



            Cleanup();
        }
    }
}
