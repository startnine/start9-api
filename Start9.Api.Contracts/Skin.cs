using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Start9.Api.Contracts
{
    [Serializable]
    public abstract class Skin
    {
        public static Skin Default { get; } = new PlexSkin();

        public abstract IDictionary Resources { get; } // WPF makes me sad, it appears they never updated it past the no-generics days :(
    }

    [Serializable]
    sealed class PlexSkin : Skin
    {
        public override IDictionary Resources => throw new NotImplementedException();
    }
}
