using DeliverySystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FieldValidation
{
    [TestClass]
    public class FieldsValidations
    {
        InputValidation inputvalidation;

        [TestInitialize]
        public void TestInitialize()
        {
            inputvalidation = new InputValidation();
        }
        
        [TestMethod]
        public void TestName_NameIsNull_ReturnsFalse()
        {
            string name = "";
            bool result = inputvalidation.ValidateName(name);

            Assert.AreEqual(result,false,"Name is null");
        }

        [TestMethod]
        public void TestName_NameLengthLessThan2_ReturnsFalse()
        {
            string name = "A";
            bool result = inputvalidation.ValidateName(name);

            Assert.AreEqual(result, false, "Name length less than 2!");
        }

        [TestMethod]
        public void TestName_NameLengthGreaterThan50_ReturnsFalse()
        {
            string name = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            bool result = inputvalidation.ValidateName(name);

            Assert.AreEqual(result, false, "Name length greater than 50!");
        }

        [TestMethod]
        public void TestName_NameFormatError_ReturnsFalse()
        {
            string name = "$$%%^^";
            bool result = inputvalidation.ValidateName(name);

            Assert.AreEqual(result, false, "Name is in wrong format!");
        }

        [TestMethod]
        public void TestName_NameOk_ReturnsTrue()
        {
            string name = "Tom";
            bool result = inputvalidation.ValidateName(name);

            Assert.AreEqual(result, true, "Name format is ok!");
        }

        [TestMethod]
        public void TestAddress_AddressIsNull_ReturnsFalse()
        {
            string address = "";
            bool result = inputvalidation.ValidateAddress(address);

            Assert.AreEqual(result, false, "Address is null");
        }

        [TestMethod]
        public void TestAddress_AddressFormatError_ReturnsFalse()
        {
            string address = "$$%%^^";
            bool result = inputvalidation.ValidateAddress(address);

            Assert.AreEqual(result, false, "Address is in wrong format!");
        }

        [TestMethod]
        public void TestAddress_AddressOk_ReturnsTrue()
        {
            string address = "66,Boul BeaconsField";
            bool result = inputvalidation.ValidateAddress(address);

            Assert.AreEqual(result, true, "Address format is right!");
        }


        [TestMethod]
        public void TestPhone_PhoneIsNull_ReturnsFalse()
        {
            string phone = "";
            bool result = inputvalidation.ValidatePhone(phone);

            Assert.AreEqual(result, false, "Phone is null");
        }

        [TestMethod]
        public void TestPhone_PhoneFormatError_ReturnsFalse()
        {
            string phone = "5145554443";
            bool result = inputvalidation.ValidatePhone(phone);

            Assert.AreEqual(result, false, "Phone is in wrong format!");
        }

        [TestMethod]
        public void TestPhone_PhoneOk_ReturnsTrue()
        {
            string phone = "(514) 443-7211";
            bool result = inputvalidation.ValidatePhone(phone);

            Assert.AreEqual(result, true, "Phone format is right!");
        }

        [TestMethod]
        public void TestZipcode_ZipcodeIsNull_ReturnsFalse()
        {
            string zipcode = "";
            bool result = inputvalidation.ValidateZipcode(zipcode);

            Assert.AreEqual(result, false, "Zipcode is null");
        }

        [TestMethod]
        public void TestZipcode_ZipcodeFormatError_ReturnsFalse()
        {
            string zipcode = "AsD9876";
            bool result = inputvalidation.ValidateZipcode(zipcode);

            Assert.AreEqual(result, false, "Zipcode is in wrong format!");
        }

        [TestMethod]
        public void TestZipcode_ZipcodeOk_ReturnsTrue()
        {
            string zipcode = "H9J 1N6";
            bool result = inputvalidation.ValidateZipcode(zipcode);

            Assert.AreEqual(result, true, "Zipcode format is right!");
        }

        [TestMethod]
        public void TestEmail_EmailIsNull_ReturnsFalse()
        {
            string email = "";
            bool result = inputvalidation.ValidateEmail(email);

            Assert.AreEqual(result, false, "Email is null");
        }

        [TestMethod]
        public void TestEmail_EmailFormatError_ReturnsFalse()
        {
            string email = "AASSDDFF";
            bool result = inputvalidation.ValidateEmail(email);

            Assert.AreEqual(result, false, "Email is in wrong format!");
        }

        [TestMethod]
        public void TestEmail_EmailOk_ReturnsTrue()
        {
            string email = "Jerry@gmail.com";
            bool result = inputvalidation.ValidateEmail(email);

            Assert.AreEqual(result, true, "Email format is right!");
        }
    }
}
