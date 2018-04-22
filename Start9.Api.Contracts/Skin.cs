using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Start9.Api.Contracts
{
    [Serializable]
    public abstract class Skin
    {
        public static Skin Default { get; } = new PlexSkin();
        // TODO: everything
    }

    [Serializable]
    sealed class PlexSkin : Skin
    {

    }
}
