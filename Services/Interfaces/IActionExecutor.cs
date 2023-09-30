﻿using GenshinVybyu.Actions.Utils;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IActionExecutor : IService
    {
        public Task Exec(
            ChatId chatId,
            Func<ActionContext, CancellationToken, Task> actionCb,
            ActionArgs args,
            CancellationToken cancellationToken
        );
    }
}
