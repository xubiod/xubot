using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot_core.src
{
    public static class SpecialException
    {
        public class ItsFuckingBrokenException : Exception
        {
            public ItsFuckingBrokenException() { }
            public ItsFuckingBrokenException(string message) : base(message) { }
            public ItsFuckingBrokenException(string message, Exception inner) : base(message, inner) { }
        }

        public class IHaveNoFuckingIdeaException : Exception
        {
            public IHaveNoFuckingIdeaException() { }
            public IHaveNoFuckingIdeaException(string message) : base(message) { }
            public IHaveNoFuckingIdeaException(string message, Exception inner) : base(message, inner) { }
        }

        public class PleaseKillMeException : Exception
        {
            public PleaseKillMeException() { }
            public PleaseKillMeException(string message) : base(message) { }
            public PleaseKillMeException(string message, Exception inner) : base(message, inner) { }
        }

        public class ShitCodeException : Exception
        {
            public ShitCodeException() { }
            public ShitCodeException(string message) : base(message) { }
            public ShitCodeException(string message, Exception inner) : base(message, inner) { }
        }

        public class StopDoingThisMethodException : Exception
        {
            public StopDoingThisMethodException() { }
            public StopDoingThisMethodException(string message) : base(message) { }
            public StopDoingThisMethodException(string message, Exception inner) : base(message, inner) { }
        }

        public class ExceptionException : Exception
        {
            public ExceptionException() { }
            public ExceptionException(string message) : base(message) { }
            public ExceptionException(string message, Exception inner) : base(message, inner) { }
        }

        public class InsertBetterExceptionNameException : Exception
        {
            public InsertBetterExceptionNameException() { }
            public InsertBetterExceptionNameException(string message) : base(message) { }
            public InsertBetterExceptionNameException(string message, Exception inner) : base(message, inner) { }
        }

        public class IHateDiscordDotNetException : Exception
        {
            public IHateDiscordDotNetException() { }
            public IHateDiscordDotNetException(string message) : base(message) { }
            public IHateDiscordDotNetException(string message, Exception inner) : base(message, inner) { }
        }

        public class FuckYouException : Exception
        {
            public FuckYouException() { }
            public FuckYouException(string message) : base(message) { }
            public FuckYouException(string message, Exception inner) : base(message, inner) { }
        }

        public class IfThisExceptionShowsUpMyWillToLiveWillDropByARatherLot : Exception
        {
            public IfThisExceptionShowsUpMyWillToLiveWillDropByARatherLot() { }
            public IfThisExceptionShowsUpMyWillToLiveWillDropByARatherLot(string message) : base(message) { }
            public IfThisExceptionShowsUpMyWillToLiveWillDropByARatherLot(string message, Exception inner) : base(message, inner) { }
        }
    }
}
