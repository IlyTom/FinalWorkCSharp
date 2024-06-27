using AutoMapper;
using MessageApi.Abstraction;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.MessageModels.DTO;
using System.IdentityModel.Tokens.Jwt;

namespace MessageApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessageController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Administrator, User")]
        [HttpGet("GetMessages")]
        public ActionResult GetNewMessage()
        {
            var senderEmail = GetUserEmailFromToken().GetAwaiter().GetResult();

            var response = _messageService.GetNewMessages(senderEmail);
            if (!response.IsSuccess)
                return BadRequest(response.Errors.FirstOrDefault()?.Message);

            return Ok(response.Messages);
        }

        [Authorize(Roles = "Administrator, User")]
        [HttpPost("SendMessage")]
        public ActionResult SendMessage(string recipientEmail, string text)
        {
            var senderEmail = GetUserEmailFromToken().GetAwaiter().GetResult();

            var message = new MessageModel
            {
                RecipientEmail = recipientEmail,
                SenderEmail = senderEmail,
                Text = text
            };

            var response = _messageService.SendMessage(message);
            if (!response.IsSuccess)
                return BadRequest(response.Errors.FirstOrDefault()?.Message);

            return Ok(response.Messages);
        }

        private async Task<string> GetUserEmailFromToken()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var claim = jwtToken!.Claims.Single(x => x.Type.Contains("emailaddress"));

            return claim.Value;

        }

    }
}
