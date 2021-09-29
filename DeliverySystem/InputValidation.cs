using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DeliverySystem
{
    public class InputValidation
    {
        String NAME_PATTERN = @"[a-zA-Z0-9,_\\-]{1,50}";
        String EMAIL_PATTERN = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        String PHONE_PATTERN = @"^\(\d{3}\) \d{3}-\d{4}$";
        String ZIPCODE_PATTERN = @"[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]";
        String ADDRESS_PATTERN = @"[a-zA-Z0-9,_\\-]{1,100}";

        public bool ValidateName(string name)
        {
            if (name.Length < 2 || name.Length > 50)
            {
                return false;
            }
            Regex reg = new Regex(NAME_PATTERN);
            if (!reg.IsMatch(name))
            {
                return false;
            }
            return true;
        }

        public bool ValidateEmail(string email)
        {
            if (email.Length == 0)
            {
                return false;
            }
            Regex reg = new Regex(EMAIL_PATTERN);
            if (!reg.IsMatch(email))
            {
                return false;
            }
            return true;
        }

        public bool ValidatePhone(string phone)
        {
            if (phone.Length == 0)
            {
                return false;
            }
            Regex reg = new Regex(PHONE_PATTERN);
            if (!reg.IsMatch(phone))
            {
                return false;
            }
            return true;
        }

        public bool ValidateZipcode(string zipcode)
        {
            if (zipcode.Length == 0)
            {
                return false;
            }
            Regex reg = new Regex(ZIPCODE_PATTERN);
            if (!reg.IsMatch(zipcode))
            {
                return false;
            }
            return true;
        }

        public bool ValidateAddress(string address)
        {
            if (address.Length == 0)
            {
                return false;
            }
            Regex reg = new Regex(ADDRESS_PATTERN);
            if (!reg.IsMatch(address))
            {
                return false;
            }
            return true;
        }

    }
}
