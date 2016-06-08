
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Data.Entities;
using Data.Interfaces;
using Fr8.Infrastructure.Utilities;
using Fr8.Infrastructure.Utilities.Logging;
using Hub.Interfaces;
using Hub.Managers;
using Hub.Services;
using HubWeb.ViewModels;
using StructureMap;

namespace HubWeb.Controllers
{
    [DockyardAuthorize]
    public class DockyardAccountController : Controller
    {
        private readonly Fr8Account _account;
        private readonly IOrganization _organization;

        public DockyardAccountController()
        {
            _account = ObjectFactory.GetInstance<Fr8Account>();
            _organization = ObjectFactory.GetInstance<IOrganization>();
        }

        [AllowAnonymous]
        public ActionResult InterceptLogin(string returnUrl, string urlReferrer)
        {
            ViewBag.ReturnUrl = returnUrl;
            TempData["urlReferrer"] = urlReferrer;
            if (this.UserIsAuthenticated())
                throw new HttpException(403, "We're sorry, but you don't have permission to access this page.");
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult AccessDenied(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.UrlReferrer = TempData["urlReferrer"];
            return View();
        }

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.GuestUserEmail = TempData["tempEmail"];
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegistrationSuccessful()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            this.Logout();
            return RedirectToAction("Index", "DockyardAccount");
        }

        [AllowAnonymous]
        public ActionResult Confirm(RegistrationVM model)
        {
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
#if !DEBUG
        [ValidateAntiForgeryToken]
#endif
        public async Task<ActionResult> ProcessRegistration(RegistrationVM submittedRegData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RegistrationStatus curRegStatus;
                    OrganizationDO organizationDO = null;
                    bool isNewOrganization = false;

                    using (var uow = ObjectFactory.GetInstance<IUnitOfWork>())
                    {
                        //check for organizations 
                        if (submittedRegData.HasOrganization && !string.IsNullOrEmpty(submittedRegData.OrganizationName))
                        {
                            organizationDO = _organization.GetOrCreateOrganization(uow, submittedRegData.OrganizationName, out isNewOrganization);
                        }
                        
                        if (!String.IsNullOrWhiteSpace(submittedRegData.GuestUserTempEmail))
                        {
                            curRegStatus = await _account.UpdateGuestUserRegistration(uow, submittedRegData.Email.Trim()
                                , submittedRegData.Password.Trim()
                                , submittedRegData.GuestUserTempEmail, organizationDO);
                        }
                        else
                        {
                            curRegStatus = _account.ProcessRegistrationRequest(uow, submittedRegData.Email.Trim(), submittedRegData.Password.Trim(), organizationDO, isNewOrganization);
                        }

                        uow.SaveChanges();
                    }

                    if (curRegStatus == RegistrationStatus.UserMustLogIn)
                    {
                        ModelState.AddModelError("", @"You are already registered with us. Please login.");
                    }
                    else
                    {
                        return this.Login(new LoginVM
                        {
                            Email = submittedRegData.Email.Trim(),
                            Password = submittedRegData.Password.Trim(),
                            RememberMe = false
                        }, string.Empty).Result;
                    }
                }
            }
            catch (ApplicationException appEx)
            {
                ModelState.AddModelError("", appEx.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
#if !DEBUG
        [ValidateAntiForgeryToken]
#endif
        public async Task<ActionResult> Login(LoginVM model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string username = model.Email.Trim();
                    LoginStatus curLoginStatus =
                        await new Fr8Account().ProcessLoginRequest(username, model.Password, model.RememberMe);
                    switch (curLoginStatus)
                    {
                        case LoginStatus.InvalidCredential:
                            ModelState.AddModelError("", @"Invalid Email id or Password.");
                            break;

                        case LoginStatus.ImplicitUser:
                            ModelState.AddModelError("",
                                @"We already have a record of that email address, but No password exists for this Email id. 
Please register first.");
                            break;

                        case LoginStatus.UnregisteredUser:
                            ModelState.AddModelError("",
                                @"We do not have a registered account associated with this email address. 
Please register first.");
                            break;

                        default:
                            if (curLoginStatus == LoginStatus.Successful)
                            {
                                if (!String.IsNullOrEmpty(returnUrl))
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Welcome");
                                }
                            }
                            break;
                    }
                }
            }
            catch (ApplicationException appEx)
            {
                ModelState.AddModelError("", appEx.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            // If we got this far, something failed, redisplay form
            return View("Index", model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId, string code)
        {
            try
            {
                using (var uow = ObjectFactory.GetInstance<IUnitOfWork>())
                {
                    Fr8AccountDO curDockyardAccountDO = uow.UserRepository.FindOne(u => u.Id == userId);
                    if (curDockyardAccountDO != null)
                    {
                        curDockyardAccountDO.EmailConfirmed = true;
                        uow.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Register");
            }

            return RedirectToAction("Index", "Welcome");
        }

        [System.Web.Http.HttpPost]
        public ActionResult Edit(UserVM usersAdminVM)
        {
            using (var uow = ObjectFactory.GetInstance<IUnitOfWork>())
            {
                var userDO = uow.UserRepository.GetByKey(usersAdminVM.Id);
                userDO.Id = usersAdminVM.Id;
                userDO.FirstName = usersAdminVM.FirstName;
                userDO.LastName = usersAdminVM.LastName;
                userDO.EmailAddress = new EmailAddressDO
                {
                    Id = usersAdminVM.EmailAddressID,
                    Address = usersAdminVM.EmailAddress
                };

                userDO.EmailAddressID = usersAdminVM.EmailAddressID;
                userDO.UserName = usersAdminVM.EmailAddress;

                // Set RoleId & UserId if role is changed on the font-end other wise IdentityUserRole is set to null and user's role will not be updated.
                //uow.AspNetUserRolesRepository.AssignRoleIDToUser(usersAdminVM.RoleId, usersAdminVM.Id);

                uow.SaveChanges();
                return RedirectToAction("Index", "User");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _account.ForgotPasswordAsync(model.Email);
                    return View("ForgotPasswordConfirmation", model);
                }
                catch (Exception ex)
                {
                    //Logger.GetLogger().Error("ForgotPassword failed.", ex);
                    Logger.LogError($"ForgotPassword failed. Exception = {ex}");
                    ModelState.AddModelError("", ex);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            using (var uow = ObjectFactory.GetInstance<IUnitOfWork>())
            {
                var userDO = uow.UserRepository.GetByKey(userId);
                if (userDO == null)
                    return HttpNotFound();
                return View(
                    new ResetPasswordVM()
                    {
                        UserId = userId,
                        Code = code,
                        Email = userDO.EmailAddress.Address
                    });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordVM viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _account.ResetPasswordAsync(viewModel.UserId, viewModel.Code, viewModel.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation", viewModel);
                    }
                    else
                    {
                        // http://forums.asp.net/t/1934149.aspx?Password+Reset+Token+Expiration
                        // Refer above link for checking the reset password is link expired or not
                        ModelState.AddModelError("", "Reset password link has been expired. Please generate the new link.");
                        Array.ForEach(result.Errors.ToArray(), e => ModelState.AddModelError("", e));
                    }
                }
                catch (Exception ex)
                {
                    //Logger.GetLogger().Error("ResetPassword failed.", ex);
                    Logger.LogError($"ResetPassword failed. Email = {viewModel.Email}; UserId = {viewModel.UserId} Exception = {ex}");
                    ModelState.AddModelError("", ex);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        public RedirectToRouteResult RegisterGuestUser()
        {
            TempData["tempEmail"] = User.Identity.Name;
            this.Logout();
            return RedirectToAction("Register");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ProcessGuestUserMode()
        {
            LoginStatus loginStatus = await _account.CreateAuthenticateGuestUser();
            return RedirectToAction("Index", "Welcome");
        }
    }

}