﻿using AutoMapper;
using Chat.Api.Helper.Filters;
using Chat.Api.Requests;
using Chat.Api.Requests.UserRequests;
using Chat.Api.Respones.ResponesSever;
using Chat.Application.DTOs.UserApp;
using Chat.Application.Features.UserApplication.Requests.Commads;
using Chat.Application.Features.UserApplication.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/chat/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IMapper _mapper;

        public UserController(IMapper mapper, IMediator mediator,
            IWebHostEnvironment webHostEnvironment)
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper; 
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var query = new GetUserQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("/register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] UserDTO user)
        {
            var commad = new RegisterAdminCommad
            {
                UserDTOUser = user
            };
            var result = await _mediator.Send(commad);
            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(FileFormatFilter))]
        [Route("/avatar")]
        public async Task<IActionResult> UploadAvatarAsync([FromForm] AvatarRequest request)
        {
            var commad = new UpLoadAvatarCommad
            {
                Avatar = request.Image
            };

            var result = await _mediator.Send(commad);

            if(result.Success)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("/login/user")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAsync([FromBody] LoginModel user)
        {
            var commad = new LoginUserCommad
            {
                User = _mapper.Map<LoginDTO>(user)

            };
            
            var result = await _mediator.Send(commad);
            return Ok(result.Data);
        }

        [HttpPost("/refreshtoken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest authRefresh)
        {
            if (authRefresh is null)
            {
                return BadRequest();
            }
            var commad = new RefreshTokenCommad
            {
                AccessToken = authRefresh.AccessToken,
                RefreshToken = authRefresh.RefreshToken
            };
            var result = await _mediator.Send(commad);
            return Ok(result);
        }

        [HttpGet]
        [Route("/getlistfriend")]
        public async Task<IActionResult> GetListFriendAsync([FromQuery] GetFriendsRequest request)
        {
            var query = new GetListFriendQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SearchKey = request.SearchKey
            };
            var result = await _mediator.Send(query);
            var pageList = new ReponseListFriend
            {
                Data = result,
                Total = result.TotalPages,
                Success = true
            };

            return Ok(pageList);
        }
    }
}
