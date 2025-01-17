﻿using System.Collections.Generic;

namespace Indice.AspNetCore.Identity.Models
{
    /// <summary>The register request model.</summary>
    public class RegisterRequest
    {
        /// <summary>Helper class that allows for extending the register request with custom attributes as needed per application.</summary>
        public class Attribute
        {
            /// <summary>The equivelant for a claim type.</summary>
            public string Name { get; set; }
            /// <summary>The value.</summary>
            public string Value { get; set; }
        }

        /// <summary>The first name.</summary>
        public string FirstName { get; set; }
        /// <summary>The last name.</summary>
        public string LastName { get; set; }
        /// <summary>The username that will be used.</summary>
        public string UserName { get; set; }
        /// <summary>The password.</summary>
        public string Password { get; set; }
        /// <summary>The password confirmed (optional).</summary>
        public string PasswordConfirmation { get; set; }
        /// <summary>The users email.</summary>
        public string Email { get; set; }
        /// <summary>The users phone number. Usualy used to store the mobile phone in order later on enable 2 factor authentication scenarios through SMS.</summary>
        public string PhoneNumber { get; set; }
        /// <summary>The return url is used to keep track of the origilal intent of the user when he landed on login and switched over to register.</summary>
        public string ReturnUrl { get; set; }
        /// <summary>The privacy policy is read.</summary>
        public bool HasReadPrivacyPolicy { get; set; }
        /// <summary>The terms and conditions have been accepted.</summary>
        public bool HasAcceptedTerms { get; set; }
        /// <summary>List of claims where each item is formatted as claimType:claimValue collection of strings.</summary>
        public List<Attribute> Claims { get; set; } = new List<Attribute>();
        /// <summary>The id of the current client in the request. </summary>
        public string ClientId { get; set; }

        /// <summary>Replace claim.</summary>
        /// <param name="name">Claim name.</param>
        /// <param name="value">Claim value.</param>
        protected void ReplaceClaim(string name, string value) {
            Claims.RemoveAll(x => x.Name.Equals(name));
            Claims.Add(new Attribute { Name = name, Value = value });
        }
    }

    /// <summary>Change password model.</summary>
    public class ChangePasswordModel
    {
        /// <summary>The original password.</summary>
        public string OldPassword { get; set; }
        /// <summary>The new password.</summary>
        public string NewPassword { get; set; }
        /// <summary>The new password confirmed (optional).</summary>
        public string NewPasswordConfirmation { get; set; }
    }

    /// <summary>Triggers the initiation for a password reset.</summary>
    public class ForgotPasswordRequest
    {
        /// <summary>The user's email.</summary>
        public string Email { get; set; }
        /// <summary>The url to return to.</summary>
        public string ReturnUrl { get; set; }
    }

    /// <summary>Completes the password reset.</summary>
    public class ResetPasswordModel
    {
        /// <summary>Username of the user to reset.</summary>
        public string Username { get; set; }
        /// <summary>The reset token sent.</summary>
        public string Token { get; set; }
        /// <summary>The new password.</summary>
        public string NewPassword { get; set; }
        /// <summary>The new password confirmed (optional).</summary>
        public string NewPasswordConfirmation { get; set; }
    }

    /// <summary>The model containing a verification token generated by the system.</summary>
    public class VerifyTokenModel
    {
        /// <summary>The token.</summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// Completes the password reset process. This is exact equivelant of the <see cref="ResetPasswordModel"/>. 
    /// Maybe we need to retire one of them. They only differ by their use of the Email or username fields for user retrieval.
    /// </summary>
    public class ForgotPasswordVerifyModel : VerifyTokenModel
    {
        /// <summary>The user's email.</summary>
        public string Email { get; set; }
        /// <summary>The new password.</summary>
        public string NewPassword { get; set; }
        /// <summary>The new password confirmed (optional).</summary>
        public string NewPasswordConfirmation { get; set; }
        /// <summary>The url to return to.</summary>
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// The password expired model is used after login when a user must change his password.
    /// Notice there is no requirement for the existing password here.
    /// </summary>
    public class PasswordExpiredModel
    {
        /// <summary>The new password.</summary>
        public string NewPassword { get; set; }
        /// <summary>The new password confirmed.</summary>
        public string NewPasswordConfirmation { get; set; }
    }
}
