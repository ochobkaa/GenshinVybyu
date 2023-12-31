﻿using GenshinVybyu.Actions.Utils;

namespace GenshinVybyu.Actions.Interfaces
{
    public interface IBotAction
    {
        public string Name { get; }
        public IEnumerable<string>? Tokens { get; }

        public Task Run(ActionContext actionContext, CancellationToken cancellationToken);
    }
}
