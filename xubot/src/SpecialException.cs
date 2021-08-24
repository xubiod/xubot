using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src
{
    public static class SpecialException
    {
        public class ItsFuckingBrokenException : Exception
        {
            public ItsFuckingBrokenException() { }
            public ItsFuckingBrokenException(string message) : base(message) { }
            public ItsFuckingBrokenException(string message, Exception inner) : base(message, inner) { }
        }

        public class HaveNoFuckingIdeaException : Exception
        {
            public HaveNoFuckingIdeaException() { }
            public HaveNoFuckingIdeaException(string message) : base(message) { }
            public HaveNoFuckingIdeaException(string message, Exception inner) : base(message, inner) { }
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

        public class HateDiscordDotNetException : Exception
        {
            public HateDiscordDotNetException() { }
            public HateDiscordDotNetException(string message) : base(message) { }
            public HateDiscordDotNetException(string message, Exception inner) : base(message, inner) { }
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

        public class CannotBeArsedToFixThisException : Exception
        {
            public CannotBeArsedToFixThisException() { }
            public CannotBeArsedToFixThisException(string message) : base(message) { }
            public CannotBeArsedToFixThisException(string message, Exception inner) : base(message, inner) { }
        }

        public class DeprecatedToBeRemoved : Exception
        {
            public DeprecatedToBeRemoved(string message = "This command is going to be removed.") : base(message) { }
            public DeprecatedToBeRemoved(Exception inner, string message = "This command is going to be removed.") : base(message, inner) { }
        }
    }
}
