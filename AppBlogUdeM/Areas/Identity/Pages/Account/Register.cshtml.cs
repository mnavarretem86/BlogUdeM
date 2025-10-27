// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BlogCore.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>




        public class EmailDomainAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var email = value as string;
                if (string.IsNullOrEmpty(email) || !email.EndsWith("@udem.edu.ni"))
                {
                    return new ValidationResult("Se requiere un correo con el dominio '@udem.edu.ni'.");
                }

                return ValidationResult.Success;
            }
        }





        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required (ErrorMessage ="Favor Ingrese una dirección valida")]
            [EmailDomain] // 
            [EmailAddress]
            [Display(Name = "Correo")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
            [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} y como máximo {1} caracteres de longitud.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Contraseña")]
            [Compare("Password", ErrorMessage = "Las contraseñas ingresadas no coinciden.")]
            public string ConfirmPassword { get; set; }

            //Campos personalizados, los mismos de el modelo ApplicationUser
            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "La dirección es obligatoria")]
            public string Direccion { get; set; }

            [Required(ErrorMessage = "La ciuedad es obligatoria")]
            public string Ciudad { get; set; }

            [Required(ErrorMessage = "El número de carnet es obligatorio")]
            [RegularExpression(@"^\d{6}$", ErrorMessage = "El número de carnet no es valido")]
            public string carnet { get; set; }

            [Required(ErrorMessage = "El teléfono es obligatorio")]
            [RegularExpression(@"^\d{8}$", ErrorMessage = "El número de telefono no es valido")]
            [Display(Name = "Teléfono")]
            public string PhoneNumber { get; set; }

            public string Rol { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Nombre = Input.Nombre;
                user.Direccion = Input.Direccion;
                user.Ciudad = Input.Ciudad;
                user.Carnet = Input.carnet;
                user.PhoneNumber = Input.PhoneNumber;


                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(CNT.Administrador))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(CNT.Administrador));
                        await _roleManager.CreateAsync(new IdentityRole(CNT.Moderador));
                        await _roleManager.CreateAsync(new IdentityRole(CNT.Registrado));
                        await _roleManager.CreateAsync(new IdentityRole(CNT.Cliente));
                    }

                    // Obtiene el rol seleccionado correctamente
                    string rol = Request.Form["Input.UserRole"].ToString();

                    if (string.IsNullOrEmpty(rol))
                    {
                        rol = CNT.Cliente;
                    }


                    user.Rol = rol;

                    user.Rol = rol;

                    if (rol == CNT.Administrador)
                    {
                        await _userManager.AddToRoleAsync(user, CNT.Administrador);

                    }
                    else if (rol == CNT.Registrado)
                    {
                        await _userManager.AddToRoleAsync(user, CNT.Registrado);

                    }
                    else if (rol == CNT.Moderador)
                    {
                        await _userManager.AddToRoleAsync(user, CNT.Moderador);

                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, CNT.Cliente);
                    }

                    _logger.LogInformation("Usuario creado con éxito con el rol seleccionado.");
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: true);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }



        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}