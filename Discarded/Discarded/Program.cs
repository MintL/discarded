using System;

namespace Discarded
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DiscardedGame game = new DiscardedGame())
            {
                game.Run();
            }
        }
    }
#endif
}

