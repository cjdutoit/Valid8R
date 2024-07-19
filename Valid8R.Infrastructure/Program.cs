// ------------------------------------------------------------
// Copyright (c)  The Standard Community - All rights reserved.
// ------------------------------------------------------------


// ------------------------------------------------------------
// Copyright (c)  The Standard Community - All rights reserved.
// ------------------------------------------------------------

using Valid8.Infrastructure.Services;

namespace Valid8R.Infrastructure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var scriptGenerationService = new ScriptGenerationService();
            scriptGenerationService.GenerateBuildScript();
        }
    }
}