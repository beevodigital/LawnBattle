using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LawnBattle.Models;

namespace LawnBattle.Helpers.DataAnnotations
{
    public class CheckEventName : ValidationAttribute
    {
        private LawnBattleContext db = new LawnBattleContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //object is a string
            if(value != null)
            {
                var CheckName = db.Events.Where(x => x.EventKey.Equals(value.ToString())).FirstOrDefault();
                if(CheckName != null)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return ValidationResult.Success;
        }
    }
}