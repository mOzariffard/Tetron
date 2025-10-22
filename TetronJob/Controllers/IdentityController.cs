﻿using Framework.CQRS.Command.Admin.User;
using Framework.ViewModels.Category;
using Framework.ViewModels.City;
using Framework.ViewModels.Province;
using Framework.ViewModels.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Framework.CQRS.Notification.Master.User;


namespace TetronJob.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/Profile/Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var result = await _mediator.Send(command);
            return Json(result);
        }

        public async Task<IActionResult> SignOut()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("/");
            }

            await _mediator.Publish(new SignOut());
            return Redirect("/");
        }




        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/Profile/Dashboard");
            }
            await Provinces();
            await Categories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>
            SignUpStepOne([FromBody] InsertUserViewModel model)
        {
            var result = await _mediator.Send(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SetAddress([FromBody]UserAddress address)
        {
            await _mediator.Send(new SetUserAddressViewModel()
            {
                CityId = Guid.Parse(address.CityId!) ,UserId = Guid.Parse(address.UserId!),ProvinceId = Guid.Parse(address.ProvinceId!),Get = false
            });

            return Ok(StatusCode(200));
        }



        [HttpPost]
        public async Task<IActionResult> SetCategories([FromBody]UserCategoryViewModel model)
        {
            var result = await _mediator.Send(new UserCategoryViewModel
            {
                UserId = model.UserId
                ,
                CategoryIds = model.CategoryIds,
                Get = false
            });
            return Ok(StatusCode(200));
        }

        [HttpPost]
        public async Task<IActionResult> SendOtp([FromBody] SendOtp model)
        {
            var result=  await _mediator.Send(model);
            return Json(result);
        }
        public IActionResult SignInOtp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignInOtp([FromBody] SignInOtp model)
        {
            var result = await _mediator.Send(model);
            return Json(result);
        }















        public async Task Provinces()
        {
            var result = await _mediator.Send(new RequestGetProvinces());
            ViewBag.Provinces = new SelectList(result, "Id", "Name");
        }
        [HttpGet]
        public async Task<JsonResult> Cities(Guid id)
        {
            var result = await _mediator.Send(new RequestGetCities()
            {
                Id = id
            });
            return Json(result);
        }

        public async Task Categories()
        {
            var result = await _mediator.Send(new RequestGetCategories());
            ViewBag.Categories = result;
        }

    }
}
