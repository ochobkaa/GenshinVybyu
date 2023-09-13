﻿using GenshinVybyu.Controllers.Attributes;
using GenshinVybyu.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace GenshinVybyu.Controllers
{
    public class BotController : ControllerBase
    {
        [HttpPost]
        [ValidateTelegramBot]
        public async Task<IActionResult> Post(
            [FromBody] Update update,
            [FromServices] BotHandler handler,
            CancellationToken cancellationToken)
        {
            await handler.HandleUpdateAsync(update, cancellationToken);
            return Ok();
        }
    }
}
